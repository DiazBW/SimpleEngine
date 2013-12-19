using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using SimpleEngine.Classes.Game;

namespace MvcApp.Models
{
    public class BoardModel
    {
        public Int32[,] Board { get; set; }
        public Int32 Size { get; set; }

        public BoardModel(Game theGame)
        {
            Size = theGame.Board.Size;
            
            Board = new Int32[Size, Size];
            for (var i = 0; i < Size; i++)
            {
                for (var j = 0; j < Size; j++)
                {
                    Board[i, j] = (Int32)theGame.Board.Cells[i, j];
                }
            }
        }

        public String GetHtml()
        {
            var sb = new StringBuilder();
            for (var i = 0; i < Size; i++)
            {
                for (var j = 0; j < Size; j++)
                {
                    sb.Append(Board[i, j].ToString() + "   ");
                }
                sb.Append(@"<br\>");
            }
            return sb.ToString();
        }
    }
}