using System.Collections.Generic;
using System.IO;

namespace FEngLib.Chunks
{
    public class ObjectContainerChunk : FrontendChunk
    {
        public List<FrontendObject> Objects { get; set; }

        public ObjectContainerChunk()
        {
            Objects = new List<FrontendObject>();
        }

        public override void Read(FrontendPackage package, FrontendChunkBlock chunkBlock, FrontendChunkReader chunkReader, BinaryReader reader)
        {
            foreach (var frontendObject in chunkReader.ReadObjects(chunkBlock.Size))
            {
                Objects.Add(frontendObject);
            }
        }

        public override FrontendChunkType GetChunkType()
        {
            return FrontendChunkType.ObjectContainer;
        }
    }
}