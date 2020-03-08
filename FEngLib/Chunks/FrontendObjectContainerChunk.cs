using System.Diagnostics;
using System.IO;

namespace FEngLib.Chunks
{
    public class FrontendObjectContainerChunk : FrontendObjectChunk
    {
        public override FrontendObject Read(FrontendPackage package, FrontendChunkBlock chunkBlock, FrontendChunkReader chunkReader, BinaryReader reader)
        {
            return chunkReader.ReadFrontendObjectChunks(FrontendObject, chunkBlock.Size);
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