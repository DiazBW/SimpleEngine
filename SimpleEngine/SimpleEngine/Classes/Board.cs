namespace SimpleEngine.Classes
{
    public class Board
    {
        private readonly CellType DefaultCellType = CellType.Empty;
        public readonly int Size;
        public CellType[,] Cells;

        public Board(int size)
        {
            Size = size;
            Cells = new CellType [Size, Size];
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    Cells[i, j] = DefaultCellType;
                }
            }
        }
    }

    public enum CellType
    {
        Empty,
        Black,
        White
    }
}