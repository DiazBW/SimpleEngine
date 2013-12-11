using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using SimpleEngine.Interfaces;

namespace SimpleEngine.Classes
{
    class TurnResultCalculator : ITurnResultCalculator
    {
        //TODO: without ref ?
        //TODO: use Turn - turn can suicide shape but with elimination opponent`s shape.
        public void CalculateNewBoardState(ref Board board)
        {
            //throw new NotImplementedException();
        }
    }

    // TODO: move into TurnResultCalculator
    internal class DefaultCalculator
    {
        private Board _board;
        public readonly List<Shape> Shapes;

        // TODO: deep copy
        public DefaultCalculator(Board board)
        {
            _board = board;
            Shapes = new List<Shape>();
        }

        public void Calclulate()
        {
            //RegenerateShapes(_board);
        }

        // TODO: pass turn for suicide-attack possibility
        //private void RegenerateShapes(Board board)
        //{
        //    Shapes.Clear();
        //    for (var i = 0; i < board.Size; i++)
        //    {
        //        for (var j = 0; j < board.Size; j++)
        //        {
        //            var cellValue = board.Cells[i, j];
        //            if (cellValue == CellType.Empty || Shapes.Any(s => s.Contains(j, i)))
        //                continue;
                    
        //            var currentCell = new CellStruct() { x = j, y = i };
        //            var connectedShapeIdx = Shapes.FindIndex(s => s.GetConnectionCells(board.Size, board.Size).Contains(currentCell) && s.CellTypeValue == cellValue);
        //            if (connectedShapeIdx >= 0)
        //            {
        //                Shapes[connectedShapeIdx].Add(currentCell.x, currentCell.y);
        //            }
        //            else
        //            {
        //                var newShape = new Shape(cellValue);
        //                newShape.Add(currentCell.x, currentCell.y);
        //                Shapes.Add(newShape);
        //            }
        //        }
        //    }
        //}
        
    }

    //public class Shape
    //{
    //    public CellType CellTypeValue { get; private set; }
    //    private List<CellStruct> Cells;

    //    public Shape(CellType cellTypeValue)
    //    {
    //        CellTypeValue = cellTypeValue;
    //        Cells = new List<CellStruct>();
    //    }

    //    public bool Contains(int x, int y)
    //    {
    //        return Cells.Any(cell => cell.x == x && cell.y == y);
    //    }

    //    public void Add(int x, int y)
    //    {
    //        if (Contains(x, y))
    //            return;
    //        var newCell = new CellStruct() { x = x, y = y };
    //        Cells.Add(newCell);
    //    }

    //    //public void Remove(int x, int y, CellType cellType)
    //    //{
    //    //    if (!Contains(x, y, cellType))
    //    //        return;
    //    //    var cell = new CellStruct() { x = x, y = y, Value = cellType };
    //    //    Cells.Remove(cell);
    //    //}

    //    public List<CellStruct> GetConnectionCells(int maxX, int maxY)
    //    {
    //        var connections = new List<CellStruct>();
    //        foreach (var cellStruct in Cells)
    //        {

    //            if (cellStruct.x - 1 > 0 && cellStruct.x - 1 <= maxX)
    //            {
    //                var x = cellStruct.x - 1;
    //                var y = cellStruct.y;
    //                if (!Contains(x, y))
    //                    connections.Add(new CellStruct() { x = x, y = y });
    //            }

    //            if (cellStruct.x + 1 > 0 && cellStruct.x + 1 <= maxX)
    //            {
    //                var x = cellStruct.x + 1;
    //                var y = cellStruct.y;
    //                if (!Contains(x, y))
    //                    connections.Add(new CellStruct() { x = x, y = y });
    //            }

    //            if (cellStruct.y - 1 > 0 && cellStruct.y - 1 <= maxY)
    //            {
    //                var x = cellStruct.x;
    //                var y = cellStruct.y - 1;
    //                if (!Contains(x, y))
    //                    connections.Add(new CellStruct() { x = x, y = y });
    //            }

    //            if (cellStruct.y + 1 > 0 && cellStruct.y + 1 <= maxY)
    //            {
    //                var x = cellStruct.x;
    //                var y = cellStruct.y + 1;
    //                if (!Contains(x, y))
    //                    connections.Add(new CellStruct() { x = x, y = y });
    //            }
    //        }
    //        return connections;
    //    }
    //}

    //public struct CellStruct
    //{
    //    public int x;
    //    public int y;
    //    //TODO: useless - duplicated data variable
    //    //TODO: leave it only here ? cut from game - works only with shapes.
    //    //public CellType Value;
    //}
}
