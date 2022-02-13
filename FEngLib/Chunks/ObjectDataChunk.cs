using System;
using System.IO;
using FEngLib.Objects;
using FEngLib.Objects.Tags;
using FEngLib.Packages;
using FEngLib.Tags;
using FEngLib.Utils;

namespace FEngLib.Chunks;

public class ObjectDataChunk : FrontendObjectChunk
{
    public ObjectDataChunk(IObject<ObjectData> frontendObject) : base(frontendObject)
    {
    }

    public override IObject<ObjectData> Read(Package package, ObjectReaderState readerState, BinaryReader reader)
    {
        var newFrontendObject = FrontendObject;
        var tagStream = new ObjectTagStream(reader, readerState.CurrentChunkBlock.Size, newFrontendObject);

        while (tagStream.HasTag())
        {
            var tag = tagStream.NextTag();
            newFrontendObject = tagStream.Object = ProcessTag(package, newFrontendObject, tag);
        }

        return newFrontendObject;
    }

    private IObject<ObjectData> ProcessTag(Package package, IObject<ObjectData> frontendObject, Tag tag)
    {
        switch (tag)
        {
            case ObjectTypeTag objectTypeTag:
                return ProcessObjectTypeTag(objectTypeTag);
            case StringBufferLengthTag stringBufferLengthTag when frontendObject is Text frontendString:
                return ProcessStringBufferLengthTag(frontendString, stringBufferLengthTag);
            case StringBufferTextTag stringBufferTextTag when frontendObject is Text frontendString:
                return ProcessStringBufferTextTag(frontendString, stringBufferTextTag);
            case StringBufferFormattingTag stringBufferFormattingTag
                when frontendObject is Text frontendString:
                return ProcessStringBufferFormattingTag(frontendString, stringBufferFormattingTag);
            case StringBufferLeadingTag stringBufferLeadingTag when frontendObject is Text frontendString:
                return ProcessStringBufferLeadingTag(frontendString, stringBufferLeadingTag);
            case StringBufferLabelHashTag stringBufferLabelHashTag
                when frontendObject is Text frontendString:
                return ProcessStringBufferLabelHashTag(frontendString, stringBufferLabelHashTag);
            case StringBufferMaxWidthTag stringBufferMaxWidthTag
                when frontendObject is Text frontendString:
                return ProcessStringBufferMaxWidthTag(frontendString, stringBufferMaxWidthTag);
            case StringBufferLabelTag stringBufferLabelTag when frontendObject is Text frontendString:
                frontendString.Label = stringBufferLabelTag.Label;
                frontendString.Hash = Hashing.BinHash(stringBufferLabelTag.Label.ToUpper());
                break;
            case StringBufferLengthTag _:
                break;
            case ObjectHashTag objectHashTag:
                frontendObject.NameHash = objectHashTag.Hash;
                break;
            case ObjectReferenceTag objectReferenceTag:
                ProcessObjectReferenceTag(package, frontendObject, objectReferenceTag);
                break;
            case ImageInfoTag imageInfoTag when frontendObject is IImage<ImageData> frontendImage:
                ProcessImageInfoTag(frontendImage, imageInfoTag);
                break;
            case ObjectParentTag objectParentTag:
                ProcessObjectParentTag(package, frontendObject, objectParentTag);
                break;
            case ObjectNameTag objectNameTag:
                frontendObject.Name = objectNameTag.Name;
                frontendObject.NameHash = objectNameTag.NameHash;
                break;
            case MultiImageTextureTag _:
            case MultiImageTextureFlagsTag _:
            case ObjectDataTag _: // ObjectDataTag calls IObject.InitializeData and then IObject.Data.Read
                break;
            default:
                throw new NotImplementedException($"Unsupported tag type: {tag.GetType()}");
        }

        return frontendObject;
    }

    private void ProcessObjectParentTag(Package package, IObject<ObjectData> frontendObject,
        ObjectParentTag objectParentTag)
    {
        frontendObject.Parent = package.FindObjectByGuid(objectParentTag.ParentId);
    }

    private BaseObject ProcessStringBufferMaxWidthTag(Text frontendString,
        StringBufferMaxWidthTag stringBufferMaxWidthTag)
    {
        frontendString.MaxWidth = stringBufferMaxWidthTag.MaxWidth;
        return frontendString;
    }

    private BaseObject ProcessStringBufferLabelHashTag(Text frontendString,
        StringBufferLabelHashTag stringBufferLabelHashTag)
    {
        frontendString.Hash = stringBufferLabelHashTag.Hash;
        return frontendString;
    }

    private BaseObject ProcessStringBufferLeadingTag(Text frontendString,
        StringBufferLeadingTag stringBufferLeadingTag)
    {
        frontendString.Leading = stringBufferLeadingTag.Leading;
        return frontendString;
    }

    private BaseObject ProcessStringBufferFormattingTag(Text frontendString,
        StringBufferFormattingTag stringBufferFormattingTag)
    {
        frontendString.Formatting = stringBufferFormattingTag.Formatting;
        return frontendString;
    }

    private BaseObject ProcessStringBufferTextTag(Text frontendString,
        StringBufferTextTag stringBufferTextTag)
    {
        frontendString.Value = stringBufferTextTag.Value;
        return frontendString;
    }

    private BaseObject ProcessStringBufferLengthTag(Text frontendString,
        StringBufferLengthTag stringBufferLengthTag)
    {
        frontendString.BufferLength = stringBufferLengthTag.BufferLength;
        return frontendString;
    }

    private IObject<ObjectData> ProcessObjectTypeTag(ObjectTypeTag objectTypeTag)
    {
        IObject<ObjectData> newInstance = objectTypeTag.Type switch
        {
            ObjectType.Image => new Image(null),
            ObjectType.Group => new Group(null),
            ObjectType.String => new Text(null),
            ObjectType.MultiImage => new MultiImage(null),
            ObjectType.ColoredImage => new ColoredImage(null),
            ObjectType.SimpleImage => new SimpleImage(null),
            ObjectType.Movie => new Movie(null),
            _ => throw new IndexOutOfRangeException($"cannot handle object type: {objectTypeTag.Type}")
        };

        return newInstance;
    }

    private void ProcessImageInfoTag(IImage<ImageData> image, ImageInfoTag imageInfoTag)
    {
        image.ImageFlags = imageInfoTag.ImageFlags;
    }

    private void ProcessObjectReferenceTag(Package package, IObject<ObjectData> frontendObject,
        ObjectReferenceTag objectReferenceTag)
    {
        frontendObject.Flags = objectReferenceTag.Flags;
        frontendObject.Guid = objectReferenceTag.Guid;

        if (objectReferenceTag.ResourceIndex > -1)
        {
            frontendObject.ResourceRequest = package.ResourceRequests[objectReferenceTag.ResourceIndex];
        }
    }

    public override FrontendChunkType GetChunkType()
    {
        return FrontendChunkType.ObjectData;
    }
}