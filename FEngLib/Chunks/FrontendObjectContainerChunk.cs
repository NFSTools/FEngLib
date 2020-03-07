using System.Diagnostics;
using System.IO;

namespace FEngLib.Chunks
{
    public class FrontendObjectContainerChunk : FrontendObjectChunk
    {
        public override void Read(FrontendPackage package, FrontendChunkBlock chunkBlock, FrontendChunkReader chunkReader, BinaryReader reader)
        {
            foreach (var chunk in chunkReader.ReadObjectChunks(FrontendObject, chunkBlock.Size))
            {
                Debugger.Break();
            }
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