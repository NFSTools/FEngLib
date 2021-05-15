using System.IO;
using FEngLib.Data;
using FEngLib.Object;

namespace FEngLib.Tags
{
    public class ObjectReferenceTag : FrontendTag
    {
        public ObjectReferenceTag(IObject<ObjectData> frontendObject) : base(frontendObject)
        {
        }

        public uint Guid { get; set; }
        public uint NameHash { get; set; }
        public ObjectFlags Flags { get; set; }
        public int ResourceIndex { get; set; }

        public override void Read(BinaryReader br, FrontendChunkBlock chunkBlock, FrontendPackage package,
            ushort id,
            ushort length)
        {
            Guid = br.ReadUInt32();
            NameHash = br.ReadUInt32();
            Flags = br.ReadEnum<ObjectFlags>();
            ResourceIndex = br.ReadInt32();
        }
    }
}