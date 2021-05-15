using System.IO;
using FEngLib.Object;

namespace FEngLib
{
    public abstract class FrontendObjectChunk
    {
        public IObject<ObjectData> FrontendObject { get; }

        protected FrontendObjectChunk(IObject<ObjectData> frontendObject)
        {
            FrontendObject = frontendObject;
        }

        public abstract IObject<ObjectData> Read(FrontendPackage package, ObjectReaderState readerState, BinaryReader reader);
        public abstract FrontendChunkType GetChunkType();
    }
}