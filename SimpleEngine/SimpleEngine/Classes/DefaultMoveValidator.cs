using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using SimpleEngine;
using SimpleEngine.Interfaces;

namespace SimpleEngine.Classes
{
    class DefaultMoveValidator : ITurnValidator
    {
        //public bool Validate(Turn turn, Board board)
        public bool Validate(int rowIndex, int columnIndex, CellType newCellValue, Board board)
        {
            throw new NotImplementedException();
        }
    }
}
