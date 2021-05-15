using System.IO;
using FEngLib.Structures;

namespace FEngLib.Object
{
    public class ColoredImageData : ImageData
    {
        public Color4 TopLeft { get; set; }
        public Color4 TopRight { get; set; }
        public Color4 BottomRight { get; set; }
        public Color4 BottomLeft { get; set; }

        public override void Read(BinaryReader br)
        {
            base.Read(br);
            TopLeft = br.ReadColor();
            TopRight = br.ReadColor();
            BottomRight = br.ReadColor();
            BottomLeft = br.ReadColor();
        }

        public override void Write(BinaryWriter bw)
        {
            base.Write(bw);
            bw.Write(TopLeft);
            bw.Write(TopRight);
            bw.Write(BottomRight);
            bw.Write(BottomLeft);
        }
    }

    public class ColoredImage : Image<ColoredImageData>
    {
        public ColoredImage(ColoredImageData data) : base(data)
        {
            Data = data;
        }

        public override void InitializeData()
        {
            Data = new ColoredImageData();
        }
    }
}