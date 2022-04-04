using System.Numerics;

namespace ChessV3.Figures
{
    /// <summary>
    /// Figur Bauer
    /// </summary>
    public class Pawn : Figure
    {
        public override bool IsWhite { get; set; }

        public Pawn(bool isWhite = true)
        {
            this.IsWhite = isWhite;
        }

        public override void Show(Vector2 pos)
        {
            if (IsWhite)
                Config.grObj.DrawImage(Properties.Resources.pawn_white, pos.X, pos.Y, Config.Image_Width, Config.Image_Height);
            else
                Config.grObj.DrawImage(Properties.Resources.pawn_black, pos.X, pos.Y, Config.Image_Width, Config.Image_Height);
        }
    }
}
