using System;
using SimpleEngine.Classes;

namespace SimpleEngine.Exceptions
{
    public abstract class TurnValidatorException : Exception
    {
        public int RowIndex { get; private set; }
        public int ColumnIndex { get; private set; }
        public CellType CellValue { get; private set; }

        protected TurnValidatorException(Int32 rowIndex, Int32 columnIndex, CellType newCellValue)
        {
            RowIndex = rowIndex;
            ColumnIndex = columnIndex;
            CellValue = newCellValue;
        }
    }

    public class TurnOutOfRangeException : TurnValidatorException
    {
        public TurnOutOfRangeException(Int32 rowIndex, Int32 columnIndex, CellType newCellValue)
            : base(rowIndex, columnIndex, newCellValue)
        {
        }
    }

    public class TurnToBusyCellException : TurnValidatorException
    {
        public TurnToBusyCellException(Int32 rowIndex, Int32 columnIndex, CellType newCellValue)
            : base(rowIndex, columnIndex, newCellValue)
        {
        }
    }

    public class SuicideException : TurnValidatorException
    {
        public SuicideException(Int32 rowIndex, Int32 columnIndex, CellType newCellValue)
            : base(rowIndex, columnIndex, newCellValue)
        {
        }
    }

    public class RepeatBoardStateException : TurnValidatorException
    {
        public String PreviousBoardStateHash { get; private set; }

        public RepeatBoardStateException(Int32 rowIndex, Int32 columnIndex, CellType newCellValue, String previousStateHash)
            : base(rowIndex, columnIndex, newCellValue)
        {
            PreviousBoardStateHash = previousStateHash;
        }
    }
}
