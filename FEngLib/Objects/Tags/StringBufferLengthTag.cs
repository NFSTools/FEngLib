using System.IO;
using FEngLib.Packages;
using FEngLib.Tags;

namespace FEngLib.Objects.Tags;

public class StringBufferLengthTag : Tag
{
    public StringBufferLengthTag(IObject<ObjectData> frontendObject) : base(frontendObject)
    {
    }

    public uint BufferLength { get; set; }
    
    public override void Read(BinaryReader br, FrontendChunkBlock chunkBlock, Package package,
        ushort id,
        ushort length)
    {
        BufferLength = br.ReadUInt32();
    }
}