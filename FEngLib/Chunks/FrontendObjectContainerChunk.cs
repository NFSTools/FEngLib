using System.IO;

namespace FEngLib.Chunks
{
    public class FrontendObjectContainerChunk : FrontendObjectChunk
    {
        public override FrontendObject Read(FrontendPackage package, ObjectReaderState readerState, BinaryReader reader)
        {
            return readerState.ChunkReader.ReadFrontendObjectChunks(FrontendObject, readerState.CurrentChunkBlock.Size);
        }

        public override FrontendChunkType GetChunkType()
        {
            return FrontendChunkType.FrontendObjectContainer;
        }

        public FrontendObjectContainerChunk(FrontendObject frontendObject) : base(frontendObject)
        {
        }
    }
}