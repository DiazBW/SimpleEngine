using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SimpleEngine.Classes
{
    public class GameScore
    {
        //TODO: change to CLR type
        public Single BlackScore { get; private set; }
        public Single WhiteScore { get; private set; }
        private const Single KOMI = 5.5F;

        public GameScore()
        {
            WhiteScore += KOMI;
        }

        public void RocksCaptured(Int32 count, CellType capturedRocksType)
        {
            if (capturedRocksType == CellType.Empty)
            {
                throw new ArgumentException("Empty cell can not be captured!");
            }
            if (count < 0)
            {
                throw new ArgumentException("Can not be captures less than zero rocks!");
            }

            if (capturedRocksType == CellType.Black)
            {
                WhiteScore += count;
            }
            else
            {
                BlackScore += count;
            }
        }

        //TODO: use generics ?
        public void GameFinished(Board resultBoard)
        {
            BlackScore += GetAreaScore(resultBoard, CellType.Black);
            WhiteScore += GetAreaScore(resultBoard, CellType.White);
        }

        private UInt32 GetAreaScore(Board board, CellType rocksForCountingType)
        {
            UInt32 res = 0;
            for (var i = 0; i < board.Size; i++)
            {
                for (var j = 0; j < board.Size; j++)
                {
                    if (board.Cells[i, j] == rocksForCountingType)
                        res++;
                }
            }
            return res;
        }
    }
}
