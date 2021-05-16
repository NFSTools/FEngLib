using System.Collections.Generic;
using System.IO;
using FEngLib.Packages;

namespace FEngLib.Chunks
{
    public class TypeListChunk : FrontendChunk
    {
        public List<TypeSizeEntry> TypeSizeList { get; set; }

        public override void Read(Package package, FrontendChunkBlock chunkBlock,
            FrontendChunkReader chunkReader, BinaryReader reader)
        {
            if (chunkBlock.Size % 8 != 0)
            {
                throw new ChunkReadingException("Invalid TypeList chunk");
            }

            TypeSizeList = new List<TypeSizeEntry>(chunkBlock.Size >> 3);

            for (int i = 0; i < chunkBlock.Size >> 3; i++)
            {
                TypeSizeList.Add(new TypeSizeEntry
                {
                    ID = reader.ReadUInt32(),
                    Size = reader.ReadUInt32()
                });
            }
        }

        public override FrontendChunkType GetChunkType()
        {
            return FrontendChunkType.TypeList;
        }
    }
}