using System.IO;
using FEngLib.Chunks;
using FEngLib.Tags;
using static FEngLib.FrontendTagType;

namespace FEngLib.Objects.Tags;

public class ObjectTagStream : TagStream
{
    public ObjectTagStream(BinaryReader reader, long length, IObject<ObjectData> o) : base(
        reader, length)
    {
        Object = o;
    }

    public IObject<ObjectData> Object { get; set; }

    public override Tag NextTag()
    {
        var (id, size) = (Reader.ReadUInt16(), Reader.ReadUInt16());
        var pos = Reader.BaseStream.Position;
        Tag tag = (FrontendTagType)id switch
        {
            FrontendTagType.ObjectType => new ObjectTypeTag(Object),
            ObjectHash => new ObjectHashTag(Object),
            ObjectReference => new ObjectReferenceTag(Object),
            ImageInfo => new ImageInfoTag(Object),
            FrontendTagType.ObjectData => new ObjectDataTag(Object),
            ObjectParent => new ObjectParentTag(Object),
            StringBufferLength => new StringBufferLengthTag(Object),
            StringBufferText => new StringBufferTextTag(Object),
            StringBufferFormatting => new StringBufferFormattingTag(Object),
            StringBufferLeading => new StringBufferLeadingTag(Object),
            StringBufferMaxWidth => new StringBufferMaxWidthTag(Object),
            StringBufferLabelHash => new StringBufferLabelHashTag(Object),
            MultiImageTexture1 => new MultiImageTextureTag(Object),
            MultiImageTexture2 => new MultiImageTextureTag(Object),
            MultiImageTexture3 => new MultiImageTextureTag(Object),
            MultiImageTextureFlags1 => new MultiImageTextureFlagsTag(Object),
            MultiImageTextureFlags2 => new MultiImageTextureFlagsTag(Object),
            MultiImageTextureFlags3 => new MultiImageTextureFlagsTag(Object),
            ObjectName => new ObjectNameTag(Object),
            StringBufferLabel => new StringBufferLabelTag(Object),
            _ => throw new ChunkReadingException($"Unrecognized tag: 0x{id:X4}")
        };

        tag.Read(Reader, id, size);

        if (Reader.BaseStream.Position - pos != size)
            throw new ChunkReadingException(
                $"Expected {size} bytes to be read by {tag.GetType()} but {Reader.BaseStream.Position - pos} bytes were read");

        return tag;
    }
}