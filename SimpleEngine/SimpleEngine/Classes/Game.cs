using System;
using System.Collections.Generic;
using System.Linq;
using SimpleEngine.Interfaces;

namespace SimpleEngine.Classes
{
    public class Game : IGame
    {
        private List<Turn> _history;
        private const Int32 BOARD_SIZE = 10;
        private Int32 _activePlayerId;

        private readonly ITurnResultCalculator _turnResultCalculator;
        private readonly ITurnValidator _turnValidator;

        public readonly Int32 PlayerOneId;
        public readonly Int32 PlayerTwoId;
        public Board Board;

        public Game(Int32 playerOneId, Int32 playerTwoId)
        {
            PlayerOneId = playerOneId;
            PlayerTwoId = playerTwoId;
            _activePlayerId = PlayerOneId;
            Board = new Board(BOARD_SIZE);

            //TODO: inject via autofac
            _turnValidator = new DefaultMoveValidator();
            _turnResultCalculator = new TurnResultCalculator();
            _history = new List<Turn>();
            
        }

        public bool IsPlayerInGame(Int32 playerId)
        {
            return PlayerOneId == playerId || PlayerTwoId == playerId;
        }

        public Int32 GetWinPlayerId()
        {
            throw  new NotImplementedException();
        }

        public bool IsGameOver()
        {
            throw new NotImplementedException();
        }

        public Int32 GetActivePlayerId()
        {
            throw new NotImplementedException();
        }

        public bool IsPlayerActive(Int32 playerId)
        {
            return _activePlayerId == playerId;
        }

        public CellType GetActivePlayerCellType()
        {
            throw new NotImplementedException();
        }

        public Int32 Turn(Int32 rowIndex, Int32 columnIndex, Int32 playerId)
        {
            PlayerIdValidation(playerId);

            var newCellType = GetActivePlayerCellType();
            _turnValidator.Validate(rowIndex, columnIndex, newCellType, Board);

            //SetCellStatus
            //_turnResultCalculator.CalculateNewBoardState(ref Board);
            // GetAnotherUserId
            // SaveHistory
            // var newTurn = new Turn() { RowIndex = rowIndex, ColumnIndex = columnIndex, PlayerId = playerId};
            return 0;
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

        //private static bool IsCoordinatePairInRange(Int32 x, Int32 y)
        //{
        //    return (x >= 0 && x < BOARD_SIZE)
        //           && (y >= 0 && y < BOARD_SIZE);
        //}

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

    struct Turn
    {
        public Int32 RowIndex;
        public Int32 ColumnIndex;
        public Int32 PlayerId;
    }
}
