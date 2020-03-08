using System.IO;
using FEngLib.Tags;

namespace FEngLib
{
    public class FrontendObjectTagStream : FrontendTagStream
    {
        public FrontendObjectTagStream(BinaryReader reader, FrontendChunkBlock frontendChunkBlock, long length) : base(
            reader, frontendChunkBlock, length)
        {
        }

        public override FrontendTag NextTag(FrontendObject frontendObject)
        {
            var (id, size) = (Reader.ReadUInt16(), Reader.ReadUInt16());
            var pos = Reader.BaseStream.Position;
            FrontendTag tag = id switch
            {
                0x744F => new ObjectTypeTag(frontendObject),
                0x684F => new ObjectHashTag(frontendObject),
                0x504F => new ObjectReferenceTag(frontendObject),
                0x6649 => new ImageInfoTag(frontendObject),
                0x4153 => new ObjectDataTag(frontendObject),
                0x4150 => new ObjectParentTag(frontendObject),
                0x6253 => new StringBufferLengthTag(frontendObject),
                0x7453 => new StringBufferTextTag(frontendObject),
                0x6A53 => new StringBufferFormattingTag(frontendObject),
                0x6C53 => new StringBufferLeadingTag(frontendObject),
                0x7753 => new StringBufferMaxWidthTag(frontendObject),
                0x4853 => new StringBufferLabelHashTag(frontendObject),
                0x314D => new MultiImageTextureTag(frontendObject),
                0x324D => new MultiImageTextureTag(frontendObject),
                0x334D => new MultiImageTextureTag(frontendObject),
                0x614D => new MultiImageTextureFlagsTag(frontendObject),
                0x624D => new MultiImageTextureFlagsTag(frontendObject),
                0x634D => new MultiImageTextureFlagsTag(frontendObject),
                _ => throw new ChunkReadingException($"Unrecognized tag: 0x{id:X4}")
            };

            tag.Read(Reader, FrontendChunkBlock, frontendObject.Package, id, size);

            if (Reader.BaseStream.Position - pos != size)
            {
                throw new ChunkReadingException($"Expected {size} bytes to be read by {tag.GetType()} but {Reader.BaseStream.Position - pos} bytes were read");
            }

            return tag;
        }
    }
}