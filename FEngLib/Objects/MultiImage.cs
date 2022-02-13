using System.IO;
using System.Numerics;
using FEngLib.Scripts;
using FEngLib.Utils;

namespace FEngLib.Objects;

public class MultiImageData : ImageData
{
    public Vector2 TopLeft1 { get; set; }
    public Vector2 TopLeft2 { get; set; }
    public Vector2 TopLeft3 { get; set; }
    public Vector2 BottomRight1 { get; set; }
    public Vector2 BottomRight2 { get; set; }
    public Vector2 BottomRight3 { get; set; }
    public Vector3 PivotRotation { get; set; }

    public override void Read(BinaryReader br)
    {
        base.Read(br);

        TopLeft1 = br.ReadVector2();
        TopLeft2 = br.ReadVector2();
        TopLeft3 = br.ReadVector2();
        BottomRight1 = br.ReadVector2();
        BottomRight2 = br.ReadVector2();
        BottomRight3 = br.ReadVector2();
        PivotRotation = br.ReadVector3();
    }

    public override void Write(BinaryWriter bw)
    {
        base.Write(bw);

        bw.Write(TopLeft1);
        bw.Write(TopLeft2);
        bw.Write(TopLeft3);
        bw.Write(BottomRight1);
        bw.Write(BottomRight2);
        bw.Write(BottomRight3);
        bw.Write(PivotRotation);
    }
}

public class MultiImageScriptTracks : ImageScriptTracks
{
    public Vector2Track TopLeft1 { get; set; }
    public Vector2Track TopLeft2 { get; set; }
    public Vector2Track TopLeft3 { get; set; }
    public Vector2Track BottomRight1 { get; set; }
    public Vector2Track BottomRight2 { get; set; }
    public Vector2Track BottomRight3 { get; set; }
    public Vector3Track PivotRotation { get; set; }
}

public class MultiImage : Image<MultiImageData, ImageScript<MultiImageScriptTracks>>
{
    public MultiImage(MultiImageData data) : base(data)
    {
        Type = ObjectType.MultiImage;
    }

    public uint Texture1 { get; set; }
    public uint TextureFlags1 { get; set; }
    public uint Texture2 { get; set; }
    public uint TextureFlags2 { get; set; }
    public uint Texture3 { get; set; }
    public uint TextureFlags3 { get; set; }

    public override void InitializeData()
    {
        Data = new MultiImageData();
    }
}