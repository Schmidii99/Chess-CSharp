using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessV3
{
    public static class Config
    {
        // grid -------------------------
        public static int Colum_count { get; set; } = 8;
        public static int Row_count { get; set; } = 8;

        public static int X_offset { get; } = 560;
        public static int Y_offset { get; } = 50;
        // ------------------------------


        // Cell -------------------------
        public static Color Cell_Background_Color_Light { get; set; } = Color.FromArgb(255, 227, 193, 111);
        public static SolidBrush Cell_Background_Brush_Light { get; set; } = new SolidBrush(Cell_Background_Color_Light);
        public static Color Cell_Background_Color_Dark { get; set; } = Color.FromArgb(255, 184, 139, 74);
        public static SolidBrush Cell_Background_Brush_Dark { get; set; } = new SolidBrush(Cell_Background_Color_Dark);

        public static int Cell_Width { get; set; } = 100;
        public static int Cell_Height { get; set; } = 100;

        public static int Cell_Border_Width { get; set; } = 1;
        public static Color Cell_Border_Color { get; set; } = Color.Black;
        public static Pen Cell_Border_Pen { get; } = new Pen(Cell_Border_Color, Cell_Border_Width);

        public static Color Cell_IsValidMove_Color { get; set; } = Color.FromArgb(128, 235, 255, 0);
        public static SolidBrush Cell_IsValidMove_Brush { get; } = new SolidBrush(Cell_IsValidMove_Color);
        // ------------------------------


        // graphics stuff ---------------
        public static Graphics grObj { get; set; }

        public static SolidBrush BrushBlack { get; } = new SolidBrush(Color.Black);
        // ------------------------------

        
        // image stuff ------------------
        public static int Image_Width { get; } = Cell_Width;
        public static int Image_Height { get; } = Cell_Height;
        // ------------------------------
    }
}
