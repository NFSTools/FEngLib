using System.IO;
using FEngLib.Objects;
using FEngLib.Tags;

namespace FEngLib.Packages.Tags;

public class MessageTargetCountTag : Tag
{
    public MessageTargetCountTag(IObject<ObjectData> frontendObject) : base(frontendObject)
    {
    }

    public override void Read(BinaryReader br, FrontendChunkBlock chunkBlock, Package package, ushort id,
        ushort length)
    {
        br.ReadUInt32();
    }
}