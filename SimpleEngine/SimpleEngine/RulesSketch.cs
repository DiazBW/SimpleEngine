using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleEngine
{
    class RulesSketch
    {
        //using System;
        //using System.Collections.Generic;
        //using System.Linq;
        //using System.Runtime.CompilerServices;
        //using System.Text;

        //namespace BoardTest
        //{
        //    internal class Program
        //    {
        //        private static void Main(string[] args)
        //        {
        //            var defBoard = new Board();

        //            Process(defBoard);
        //        }

        //        private static void Process(Board board)
        //        {
        //            var input = String.Empty;
        //            while (input != "exit")
        //            {
        //                ShowBoardWithShapes(board);
        //                input = Console.ReadLine();
        //                var command = input.Split(' ')[0];
        //                switch (command)
        //                {
        //                    case "set":
        //                        var x = Int32.Parse(input.Split(' ')[1]);
        //                        var y = Int32.Parse(input.Split(' ')[2]);
        //                        CellValueType newValueType;
        //                        Enum.TryParse(input.Split(' ')[3], true, out newValueType);
        //                        board.SetCell(x, y, newValueType);
        //                        break;
        //                }
        //            }
        //        }

        //        private static void ShowBoard(Board board)
        //        {
        //            Console.Clear();
        //            string separator = "-------------------------------------------------------------------------------";
        //            Console.WriteLine(separator);
        //            for (var i = 0; i < Board.BOARD_SIZE; i++)
        //            {
        //                for (var j = 0; j < Board.BOARD_SIZE; j++)
        //                {
        //                    Console.Write("  ");
        //                    switch (board.Cells[i, j])
        //                    {
        //                        case CellValueType.Empty:
        //                            Console.Write('.');
        //                            break;
        //                        case CellValueType.Black:
        //                            Console.Write('X');
        //                            break;
        //                        case CellValueType.White:
        //                            Console.Write('O');
        //                            break;
        //                    }
        //                    Console.Write("  ");
        //                }
        //                Console.WriteLine();
        //            }
        //            Console.WriteLine(separator);
        //        }

        //        private static void ShowBoardWithShapes(Board board)
        //        {
        //            Console.Clear();
        //            string separator = "-------------------------------------------------------------------------------";
        //            Console.WriteLine(separator);
        //            for (var i = 0; i < Board.BOARD_SIZE; i++)
        //            {
        //                for (var j = 0; j < Board.BOARD_SIZE; j++)
        //                {
        //                    var shapeId = board.Shapes.FindIndex(s => s.Contains(j, i, board.Cells[i, j]));
        //                    Console.Write(" ");
        //                    switch (board.Cells[i, j])
        //                    {
        //                        case CellValueType.Empty:
        //                            Console.Write(".(" + shapeId + ")");
        //                            break;
        //                        case CellValueType.Black:
        //                            Console.Write("X(" + shapeId + ")");
        //                            break;
        //                        case CellValueType.White:
        //                            Console.Write("O(" + shapeId + ")");
        //                            break;
        //                    }
        //                    Console.Write(" ");
        //                }
        //                Console.WriteLine();
        //            }
        //            Console.WriteLine(separator);
        //        }
        //    }

        //    public class Board
        //    {
        //        public const Int32 BOARD_SIZE = 10;
        //        public CellValueType[,] Cells;
        //        public List<Shape> Shapes;

        //        public Board()
        //        {
        //            Cells = new CellValueType[BOARD_SIZE, BOARD_SIZE];
        //            Shapes = new List<Shape>();
        //        }

        //        public void SetCell(Int32 x, Int32 y, CellValueType newValueType)
        //        {
        //            Cells[y, x] = newValueType;
        //            RegenerateShapes();
        //            Step();
        //        }


        //        // TODO: нахер выкиунть это ссаное говно после того как фейс заработает и написать нормальный алгоритм.
        //        // ибо котелок сегодня совершенно чугунный =\
        //        public void Step()
        //        {
        //            var shape = Shapes.Find(sh => !HaveShapeBreath(sh));
        //            if (shape != null)
        //            {
        //                for (var i = 0; i < Board.BOARD_SIZE; i++)
        //                {
        //                    for (var j = 0; j < Board.BOARD_SIZE; j++)
        //                    {
        //                        if (shape.Contains(j, i, Cells[i, j]))
        //                        {
        //                            Cells[i, j] = CellValueType.Empty;
        //                        }
        //                    }
        //                }

        //                Shapes.Remove(shape);
        //            }
        //        }

        //        private void RegenerateShapes()
        //        {
        //            Shapes.Clear();
        //            for (var i = 0; i < Board.BOARD_SIZE; i++)
        //            {
        //                for (var j = 0; j < Board.BOARD_SIZE; j++)
        //                {
        //                    if(Cells[i,j] == CellValueType.Empty || Shapes.Any(s => s.Contains(j, i, Cells[i, j])))
        //                        continue;
        //                    var currentCell = new CellStruct() { x = j, y = i, Value = Cells[i, j] };
        //                    var connectedShapeIdx = Shapes.FindIndex(s => s.GetConnectionCells(BOARD_SIZE, BOARD_SIZE).Contains(currentCell));
        //                    if (connectedShapeIdx >= 0)
        //                    {
        //                        Shapes[connectedShapeIdx].Add(currentCell.x, currentCell.y, currentCell.Value);
        //                    }
        //                    else
        //                    {
        //                        var newShape = new Shape();
        //                        newShape.Add(currentCell.x, currentCell.y, currentCell.Value);
        //                        Shapes.Add(newShape);
        //                    }
        //                }
        //            }
        //        }

        //        private bool HaveShapeBreath(Shape shape)
        //        {
        //            //return shape.GetConnectionCells(BOARD_SIZE, BOARD_SIZE).Any(c => c.Value == CellValueType.Empty);
        //            var connections = shape.GetConnectionCells(BOARD_SIZE, BOARD_SIZE);
        //            foreach (var connection in connections)
        //            {
        //               if (Cells[connection.y, connection.x] == CellValueType.Empty)
        //                    return true;
        //            }
        //            return false;
        //        }
        //    }

        //    public class Shape
        //    {
        //        private List<CellStruct> Cells;

        //        public Shape()
        //        {
        //            Cells = new List<CellStruct>();
        //        }

        //        public bool Contains(int x, int y, CellValueType cellValue)
        //        {
        //            return Cells.Any(cell => cell.x == x && cell.y == y && cell.Value == cellValue);
        //        }

        //        public bool Contains(CellStruct cell)
        //        {
        //            return Contains(cell.x, cell.y, cell.Value);
        //        }

        //        public void Add(int x, int y, CellValueType cellType)
        //        {
        //            if (Contains(x, y, cellType)) 
        //                return;
        //            var newCell = new CellStruct() { x = x, y = y, Value = cellType};
        //            Cells.Add(newCell);
        //        }

        //        public void Remove(int x, int y, CellValueType cellType)
        //        {
        //            if (!Contains(x, y, cellType))
        //                return;
        //            var cell = new CellStruct() { x = x, y = y, Value = cellType };
        //            Cells.Remove(cell);
        //        }

        //        public List<CellStruct> GetConnectionCells(int maxX, int maxY)
        //        {
        //            var connections = new List<CellStruct>();
        //            foreach (var cellStruct in Cells)
        //            {
        //                if ( cellStruct.x - 1 > 0 && cellStruct.x - 1 <= maxX )
        //                {
        //                    var leftCell = new CellStruct() {x = cellStruct.x - 1, y = cellStruct.y, Value = cellStruct.Value};
        //                    if (!Contains(leftCell))
        //                        connections.Add(leftCell);
        //                }

        //                if (cellStruct.x + 1 > 0 && cellStruct.x + 1 <= maxX)
        //                {
        //                    var rightCell = new CellStruct() {x = cellStruct.x + 1, y = cellStruct.y, Value = cellStruct.Value};
        //                    if (!Contains(rightCell))
        //                        connections.Add(rightCell);
        //                }

        //                if (cellStruct.y - 1 > 0 && cellStruct.y - 1 <= maxY)
        //                {
        //                    var topCell = new CellStruct() {x = cellStruct.x, y = cellStruct.y - 1, Value = cellStruct.Value};
        //                    if (!Contains(topCell))
        //                        connections.Add(topCell);
        //                }

        //                if (cellStruct.y + 1 > 0 && cellStruct.y + 1 <= maxY)
        //                {
        //                    var bottomCell = new CellStruct() {x = cellStruct.x, y = cellStruct.y + 1, Value = cellStruct.Value};
        //                    if (!Contains(bottomCell))
        //                        connections.Add(bottomCell);
        //                }
        //            }
        //            return connections;
        //        }
        //    }

        //    public struct CellStruct
        //    {
        //        public int x;
        //        public int y;
        //        //TODO: useless - duplicated data variable
        //        public CellValueType Value;
        //    }

        //    public enum CellValueType
        //    {
        //        Empty,
        //        Black,
        //        White
        //    }
        //}

    }
}
