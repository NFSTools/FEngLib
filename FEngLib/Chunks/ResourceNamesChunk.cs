using System.Collections.Generic;
using System.IO;
using FEngLib.Packages;
using FEngLib.Utils;

namespace FEngLib.Chunks
{
    public class ResourceNamesChunk : FrontendChunk
    {
        public Dictionary<long, string> Names { get; set; }

        public override void Read(Package package, FrontendChunkBlock chunkBlock,
            FrontendChunkReader chunkReader, BinaryReader reader)
        {
            Names = new Dictionary<long, string>();

            while (reader.BaseStream.Position < chunkBlock.EndOffset)
            {
                var pos = reader.BaseStream.Position;
                var str = NullTerminatedString.Read(reader);
                if (!string.IsNullOrEmpty(str))
                    Names[pos - chunkBlock.DataOffset] = str;
            }
        }

        public override FrontendChunkType GetChunkType()
        {
            return FrontendChunkType.ResourceNames;
        }
    }
}