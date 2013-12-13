using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SimpleEngine.Classes;
using SimpleEngine.Classes.Game;
using SimpleEngine.Interfaces;

namespace EngineTestApp
{
    public partial class MainForm : Form
    {
        public static Image BlackCell { get; private set; }
        public static Image WhiteCell { get; private set; }

        public static Image EmptyMiddleCell { get; private set; }
        public static Image EmptyMiddlePointCell { get; private set; }

        public static Image EmptyTopLeftCell { get; private set; }
        public static Image EmptyTopRightCell { get; private set; }
        public static Image EmptyBottomLeftCell { get; private set; }
        public static Image EmptyBottomRightCell { get; private set; }

        public static Image EmptyTopCell { get; private set; }
        public static Image EmptyRightCell { get; private set; }
        public static Image EmptyLeftCell { get; private set; }
        public static Image EmptyBottomCell { get; private set; }

        public Game TheGame;

        public const Int32 PLAYER_ONE_ID = 0;
        public const Int32 PLAYER_TWO_ID = 1;
        public Int32 ActivePlayerId = PLAYER_ONE_ID;
        
        public MainForm()
        {
            InitializeComponent();
            //var dir = Path.GetDirectoryName(Application.ExecutablePath);
            //var filename = Path.Combine(dir, @"MyImage.jpg");

            BlackCell = Image.FromFile(@"Content\Images\WhiteCell.png");
            WhiteCell = Image.FromFile(@"Content\Images\WhiteCell.png");

            EmptyMiddleCell = Image.FromFile(@"Content\Images\MiddleCell.png");
            EmptyMiddlePointCell = Image.FromFile(@"Content\Images\MiddlePointCell.png");

            EmptyTopLeftCell = Image.FromFile(@"Content\Images\TopLeftCell.png");
            EmptyTopRightCell = Image.FromFile(@"Content\Images\TopRightCell.png");
            EmptyBottomLeftCell = Image.FromFile(@"Content\Images\BottomLeftCell.png");
            EmptyBottomRightCell = Image.FromFile(@"Content\Images\BottomRightCell.png");

            EmptyTopCell = Image.FromFile(@"Content\Images\TopCell.png");
            EmptyRightCell = Image.FromFile(@"Content\Images\RightCell.png");
            EmptyLeftCell = Image.FromFile(@"Content\Images\LeftCell.png");
            EmptyBottomCell = Image.FromFile(@"Content\Images\BottomCell.png");
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            CreateImages();

            TheGame = new Game(PLAYER_ONE_ID, PLAYER_TWO_ID);
        }

        private void CreateImages(int gameSize = 19)
        {
            var cellSize = 39;

            for (var i = 0; i < gameSize; i++)
            {
                for (var j = 0; j < gameSize; j++)
                {
                    var newPicBox = new CellPictureBox(rowIndex: i, columnIndex: j, gameSize: gameSize);

                    newPicBox.Size = new Size(cellSize, cellSize);
                    newPicBox.SizeMode = PictureBoxSizeMode.StretchImage;
                    newPicBox.Location = new Point(j * cellSize, i * cellSize);

                    //newPicBox.Image = GetImageForCell(i, j, gameSize);
                    newPicBox.Click += Cell_Click;

                    boardPanel.Controls.Add(newPicBox);
                }
            }
        }

        void Cell_Click(object sender, EventArgs e)
        {
            var cell = sender as CellPictureBox;
            //var msg = String.Format("{0} : {1}     {2}", cell.RowIndex, cell.ColumnIndex, cell.CellValue);
            //MessageBox.Show(msg);

            try
            {
                TheGame.Turn(cell.RowIndex, cell.ColumnIndex, ActivePlayerId);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            ActivePlayerId = ActivePlayerId == PLAYER_ONE_ID ? PLAYER_TWO_ID : PLAYER_ONE_ID;

            //CellType newValue = TheGame.GetCellValue(cell.RowIndex, cell.ColumnIndex);
            //cell.SetNewValue(newValue);

            Board board = TheGame.GetBoard();

            var gameSize = 19;
            for (var i = 0; i < gameSize; i++)
            {
                for (var j = 0; j < gameSize; j++)
                {
                    CellType newValue = board.Cells[i, j];
                    foreach (var control in this.boardPanel.Controls)
                    {
                        var cellPicBox = control as CellPictureBox;
                        if (cellPicBox.RowIndex == i && cellPicBox.ColumnIndex == j)
                        {
                            cellPicBox.SetNewValue(newValue);
                        }
                    }
                    //cell.SetNewValue(newValue);
                }
            }
            //throw new NotImplementedException();
        }

        //private static Image GetImageForCell(int i, int j, int gameSize)
        //{
        //    // middle
        //    var resImage = MainForm.EmptyMiddleCell;

        //    // top - left
        //    if (i == 0 && j == 0)
        //    {
        //        resImage = MainForm.EmptyTopLeftCell;
        //    }
        //    // top - right
        //    else if (i == 0 && j == gameSize - 1)
        //    {
        //        resImage = MainForm.EmptyTopRightCell;
        //    }
        //    // bottom - right
        //    else if (i == gameSize - 1 && j == gameSize - 1)
        //    {
        //        resImage = MainForm.EmptyBottomRightCell;
        //    }
        //    // bottom - left
        //    else if (i == gameSize - 1 && j == 0)
        //    {
        //        resImage = MainForm.EmptyBottomLeftCell;
        //    }

        //    // left border
        //    else if (i != 0 && i != gameSize - 1 && j == 0)
        //    {
        //        resImage = MainForm.EmptyLeftCell;
        //    }
        //    // right border
        //    else if (i != 0 && i != gameSize - 1 && j == gameSize - 1)
        //    {
        //        resImage = MainForm.EmptyRightCell;
        //    }
        //    // top border
        //    else if (i == 0 && j != 0 && j != gameSize - 1)
        //    {
        //        resImage = MainForm.EmptyTopCell;
        //    }
        //    // bottom border
        //    else if (i == gameSize - 1 && j != 0 && j != gameSize - 1)
        //    {
        //        resImage = MainForm.EmptyBottomCell;
        //    }

        //    return resImage;
        //}
    }

    public class CellPictureBox : PictureBox
    {
        public static Image BlackCell = Image.FromFile(@"Content\Images\BlackCell.png");
        public static Image WhiteCell = Image.FromFile(@"Content\Images\WhiteCell.png");
        //public static Image EmptyMiddleCell = Image.FromFile(@"Content\Images\MiddleCell.png");

        public Int32 RowIndex { get; private set; }
        public Int32 ColumnIndex { get; private set; }
        public Int32 GameSize { get; private set; }

        public CellType CellValue { get; private set; }

        public CellPictureBox(Int32 rowIndex, Int32 columnIndex, Int32 gameSize)
        {
            RowIndex = rowIndex;
            ColumnIndex = columnIndex;
            GameSize = gameSize;

            SetImage();
        }

        public void SetNewValue(CellType newValue)
        {
            CellValue = newValue;
            SetImage();
        }

        private void SetImage()
        {
            switch (CellValue)
            {
                case CellType.Black:
                    Image = BlackCell;
                    break;
                case CellType.White:
                    Image = WhiteCell;
                    break;
                case CellType.Empty:
                    SetEmptyImage();
                    break;
            }
        }

        private void SetEmptyImage()
        {
            var i = RowIndex;
            var j = ColumnIndex;
            var gameSize = GameSize;

            // middle
            var resImage = MainForm.EmptyMiddleCell;

            // top - left
            if (i == 0 && j == 0)
            {
                resImage = MainForm.EmptyTopLeftCell;
            }
            // top - right
            else if (i == 0 && j == gameSize - 1)
            {
                resImage = MainForm.EmptyTopRightCell;
            }
            // bottom - right
            else if (i == gameSize - 1 && j == gameSize - 1)
            {
                resImage = MainForm.EmptyBottomRightCell;
            }
            // bottom - left
            else if (i == gameSize - 1 && j == 0)
            {
                resImage = MainForm.EmptyBottomLeftCell;
            }

            // left border
            else if (i != 0 && i != gameSize - 1 && j == 0)
            {
                resImage = MainForm.EmptyLeftCell;
            }
            // right border
            else if (i != 0 && i != gameSize - 1 && j == gameSize - 1)
            {
                resImage = MainForm.EmptyRightCell;
            }
            // top border
            else if (i == 0 && j != 0 && j != gameSize - 1)
            {
                resImage = MainForm.EmptyTopCell;
            }
            // bottom border
            else if (i == gameSize - 1 && j != 0 && j != gameSize - 1)
            {
                resImage = MainForm.EmptyBottomCell;
            }

            Image = resImage;
        }
    }
}