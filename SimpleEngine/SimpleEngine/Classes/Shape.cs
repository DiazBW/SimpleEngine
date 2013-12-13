using System;
using System.Collections.Generic;
using System.Linq;
using SimpleEngine.Classes.Game;

namespace SimpleEngine.Classes
{
    public class Shape
    {
        public Int32 Id { get; private set; }
        public CellType CellTypeValue { get; private set; }
        public readonly List<CellStruct> Cells;

        public Shape(CellType cellTypeValue, Int32 id)
        {
            CellTypeValue = cellTypeValue;
            Id = id;
            Cells = new List<CellStruct>();
        }

        public static List<Shape> GetDeepCopy(List<Shape> shapes)
        {
            return shapes.Select(s => s.GetDeepCopy()).ToList();
        }

        public Shape GetDeepCopy()
        {
            var newShape = new Shape(CellTypeValue, Id);
            foreach (var cell in Cells)
            {
                newShape.Add(cell.RowIndex, cell.ColumnIndex);
            }
            return newShape;
        }

        public bool Contains(int rowIndex, int columnIndex)
        {
            return Cells.Any(cell => cell.RowIndex == rowIndex && cell.ColumnIndex == columnIndex);
        }

        public void Add(int rowIndex, int columnIndex)
        {
            if (Contains(rowIndex, columnIndex))
                return;
            var newCell = new CellStruct() { RowIndex = rowIndex, ColumnIndex = columnIndex };
            Cells.Add(newCell);
        }

        public bool IsConnectedWith(int rowIndex, int columnIndex, CellType cellValue, int boardSize)
        {
            if (CellTypeValue != cellValue) return false;

            var cells = GetConnectionCells(boardSize, boardSize);
            return cells.Any(c => c.RowIndex == rowIndex && c.ColumnIndex == columnIndex);
        }

        public List<CellStruct> GetConnectionCells(int maxRowValue, int maxColumnValue)
        {
            var connections = new List<CellStruct>();
            foreach (var cellStruct in Cells)
            {
                if (cellStruct.RowIndex - 1 >= 0 && cellStruct.RowIndex - 1 < maxRowValue)
                {
                    var row = cellStruct.RowIndex - 1;
                    var col = cellStruct.ColumnIndex;
                    if (!Contains(row, col))
                        connections.Add(new CellStruct() { RowIndex = row, ColumnIndex = col });
                }

                if (cellStruct.RowIndex + 1 >= 0 && cellStruct.RowIndex + 1 < maxRowValue)
                {
                    var row = cellStruct.RowIndex + 1;
                    var col = cellStruct.ColumnIndex;
                    if (!Contains(row, col))
                        connections.Add(new CellStruct() { RowIndex = row, ColumnIndex = col });
                }

                if (cellStruct.ColumnIndex - 1 >= 0 && cellStruct.ColumnIndex - 1 < maxColumnValue)
                {
                    var row = cellStruct.RowIndex;
                    var col = cellStruct.ColumnIndex - 1;
                    if (!Contains(row, col))
                        connections.Add(new CellStruct() { RowIndex = row, ColumnIndex = col });
                }

                if (cellStruct.ColumnIndex + 1 >= 0 && cellStruct.ColumnIndex + 1 < maxColumnValue)
                {
                    var row = cellStruct.RowIndex;
                    var col = cellStruct.ColumnIndex + 1;
                    if (!Contains(row, col))
                        connections.Add(new CellStruct() { RowIndex = row, ColumnIndex = col });
                }
            }
            return connections;
        }
    }
}