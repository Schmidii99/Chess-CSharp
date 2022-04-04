using System.Numerics;

namespace ChessV3.Figures
{
    public class King : Figure
    {
        public override bool IsWhite { get; set; }

        public King(bool isWhite = true)
        {
            this.IsWhite = isWhite;
        }

        public override void Show(Vector2 pos)
        {
            if (IsWhite)
                Config.grObj.DrawImage(Properties.Resources.king_white, pos.X, pos.Y, Config.Image_Width, Config.Image_Height);
            else
                Config.grObj.DrawImage(Properties.Resources.king_white, pos.X, pos.Y, Config.Image_Width, Config.Image_Height);
        }
    }
}
