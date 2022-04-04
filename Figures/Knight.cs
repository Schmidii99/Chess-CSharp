using System.Numerics;

namespace ChessV3.Figures
{
    public class Knight : Figure
    {
        public override bool IsWhite { get; set; } = true;

        public override void Show(Vector2 pos)
        {
            if (IsWhite)
                Config.grObj.DrawImage(Properties.Resources.knight_white, pos.X, pos.Y, Config.Image_Width, Config.Image_Height);
            else
                Config.grObj.DrawImage(Properties.Resources.knight_black, pos.X, pos.Y, Config.Image_Width, Config.Image_Height);
        }
    }
}
