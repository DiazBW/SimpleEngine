using System;
using SimpleEngine.Exceptions;
using SimpleEngine.Interfaces;

namespace SimpleEngine.Classes.Game
{
    public partial class Game : IGame
    {
        private class PlayerValidator
        {
            private readonly Game _game;

            public PlayerValidator(Game game)
            {
                _game = game;
            }

            public void ValidateTurn(GameTurnStruct turn, int playerId)
            {
                if (!IsPlayerInGame(playerId))
                {
                    throw new PlayerNotInGameException(playerId);
                }

                if (!IsPlayerActive(playerId))
                {
                    throw new PlayerNotActiveException(playerId);
                }

                if (!IsTurnValueCorrect(turn))
                {
                    throw new PlayerInvalidTurnValueException(playerId, turn);
                }
            }

            public void ValidateTurnSkiping(int playerId)
            {
                if (!IsPlayerInGame(playerId))
                {
                    throw new PlayerNotInGameException(playerId);
                }

                if (!IsPlayerActive(playerId))
                {
                    throw new PlayerNotActiveException(playerId);
                }
            }

            private bool IsPlayerInGame(Int32 playerId)
            {
                return _game.CurrentGameState.PlayerOneId == playerId || _game.CurrentGameState.PlayerTwoId == playerId;
            }

            private bool IsPlayerActive(Int32 playerId)
            {
                return _game.CurrentGameState.ActivePlayerId == playerId;
            }

            private bool IsTurnValueCorrect(GameTurnStruct turn)
            {
                return _game.CurrentGameState.ActiveCellType == turn.Value;
            }
        }
    }
}