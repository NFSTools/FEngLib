using System.IO;
using FEngLib.Object;
using FEngLib.Objects;
using FEngLib.Packages;
using FEngLib.Tags;

namespace FEngLib.Scripts.Tags
{
    public class ResponseIdTag : Tag
    {
        public ResponseIdTag(IObject<ObjectData> frontendObject) : base(frontendObject)
        {
        }

        public uint Id { get; set; }

        public override void Read(BinaryReader br, FrontendChunkBlock chunkBlock, Package package, ushort id,
            ushort length)
        {
            Id = br.ReadUInt32();
        }
    }
}