using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApp.Models
{
    public class TurnModel
    {
        public Int32 PlayerId { get; set; }

        public Int32 RowIndex { get; set; }
        public Int32 ColumnIndex { get; set; }
    }
}