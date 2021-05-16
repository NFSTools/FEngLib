using System;
using System.Diagnostics;
using System.IO;
using FEngLib.Object;
using FEngLib.Object.Tags;
using FEngLib.Objects;
using FEngLib.Objects.Tags;
using FEngLib.Packages;
using FEngLib.Tags;
using FEngLib.Utils;

namespace FEngLib.Chunks
{
    public class ObjectDataChunk : FrontendObjectChunk
    {
        public ObjectDataChunk(IObject<ObjectData> frontendObject) : base(frontendObject)
        {
        }

        public override IObject<ObjectData> Read(Package package, ObjectReaderState readerState, BinaryReader reader)
        {
            var newFrontendObject = FrontendObject;
            TagStream tagStream = new ObjectTagStream(reader, readerState.CurrentChunkBlock,
                readerState.CurrentChunkBlock.Size);

            while (tagStream.HasTag())
            {
                var tag = tagStream.NextTag(newFrontendObject);
                newFrontendObject = ProcessTag(package, newFrontendObject, tag);
            }

            return newFrontendObject;
        }

        private IObject<ObjectData> ProcessTag(Package package, IObject<ObjectData> frontendObject, Tag tag)
        {
            switch (tag)
            {
                case ObjectTypeTag objectTypeTag:
                    return ProcessObjectTypeTag(frontendObject, objectTypeTag);
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
                case StringBufferLengthTag stringBufferLengthTag:
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
                    throw new InvalidDataException($"Unknown tag: {tag}");
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

        private IObject<ObjectData> ProcessObjectTypeTag(IObject<ObjectData> frontendObject, ObjectTypeTag objectTypeTag)
        {
            IObject<ObjectData> newInstance;

            switch (objectTypeTag.Type)
            {
                case ObjectType.Image:
                    newInstance = new Image(null);
                    break;
                case ObjectType.Group:
                    newInstance = new Group(null);
                    break;
                case ObjectType.String:
                    newInstance = new Text(null);
                    break;
                case ObjectType.MultiImage:
                    newInstance = new MultiImage(null);
                    break;
                case ObjectType.ColoredImage:
                    newInstance = new ColoredImage(null);
                    break;
                case ObjectType.SimpleImage:
                    newInstance = new SimpleImage(null);
                    break;
                case ObjectType.Movie:
                    newInstance = new Movie(null);
                    break;
                default:
                    throw new IndexOutOfRangeException($"cannot handle object type: {objectTypeTag.Type}");
            }

            newInstance.Type = objectTypeTag.Type;

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
}