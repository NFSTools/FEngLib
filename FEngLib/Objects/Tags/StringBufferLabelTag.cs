using System.IO;
using FEngLib.Packages;
using FEngLib.Tags;

namespace FEngLib.Objects.Tags;

public class StringBufferLabelTag : Tag
{
    public StringBufferLabelTag(IObject<ObjectData> frontendObject) : base(frontendObject)
    {
    }

    public string Label { get; set; }

    public override void Read(BinaryReader br, FrontendChunkBlock chunkBlock, Package package,
        ushort id,
        ushort length)
    {
        Label = new string(br.ReadChars(length)).Trim('\x00');
    }
}