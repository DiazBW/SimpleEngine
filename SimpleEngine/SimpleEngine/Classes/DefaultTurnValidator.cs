using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using SimpleEngine;
using SimpleEngine.Interfaces;

namespace SimpleEngine.Classes
{
    internal class DefaultTurnValidator : ITurnValidator
    {
        /// <exception cref="TurnOutOfRangeException">Trying to turn out of board.</exception>
        /// <exception cref="TurnToBusyCellException">Trying to turn on not-empty cell.</exception>
        public void Validate(int rowIndex, int columnIndex, CellType newCellValue, Board board)
        {
            if (rowIndex >= board.Size || rowIndex < 0 || columnIndex >= board.Size || columnIndex < 0)
            {
                throw new TurnOutOfRangeException(rowIndex, columnIndex, newCellValue);
            }

            if (board.Cells[columnIndex, rowIndex] != CellType.Empty)
            {
                throw new TurnToBusyCellException(rowIndex, columnIndex, newCellValue);
            }

            //TODO: add check for suicide for areas
        }

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
    }
}
