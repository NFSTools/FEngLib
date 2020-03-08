using System.Diagnostics;
using System.IO;

namespace FEngLib.Chunks
{
    public class ObjectContainerChunk : FrontendChunk
    {
        public override void Read(FrontendPackage package, FrontendChunkBlock chunkBlock, FrontendChunkReader chunkReader, BinaryReader reader)
        {
            foreach (var frontendObject in chunkReader.ReadObjects(chunkBlock.Size))
            {
                Debug.WriteLine(frontendObject);
                //Debugger.Break();
            }
        }

        public override FrontendChunkType GetChunkType()
        {
            return FrontendChunkType.ObjectContainer;
        }
    }
}