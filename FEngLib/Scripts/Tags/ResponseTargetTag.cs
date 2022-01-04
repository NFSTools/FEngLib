using System.IO;
using FEngLib.Objects;
using FEngLib.Packages;
using FEngLib.Tags;

namespace FEngLib.Scripts.Tags;

public class ResponseTargetTag : Tag
{
    public ResponseTargetTag(IObject<ObjectData> frontendObject) : base(frontendObject)
    {
    }

    public uint Target { get; set; }

    public override void Read(BinaryReader br, FrontendChunkBlock chunkBlock, Package package, ushort id,
        ushort length)
    {
        Target = br.ReadUInt32();
    }
}