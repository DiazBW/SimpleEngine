using SimpleEngine.Classes;

namespace SimpleEngine.Interfaces
{
    internal interface ITurnValidator
    {
        //TODO: description about exceptions
        //TODO: exceptions into interface ?
        void Validate(int rowIndex, int columnIndex, CellType newCellValue, Board board);
    }
}