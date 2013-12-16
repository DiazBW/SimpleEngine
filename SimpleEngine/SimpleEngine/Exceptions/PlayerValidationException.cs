using System;
using SimpleEngine.Classes.Game;

namespace SimpleEngine.Exceptions
{
    public abstract class PlayerValidationException : Exception
    {
        public Int32 PlayerId { get; private set; }
        
        protected PlayerValidationException(Int32 playerId)
        {
            PlayerId = playerId;
        }
    }

    public class PlayerNotInGameException : PlayerValidationException
    {
        public PlayerNotInGameException(Int32 playerId)
            : base(playerId)
        {
        }
    }

    public class PlayerNotActiveException : PlayerValidationException
    {
        public PlayerNotActiveException(Int32 playerId)
            : base(playerId)
        {
        }
    }

    public class PlayerInvalidTurnValueException : PlayerValidationException
    {
        public GameTurnStruct Turn { get; private set; }

        public PlayerInvalidTurnValueException(Int32 playerId, GameTurnStruct turn)
            : base(playerId)
        {
            Turn = turn;
        }
    }
}
