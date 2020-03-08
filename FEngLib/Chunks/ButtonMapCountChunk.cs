using System.IO;

namespace FEngLib.Chunks
{
    public class ButtonMapCountChunk : FrontendObjectChunk
    {
        public uint NumEntries { get; set; }

        public override FrontendObject Read(FrontendPackage package, FrontendChunkBlock chunkBlock, FrontendChunkReader chunkReader, BinaryReader reader)
        {
            NumEntries = reader.ReadUInt32();
            return FrontendObject;
        }

        public override FrontendChunkType GetChunkType()
        {
            return FrontendChunkType.ButtonMapCount;
        }

        public ButtonMapCountChunk(FrontendObject frontendObject) : base(frontendObject)
        {
        }
    }
}