using System.IO;

namespace FEngLib.Objects.Tags;

public class StringBufferLengthTag : ObjectTag
{
    public StringBufferLengthTag(IObject<ObjectData> frontendObject) : base(frontendObject)
    {
    }

    public uint BufferLength { get; set; }

    public override void Read(BinaryReader br,
        ushort id,
        ushort length)
    {
        BufferLength = br.ReadUInt32();
    }
}