using System.Diagnostics;
using System.IO;

namespace FEngLib.Chunks
{
    public class ObjectDataChunk : FrontendObjectChunk
    {
        public override void Read(FrontendPackage package, FrontendChunkBlock chunkBlock, FrontendChunkReader chunkReader, BinaryReader reader)
        {
            FrontendTagReader tagReader = new FrontendTagReader(reader);

            foreach (var tag in tagReader.ReadTags(chunkBlock.Size))
            {
                Debugger.Break();
            }
        }

        public override FrontendChunkType GetChunkType()
        {
            return FrontendChunkType.ObjectData;
        }

        public ObjectDataChunk(FrontendObject frontendObject) : base(frontendObject)
        {
        }
    }
}