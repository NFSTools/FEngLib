using System;
using System.IO;
using FEngLib.Tags;

namespace FEngLib
{
    public class FrontendMessagesTagStream : FrontendTagStream
    {
        public FrontendPackage Package { get; }

        public FrontendMessagesTagStream(BinaryReader reader, FrontendPackage package, FrontendChunkBlock frontendChunkBlock, long length) :
            base(reader, frontendChunkBlock, length)
        {
            Package = package;
        }

        public override FrontendTag NextTag(FrontendObject frontendObject)
        {
            var (id, size) = (Reader.ReadUInt16(), Reader.ReadUInt16());
            var pos = Reader.BaseStream.Position;
            FrontendTag tag = id switch
            {
                0x694D => new MessageResponseInfoTag(frontendObject),
                0x434D => new MessageResponseCountTag(frontendObject),
                0x6952 => new ResponseIdTag(frontendObject),
                0x7552 => new ResponseParamTag(frontendObject),
                0x7452 => new ResponseTargetTag(frontendObject),
                _ => throw new ChunkReadingException($"Unrecognized tag: 0x{id:X4}")
            };

            tag.Read(Reader, FrontendChunkBlock, Package, size);

            if (Reader.BaseStream.Position - pos != size)
            {
                throw new ChunkReadingException($"Expected {size} bytes to be read by {tag.GetType()} but {Reader.BaseStream.Position - pos} bytes were read");
            }

            return tag;
        }
    }
}