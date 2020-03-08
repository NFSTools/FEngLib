using System.IO;

namespace FEngLib
{
    public abstract class FrontendTag
    {
        public FrontendObject FrontendObject { get; }

        protected FrontendTag(FrontendObject frontendObject)
        {
            FrontendObject = frontendObject;
        }

        public abstract void Read(BinaryReader br, FrontendChunkBlock chunkBlock, FrontendPackage package,
            ushort id,
            ushort length);
    }
}