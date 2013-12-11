using System;
using System.Collections.Generic;
using System.Linq;
using SimpleEngine.Interfaces;

namespace SimpleEngine.Classes
{
    public class GameWithShapes : IGame
    {
        private readonly List<Turn> _history;
        private const Int32 BOARD_SIZE = 19;

        public Boolean IsGameOver { get; private set; }

        public Int32 ActivePlayerId { get; private set; }
        public CellType ActiveCellType
        {
            get { return ActivePlayerId == PlayerOneId ? CellType.Black : CellType.White; }
        }

        private readonly ITurnResultCalculator _turnResultCalculator;
        private readonly ITurnValidator _turnValidator;

        public readonly Int32 PlayerOneId;
        public readonly Int32 PlayerTwoId;
        public Board Board;
        public readonly List<Shape> Shapes;

        public GameWithShapes(Int32 playerOneId, Int32 playerTwoId)
        {
            PlayerOneId = playerOneId;
            PlayerTwoId = playerTwoId;
            ActivePlayerId = PlayerOneId;
            Board = new Board(BOARD_SIZE);

            //TODO: inject via autofac
            _turnValidator = new DefaultTurnValidator();
            _turnResultCalculator = new TurnResultCalculator();
            _history = new List<Turn>();

            Shapes = new List<Shape>();
        }

        public Int32 GetWinPlayerId()
        {
            throw new NotImplementedException();
        }

        public CellType GetCellValue(Int32 rowIndex, Int32 columnIndex)
        {
            return Board.Cells[rowIndex, columnIndex];
        }

        //public bool IsGameOver()
        //{
        //    throw new NotImplementedException();
        //}

        public void Surrender(Int32 playerId)
        {
            PlayerIdValidation(playerId);

            IsGameOver = true;
        }

        public void Turn(Int32 rowIndex, Int32 columnIndex, Int32 playerId)
        {
            if (IsGameOver) return;

            ValidateTurn(rowIndex, columnIndex, playerId);

            TurnProceed(rowIndex, columnIndex);
            AddHistoryItem(rowIndex, columnIndex);
            ChangeActivePlayer();

            RecalculateBoardState();
        }

        //TODO: remove later
        public void DevTurn(Int32 rowIndex, Int32 columnIndex)
        {
            if (IsGameOver) return;
            //ValidateTurn(rowIndex, columnIndex, playerId);

            TurnProceed(rowIndex, columnIndex);
            AddHistoryItem(rowIndex, columnIndex);
            ChangeActivePlayer();

            RecalculateBoardState();
        }

        private void TurnProceed(Int32 rowIndex, Int32 columnIndex)
        {
            Board.Cells[rowIndex, columnIndex] = ActiveCellType;
        }

        private void AddHistoryItem(int rowIndex, int columnIndex)
        {
            var newTurn = new Turn() { RowIndex = rowIndex, ColumnIndex = columnIndex, PlayerId = ActivePlayerId };
            _history.Add(newTurn);
        }

        private void ChangeActivePlayer()
        {
            ActivePlayerId = ActivePlayerId == PlayerOneId ? PlayerTwoId : PlayerOneId;
        }

        private void ValidateTurn(int rowIndex, int columnIndex, int playerId)
        {
            PlayerIdValidation(playerId);
            _turnValidator.Validate(rowIndex, columnIndex, ActiveCellType, Board);
        }

        private void PlayerIdValidation(int playerId)
        {
            if (!IsPlayerInGame(playerId))
            {
                var msg = String.Format("The game has not player with id {0}.", playerId);
                throw new ArgumentException(msg);
            }

            if (!IsPlayerActive(playerId))
            {
                var msg = String.Format("Player with id {0} can not perform turn. Now is another player's turn.", playerId);
                throw new ArgumentException(msg);
            }
        }

        public bool IsPlayerInGame(Int32 playerId)
        {
            return PlayerOneId == playerId || PlayerTwoId == playerId;
        }

        public bool IsPlayerActive(Int32 playerId)
        {
            return ActivePlayerId == playerId;
        }

        private void RecalculateBoardState()
        {
            RegenerateShapes(Board);
            RemoveWithoutBreath();
            //_turnResultCalculator.CalculateNewBoardState(ref Board);

            ClearBoard();
            foreach (var shape in Shapes)
            {
                foreach (var cell in shape.Cells)
                {
                    Board.Cells[cell.RowIndex, cell.ColumnIndex] = shape.CellTypeValue;
                }
            }
        }

        //TODO: clear active player id also
        public void ClearBoard()
        {
            if (IsGameOver) return;

            for (var i = 0; i < BOARD_SIZE; i++)
            {
                for (var j = 0; j < BOARD_SIZE; j++)
                {
                    Board.Cells[i, j] = CellType.Empty;
                }
            }
        }

        private void RegenerateShapes(Board board)
        {
            Shapes.Clear();
            for (var i = 0; i < board.Size; i++)
            {
                for (var j = 0; j < board.Size; j++)
                {
                    var cellValue = board.Cells[i, j];
                    if (cellValue == CellType.Empty || Shapes.Any(s => s.Contains(rowIndex: i, columnIndex: j)))
                        continue;

                    var currentCell = new CellStruct() { RowIndex = i, ColumnIndex = j };
                    var connectedShapeIdx = Shapes.FindIndex(s => s.GetConnectionCells(board.Size, board.Size).Contains(currentCell) && s.CellTypeValue == cellValue);
                    if (connectedShapeIdx >= 0)
                    {
                        Shapes[connectedShapeIdx].Add(currentCell.RowIndex, currentCell.ColumnIndex);
                    }
                    else
                    {
                        var newShape = new Shape(cellValue);
                        newShape.Add(currentCell.RowIndex, currentCell.ColumnIndex);
                        Shapes.Add(newShape);
                    }
                }
            }
        }

        public void RemoveWithoutBreath()
        {
            for (var i = 0; i < Shapes.Count; i++)
            {
                if (!HaveShapeBreath(Shapes[i]))
                {
                    Shapes.RemoveAt(i);
                }
            }
        }

        private bool HaveShapeBreath(Shape shape)
        {
            //return shape.GetConnectionCells(BOARD_SIZE, BOARD_SIZE).Any(c => c.Value == CellValueType.Empty);
            var connections = shape.GetConnectionCells(BOARD_SIZE, BOARD_SIZE);
            foreach (var connection in connections)
            {
                if (Board.Cells[connection.RowIndex, connection.ColumnIndex] == CellType.Empty)
                    return true;
            }
            return false;

            // TODO: change after debug
            //var connections = shape.GetConnectionCells(BOARD_SIZE, BOARD_SIZE);
            //return connections.Any(connection => Board.Cells[connection.RowIndex, connection.ColumnIndex] == CellType.Empty);
        }

        public List<String> GetBoardTextRepresentation()
        {
            var res = new List<String>();
            for (var i = 0; i < BOARD_SIZE; i++)
            {
                var str = String.Empty;
                for (var j = 0; j < BOARD_SIZE; j++)
                {
                    var ch = String.Empty;
                    var cellValue = Board.Cells[i, j];
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

    public class Shape
    {
        public CellType CellTypeValue { get; private set; }
        // Switch to set ?
        public readonly List<CellStruct> Cells;

        public Shape(CellType cellTypeValue)
        {
            CellTypeValue = cellTypeValue;
            Cells = new List<CellStruct>();
        }

        public bool Contains(int rowIndex, int columnIndex)
        {
            return Cells.Any(cell => cell.RowIndex == rowIndex && cell.ColumnIndex == columnIndex);
        }

        public void Add(int rowIndex, int columnIndex)
        {
            if (Contains(rowIndex, columnIndex))
                return;
            var newCell = new CellStruct() { RowIndex = rowIndex, ColumnIndex = columnIndex };
            Cells.Add(newCell);
        }

        //public void Remove(int x, int y, CellType cellType)
        //{
        //    if (!Contains(x, y, cellType))
        //        return;
        //    var cell = new CellStruct() { x = x, y = y, Value = cellType };
        //    Cells.Remove(cell);
        //}

        public List<CellStruct> GetConnectionCells(int maxRowValue, int maxColumnValue)
        {
            var connections = new List<CellStruct>();
            foreach (var cellStruct in Cells)
            {
                if (cellStruct.RowIndex - 1 >= 0 && cellStruct.RowIndex - 1 < maxRowValue)
                {
                    var row = cellStruct.RowIndex - 1;
                    var col = cellStruct.ColumnIndex;
                    if (!Contains(row, col))
                        connections.Add(new CellStruct() { RowIndex = row, ColumnIndex = col });
                }

                if (cellStruct.RowIndex + 1 >= 0 && cellStruct.RowIndex + 1 < maxRowValue)
                {
                    var row = cellStruct.RowIndex + 1;
                    var col = cellStruct.ColumnIndex;
                    if (!Contains(row, col))
                        connections.Add(new CellStruct() { RowIndex = row, ColumnIndex = col });
                }

                if (cellStruct.ColumnIndex - 1 >= 0 && cellStruct.ColumnIndex - 1 < maxColumnValue)
                {
                    var row = cellStruct.RowIndex;
                    var col = cellStruct.ColumnIndex - 1;
                    if (!Contains(row, col))
                        connections.Add(new CellStruct() { RowIndex = row, ColumnIndex = col });
                }

                if (cellStruct.ColumnIndex + 1 >= 0 && cellStruct.ColumnIndex + 1 < maxColumnValue)
                {
                    var row = cellStruct.RowIndex;
                    var col = cellStruct.ColumnIndex + 1;
                    if (!Contains(row, col))
                        connections.Add(new CellStruct() { RowIndex = row, ColumnIndex = col });
                }
            }
            return connections;
        }
    }

    public struct CellStruct
    {
        public Int32 RowIndex;
        public Int32 ColumnIndex;
    }
}
