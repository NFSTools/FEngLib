using System.IO;

namespace FEngLib.Chunks
{
    public class ButtonMapCountChunk : FrontendChunk
    {
        public uint NumEntries { get; set; }

        public override void Read(FrontendPackage package, FrontendChunkBlock chunkBlock, FrontendChunkReader chunkReader, BinaryReader reader)
        {
            NumEntries = reader.ReadUInt32();
        }

        public override FrontendChunkType GetChunkType()
        {
            return FrontendChunkType.ButtonMapCount;
        }
    }
}