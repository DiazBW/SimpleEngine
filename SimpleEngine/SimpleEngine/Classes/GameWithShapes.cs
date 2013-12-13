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
            //return Board.Cells[rowIndex, columnIndex];
            var board = GetBoard();
            return board.Cells[rowIndex, columnIndex];
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
            //RecalculateBoardState(rowIndex, columnIndex, ActiveCellType);
        }

        //TODO: remove later
        public void DevTurn(Int32 rowIndex, Int32 columnIndex)
        {
            throw new NotImplementedException();


            if (IsGameOver) return;
            //ValidateTurn(rowIndex, columnIndex, playerId);

            TurnProceed(rowIndex, columnIndex);
            AddHistoryItem(rowIndex, columnIndex);
            ChangeActivePlayer();

            //TODO: move into TurnProcess
            //RecalculateBoardState();
        }

        private void TurnProceed(Int32 rowIndex, Int32 columnIndex)
        {
            var newShapeId = CreateNewShape(rowIndex, columnIndex);

            MergeNewShapeIfPossible(newShapeId);

            //TODO: remove duplicated
            //UpdateBoardByShapes();

            RemoveWithoutBreathAlt(ignoredShapeId : newShapeId);

            //TODO: remove duplicated - refactoring somehow
            //UpdateBoardByShapes();
        }

        //private void UpdateBoardByShapes()
        //{
        //    ClearBoard();
        //    foreach (var shape in Shapes)
        //    {
        //        foreach (var cell in shape.Cells)
        //        {
        //            Board.Cells[cell.RowIndex, cell.ColumnIndex] = shape.CellTypeValue;
        //        }
        //    }
        //}

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

        private void RemoveWithoutBreathAlt(Int32 ignoredShapeId)
        {
            Shapes.RemoveAll(shape => 
                shape.Id != ignoredShapeId && 
                !HaveShapeBreath(shape)
            );
        }

        private Int32 CreateNewShape(int rowIndex, int columnIndex)
        {
            var newShapeId = GetNewShapeId();
            var newShape = new Shape(ActiveCellType, newShapeId);
            newShape.Add(rowIndex, columnIndex);

            Shapes.Add(newShape);

            return newShapeId;
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
            var board = GetBoard();
            _turnValidator.Validate(rowIndex, columnIndex, ActiveCellType, board);
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

        
        //private void RecalculateBoardState()
        //private void RecalculateBoardState(int rowIndex, int colIndex, CellType newCellValue)
        //{
        //    RegenerateShapesAlt(rowIndex, colIndex, newCellValue);
        //    RemoveWithoutBreath();

        //    //RegenerateShapes(Board);
        //    //RemoveWithoutBreath();
        //    ////_turnResultCalculator.CalculateNewBoardState(ref Board);

        //    //ClearBoard();
        //    //foreach (var shape in Shapes)
        //    //{
        //    //    foreach (var cell in shape.Cells)
        //    //    {
        //    //        Board.Cells[cell.RowIndex, cell.ColumnIndex] = shape.CellTypeValue;
        //    //    }
        //    //}
        //}

        //private void RecalculateBoardState()
        //{
        //    RegenerateShapes(Board);
        //    RemoveWithoutBreath();
        //    //_turnResultCalculator.CalculateNewBoardState(ref Board);

        //    ClearBoard();
        //    foreach (var shape in Shapes)
        //    {
        //        foreach (var cell in shape.Cells)
        //        {
        //            Board.Cells[cell.RowIndex, cell.ColumnIndex] = shape.CellTypeValue;
        //        }
        //    }
        //}

        //TODO: clear active player id also
        //public void ClearBoard()
        //{
        //    if (IsGameOver) return;

        //    for (var i = 0; i < BOARD_SIZE; i++)
        //    {
        //        for (var j = 0; j < BOARD_SIZE; j++)
        //        {
        //            Board.Cells[i, j] = CellType.Empty;
        //        }
        //    }
        //}

        //TODO: clear active player id also
        public void ClearBoard()
        {
            if (IsGameOver) return;

            Shapes.Clear();
            //for (var i = 0; i < BOARD_SIZE; i++)
            //{
            //    for (var j = 0; j < BOARD_SIZE; j++)
            //    {
            //        Board.Cells[i, j] = CellType.Empty;
            //    }
            //}
        }

        //private void RegenerateShapes(Board board)
        //{
        //    Shapes.Clear();
        //    for (var i = 0; i < board.Size; i++)
        //    {
        //        for (var j = 0; j < board.Size; j++)
        //        {
        //            var cellValue = board.Cells[i, j];
        //            if (cellValue == CellType.Empty || Shapes.Any(s => s.Contains(rowIndex: i, columnIndex: j)))
        //                continue;

        //            var currentCell = new CellStruct() { RowIndex = i, ColumnIndex = j };
        //            var connectedShapeIdx = Shapes.FindIndex(s => s.GetConnectionCells(board.Size, board.Size).Contains(currentCell) && s.CellTypeValue == cellValue);
        //            if (connectedShapeIdx >= 0)
        //            {
        //                Shapes[connectedShapeIdx].Add(currentCell.RowIndex, currentCell.ColumnIndex);
        //            }
        //            else
        //            {
        //                var newShapeId = GetNewShapeId();
        //                var newShape = new Shape(cellValue, newShapeId);
        //                newShape.Add(currentCell.RowIndex, currentCell.ColumnIndex);
        //                Shapes.Add(newShape);
        //            }
        //        }
        //    }
        //}

        //private void RegenerateShapesAlt(Board board, Int32 rowIndex, Int32 columnIndex, CellType newCellValue)
        //private void RegenerateShapesAlt(Int32 rowIndex, Int32 columnIndex, CellType newCellValue)
        //{
        //    var newShapeId = GetNewShapeId();
        //    var newShape = new Shape(newCellValue, newShapeId);
        //    newShape.Add(rowIndex, columnIndex);
        //    Shapes.Add(newShape);

        //    List<Int32> shapeIds = GetConnectedShapeIds(rowIndex, columnIndex, newCellValue);
        //    MergeShapesInto(newShapeId, shapeIds);
        //    //MergeShapes(rowIndex, columnIndex, newCellValue, shapeIds);
            
        //    //Shapes.Clear();
        //    //for (var i = 0; i < board.Size; i++)
        //    //{
        //    //    for (var j = 0; j < board.Size; j++)
        //    //    {
        //    //        var cellValue = board.Cells[i, j];
        //    //        if (cellValue == CellType.Empty || Shapes.Any(s => s.Contains(rowIndex: i, columnIndex: j)))
        //    //            continue;

        //    //        var currentCell = new CellStruct() { RowIndex = i, ColumnIndex = j };
        //    //        var connectedShapeIdx = Shapes.FindIndex(s => s.GetConnectionCells(board.Size, board.Size).Contains(currentCell) && s.CellTypeValue == cellValue);
        //    //        if (connectedShapeIdx >= 0)
        //    //        {
        //    //            Shapes[connectedShapeIdx].Add(currentCell.RowIndex, currentCell.ColumnIndex);
        //    //        }
        //    //        else
        //    //        {
        //    //            var newShapeId = GetNewShapeId();
        //    //            var newShape = new Shape(cellValue, newShapeId);
        //    //            newShape.Add(currentCell.RowIndex, currentCell.ColumnIndex);
        //    //            Shapes.Add(newShape);
        //    //        }
        //    //    }
        //    //}
        //}

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

        //private void MergeShapes(int rowIndex, int columnIndex, CellType newCellValue, List<Int32> shapeIds)
        //private void MergeShapes(Shape sh, List<Int32> shapeIds)
        //private void MergeShapes(int rowIndex, int columnIndex, CellType newCellValue, List<Int32> shapeIds)
        //{
        //    var newShapeId = GetNewShapeId();
        //    var newShape = new Shape(newCellValue, newShapeId);
        //    var requiredShapes = Shapes.Where(s => shapeIds.Contains(s.Id));
        //    foreach (var shape in requiredShapes)
        //    {
        //        foreach (var cell in shape.Cells)
        //        {
        //            newShape.Add(cell.RowIndex, cell.ColumnIndex);
        //        }
        //    }
        //    Shapes.RemoveAll(s => shapeIds.Contains(s.Id));
        //}

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

        //public void RemoveWithoutBreath()
        //{
        //    for (var i = 0; i < Shapes.Count; i++)
        //    {
        //        if (!HaveShapeBreath(Shapes[i]))
        //        {
        //            Shapes.RemoveAt(i);
        //        }
        //    }
        //}

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

    public class Shape
    {
        public Int32 Id { get; private set; }
        public CellType CellTypeValue { get; private set; }
        // Switch to set ?
        public readonly List<CellStruct> Cells;

        public Shape(CellType cellTypeValue, Int32 id)
        {
            CellTypeValue = cellTypeValue;
            Id = id;
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

        public bool IsConnectedWith(int rowIndex, int columnIndex, CellType cellValue, int boardSize)
        {
            if (CellTypeValue != cellValue) return false;

            var cells = GetConnectionCells(boardSize, boardSize);
            return cells.Any(c => c.RowIndex == rowIndex && c.ColumnIndex == columnIndex);
        }

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
