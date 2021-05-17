using System.IO;
using FEngLib.Objects;
using FEngLib.Packages;

namespace FEngLib.Chunks
{
    public class FrontendObjectContainerChunk : FrontendObjectChunk
    {
        public override IObject<ObjectData> Read(Package package, ObjectReaderState readerState, BinaryReader reader)
        {
            return readerState.ChunkReader.ReadFrontendObjectChunks(FrontendObject, readerState.CurrentChunkBlock.Size);
        }

        public override FrontendChunkType GetChunkType()
        {
            return FrontendChunkType.FrontendObjectContainer;
        }

        public FrontendObjectContainerChunk(IObject<ObjectData> frontendObject) : base(frontendObject)
        {
        }
    }
}