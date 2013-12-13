using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using SimpleEngine;
using SimpleEngine.Interfaces;

namespace SimpleEngine.Classes
{
    internal class TurnValidator : ITurnValidator
    {
        /// <exception cref="TurnOutOfRangeException">Trying to turn out of board.</exception>
        /// <exception cref="TurnToBusyCellException">Trying to turn on not-empty cell.</exception>
        /// <exception cref="RepeatBoardStateException">Player is trying to turn that brings board to same state as at previous turn.</exception>
        public void Validate(int rowIndex, int columnIndex, CellType newCellValue, Board board, String previousBoardStateHash)
        {
            if (rowIndex >= board.Size || rowIndex < 0 || columnIndex >= board.Size || columnIndex < 0)
            {
                throw new TurnOutOfRangeException(rowIndex, columnIndex, newCellValue);
            }
            //TODO: col and row places!!
            if (board.Cells[rowIndex, columnIndex] != CellType.Empty)
            {
                throw new TurnToBusyCellException(rowIndex, columnIndex, newCellValue);
            }

            if (previousBoardStateHash == board.GetCustomHash())
            {
                throw new RepeatBoardStateException(rowIndex, columnIndex, newCellValue, previousBoardStateHash);
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
}
