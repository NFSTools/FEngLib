using System.IO;
using FEngLib.Packages;
using FEngLib.Tags;

namespace FEngLib.Objects.Tags;

public class StringBufferLabelHashTag : Tag
{
    public StringBufferLabelHashTag(IObject<ObjectData> frontendObject) : base(frontendObject)
    {
    }

    public uint Hash { get; set; }

    public override void Read(BinaryReader br, FrontendChunkBlock chunkBlock, Package package,
        ushort id,
        ushort length)
    {
        Hash = br.ReadUInt32();
    }
}