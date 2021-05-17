using System.IO;
using FEngLib.Packages;
using FEngLib.Tags;

namespace FEngLib.Objects.Tags
{
    public class ObjectDataTag : Tag
    {
        public override void Read(BinaryReader br, FrontendChunkBlock chunkBlock, Package package,
            ushort id,
            ushort length)
        {
            FrontendObject.InitializeData();
            FrontendObject.Data.Read(br);
        }

        public ObjectDataTag(IObject<ObjectData> frontendObject) : base(frontendObject)
        {
        }
    }
}