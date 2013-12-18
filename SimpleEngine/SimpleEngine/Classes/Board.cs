﻿using System;

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
    }

    public enum CellType
    {
        Empty,
        Black,
        White
    }
}