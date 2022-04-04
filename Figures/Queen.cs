using System.Numerics;

namespace ChessV3.Figures
{
    public class Queen : Figure
    {
        public override bool IsWhite { get; set; } = true;

        public override void Show(Vector2 pos)
        {
            if (IsWhite)
                Config.grObj.DrawImage(Properties.Resources.queen_white, pos.X, pos.Y, Config.Image_Width, Config.Image_Height);
            else
                Config.grObj.DrawImage(Properties.Resources.queen_black, pos.X, pos.Y, Config.Image_Width, Config.Image_Height);
        }
    }
}
