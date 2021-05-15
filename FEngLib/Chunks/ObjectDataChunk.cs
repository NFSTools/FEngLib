using System;
using System.Diagnostics;
using System.IO;
using FEngLib.Data;
using FEngLib.Object;
using FEngLib.Tags;

namespace FEngLib.Chunks
{
    public class ObjectDataChunk : FrontendObjectChunk
    {
        public ObjectDataChunk(IObject<ObjectData> frontendObject) : base(frontendObject)
        {
        }

        public override IObject<ObjectData> Read(FrontendPackage package, ObjectReaderState readerState, BinaryReader reader)
        {
            var newFrontendObject = FrontendObject;
            FrontendTagStream tagStream = new FrontendObjectTagStream(reader, readerState.CurrentChunkBlock,
                readerState.CurrentChunkBlock.Size);

            while (tagStream.HasTag())
            {
                var tag = tagStream.NextTag(newFrontendObject);
                newFrontendObject = ProcessTag(package, newFrontendObject, tag);
            }

            return newFrontendObject;
        }

        private IObject<ObjectData> ProcessTag(FrontendPackage package, IObject<ObjectData> frontendObject, FrontendTag tag)
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
                case ImageInfoTag imageInfoTag when frontendObject is Image frontendImage:
                    ProcessImageInfoTag(frontendImage, imageInfoTag);
                    break;
                case ObjectDataTag objectDataTag:
                    ProcessObjectDataTag(frontendObject, objectDataTag);
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
                    break;
                default:
                    throw new InvalidDataException($"Unknown tag: {tag}");
            }

            return frontendObject;
        }

        private void ProcessObjectParentTag(FrontendPackage package, IObject<ObjectData> frontendObject,
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

        private void ProcessObjectDataTag(IObject<ObjectData> frontendObject, ObjectDataTag objectDataTag)
        {
            frontendObject.InitializeData();

            var frontendObjectData = frontendObject.Data;
            Debug.Assert(frontendObjectData != null, "frontendObjectData != null");

            var objectData = objectDataTag.Data;
            frontendObjectData.Position = objectData.Position;
            frontendObjectData.Pivot = objectData.Pivot;
            frontendObjectData.Color = objectData.Color;
            frontendObjectData.Rotation = objectData.Rotation;
            frontendObjectData.Size = objectData.Size;

            if (objectData is ImageData imageData)
            {
                var image = (Image) frontendObject;
                image.Data.UpperLeft = imageData.UpperLeft;
                image.Data.LowerRight = imageData.LowerRight;
            }

            if (objectData is MultiImageData multiImageData)
            {
                var multiImage = (MultiImage) frontendObject;
                multiImage.Data.PivotRotation = multiImageData.PivotRotation;
                multiImage.Data.TopLeft1 = multiImageData.TopLeft1;
                multiImage.Data.TopLeft2 = multiImageData.TopLeft2;
                multiImage.Data.TopLeft3 = multiImageData.TopLeft3;
                multiImage.Data.BottomRight1 = multiImageData.BottomRight1;
                multiImage.Data.BottomRight2 = multiImageData.BottomRight2;
                multiImage.Data.BottomRight3 = multiImageData.BottomRight3;
            }

            if (objectData is ColoredImageData coloredImageData)
            {
                var coloredImage = (ColoredImage) frontendObject;
                coloredImage.Data.TopLeft = coloredImageData.TopLeft;
                coloredImage.Data.TopRight = coloredImageData.TopRight;
                coloredImage.Data.BottomRight = coloredImageData.BottomRight;
                coloredImage.Data.BottomLeft = coloredImageData.BottomLeft;
            }
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

        private void ProcessImageInfoTag(Image image, ImageInfoTag imageInfoTag)
        {
            image.ImageFlags = imageInfoTag.ImageFlags;
        }

        private void ProcessObjectReferenceTag(FrontendPackage package, IObject<ObjectData> frontendObject,
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