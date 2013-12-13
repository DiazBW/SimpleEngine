using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleEngine.Classes.Game
{
    public partial class Game
    {
        private const Int32 BOARD_SIZE = 19;

        public Boolean IsGameOver { get; private set; }

        public Int32 ActivePlayerId { get; private set; }
        public CellType ActiveCellType
        {
            get { return ActivePlayerId == PlayerOneId ? CellType.Black : CellType.White; }
        }

        public List<Shape> Shapes;

        public readonly Int32 PlayerOneId;
        public readonly Int32 PlayerTwoId;

        private string _previousBoardHashForPlayerOne = String.Empty;
        private string _previousBoardHashForPlayerTwo = String.Empty;
        private readonly PlayerValidator _playerValidator;
        private readonly TurnValidator _turnValidator;

        public Game(Int32 playerOneId, Int32 playerTwoId)
        {
            PlayerOneId = playerOneId;
            PlayerTwoId = playerTwoId;
            ActivePlayerId = PlayerOneId;

            Shapes = new List<Shape>();
            _playerValidator = new PlayerValidator(this);
            _turnValidator = new TurnValidator(this);
        }

        public Board GetBoard()
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

        public void Turn(Int32 rowIndex, Int32 columnIndex, Int32 playerId)
        {
            if (IsGameOver) return;

            ValidateTurn(rowIndex, columnIndex, playerId);

            TurnProceed(rowIndex, columnIndex);

            SetBoardStateForActiveUser();

            ChangeActivePlayer();
        }

        #region Validation

        private List<Shape> DeepCopy(List<Shape> originObject)
        {
            var res = new List<Shape>();
            foreach (var shape in originObject)
            {
                var newShape = new Shape(shape.CellTypeValue, shape.Id);
                foreach (var cell in shape.Cells)
                {
                    newShape.Add(cell.RowIndex, cell.ColumnIndex);
                }
                res.Add(newShape);
            }
            return res;
        }
        #endregion Validation

        private void TurnProceed(Int32 rowIndex, Int32 columnIndex)
        {
            var newShapeId = CreateNewShape(rowIndex, columnIndex);

            MergeNewShapeIfPossible(newShapeId);

            RemoveWithoutBreathAlt(ignoredShapeId : newShapeId);
        }

        // TODO: chech for new shape existensce
        // TODO: check for cells count
        // TODO: search connections interraction ?
        private void MergeNewShapeIfPossible(int newShapeId)
        {
            var newShape = Shapes.FirstOrDefault(s => s.Id == newShapeId);
            var rowIndex = newShape.Cells[0].RowIndex;
            var columnIndex = newShape.Cells[0].ColumnIndex;

            List<Int32> connectedShapeIds = GetConnectedShapeIds(rowIndex, columnIndex, ActiveCellType);
            MergeShapesInto(newShapeId, connectedShapeIds);
        }

        // TODO: replace with GetShapesWithoutBreath
        private void RemoveWithoutBreathAlt(Int32 ignoredShapeId)
        {
            Shapes.RemoveAll(shape => 
                shape.Id != ignoredShapeId && 
                !HaveShapeBreath(shape)
            );
        }

        private List<Shape> GetShapesWithoutBreath()
        {
            return Shapes.Where(shape => !HaveShapeBreath(shape)).ToList();
        }

        private Int32 CreateNewShape(int rowIndex, int columnIndex)
        {
            var newShapeId = GetNewShapeId();
            var newShape = new Shape(ActiveCellType, newShapeId);
            newShape.Add(rowIndex, columnIndex);

            Shapes.Add(newShape);

            return newShapeId;
        }

        private void ChangeActivePlayer()
        {
            ActivePlayerId = ActivePlayerId == PlayerOneId ? PlayerTwoId : PlayerOneId;
        }

        private void ValidateTurn(int rowIndex, int columnIndex, int playerId)
        {
            _playerValidator.Validation(playerId);
            var board = GetBoard();
            var previousHash = GetBoardStateForActiveUser();
            _turnValidator.Validate(rowIndex, columnIndex, ActiveCellType, board, previousHash);
        }

        private string GetBoardStateForActiveUser()
        {
            return ActivePlayerId == PlayerOneId ? _previousBoardHashForPlayerTwo : _previousBoardHashForPlayerOne;
        }

        //TODO: refactoring _hashes
        private void SetBoardStateForActiveUser()
        {
            var currentHash = GetBoard().GetCustomHash();
            if (ActivePlayerId == PlayerOneId)
            {
                _previousBoardHashForPlayerTwo = currentHash;
            }
            else
            {
                _previousBoardHashForPlayerOne = currentHash;
            }
        }

        //TODO: clear active player id also
        public void ClearBoard()
        {
            if (IsGameOver) return;

            Shapes.Clear();
        }

        //TODO: shapeIds.Contains(newShapeId) have to be false! add check
        //TODO: merge only same cellValue
        private void MergeShapesInto(int shapeId, List<int> shapesForMergeIds)
        {
            //TODO: exception handling
            var shapeForMergeInto = Shapes.FirstOrDefault(s => s.Id == shapeId);
            
            var requiredShapes = Shapes.Where(s => shapesForMergeIds.Contains(s.Id) && s.CellTypeValue == shapeForMergeInto.CellTypeValue);
            foreach (var shape in requiredShapes)
            {
                foreach (var cell in shape.Cells)
                {
                    shapeForMergeInto.Add(cell.RowIndex, cell.ColumnIndex);
                }
            }
            Shapes.RemoveAll(s => shapesForMergeIds.Contains(s.Id));
        }

        //TODO: change it with something else - cause id just grown.
        private Int32 GetNewShapeId()
        {
            if (Shapes.Count <= 0) 
                return 0;

            Int32 maxId = Shapes.Max(s => s.Id);
            return maxId + 1;
        }

        // TODO: NOT CHECK VALUE! only places
        private List<int> GetConnectedShapeIds(int rowIndex, int columnIndex, CellType newCellValue)
        {
            var res = new List<int>();
            for(var i=0; i<Shapes.Count; i++)
            {
                if(Shapes[i].IsConnectedWith(rowIndex, columnIndex, newCellValue, BOARD_SIZE))
                {
                    res.Add(Shapes[i].Id);
                }
            }
            return res;
        }

        // TODO: change to linq after tests
        private bool HaveShapeBreath(Shape shape)
        {
            var connections = shape.GetConnectionCells(BOARD_SIZE, BOARD_SIZE);
            var board = GetBoard();
            foreach (var connection in connections)
            {

                if (board.Cells[connection.RowIndex, connection.ColumnIndex] == CellType.Empty)
                    return true;
            }
            return false;
        }

        public List<String> GetBoardTextRepresentation()
        {
            var board = GetBoard();
            var res = new List<String>();
            for (var i = 0; i < BOARD_SIZE; i++)
            {
                var str = String.Empty;
                for (var j = 0; j < BOARD_SIZE; j++)
                {
                    var ch = String.Empty;
                    var cellValue = board.Cells[i, j];
                    var shapeIdx = Shapes.FindIndex(s => s.Contains(rowIndex: i, columnIndex: j)).ToString();
                    if (shapeIdx == "-1") shapeIdx = "-";
                    switch (cellValue)
                    {
                        case CellType.Empty:
                            ch = "._" + shapeIdx.ToString();
                            break;
                        case CellType.Black:
                            ch = "o_" + shapeIdx.ToString();
                            break;
                        case CellType.White:
                            ch = "x_" + shapeIdx.ToString();
                            break;
                    }
                    str += ch + "   ";
                }
                res.Add(str);
            }
            return res;
        }
    }

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
