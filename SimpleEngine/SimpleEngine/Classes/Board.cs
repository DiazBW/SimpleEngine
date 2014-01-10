using System;

namespace SimpleEngine.Classes
{
    // TODO: add indecsator
    public class Board
    {
        private readonly CellType DefaultCellType = CellType.Empty;
        public readonly int Size;
        public CellType[,] Cells;

        //TODO: oprimize!
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

        //TODO: TESTS! and other equals methods!
        public String GetCustomHash()
        {
            var hash = String.Empty;

            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    hash += ((int)Cells[i, j]).ToString();
                }
            }

            return hash;
        }

        //TODO: rewitre to linq.Any ?
        public bool HasEmptyCell()
        {
            for (var i = 0; i < Size; i++)
            {
                for (var j = 0; j < Size; j++)
                {
                    if (Cells[i, j] == CellType.Empty)
                        return true;
                }
            }
            return false;
        }

        public static Board GetDeepCopy(Board source)
        {
            var destination = new Board(source.Size);
            for (var i = 0; i < source.Size; i++)
            {
                for (var j = 0; j < source.Size; j++)
                {
                    destination.Cells[i, j] = source.Cells[i, j];
                }
            }
            return destination;
        }
    }

    public enum CellType
    {
        Empty,
        Black,
        White
    }
}