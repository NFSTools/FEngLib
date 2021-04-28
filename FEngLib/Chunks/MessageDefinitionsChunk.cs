using System.Collections.Generic;
using System.IO;

namespace FEngLib.Chunks
{
    public class MessageDefinitionsChunk : FrontendChunk
    {
        public List<FrontendPackage.MessageDefinition> Definitions { get; set; } =
            new List<FrontendPackage.MessageDefinition>();

        public override void Read(FrontendPackage package, FrontendChunkBlock chunkBlock,
            FrontendChunkReader chunkReader, BinaryReader reader)
        {
            while (reader.BaseStream.Position < chunkBlock.EndOffset)
            {
                var tagId = reader.ReadUInt16();
                var tagLen = reader.ReadUInt16();

                switch (tagId)
                {
                    case 0x4E4D:
                        Definitions.Add(new FrontendPackage.MessageDefinition
                        {
                            Name = new string(reader.ReadChars(tagLen)).Trim('\x00')
                        });
                        break;
                    case 0x434D:
                        Definitions[^1].Category = new string(reader.ReadChars(tagLen)).Trim('\x00');
                        break;
                }
            }
        }

        public override FrontendChunkType GetChunkType()
        {
            return FrontendChunkType.MessageDefinitions;
        }
    }
}