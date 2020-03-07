using System.Diagnostics;
using System.IO;

namespace FEngLib.Chunks
{
    public class FrontendObjectContainerChunk : FrontendChunk
    {
        public override void Read(FrontendPackage package, FrontendChunkBlock chunkBlock, FrontendChunkReader chunkReader, BinaryReader reader)
        {
            foreach (var chunk in chunkReader.ReadChunks(chunkBlock.Size))
            {
                Debugger.Break();
            }
        }

        public override FrontendChunkType GetChunkType()
        {
            return FrontendChunkType.FrontendObjectContainer;
        }
    }
}