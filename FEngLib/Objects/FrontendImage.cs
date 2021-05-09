using System.Numerics;
using FEngLib.Structures;

namespace FEngLib.Objects
{
    public class FrontendImage : FrontendObject
    {
        public uint ImageFlags { get; set; }

        public Vector2 UpperLeft { get; set; }
        public Vector2 LowerRight { get; set; }

        public FrontendImage()
        {
        }

        public FrontendImage(FrontendObject original) : base(original)
        {
        }
        public FrontendImage(FrontendImage original) : this(original as FrontendObject)
        {
            ImageFlags = original.ImageFlags;
            UpperLeft = original.UpperLeft;
            LowerRight = original.LowerRight;
        }
    }
}