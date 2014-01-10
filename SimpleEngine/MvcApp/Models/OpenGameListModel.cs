using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApp.Models
{
    public class OpenGameListModel
    {
        public List<OpenGameRequestModel> OpenGames { get; set; }
    }

    public class OpenGameRequestModel
    {
        public Int32 RequestId { get; set; }
        public Int32 PlayerId { get; set; }
    }
}