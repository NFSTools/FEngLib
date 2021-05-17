using System.IO;
using FEngLib.Objects;
using FEngLib.Packages;
using FEngLib.Tags;
using FEngLib.Utils;

namespace FEngLib.Objects.Tags
{
    public class ObjectTypeTag : Tag
    {
        public ObjectTypeTag(IObject<ObjectData> frontendObject) : base(frontendObject)
        {
        }

        public ObjectType Type { get; set; }

        public override void Read(BinaryReader br, FrontendChunkBlock chunkBlock, Package package,
            ushort id,
            ushort length)
        {
            Type = br.ReadEnum<ObjectType>();
        }
    }
}