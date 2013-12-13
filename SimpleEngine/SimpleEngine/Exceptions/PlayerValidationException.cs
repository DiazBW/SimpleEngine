using System;
using SimpleEngine.Classes.Game;

namespace SimpleEngine.Exceptions
{
    public abstract class PlayerValidationException : Exception
    {
        public Int32 PlayerId { get; private set; }
        public GameTurnStruct Turn { get; private set; }

        protected PlayerValidationException(Int32 playerId, GameTurnStruct turn)
        {
            PlayerId = playerId;
            Turn = turn;
        }
    }

    public class PlayerNotInGameException : PlayerValidationException
    {
        public PlayerNotInGameException(Int32 playerId, GameTurnStruct turn)
            : base(playerId, turn)
        {
        }
    }

    public class PlayerNotActiveException : PlayerValidationException
    {
        public PlayerNotActiveException(Int32 playerId, GameTurnStruct turn)
            : base(playerId, turn)
        {
        }
    }

    public class PlayerInvalidTurnValueException : PlayerValidationException
    {
        public PlayerInvalidTurnValueException(Int32 playerId, GameTurnStruct turn)
            : base(playerId, turn)
        {
        }
    }
}
