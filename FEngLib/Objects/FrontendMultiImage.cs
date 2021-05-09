using System.Numerics;
using FEngLib.Structures;

namespace FEngLib.Objects
{
    public class FrontendMultiImage : FrontendImage
    {
        public Vector2[] TopLeftUV { get; set; }
        public Vector2[] BottomRightUV { get; set; }
        public Vector3 PivotRotation { get; set; }
        public uint[] Texture { get; set; }
        public uint[] TextureFlags { get; set; }

        public FrontendMultiImage()
        {
            Texture = new uint[3];
            TextureFlags = new uint[3];
        }

        public FrontendMultiImage(FrontendObject original) : base(original)
        {
            Texture = new uint[3];
            TextureFlags = new uint[3];
        }

        public FrontendMultiImage(FrontendMultiImage original) : this(original as FrontendObject)
        {
            TopLeftUV = original.TopLeftUV;
            BottomRightUV = original.BottomRightUV;
            PivotRotation = original.PivotRotation;
            Texture = original.Texture;
            TextureFlags = original.TextureFlags;
        }
    }
}