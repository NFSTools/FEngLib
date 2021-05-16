using System.IO;
using FEngLib.Object;
using FEngLib.Objects;
using FEngLib.Packages;
using FEngLib.Tags;

namespace FEngLib.Scripts.Tags
{
    public class MessageResponseInfoTag : Tag
    {
        public uint Hash { get; set; }

        public MessageResponseInfoTag(IObject<ObjectData> frontendObject) : base(frontendObject)
        {
        }

        public override void Read(BinaryReader br, FrontendChunkBlock chunkBlock, Package package,
            ushort id,
            ushort length)
        {
            Hash = br.ReadUInt32();
        }
    }
}