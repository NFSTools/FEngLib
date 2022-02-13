using System.IO;

namespace FEngLib.Objects.Tags;

public class ImageInfoTag : ObjectTag
{
    public ImageInfoTag(IObject<ObjectData> frontendObject) : base(frontendObject)
    {
    }

    public uint ImageFlags { get; set; }

    public override void Read(BinaryReader br,
        ushort id,
        ushort length)
    {
        ImageFlags = br.ReadUInt32();
    }
}