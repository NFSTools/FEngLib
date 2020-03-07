using System.IO;

namespace FEngLib
{
    public abstract class FrontendChunk
    {
        public abstract void Read(FrontendPackage package, FrontendChunkBlock chunkBlock,
            FrontendChunkReader chunkReader, BinaryReader reader);

        public abstract FrontendChunkType GetChunkType();
    }
}