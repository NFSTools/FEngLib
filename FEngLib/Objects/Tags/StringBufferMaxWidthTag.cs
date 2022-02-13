using System.IO;

namespace FEngLib.Objects.Tags;

public class StringBufferMaxWidthTag : ObjectTag
{
    public StringBufferMaxWidthTag(IObject<ObjectData> frontendObject) : base(frontendObject)
    {
    }

    public uint MaxWidth { get; set; }

    public override void Read(BinaryReader br,
        ushort id,
        ushort length)
    {
        MaxWidth = br.ReadUInt32();
    }
}