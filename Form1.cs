using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using System.Windows.Forms;

namespace ChessV3
{
    public partial class Form1 : Form
    {
        public static List<List<Cell>> gameBoard = new List<List<Cell>>();

        public static bool WhiteOnBottom = true;
        
        public static Cell Selected_Cell { get; set; }

        public Form1()
        {
            InitializeComponent();

            // FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            Config.grObj = this.CreateGraphics();
        }

        private void btn_start_Click(object sender, EventArgs e)
        {
            btn_start.Visible = false;

            for (int y = 0; y < Config.Row_count; y++)
            {
                gameBoard.Add(new List<Cell>());
                for (int x = 0; x < Config.Colum_count; x++)
                {
                    gameBoard[y].Add(new Cell(new Vector2(x * Config.Cell_Width + Config.X_offset, y * Config.Cell_Height + Config.Y_offset), new Vector2(Config.Cell_Width, Config.Cell_Height)));
                }
            }

            add_Default_Figures();


            Show_GameBoard();
        }

        private static void add_Default_Figures()
        {
            bool isWhite = false;
            for (int i = 0; i < Config.Colum_count; i++)
            {
                gameBoard[1][i].figure = new Figures.Pawn(isWhite: isWhite);
                isWhite = !isWhite;
                gameBoard[6][i].figure = new Figures.Pawn(isWhite: isWhite);
                isWhite = !isWhite;
            }

            gameBoard[0][0].figure = new Figures.Rook(isWhite: false);
            gameBoard[0][1].figure = new Figures.Knight(isWhite: false);
            gameBoard[0][2].figure = new Figures.Bishop(isWhite: false);
            gameBoard[0][3].figure = new Figures.Queen(isWhite: false);
            gameBoard[0][4].figure = new Figures.King(isWhite: false);
            gameBoard[0][5].figure = new Figures.Bishop(isWhite: false);
            gameBoard[0][6].figure = new Figures.Knight(isWhite: false);
            gameBoard[0][7].figure = new Figures.Rook(isWhite: false);

            gameBoard[7][0].figure = new Figures.Rook(isWhite: true);
            gameBoard[7][1].figure = new Figures.Knight(isWhite: true);
            gameBoard[7][2].figure = new Figures.Bishop(isWhite: true);
            gameBoard[7][3].figure = new Figures.Queen(isWhite: true);
            gameBoard[7][4].figure = new Figures.King(isWhite: true);
            gameBoard[7][5].figure = new Figures.Bishop(isWhite: true);
            gameBoard[7][6].figure = new Figures.Knight(isWhite: true);
            gameBoard[7][7].figure = new Figures.Rook(isWhite: true);

        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            Point index = Calculate_Cell(e.Location);
            if (index.X < 0 || index.Y < 0)
                return;

            if (gameBoard[index.X][index.Y].IsValidMove)
            {
                // Security statement if Selected Cell is null
                if (Selected_Cell == null || Selected_Cell.figure == null)
                {
                    Show_Valid_Moves(reset: true);
                    return;
                }

                // remove figure if it is from different team
                if (gameBoard[index.X][index.Y].figure != null && gameBoard[index.X][index.Y].figure.IsWhite != Selected_Cell.figure.IsWhite)
                    gameBoard[index.X][index.Y].figure = null;


                Figure temp = null;
                temp = gameBoard[index.X][index.Y].figure;
                gameBoard[index.X][index.Y].figure = Selected_Cell.figure;
                Selected_Cell.figure = temp;

                Show_Valid_Moves(reset: true);
                Selected_Cell.Show();
                
                Selected_Cell = null;
                return;
            }

            
            Selected_Cell = gameBoard[index.X][index.Y];

            Calculate_Valid_Points(PointToVector2(index));
        }
        
        public static Point Calculate_Cell(Point MousePos)
        {
            // check if mouseclick is NOT in grid
            
            if (!(MousePos.X > Config.X_offset && MousePos.X < Config.Colum_count * Config.Cell_Width + Config.X_offset && MousePos.Y > Config.Y_offset && MousePos.Y < Config.Row_count * Config.Cell_Height + Config.Y_offset))
            {
                return new Point(-1);
            }
            Point GridPos = new Point(MousePos.X - Config.X_offset, MousePos.Y - Config.Y_offset);

            // prevent specific error that accures when the 800. Pixel is clicked e.g:
            // 800 / 100 -> 8 index 8 is not existing so substract one
            if (GridPos.X > 1)
                GridPos.X--;
            if (GridPos.Y > 1)
                GridPos.Y--;

            return new Point((int)(GridPos.Y / Config.Cell_Height), (int)(GridPos.X / Config.Cell_Width));
        }

        public static Vector2 PointToVector2(Point point)
            => new Vector2(point.X, point.Y);

        public static void Show_GameBoard() // IMPORTATNT - OVER DRAWING!!!!
        {
            foreach (List<Cell> item in gameBoard)
            {
                foreach (var item2 in item)
                {
                    item2.Show();
                }
            }
        }

        public static void Show_Valid_Moves(bool reset = false)
        {
            foreach (List<Cell> item in gameBoard)
            {
                foreach (var item2 in item)
                {
                    if (item2.IsValidMove == true)
                    {
                        if (reset)
                        {
                            item2.IsValidMove = false;
                        }
                        item2.Show();
                    }
                }
            }
        }

        public static void Calculate_Valid_Points(Vector2 current_index)
        {
            Show_Valid_Moves(reset: true);

            Cell current_cell = gameBoard[(int)current_index.X][(int)current_index.Y];
            Figure figure = current_cell.figure;
            
            int curr_x = (int)current_index.X;
            int curr_y = (int)current_index.Y;

            #region Figure Movement Calculations
            if (figure is Figures.Pawn)
            {
                if (figure.IsWhite == WhiteOnBottom) // obove
                {
                    #region obove
                    if (current_index.X == 0f) // check if on last Line
                    {
                        MessageBox.Show("No Valid Move");
                        return;
                    }
                    if (gameBoard[curr_x - 1][curr_y].figure == null)
                    {
                        gameBoard[curr_x - 1][curr_y].IsValidMove = true;
                    }


                    if (current_index.Y > 0)
                    {
                        if (gameBoard[curr_x - 1][curr_y - 1].figure != null && gameBoard[curr_x - 1][curr_y - 1].figure.IsWhite != figure.IsWhite) // right in Front
                            gameBoard[curr_x - 1][curr_y - 1].IsValidMove = true;
                    }
                    if (Config.Colum_count - 1 > current_index.Y)
                    {
                        if (gameBoard[curr_x - 1][curr_y + 1].figure != null && gameBoard[curr_x - 1][curr_y + 1].figure.IsWhite != figure.IsWhite) // left in Front
                            gameBoard[curr_x - 1][curr_y + 1].IsValidMove = true;
                    }


                    // special if pawn is on second lane from bottom
                    if (curr_x == Config.Row_count - 2 && gameBoard[curr_x - 1][curr_y].figure == null && gameBoard[curr_x - 2][curr_y].figure == null)
                        gameBoard[curr_x - 2][curr_y].IsValidMove = true;
                    #endregion
                }
                else // under
                {
                    #region under
                    if (current_index.X == Config.Row_count - 1) // check if on last Line
                    {
                        MessageBox.Show("No Valid Move");
                        return;
                    }
                    if (gameBoard[curr_x + 1][curr_y].figure == null)
                    {
                        gameBoard[curr_x + 1][curr_y].IsValidMove = true;
                    }


                    if (current_index.Y > 0)
                    {
                        if (gameBoard[curr_x + 1][curr_y - 1].figure != null && gameBoard[curr_x + 1][curr_y - 1].figure.IsWhite != figure.IsWhite) // right in Front
                            gameBoard[curr_x + 1][curr_y - 1].IsValidMove = true;
                    }
                    if (Config.Colum_count - 1 > current_index.Y)
                    {
                        if (gameBoard[curr_x + 1][curr_y + 1].figure != null && gameBoard[curr_x + 1][curr_y + 1].figure.IsWhite != figure.IsWhite) // left in Front 
                            gameBoard[curr_x + 1][curr_y + 1].IsValidMove = true;
                    }


                    // special if pawn is on second lane from bottom
                    if (curr_x == 1 && gameBoard[curr_x + 1][curr_y].figure == null && gameBoard[curr_x + 2][curr_y].figure == null)
                        gameBoard[curr_x + 2][curr_y].IsValidMove = true;
                    #endregion
                }
            }
            else if (figure is Figures.Rook)
            {
                // under
                for (int i = curr_x + 1; i < Config.Row_count; i++)
                {
                    if (gameBoard[i][curr_y].figure != null)
                    {
                        if (gameBoard[i][curr_y].figure.IsWhite == figure.IsWhite)
                        {
                            break;
                        }
                        else
                        {
                            gameBoard[i][curr_y].IsValidMove = true;
                            break;
                        }
                    }
                    gameBoard[i][curr_y].IsValidMove = true;
                }


                // above
                for (int i = curr_x - 1; i >= 0; i--)
                {
                    if (gameBoard[i][curr_y].figure != null)
                    {
                        if (gameBoard[i][curr_y].figure.IsWhite == figure.IsWhite)
                        {
                            break;
                        }
                        else
                        {
                            gameBoard[i][curr_y].IsValidMove = true;
                            break;
                        }
                    }
                    gameBoard[i][curr_y].IsValidMove = true;
                }
                // right
                for (int i = curr_y + 1; i < Config.Colum_count; i++)
                {
                    if (gameBoard[curr_x][i].figure != null)
                    {
                        if (gameBoard[curr_x][i].figure.IsWhite == figure.IsWhite)
                        {
                            break;
                        }
                        else
                        {
                            gameBoard[i][curr_y].IsValidMove = true;
                            break;
                        }
                    }
                    gameBoard[curr_x][i].IsValidMove = true;
                }


                // left
                for (int i = curr_y - 1; i >= 0; i--)
                {
                    if (gameBoard[curr_x][i].figure != null)
                    {
                        if (gameBoard[curr_x][i].figure.IsWhite == figure.IsWhite)
                        {
                            break;
                        }
                        else
                        {
                            gameBoard[i][curr_y].IsValidMove = true;
                            break;
                        }
                    }
                    gameBoard[curr_x][i].IsValidMove = true;
                }
            }
            else if (figure is Figures.Knight)
            {
                // up left
                if (curr_x > 1 && curr_y > 0)
                {
                    if (gameBoard[curr_x - 2][curr_y - 1].figure == null)
                        gameBoard[curr_x - 2][curr_y - 1].IsValidMove = true;
                    else if (gameBoard[curr_x - 2][curr_y - 1].figure.IsWhite != figure.IsWhite)
                        gameBoard[curr_x - 2][curr_y - 1].IsValidMove = true;
                }

                // up right
                if (curr_x > 1 && curr_y < Config.Colum_count - 1)
                {
                    if (gameBoard[curr_x - 2][curr_y + 1].figure == null)
                        gameBoard[curr_x - 2][curr_y + 1].IsValidMove = true;
                    else if (gameBoard[curr_x - 2][curr_y + 1].figure.IsWhite != figure.IsWhite)
                        gameBoard[curr_x - 2][curr_y + 1].IsValidMove = true;
                }

                // right up
                if (curr_x > 0 && curr_y < Config.Colum_count - 2)
                {
                    if (gameBoard[curr_x - 1][curr_y + 2].figure == null)
                        gameBoard[curr_x - 1][curr_y + 2].IsValidMove = true;
                    else if (gameBoard[curr_x - 1][curr_y + 2].figure.IsWhite != figure.IsWhite)
                        gameBoard[curr_x - 1][curr_y + 2].IsValidMove = true;
                }

                // right down
                if (curr_x < Config.Row_count - 1 && curr_y > 1)
                {
                    if (gameBoard[curr_x + 1][curr_y + 2].figure == null)
                        gameBoard[curr_x + 1][curr_y + 2].IsValidMove = true;
                    else if (gameBoard[curr_x + 1][curr_y + 2].figure.IsWhite != figure.IsWhite)
                        gameBoard[curr_x + 1][curr_y + 2].IsValidMove = true;
                }

                // down right
                if (curr_x < Config.Row_count - 2 && curr_y < Config.Colum_count - 1)
                {
                    if (gameBoard[curr_x + 2][curr_y + 1].figure == null)
                        gameBoard[curr_x + 2][curr_y + 1].IsValidMove = true;
                    else if (gameBoard[curr_x + 2][curr_y + 1].figure.IsWhite != figure.IsWhite)
                        gameBoard[curr_x + 2][curr_y + 1].IsValidMove = true;
                }

                // down left
                if (curr_x < Config.Row_count - 2 && curr_y > 0)
                {
                    if (gameBoard[curr_x + 2][curr_y - 1].figure == null)
                        gameBoard[curr_x + 2][curr_y - 1].IsValidMove = true;
                    else if (gameBoard[curr_x + 2][curr_y - 1].figure.IsWhite != figure.IsWhite)
                        gameBoard[curr_x + 2][curr_y - 1].IsValidMove = true;
                }

                // left down
                if (curr_x < Config.Row_count - 1 && curr_y > 1)
                {
                    if (gameBoard[curr_x + 1][curr_y - 2].figure == null)
                        gameBoard[curr_x + 1][curr_y - 2].IsValidMove = true;
                    else if (gameBoard[curr_x + 1][curr_y - 2].figure.IsWhite != figure.IsWhite)
                        gameBoard[curr_x + 1][curr_y - 2].IsValidMove = true;
                }

                //left up
                if (curr_x > 0 && curr_y > 1)
                {
                    if (gameBoard[curr_x - 1][curr_y - 2].figure == null)
                        gameBoard[curr_x - 1][curr_y - 2].IsValidMove = true;
                    else if (gameBoard[curr_x - 1][curr_y - 2].figure.IsWhite != figure.IsWhite)
                        gameBoard[curr_x - 1][curr_y - 2].IsValidMove = true;
                }
            }
            else if (figure is Figures.Bishop)
            {
                // up left
                for (int i = curr_x - 1, j = curr_y - 1; i >= 0 && j >= 0; i--, j--)
                {
                    if (gameBoard[i][j].figure != null)
                    {
                        if (gameBoard[i][j].figure.IsWhite == figure.IsWhite)
                        {
                            break;
                        }
                        else
                        {
                            gameBoard[i][j].IsValidMove = true;
                            break;
                        }
                    }
                    gameBoard[i][j].IsValidMove = true;
                }

                // up right
                for (int i = curr_x - 1, j = curr_y + 1; i >= 0 && j < Config.Colum_count; i--, j++)
                {
                    if (gameBoard[i][j].figure != null)
                    {
                        if (gameBoard[i][j].figure.IsWhite == figure.IsWhite)
                        {
                            break;
                        }
                        else
                        {
                            gameBoard[i][j].IsValidMove = true;
                            break;
                        }
                    }
                    gameBoard[i][j].IsValidMove = true;
                }

                // down right
                for (int i = curr_x + 1, j = curr_y + 1; i < Config.Row_count && j < Config.Colum_count; i++, j++)
                {
                    if (gameBoard[i][j].figure != null)
                    {
                        if (gameBoard[i][j].figure.IsWhite == figure.IsWhite)
                        {
                            break;
                        }
                        else
                        {
                            gameBoard[i][j].IsValidMove = true;
                            break;
                        }
                    }
                    gameBoard[i][j].IsValidMove = true;
                }

                //down left
                for (int i = curr_x + 1, j = curr_y - 1; i < Config.Row_count && j >= 0; i++, j--)
                {
                    if (gameBoard[i][j].figure != null)
                    {
                        if (gameBoard[i][j].figure.IsWhite == figure.IsWhite)
                        {
                            break;
                        }
                        else
                        {
                            gameBoard[i][j].IsValidMove = true;
                            break;
                        }
                    }
                    gameBoard[i][j].IsValidMove = true;
                }
            }
            else if (figure is Figures.King)
            {
                // up
                if (curr_x > 0)
                {
                    if (gameBoard[curr_x - 1][curr_y].figure == null)
                        gameBoard[curr_x - 1][curr_y].IsValidMove = true;
                    else if (gameBoard[curr_x - 1][curr_y].figure.IsWhite != figure.IsWhite)
                        gameBoard[curr_x - 1][curr_y].IsValidMove = true;
                }

                // down
                if (curr_x < Config.Row_count - 1)
                {
                    if (gameBoard[curr_x + 1][curr_y].figure == null)
                        gameBoard[curr_x + 1][curr_y].IsValidMove = true;
                    else if (gameBoard[curr_x + 1][curr_y].figure.IsWhite != figure.IsWhite)
                        gameBoard[curr_x + 1][curr_y].IsValidMove = true;
                }

                // left
                if (curr_y > 0)
                {
                    if (gameBoard[curr_x][curr_y - 1].figure == null)
                        gameBoard[curr_x][curr_y - 1].IsValidMove = true;
                    else if (gameBoard[curr_x][curr_y - 1].figure.IsWhite != figure.IsWhite)
                        gameBoard[curr_x][curr_y - 1].IsValidMove = true;
                }

                // right
                if (curr_y < Config.Colum_count - 1)
                {
                    if (gameBoard[curr_x][curr_y + 1].figure == null)
                        gameBoard[curr_x][curr_y + 1].IsValidMove = true;
                    else if (gameBoard[curr_x][curr_y + 1].figure.IsWhite != figure.IsWhite)
                        gameBoard[curr_x][curr_y + 1].IsValidMove = true;
                }

                // up left
                if (curr_x > 0 && curr_y > 0)
                {
                    if (gameBoard[curr_x - 1][curr_y - 1].figure == null)
                        gameBoard[curr_x - 1][curr_y - 1].IsValidMove = true;
                    else if (gameBoard[curr_x - 1][curr_y - 1].figure.IsWhite != figure.IsWhite)
                        gameBoard[curr_x - 1][curr_y - 1].IsValidMove = true;
                }

                // up right
                if (curr_x > 0 && curr_y < Config.Colum_count - 1)
                {
                    if (gameBoard[curr_x - 1][curr_y + 1].figure == null)
                        gameBoard[curr_x - 1][curr_y + 1].IsValidMove = true;
                    else if (gameBoard[curr_x - 1][curr_y + 1].figure.IsWhite != figure.IsWhite)
                        gameBoard[curr_x - 1][curr_y + 1].IsValidMove = true;
                }

                // down left
                if (curr_x < Config.Row_count - 1 && curr_y > 0)
                {
                    if (gameBoard[curr_x + 1][curr_y - 1].figure == null)
                        gameBoard[curr_x + 1][curr_y - 1].IsValidMove = true;
                    else if (gameBoard[curr_x + 1][curr_y - 1].figure.IsWhite != figure.IsWhite)
                        gameBoard[curr_x + 1][curr_y - 1].IsValidMove = true;
                }

                //down right
                if (curr_x < Config.Row_count - 1 && curr_y < Config.Colum_count - 1)
                {
                    if (gameBoard[curr_x + 1][curr_y + 1].figure == null)
                        gameBoard[curr_x + 1][curr_y + 1].IsValidMove = true;
                    else if (gameBoard[curr_x + 1][curr_y + 1].figure.IsWhite != figure.IsWhite)
                        gameBoard[curr_x + 1][curr_y + 1].IsValidMove = true;
                }
            }
            else if (figure is Figures.Queen)
            {
                // From Rook -------------------------------------
                #region From Rook
                // under
                for (int i = curr_x + 1; i < Config.Row_count; i++)
                {
                    if (gameBoard[i][curr_y].figure != null)
                    {
                        if (gameBoard[i][curr_y].figure.IsWhite == figure.IsWhite)
                        {
                            break;
                        }
                        else
                        {
                            gameBoard[i][curr_y].IsValidMove = true;
                            break;
                        }
                    }
                    gameBoard[i][curr_y].IsValidMove = true;
                }


                // above
                for (int i = curr_x - 1; i >= 0; i--)
                {
                    if (gameBoard[i][curr_y].figure != null)
                    {
                        if (gameBoard[i][curr_y].figure.IsWhite == figure.IsWhite)
                        {
                            break;
                        }
                        else
                        {
                            gameBoard[i][curr_y].IsValidMove = true;
                            break;
                        }
                    }
                    gameBoard[i][curr_y].IsValidMove = true;
                }
                // right
                for (int i = curr_y + 1; i < Config.Colum_count; i++)
                {
                    if (gameBoard[curr_x][i].figure != null)
                    {
                        if (gameBoard[curr_x][i].figure.IsWhite == figure.IsWhite)
                        {
                            break;
                        }
                        else
                        {
                            gameBoard[i][curr_y].IsValidMove = true;
                            break;
                        }
                    }
                    gameBoard[curr_x][i].IsValidMove = true;
                }


                // left
                for (int i = curr_y - 1; i >= 0; i--)
                {
                    if (gameBoard[curr_x][i].figure != null)
                    {
                        if (gameBoard[curr_x][i].figure.IsWhite == figure.IsWhite)
                        {
                            break;
                        }
                        else
                        {
                            gameBoard[i][curr_y].IsValidMove = true;
                            break;
                        }
                    }
                    gameBoard[curr_x][i].IsValidMove = true;
                }
                #endregion
                // From Bishop -----------------------------------
                #region From Bishop
                // up left
                for (int i = curr_x - 1, j = curr_y - 1; i >= 0 && j >= 0; i--, j--)
                {
                    if (gameBoard[i][j].figure != null)
                    {
                        if (gameBoard[i][j].figure.IsWhite == figure.IsWhite)
                        {
                            break;
                        }
                        else
                        {
                            gameBoard[i][j].IsValidMove = true;
                            break;
                        }
                    }
                    gameBoard[i][j].IsValidMove = true;
                }

                // up right
                for (int i = curr_x - 1, j = curr_y + 1; i >= 0 && j < Config.Colum_count; i--, j++)
                {
                    if (gameBoard[i][j].figure != null)
                    {
                        if (gameBoard[i][j].figure.IsWhite == figure.IsWhite)
                        {
                            break;
                        }
                        else
                        {
                            gameBoard[i][j].IsValidMove = true;
                            break;
                        }
                    }
                    gameBoard[i][j].IsValidMove = true;
                }

                // down right
                for (int i = curr_x + 1, j = curr_y + 1; i < Config.Row_count && j < Config.Colum_count; i++, j++)
                {
                    if (gameBoard[i][j].figure != null)
                    {
                        if (gameBoard[i][j].figure.IsWhite == figure.IsWhite)
                        {
                            break;
                        }
                        else
                        {
                            gameBoard[i][j].IsValidMove = true;
                            break;
                        }
                    }
                    gameBoard[i][j].IsValidMove = true;
                }

                //down left
                for (int i = curr_x + 1, j = curr_y - 1; i < Config.Row_count && j >= 0; i++, j--)
                {
                    if (gameBoard[i][j].figure != null)
                    {
                        if (gameBoard[i][j].figure.IsWhite == figure.IsWhite)
                        {
                            break;
                        }
                        else
                        {
                            gameBoard[i][j].IsValidMove = true;
                            break;
                        }
                    }
                    gameBoard[i][j].IsValidMove = true;
                }
                #endregion
            }
            #endregion

            Show_Valid_Moves();
        }
    }
}
