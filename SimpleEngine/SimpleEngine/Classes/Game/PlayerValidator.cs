using System;
using SimpleEngine.Exceptions;

namespace SimpleEngine.Classes.Game
{
    public partial class Game
    {
        private class PlayerValidator
        {
            private readonly Game _game;

            public PlayerValidator(Game game)
            {
                _game = game;
            }

            public void Validation(GameTurnStruct turn, int playerId)
            {
                if (!IsPlayerInGame(playerId))
                {
                    throw new PlayerNotInGameException(playerId, turn);
                }

                if (!IsPlayerActive(playerId))
                {
                    throw new PlayerNotActiveException(playerId, turn);
                }

                if (!IsTurnValueCorrect(turn))
                {
                    throw new PlayerInvalidTurnValueException(playerId, turn);
                }
            }

            private bool IsPlayerInGame(Int32 playerId)
            {
                return _game.PlayerOneId == playerId || _game.PlayerTwoId == playerId;
            }

            private bool IsPlayerActive(Int32 playerId)
            {
                return _game.ActivePlayerId == playerId;
            }

            private bool IsTurnValueCorrect(GameTurnStruct turn)
            {
                return _game.ActiveCellType == turn.Value;
            }
        }
    }
}