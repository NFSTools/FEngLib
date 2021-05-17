using System.Collections.Generic;
using System.IO;
using FEngLib.Objects;
using FEngLib.Packages;

namespace FEngLib.Chunks
{
    public class ObjectContainerChunk : FrontendChunk
    {
        public List<BaseObject> Objects { get; set; }

        public ObjectContainerChunk()
        {
            Objects = new List<BaseObject>();
        }

        public override void Read(Package package, FrontendChunkBlock chunkBlock, FrontendChunkReader chunkReader, BinaryReader reader)
        {
            chunkReader.ReadObjects(chunkBlock.Size);
            //foreach (var frontendObject in chunkReader.ReadObjects(chunkBlock.Size))
            //{
            //    Objects.Add(frontendObject);
            //}
        }

        public override FrontendChunkType GetChunkType()
        {
            return FrontendChunkType.ObjectContainer;
        }
    }
}