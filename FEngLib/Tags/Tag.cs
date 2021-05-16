using System.IO;
using FEngLib.Object;
using FEngLib.Objects;
using FEngLib.Packages;

namespace FEngLib.Tags
{
    public abstract class Tag
    {
        protected IObject<ObjectData> FrontendObject { get; }

        protected Tag(IObject<ObjectData> frontendObject)
        {
            FrontendObject = frontendObject;
        }

        public abstract void Read(BinaryReader br, FrontendChunkBlock chunkBlock, Package package,
            ushort id,
            ushort length);
    }
}