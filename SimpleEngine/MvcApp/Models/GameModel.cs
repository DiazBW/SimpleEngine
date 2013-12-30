using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace MvcApp.Models
{
    public class GameModel
    {
        public Int32 PlayerOneId { get; set; }
        public Int32 PlayerTwoId { get; set; }
        public Int32 ActivePlayerId { get; set; }
        public Boolean IsFinished { get; set; }

        public List<CellRow> Rows { get; set; }

        public static GameModel GetFake(Int32 size)
        {
            return GetFake(size, playerOneId: 1, playerTwoId: 2);
        }

        public static GameModel GetFake(Int32 size, Int32 playerOneId, Int32 playerTwoId)
        {
            var rows = new List<CellRow>();
            for (var i = 0; i < size; i++)
            {
                var cells = new List<Cell>();
                for (var j = 0; j < size; j++)
                {
                    cells.Add(new Cell() { Value = 0, RowIndex = i, ColumnIndex = j });
                }
                rows.Add(new CellRow { Cells = cells, RowIndex = i });
            }

            return new GameModel
            {
                PlayerOneId = playerOneId,
                PlayerTwoId = playerTwoId,
                ActivePlayerId = playerOneId,
                Rows = rows
            };
        }
    }

    public class CellRow
    {
        public Int32 RowIndex { get; set; }

        public List<Cell> Cells { get; set; }
    }

    public class Cell
    {
        public Int32 RowIndex { get; set; }
        public Int32 ColumnIndex { get; set; }
        public Int32 Value { get; set; }
    }

    //var mockGameObject = new
    //        {
    //            UserOneId = "1",
    //            UserTwoId = "2",
    //            ActiveUserId = "1",
    //            Rows = [
    //                new { 
    //                    RowIndex = 0,
    //                    Cells = [
    //                        new { Value = 0, RowIndex = 0, ColumnIndex = 0 },
    //                        new { Value = 0, RowIndex = 0, ColumnIndex = 1 },
    //                        new { Value = 0, RowIndex = 0, ColumnIndex = 2 },
    //                        new { Value = 0, RowIndex = 0, ColumnIndex = 3 },
    //                        new { Value = 0, RowIndex = 0, ColumnIndex = 4 }
    //                    ]},
    //                new { 
    //                    RowIndex = 1,
    //                    Cells = [
    //                        new { Value = 0, RowIndex = 1, ColumnIndex = 0 },
    //                        new { Value = 0, RowIndex = 1, ColumnIndex = 1 },
    //                        new { Value = 0, RowIndex = 1, ColumnIndex = 2 },
    //                        new { Value = 0, RowIndex = 1, ColumnIndex = 3 },
    //                        new { Value = 0, RowIndex = 1, ColumnIndex = 4 }
    //                    ]},
    //                new { 
    //                    RowIndex = 2,
    //                    Cells = [
    //                        new { Value = 0, RowIndex = 2, ColumnIndex = 0 },
    //                        new { Value = 0, RowIndex = 2, ColumnIndex = 1 },
    //                        new { Value = 0, RowIndex = 2, ColumnIndex = 2 },
    //                        new { Value = 0, RowIndex = 2, ColumnIndex = 3 },
    //                        new { Value = 0, RowIndex = 2, ColumnIndex = 4 }
    //                    ]},
    //                new { 
    //                    RowIndex = 3,
    //                    Cells = [
    //                        new { Value = 0, RowIndex = 3, ColumnIndex = 0 },
    //                        new { Value = 0, RowIndex = 3, ColumnIndex = 1 },
    //                        new { Value = 0, RowIndex = 3, ColumnIndex = 2 },
    //                        new { Value = 0, RowIndex = 3, ColumnIndex = 3 },
    //                        new { Value = 0, RowIndex = 3, ColumnIndex = 4 }
    //                    ]},
    //                new { 
    //                    RowIndex = 4,
    //                    Cells = new {
    //                        new { Value = 0, RowIndex = 4, ColumnIndex = 0 },
    //                        new { Value = 0, RowIndex = 4, ColumnIndex = 1 },
    //                        new { Value = 0, RowIndex = 4, ColumnIndex = 2 },
    //                        new { Value = 0, RowIndex = 4, ColumnIndex = 3 },
    //                        new { Value = 0, RowIndex = 4, ColumnIndex = 4 }
    //                    }}
    //            ]
    //        };
}