using System.Collections.Generic;
using System.IO;
using FEngLib.Tags;

namespace FEngLib
{
    public class FrontendTagReader
    {
        public BinaryReader Reader { get; }

        public FrontendTagReader(BinaryReader reader)
        {
            Reader = reader;
        }

        public IEnumerable<FrontendTag> ReadTags(long length)
        {
            var endPos = Reader.BaseStream.Position + length;

            while (Reader.BaseStream.Position < endPos)
            {
                var (id, size) = (Reader.ReadUInt16(), Reader.ReadUInt16());
                var pos = Reader.BaseStream.Position;
                FrontendTag tag = id switch
                {
                    0x744F => new ObjectTypeTag(),
                    0x684F => new ObjectHashTag(),
                    0x504F => new ObjectReferenceTag(),
                    _ => throw new ChunkReadingException($"Unrecognized tag: 0x{id:X4}")
                };

                tag.Read(Reader);

                if (Reader.BaseStream.Position - pos != size)
                {
                    throw new ChunkReadingException($"Expected {size} bytes to be read by {tag.GetType()} but {Reader.BaseStream.Position - pos} bytes were read");
                }

                yield return tag;
            }
        }
    }
}