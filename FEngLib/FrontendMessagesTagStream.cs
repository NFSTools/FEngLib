using System;
using System.IO;
using FEngLib.Tags;

namespace FEngLib
{
    public class FrontendMessagesTagStream : FrontendTagStream
    {
        public FrontendMessagesTagStream(BinaryReader reader, long length) : base(reader, length)
        {
        }

        public override FrontendTag NextTag(FrontendObject frontendObject)
        {
            var (id, size) = (Reader.ReadUInt16(), Reader.ReadUInt16());
            var pos = Reader.BaseStream.Position;
            FrontendTag tag = id switch
            {
                _ => throw new ChunkReadingException($"Unrecognized tag: 0x{id:X4}")
            };

            tag.Read(Reader, size);

            if (Reader.BaseStream.Position - pos != size)
            {
                throw new ChunkReadingException($"Expected {size} bytes to be read by {tag.GetType()} but {Reader.BaseStream.Position - pos} bytes were read");
            }

            return tag;
        }
    }
}