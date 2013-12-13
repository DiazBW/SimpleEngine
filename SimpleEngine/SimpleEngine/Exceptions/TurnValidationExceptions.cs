using System;
using SimpleEngine.Classes.Game;

namespace SimpleEngine.Exceptions
{
    public abstract class TurnValidationException : Exception
    {
        public GameTurnStruct Turn { get; private set; }

        protected TurnValidationException(GameTurnStruct turn)
        {
            Turn = turn;
        }
    }

    public class TurnOutOfRangeException : TurnValidationException
    {
        public TurnOutOfRangeException(GameTurnStruct turn)
            : base(turn)
        {
        }
    }

    public class TurnToBusyCellException : TurnValidationException
    {
        public TurnToBusyCellException(GameTurnStruct turn)
            : base(turn)
        {
        }
    }

    public class SuicideException : TurnValidationException
    {
        public SuicideException(GameTurnStruct turn)
            : base(turn)
        {
        }
    }

    public class RepeatBoardStateException : TurnValidationException
    {
        public String PreviousBoardStateHash { get; private set; }

        public RepeatBoardStateException(GameTurnStruct turn, String previousStateHash)
            : base(turn)
        {
            PreviousBoardStateHash = previousStateHash;
        }
    }
}
