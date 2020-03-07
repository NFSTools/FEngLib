using System.IO;

namespace FEngLib
{
    public abstract class FrontendChunk
    {
        public abstract void Read(FrontendPackage package, BinaryReader reader);

        public abstract FrontendChunkType GetChunkType();
    }
}