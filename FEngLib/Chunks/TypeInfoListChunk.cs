using System.IO;

namespace FEngLib.Chunks
{
    public class TypeInfoListChunk : FrontendChunk
    {
        public override void Read(FrontendPackage package, FrontendChunkBlock chunkBlock,
            FrontendChunkReader chunkReader, BinaryReader reader)
        {
            reader.ReadBytes(chunkBlock.Size);
        }

        public override FrontendChunkType GetChunkType()
        {
            return FrontendChunkType.TypeInfoList;
        }
    }
}