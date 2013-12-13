using System;

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

            public void Validation(int playerId)
            {
                if (!IsPlayerInGame(playerId))
                {
                    var msg = String.Format("The game has not player with id {0}.", playerId);
                    throw new ArgumentException(msg);
                }

                if (!IsPlayerActive(playerId))
                {
                    var msg = String.Format("Player with id {0} can not perform turn. Now is another player's turn.",
                        playerId);
                    throw new ArgumentException(msg);
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
        }
    }
}