using System.IO;
using FEngLib.Scripts;
using FEngLib.Structures;
using FEngLib.Utils;

namespace FEngLib.Objects;

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

public class ColoredImageScriptTracks : ImageScriptTracks
{
    public ColorTrack TopLeft { get; set; }
    public ColorTrack TopRight { get; set; }
    public ColorTrack BottomRight { get; set; }
    public ColorTrack BottomLeft { get; set; }
}

public class ColoredImage : Image<ColoredImageData, ImageScript<ColoredImageScriptTracks>>
{
    public ColoredImage(ColoredImageData data) : base(data)
    {
    }

    public override ObjectType GetObjectType()
    {
        return ObjectType.ColoredImage;
    }

    public override void InitializeData()
    {
        Data = new ColoredImageData();
    }
}