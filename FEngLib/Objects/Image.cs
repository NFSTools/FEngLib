using System.IO;
using System.Numerics;
using FEngLib.Utils;

namespace FEngLib.Objects;

public class ImageData : ObjectData
{
    public Vector2 UpperLeft { get; set; }
    public Vector2 LowerRight { get; set; }

    public override void Read(BinaryReader br)
    {
        base.Read(br);

        UpperLeft = br.ReadVector2();
        LowerRight = br.ReadVector2();
    }

    public override void Write(BinaryWriter bw)
    {
        base.Write(bw);

        bw.Write(UpperLeft);
        bw.Write(LowerRight);
    }
}

public class Image : Image<ImageData>
{
    public Image(ImageData data) : base(data)
    {
    }
}

public interface IImage<out TData> : IObject<TData> where TData : ImageData
{
    uint ImageFlags { get; set; }
}

public class Image<TData> : BaseObject<TData>, IImage<TData> where TData : ImageData, new()
{
    protected Image(TData data) : base(data)
    {
        Data = data;
    }

    public override void InitializeData()
    {
        Data = new TData();
    }

    public uint ImageFlags { get; set; }
}