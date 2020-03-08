using System.IO;

namespace FEngLib
{
    public abstract class FrontendObjectChunk
    {
        public FrontendObject FrontendObject { get; }

        protected FrontendObjectChunk(FrontendObject frontendObject)
        {
            FrontendObject = frontendObject;
        }

        public abstract FrontendObject Read(FrontendPackage package, ObjectReaderState readerState, BinaryReader reader);
        public abstract FrontendChunkType GetChunkType();
    }
}