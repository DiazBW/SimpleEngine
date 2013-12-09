// 2h

using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleEngine
{
    public class Core
    {
    }

    public class Game
    {
        private const Int32 BOARD_SIZE = 10;

        public readonly Int32 PlayerOneId;
        public readonly Int32 PlayerTwoId;
        public Board Board;

        public Game(Int32 playerOneId, Int32 playerTwoId)
        {
            PlayerOneId = playerOneId;
            PlayerTwoId = playerTwoId;
            Board = new Board(BOARD_SIZE);
        }

        public List<String> GetBoardTextRepresentation()
        {
            var res = new List<String>();
            for (var i = 0; i < BOARD_SIZE; i++)
            {
                var str = String.Empty;
                for (var j = 0; j < BOARD_SIZE; j++)
                {
                    var ch = "";
                    var cellValue = Board.Cells[i, j];
                    switch (cellValue)
                    {
                        case Cell.Empty:
                            ch = ".";
                            break;
                        case Cell.Black:
                            ch = "o";
                            break;
                        case Cell.White:
                            ch = "x";
                            break;
                    }
                    str += ch + "  ";
                }
                res.Add(str);
            }
            return res;
        }

        public void SetCellStatus(Int32 x, Int32 y, Cell newStatus, Int32 playerId)
        {
            if (!IsCoordinatePairInRange(x, y))
            {
                var msg = String.Format("Coordinates {0} {1} are invalid!", x, y);
                throw new ArgumentException(msg);
            }

            if (!Enum.IsDefined(typeof (Cell), newStatus))
            {
                var msg = String.Format("Cell status '{0}' is unavailable!", newStatus);
                throw new ArgumentException(msg);
            }

            Board.Cells[x, y] = newStatus;
        }

        public void ClearBoard()
        {
            for (var i = 0; i < BOARD_SIZE; i++)
            {
                for (var j = 0; j < BOARD_SIZE; j++)
                {
                    Board.Cells[i, j] = Cell.Empty;
                }
            }
        }

        private static bool IsCoordinatePairInRange(Int32 x, Int32 y)
        {
            return (x >= 0 && x < BOARD_SIZE)
                   && (y >= 0 && y < BOARD_SIZE);
        }
    }

    // impement Cell as main property for access by index like "Board[i,j]"
    public class Board
    {
        public readonly int Size;
        public Cell[,] Cells;

        public Board(int size)
        {
            Size = size;
            Cells = new Cell [Size, Size];
        }
    }

    //public class Player
    //{
    //    public string Name { get; set; }

    //    public Player(string name)
    //    {
    //        Name = name;
    //    }
    //}

    public enum Cell
    {
        Empty,
        Black,
        White
    }
}
