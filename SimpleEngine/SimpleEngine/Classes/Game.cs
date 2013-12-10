using System;
using System.Collections.Generic;
using System.Linq;
using SimpleEngine.Interfaces;

namespace SimpleEngine.Classes
{
    public class Game
    {
        private const Int32 BOARD_SIZE = 10;

        private ITurnResultCalculator _turnResultCalculator;
        private ITurnValidator _turnValidator;

        public readonly Int32 PlayerOneId;
        public readonly Int32 PlayerTwoId;
        public Board Board;

        public Game(Int32 playerOneId, Int32 playerTwoId)
        {
            PlayerOneId = playerOneId;
            PlayerTwoId = playerTwoId;
            Board = new Board(BOARD_SIZE);

            //TODO: inject via autofac
            _turnValidator = new DefaultMoveValidator();
            _turnResultCalculator = new TurnResultCalculator();
        }

        //TODO: playerId or cellValue ? 
        public Int32 Turn(Int32 rowIndex, Int32 columnIndex)
        {
            //GetCellTypeForTurn
            //_turnValidator.Validate(rowIndex: y, columnIndex: x, newCellValue: newStatus, board: Board);
            //SetCellStatus
            //_turnResultCalculator.CalculateNewBoardState(ref Board);
            // GetAnotherUserId
            return 0;
        }

        public void SetCellStatus(Int32 x, Int32 y, CellType newStatus, Int32 playerId)
        {
            _turnValidator.Validate(rowIndex: y, columnIndex: x, newCellValue: newStatus, board: Board);

            //if (!IsCoordinatePairInRange(x, y))
            //{
            //    var msg = String.Format("Coordinates {0} {1} are invalid!", x, y);
            //    throw new ArgumentException(msg);
            //}

            //if (!Enum.IsDefined(typeof (CellType), newStatus))
            //{
            //    var msg = String.Format("Cell status '{0}' is unavailable!", newStatus);
            //    throw new ArgumentException(msg);
            //}

            Board.Cells[x, y] = newStatus;
        }

        public void ClearBoard()
        {
            for (var i = 0; i < BOARD_SIZE; i++)
            {
                for (var j = 0; j < BOARD_SIZE; j++)
                {
                    Board.Cells[i, j] = CellType.Empty;
                }
            }
        }

        private static bool IsCoordinatePairInRange(Int32 x, Int32 y)
        {
            return (x >= 0 && x < BOARD_SIZE)
                   && (y >= 0 && y < BOARD_SIZE);
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
                        case CellType.Empty:
                            ch = ".";
                            break;
                        case CellType.Black:
                            ch = "o";
                            break;
                        case CellType.White:
                            ch = "x";
                            break;
                    }
                    str += ch + "  ";
                }
                res.Add(str);
            }
            return res;
        }
    }   
}
