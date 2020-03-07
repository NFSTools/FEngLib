using System.Diagnostics;
using System.IO;

namespace FEngLib.Chunks
{
    public class ObjectContainerChunk : FrontendChunk
    {
        public override void Read(FrontendPackage package, FrontendChunkBlock chunkBlock, FrontendChunkReader chunkReader, BinaryReader reader)
        {
            FrontendObject frontendObject = new FrontendObject();
            foreach (var chunk in chunkReader.ReadObjectChunks(frontendObject, chunkBlock.Size))
            {
                Debugger.Break();
            }
        }

        public override FrontendChunkType GetChunkType()
        {
            return FrontendChunkType.ObjectContainer;
        }
    }
}