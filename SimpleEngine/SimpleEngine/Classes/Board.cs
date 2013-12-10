namespace SimpleEngine.Classes
{
    public class Board
    {
        public readonly int Size;
        public CellType[,] Cells;

        public Board(int size)
        {
            Size = size;
            Cells = new CellType [Size, Size];
        }
    }

    public enum CellType
    {
        Empty,
        Black,
        White
    }
}