using System.IO;
using FEngLib.Data;
using FEngLib.Object;

namespace FEngLib.Tags
{
    public class ObjectTypeTag : FrontendTag
    {
        public ObjectTypeTag(IObject<ObjectData> frontendObject) : base(frontendObject)
        {
        }

        public ObjectType Type { get; set; }

        public override void Read(BinaryReader br, FrontendChunkBlock chunkBlock, FrontendPackage package,
            ushort id,
            ushort length)
        {
            Type = br.ReadEnum<ObjectType>();
        }
    }
}