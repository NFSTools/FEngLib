using System.IO;
using FEngLib.Object;

namespace FEngLib
{
    public abstract class FrontendTag
    {
        public IObject<ObjectData> FrontendObject { get; }

        protected FrontendTag(IObject<ObjectData> frontendObject)
        {
            FrontendObject = frontendObject;
        }

        public abstract void Read(BinaryReader br, FrontendChunkBlock chunkBlock, FrontendPackage package,
            ushort id,
            ushort length);
    }
}