using System;
using System.Drawing;
using System.Numerics;
using System.Windows.Forms;

namespace ChessV3
{
    public class Cell
    {
        public Vector2 pos { get; set; }
        public Vector2 size { get; set; }
        public Figure figure { get; set; }

        public bool IsValidMove { get; set; } = false;

        public Vector2 index { get;}

        public Cell(Vector2 Position, Vector2 Size, Figure Figure = null)
        {
            pos = Position;
            size = Size;
            figure = Figure;
            index = Form1.PointToVector2(Form1.Calculate_Cell(new Point((int)pos.X + 1, (int)pos.Y + 1)));
        }

        public void Show()
        {
            
            // Background Color change
            if (index.X % 2 == 0 && index.Y % 2 == 0)
                Config.grObj.FillRectangle(Config.Cell_Background_Brush_Light, pos.X, pos.Y, size.X, size.Y);
            else if (index.X % 2 == 1 && index.Y % 2 == 1)
                Config.grObj.FillRectangle(Config.Cell_Background_Brush_Light, pos.X, pos.Y, size.X, size.Y);
            else
                Config.grObj.FillRectangle(Config.Cell_Background_Brush_Dark, pos.X, pos.Y, size.X, size.Y);


            // Border
            Config.grObj.DrawRectangle(Config.Cell_Border_Pen, pos.X, pos.Y, size.X, size.Y);

            // Figure
            if (figure != null)
            {
                figure.Show(pos);
            }

            // IsValidMove
            if (IsValidMove)
            {
                Config.grObj.FillEllipse(Config.Cell_IsValidMove_Brush, pos.X + size.X / 4, pos.Y + size.Y / 4, size.X / 2, size.Y / 2);
            }
        }
    }
}
