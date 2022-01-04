using System.IO;
using FEngLib.Objects;
using FEngLib.Packages;
using FEngLib.Tags;

namespace FEngLib.Scripts.Tags;

public class MessageResponseCountTag : Tag
{
    public MessageResponseCountTag(IObject<ObjectData> frontendObject) : base(frontendObject)
    {
    }

    public override void Read(BinaryReader br, FrontendChunkBlock chunkBlock, Package package,
        ushort id,
        ushort length)
    {
        br.ReadUInt32();
    }
}