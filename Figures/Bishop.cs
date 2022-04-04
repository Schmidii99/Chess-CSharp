using System.Numerics;

namespace ChessV3.Figures
{
    public class Bishop : Figure
    {
        public override bool IsWhite { get; set; }

        public Bishop(bool isWhite = true)
        {
            this.IsWhite = isWhite;
        }

        public override void Show(Vector2 pos)
        {
            if (IsWhite)
                Config.grObj.DrawImage(Properties.Resources.bishop_white, pos.X, pos.Y, Config.Image_Width, Config.Image_Height);
            else
                Config.grObj.DrawImage(Properties.Resources.bishop_black, pos.X, pos.Y, Config.Image_Width, Config.Image_Height);
        }
    }
}
