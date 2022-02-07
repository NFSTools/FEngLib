using System.IO;
using FEngLib.Chunks;
using FEngLib.Tags;
using static FEngLib.FrontendTagType;

namespace FEngLib.Objects.Tags;

public class ObjectTagStream : TagStream
{
    public ObjectTagStream(BinaryReader reader, FrontendChunkBlock frontendChunkBlock, long length) : base(
        reader, frontendChunkBlock, length)
    {
    }

    public override Tag NextTag(IObject<ObjectData> frontendObject)
    {
        var (id, size) = (Reader.ReadUInt16(), Reader.ReadUInt16());
        var pos = Reader.BaseStream.Position;
        Tag tag = (FrontendTagType)id switch
        {
            FrontendTagType.ObjectType => new ObjectTypeTag(frontendObject),
            ObjectHash => new ObjectHashTag(frontendObject),
            ObjectReference => new ObjectReferenceTag(frontendObject),
            ImageInfo => new ImageInfoTag(frontendObject),
            FrontendTagType.ObjectData => new ObjectDataTag(frontendObject),
            ObjectParent => new ObjectParentTag(frontendObject),
            StringBufferLength => new StringBufferLengthTag(frontendObject),
            StringBufferText => new StringBufferTextTag(frontendObject),
            StringBufferFormatting => new StringBufferFormattingTag(frontendObject),
            StringBufferLeading => new StringBufferLeadingTag(frontendObject),
            StringBufferMaxWidth => new StringBufferMaxWidthTag(frontendObject),
            StringBufferLabelHash => new StringBufferLabelHashTag(frontendObject),
            MultiImageTexture1 => new MultiImageTextureTag(frontendObject),
            MultiImageTexture2 => new MultiImageTextureTag(frontendObject),
            MultiImageTexture3 => new MultiImageTextureTag(frontendObject),
            MultiImageTextureFlags1 => new MultiImageTextureFlagsTag(frontendObject),
            MultiImageTextureFlags2 => new MultiImageTextureFlagsTag(frontendObject),
            MultiImageTextureFlags3 => new MultiImageTextureFlagsTag(frontendObject),
            ObjectName => new ObjectNameTag(frontendObject),
            StringBufferLabel => new StringBufferLabelTag(frontendObject),
            _ => throw new ChunkReadingException($"Unrecognized tag: 0x{id:X4}")
        };

        tag.Read(Reader, FrontendChunkBlock, null, id, size);

        if (Reader.BaseStream.Position - pos != size)
            throw new ChunkReadingException(
                $"Expected {size} bytes to be read by {tag.GetType()} but {Reader.BaseStream.Position - pos} bytes were read");

        return tag;
    }
}