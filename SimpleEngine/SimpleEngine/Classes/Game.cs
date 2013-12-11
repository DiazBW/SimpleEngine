using System;
using System.Collections.Generic;
using System.Linq;
using SimpleEngine.Interfaces;

namespace SimpleEngine.Classes
{
    public class Game : IGame
    {
        private readonly List<Turn> _history;
        private const Int32 BOARD_SIZE = 10;

        public int ActivePlayerId { get; private set; }
        public CellType ActiveCellType
        {
            get { return ActivePlayerId == PlayerOneId ? CellType.Black : CellType.White; }
        }

        private readonly ITurnResultCalculator _turnResultCalculator;
        private readonly ITurnValidator _turnValidator;

        public readonly Int32 PlayerOneId;
        public readonly Int32 PlayerTwoId;
        public Board Board;

        public Game(Int32 playerOneId, Int32 playerTwoId)
        {
            PlayerOneId = playerOneId;
            PlayerTwoId = playerTwoId;
            ActivePlayerId = PlayerOneId;
            Board = new Board(BOARD_SIZE);

            //TODO: inject via autofac
            _turnValidator = new DefaultTurnValidator();
            _turnResultCalculator = new TurnResultCalculator();
            _history = new List<Turn>();
            
        }

        public Int32 GetWinPlayerId()
        {
            throw new NotImplementedException();
        }

        public bool IsGameOver()
        {
            throw new NotImplementedException();
        }

        public void Turn(Int32 rowIndex, Int32 columnIndex, Int32 playerId)
        {
            ValidateTurn(rowIndex, columnIndex, playerId);

            TurnProceed(rowIndex, columnIndex);
            AddHistoryItem(rowIndex, columnIndex);
            ChangeActivePlayer();

            RecalculateBoardState();
        }

        public void DevTurn(Int32 rowIndex, Int32 columnIndex)
        {
            //ValidateTurn(rowIndex, columnIndex, playerId);

            TurnProceed(rowIndex, columnIndex);
            AddHistoryItem(rowIndex, columnIndex);
            ChangeActivePlayer();

            RecalculateBoardState();
        }

        private void TurnProceed(Int32 rowIndex, Int32 columnIndex)
        {
            Board.Cells[columnIndex, rowIndex] = ActiveCellType;
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
            _turnResultCalculator.CalculateNewBoardState(ref Board);
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
