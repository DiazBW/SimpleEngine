using System;
using System.Collections.Generic;
using System.Linq;
using SimpleEngine.Interfaces;

namespace SimpleEngine.Classes.Game
{
    // TODO: add regions!
    // TODO: shapes and board - improve somehow
    public partial class Game : IGame
    {
        private readonly Int32 BoardSize = 19;
        private List<Shape> Shapes;

        public GameState CurrentGameState { get; private set; }
        public readonly GameScore Score;

        //TODO: move into GameState?
        private string _previousBoardHashForPlayerOne = String.Empty;
        private string _previousBoardHashForPlayerTwo = String.Empty;

        private readonly PlayerValidator _playerValidator;
        private readonly TurnValidator _turnValidator;

        public Game(Int32 playerOneId, Int32 playerTwoId)
        {
            CurrentGameState = new GameState
            {
                ActivePlayerId = playerOneId,
                PlayerOneId = playerOneId,
                PlayerTwoId = playerTwoId,
                IsPlayerOneSkip = false,
                IsPlayerTwoSkip = false,
                SurrenderPlayerId = null,
                Board = new Board(BoardSize)
            };

            Score = new GameScore();
            Shapes = new List<Shape>();

            _playerValidator = new PlayerValidator(this);
            _turnValidator = new TurnValidator(this);
        }

        #region IGame interface implementation
        //TODO: turn validation!
        public void Surrender(Int32 playerId)
        {
            if (CurrentGameState.IsGameOver)
                return;
            if (playerId != CurrentGameState.PlayerOneId && playerId != CurrentGameState.PlayerTwoId)
                throw new ArgumentException("playerId");

            CurrentGameState.SurrenderPlayerId = playerId;
        }

        //TODO: turn validation!
        public void SkipTurn(Int32 playerId)
        {
            if (CurrentGameState.IsGameOver) return;

            SkipTurnValidation(playerId);
            SkipTurnProceed();
        }

        public void LoadState(GameState gameState)
        {
            if (gameState.ActivePlayerId != CurrentGameState.PlayerOneId && gameState.ActivePlayerId != CurrentGameState.PlayerTwoId)
            {
                var msg = String.Format("Player {0} is not a member of this game.", gameState.ActivePlayerId);
                throw new ArgumentException(msg);
            }

            if (gameState.Board.Size != BoardSize)
            {
                var msg = String.Format("Baord size must be {0}.", BoardSize);
                throw new ArgumentException(msg);
            }

            CurrentGameState = GameState.GetDeepCopy(gameState);
            GenerateShapes(CurrentGameState.Board);
        }

        public void Turn(Int32 rowIndex, Int32 columnIndex, Int32 playerId)
        {
            if (CurrentGameState.IsGameOver) return;

            var turn = new GameTurnStruct()
            {
                RowIndex = rowIndex,
                ColumnIndex = columnIndex,
                Value = CurrentGameState.ActiveCellType
            };
            Turn(turn, playerId);
        }
        #endregion IGame interface implementation

        #region TurnValidation
        private void ValidateTurn(GameTurnStruct turn, Int32 playerId)
        {
            _playerValidator.ValidateTurn(turn, playerId);
            var previousHash = GetBoardStateForActiveUser();
            _turnValidator.Validate(turn, previousHash);
        }

        private string GetBoardStateForActiveUser()
        {
            return CurrentGameState.ActivePlayerId == CurrentGameState.PlayerOneId ? _previousBoardHashForPlayerTwo : _previousBoardHashForPlayerOne;
        }

        //BUG: rewrite state resave!
        private Boolean EmulateTurnAndCheck(GameTurnStruct turn, Int32 playerId, Func<Boolean> checkFunc)
        {
            // Save game state
            var gameStateBackup = GameState.GetDeepCopy(CurrentGameState);

            // Calculate turn 
            Turn(turn, playerId);

            // Run check
            var checkResult = checkFunc.Invoke();

            // Load game state
            LoadState(gameStateBackup);

            return checkResult;
        }
        #endregion TurnValidation

        private void GenerateShapes(Board board)
        {
            for (var rowIndex = 0; rowIndex < board.Size; rowIndex++)
            {
                for (var columnIndex = 0; columnIndex < board.Size; columnIndex++)
                {
                    if (board.Cells[rowIndex, columnIndex] == CellType.Empty)
                        continue;
                    var turn = new GameTurnStruct
                    {
                        RowIndex = rowIndex,
                        ColumnIndex = columnIndex,
                        Value = board.Cells[rowIndex, columnIndex]
                    };
                    var newShapeId = CreateNewShape(turn);
                    MergeShapesInto(newShapeId, turn);
                }
            }
        }

        private void SkipTurnValidation(Int32 playerId)
        {
            _playerValidator.ValidateTurnSkiping(playerId);
        }

        private void SkipTurnProceed()
        {
            ActivePlayerSkipTurnProceed();

            if (CurrentGameState.IsGameOver)
            {
                //TODO: into one function this pair of guys!
                FillBoardWithRocksAfterGameFinished();
                CalculateFinalScore();
            }
            else
            {
                ChangeActivePlayer();
            }
        }

        private void ActivePlayerSkipTurnProceed()
        {
            if (CurrentGameState.ActivePlayerId == CurrentGameState.PlayerOneId)
            {
                CurrentGameState.IsPlayerOneSkip = true;
            }
            else
            {
                CurrentGameState.IsPlayerTwoSkip = true;
            }
        }

        private void CalculateFinalScore()
        {
            //TODO: required?
            if (!CurrentGameState.IsGameOver) return;

            Score.GameFinished(CurrentGameState.Board);
        }

        private void DropTurnSkipingState()
        {
            CurrentGameState.IsPlayerOneSkip = false;
            CurrentGameState.IsPlayerTwoSkip = false;
        }

        private void Turn(GameTurnStruct turn, Int32 playerId)
        {
            ValidateTurn(turn, playerId);
            // TODO: ugly, think about improvement!
            SetBoardStateForActiveUser();
            UpdateGameStateExceptBoard(turn);
            UpdateShapes(turn);
            UpdateBoardByShapes();
        }

        private void UpdateGameStateExceptBoard(GameTurnStruct turn)
        {   
            DropTurnSkipingState();
            ChangeActivePlayer();
        }

        private void UpdateBoardByShapes()
        {
            CurrentGameState.Board = new Board(BoardSize);
            foreach (var shape in Shapes)
            {
                foreach (var cell in shape.Cells)
                {
                    CurrentGameState.Board.Cells[cell.RowIndex, cell.ColumnIndex] = shape.CellTypeValue;
                }
            }
        }

        private void UpdateShapes(GameTurnStruct turn)
        {
            var newShapeId = CreateNewShape(turn);
            MergeShapesInto(newShapeId, turn);
            RemoveWithoutBreath(ignoredShapeId: newShapeId);
        }

        private Int32 CreateNewShape(GameTurnStruct turn)
        {
            var newShapeId = GetNewShapeId();
            var newShape = new Shape(turn.Value, newShapeId);
            newShape.Add(turn.RowIndex, turn.ColumnIndex);

            Shapes.Add(newShape);

            return newShapeId;
        }

        //TODO: oprimize!
        private void MergeShapesInto(int shapeId, GameTurnStruct turn)
        {
            var shapeForMergeInto = Shapes.FirstOrDefault(s => s.Id == shapeId);
            var requiredShapes = GetConnectedShapes(turn);

            foreach (var shape in requiredShapes)
            {
                foreach (var cell in shape.Cells)
                {
                    shapeForMergeInto.Add(cell.RowIndex, cell.ColumnIndex);
                }
            }

            var shapeForRemoveIds = requiredShapes.Select(s => s.Id);
            Shapes.RemoveAll(s => shapeForRemoveIds.Contains(s.Id));
        }

        private void RemoveWithoutBreath(Int32 ignoredShapeId)
        {
            var shapeForRemove = GetShapesWithoutBreath();
            shapeForRemove.RemoveAll(s => s.Id == ignoredShapeId);

            shapeForRemove.ForEach(shape => Score.RocksCaptured(shape.Cells.Count, shape.CellTypeValue));

            var idsForRemove = shapeForRemove.Select(s => s.Id);
            Shapes.RemoveAll(s => idsForRemove.Contains(s.Id));
        }

        private List<Shape> GetShapesWithoutBreath()
        {
            return Shapes.Where(shape => !HaveShapeBreath(shape)).ToList();
        }

        private void ChangeActivePlayer()
        {
            CurrentGameState.ActivePlayerId = CurrentGameState.ActivePlayerId == CurrentGameState.PlayerOneId ? CurrentGameState.PlayerTwoId : CurrentGameState.PlayerOneId;
        }

        //TODO: refactoring _hashes
        private void SetBoardStateForActiveUser()
        {
            var currentHash = CurrentGameState.Board.GetCustomHash();
            if (CurrentGameState.ActivePlayerId == CurrentGameState.PlayerOneId)
            {
                _previousBoardHashForPlayerTwo = currentHash;
            }
            else
            {
                _previousBoardHashForPlayerOne = currentHash;
            }
        }

        //TODO: change it with something else - cause id just grown.
        private Int32 GetNewShapeId()
        {
            if (Shapes.Count <= 0) 
                return 0;

            Int32 maxId = Shapes.Max(s => s.Id);
            return maxId + 1;
        }

        private List<Shape> GetConnectedShapes(GameTurnStruct turn)
        {
            return Shapes.Where(s => s.IsConnectedWith(turn.RowIndex, turn.ColumnIndex, turn.Value, BoardSize)).ToList();
        }

        // TODO: change to linq after tests
        private bool HaveShapeBreath(Shape shape)
        {
            var connections = shape.GetConnectionCells(BoardSize, BoardSize);
            return connections.Any(connection => CurrentGameState.Board.Cells[connection.RowIndex, connection.ColumnIndex] == CellType.Empty);
        }

        // BUG: if board stored into memory - while never end
        private void FillBoardWithRocksAfterGameFinished()
        {
            if (!CurrentGameState.IsGameOver) return;

            while (CurrentGameState.Board.HasEmptyCell())
            {
                foreach (var shape in Shapes)
                {
                    var connections = shape.GetConnectionCells(BoardSize, BoardSize);
                    foreach (var conn in connections)
                    {
                        if (CurrentGameState.Board.Cells[conn.RowIndex, conn.ColumnIndex] == CellType.Empty)
                            shape.Add(conn.RowIndex, conn.ColumnIndex);
                    }
                }
            }
        }
    }

    // TODO: вызерать наверное это нафиг
    public struct CellStruct
    {
        public Int32 RowIndex;
        public Int32 ColumnIndex;
    }

    public struct GameTurnStruct
    {
        public Int32 RowIndex;
        public Int32 ColumnIndex;
        public CellType Value;
    }
}
