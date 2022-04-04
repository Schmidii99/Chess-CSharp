using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ChessV3
{
    /// <summary>
    /// Base class for all chess pieces
    /// </summary>
    /// <remarks>
    /// This class is the base class for all chess pieces.  It contains the basic Variables and methods.
    /// </remarks>
    public abstract class Figure
    {
        public enum FigureType
        {
            Pawn,
            Rook,
            Knight,
            Bishop,
            Queen,
            King
        }
        public abstract bool IsWhite { get; set; }
        public abstract void Show(Vector2 pos);
    }
}
