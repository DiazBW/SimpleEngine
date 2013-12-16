using System;
using System.Collections.Generic;
using System.Linq;
using SimpleEngine.Interfaces;

namespace SimpleEngine.Classes.Game
{
    public partial class Game : IGame
    {
        // TODO: remove
        private const Int32 BOARD_SIZE = 19;

        public Boolean IsGameOver
        {
            get
            {
                return IsPlayerOneSkip && IsPlayerTwoSkip;
            }
        }

        public Boolean IsPlayerOneSkip { get; private set; }
        public Boolean IsPlayerTwoSkip { get; private set; }

        public readonly Int32 PlayerOneId;
        public readonly Int32 PlayerTwoId;
        public readonly GameScore Score;

        public Int32 ActivePlayerId { get; private set; }
        public CellType ActiveCellType
        {
            get { return ActivePlayerId == PlayerOneId ? CellType.Black : CellType.White; }
        }

        public List<Shape> Shapes;

        public Board Board
        {
            get
            {
                var board = new Board(BOARD_SIZE);
                foreach (var shape in Shapes)
                {
                    foreach (var cell in shape.Cells)
                    {
                        board.Cells[cell.RowIndex, cell.ColumnIndex] = shape.CellTypeValue;
                    }
                }
                return board;
            }
        }

        private string _previousBoardHashForPlayerOne = String.Empty;
        private string _previousBoardHashForPlayerTwo = String.Empty;

        private readonly PlayerValidator _playerValidator;
        private readonly TurnValidator _turnValidator;

        public Game(Int32 playerOneId, Int32 playerTwoId)
        {
            PlayerOneId = playerOneId;
            PlayerTwoId = playerTwoId;
            ActivePlayerId = PlayerOneId;
            Score = new GameScore();

            Shapes = new List<Shape>();

            _playerValidator = new PlayerValidator(this);
            _turnValidator = new TurnValidator(this);
        }

        private Boolean EmulateTurnAndCheck(GameTurnStruct turn, Func<Boolean> checkFunc)
        {
            // Save game state
            var shapesBeforeStep = Shape.GetDeepCopy(Shapes);

            // Calculate turn 
            TurnProceed(turn);

            // Run check
            var checkResult = checkFunc.Invoke();

            // Load game state
            Shapes = shapesBeforeStep;

            return checkResult;
        }

        public void PlayerSkipTurn(Int32 playerId)
        {
            if (IsGameOver) return;

            SkipTurnValidation(playerId);
            SkipTurnProceed();
        }

        private void SkipTurnValidation(Int32 playerId)
        {
            _playerValidator.ValidateTurnSkiping(playerId);
        }

        private void SkipTurnProceed()
        {
            ActivePlayerSkipTurnProceed();

            if (IsGameOver)
            {
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
            if (ActivePlayerId == PlayerOneId)
            {
                IsPlayerOneSkip = true;
            }
            else
            {
                IsPlayerTwoSkip = true;
            }
        }

        private void CalculateFinalScore()
        {
            if (!IsGameOver) return;

            Score.GameFinished(Board);
        }

        private void DropTurnSkipingState()
        {
            IsPlayerOneSkip = false;
            IsPlayerTwoSkip = false;
        }

        public void Turn(Int32 rowIndex, Int32 columnIndex, Int32 playerId)
        {
            var turn = new GameTurnStruct()
            {
                RowIndex = rowIndex,
                ColumnIndex = columnIndex,
                Value = ActiveCellType
            };
            Turn(turn, playerId);
        }

        private void Turn(GameTurnStruct turn, Int32 playerId)
        {
            if (IsGameOver) return;

            ValidateTurn(turn, playerId);

            TurnProceed(turn);

            DropTurnSkipingState();

            SetBoardStateForActiveUser();

            ChangeActivePlayer();
        }

        private void ValidateTurn(GameTurnStruct turn, Int32 playerId)
        {
            _playerValidator.ValidateTurn(turn, playerId);
            var previousHash = GetBoardStateForActiveUser();
            _turnValidator.Validate(turn, previousHash);
        }

        private void TurnProceed(GameTurnStruct turn)
        {
            if (IsGameOver) return;

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

        // TODO: replace with GetShapesWithoutBreath
        //private void RemoveWithoutBreath(Int32 ignoredShapeId)
        //{
        //    Shapes.RemoveAll(shape => 
        //        shape.Id != ignoredShapeId && 
        //        !HaveShapeBreath(shape)
        //    );
        //}

        private List<Shape> GetShapesWithoutBreath()
        {
            return Shapes.Where(shape => !HaveShapeBreath(shape)).ToList();
        }

        private void ChangeActivePlayer()
        {
            ActivePlayerId = ActivePlayerId == PlayerOneId ? PlayerTwoId : PlayerOneId;
        }

        private string GetBoardStateForActiveUser()
        {
            return ActivePlayerId == PlayerOneId ? _previousBoardHashForPlayerTwo : _previousBoardHashForPlayerOne;
        }

        //TODO: refactoring _hashes
        private void SetBoardStateForActiveUser()
        {
            var currentHash = Board.GetCustomHash();
            if (ActivePlayerId == PlayerOneId)
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
            return Shapes.Where(s => s.IsConnectedWith(turn.RowIndex, turn.ColumnIndex, turn.Value, BOARD_SIZE)).ToList();
        }

        // TODO: change to linq after tests
        private bool HaveShapeBreath(Shape shape)
        {
            var connections = shape.GetConnectionCells(BOARD_SIZE, BOARD_SIZE);
            return connections.Any(connection => Board.Cells[connection.RowIndex, connection.ColumnIndex] == CellType.Empty);
        }

        private void FillBoardWithRocksAfterGameFinished()
        {
            if (!IsGameOver) return;

            while (HasBoardEmptyCells())
            {
                foreach (var shape in Shapes)
                {
                    var connections = shape.GetConnectionCells(Board.Size, Board.Size);
                    foreach (var conn in connections)
                    {
                        if (Board.Cells[conn.RowIndex, conn.ColumnIndex] == CellType.Empty)
                            shape.Add(conn.RowIndex, conn.ColumnIndex);
                    }
                }
            }
        }

        //TODO: for / for board.Cell - replace with enumerator.
        //TODO: board.Cell[] - replace with indexer like Board[i,j].
        private bool HasBoardEmptyCells()
        {
            for (var i = 0; i < Board.Size; i++)
            {
                for (var j = 0; j < Board.Size; j++)
                {
                    if (Board.Cells[i, j] == CellType.Empty)
                        return true;
                }
            }
            return false;
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
