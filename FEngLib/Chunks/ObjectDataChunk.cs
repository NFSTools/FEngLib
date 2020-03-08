using System;
using System.Diagnostics;
using System.IO;
using FEngLib.Data;
using FEngLib.Objects;
using FEngLib.Tags;

namespace FEngLib.Chunks
{
    public class ObjectDataChunk : FrontendObjectChunk
    {
        public override FrontendObject Read(FrontendPackage package, ObjectReaderState readerState, BinaryReader reader)
        {
            FrontendObject newFrontendObject = FrontendObject;
            FrontendTagStream tagStream = new FrontendObjectTagStream(reader, readerState.CurrentChunkBlock, readerState.CurrentChunkBlock.Size);

            while (tagStream.HasTag())
            {
                FrontendTag tag = tagStream.NextTag(newFrontendObject);
                Debug.WriteLine("OBJECT TAG {0}", tag);
                newFrontendObject = ProcessTag(newFrontendObject, tag);
            }

            return newFrontendObject;
        }

        private FrontendObject ProcessTag(FrontendObject frontendObject, FrontendTag tag)
        {
            switch (tag)
            {
                case ObjectTypeTag objectTypeTag:
                    return ProcessObjectTypeTag(frontendObject, objectTypeTag);
                case StringBufferTextTag stringBufferTextTag when frontendObject is FrontendString frontendString:
                    return ProcessStringBufferTextTag(frontendString, stringBufferTextTag);
                case ObjectHashTag objectHashTag:
                    frontendObject.NameHash = objectHashTag.Hash;
                    break;
                case ObjectReferenceTag objectReferenceTag:
                    ProcessObjectReferenceTag(frontendObject, objectReferenceTag);
                    break;
                case ImageInfoTag imageInfoTag:
                    ProcessImageInfoTag(frontendObject, imageInfoTag);
                    break;
                default:
                    Debug.WriteLine("WARN: Unprocessed tag - {0}", tag.GetType());
                    break;
            }

            return frontendObject;
        }

        private FrontendObject ProcessStringBufferTextTag(FrontendString frontendString, StringBufferTextTag stringBufferTextTag)
        {
            frontendString.Value = stringBufferTextTag.Value;
            return frontendString;
        }

        private FrontendObject ProcessObjectTypeTag(FrontendObject frontendObject, ObjectTypeTag objectTypeTag)
        {
            FrontendObject newInstance;

            switch (objectTypeTag.Type)
            {
                case FEObjType.FE_Image:
                    newInstance = new FrontendImage(frontendObject);
                    break;
                case FEObjType.FE_Group:
                    newInstance = new FrontendGroup(frontendObject);
                    break;
                case FEObjType.FE_String:
                    newInstance = new FrontendString(frontendObject);
                    break;
                default:
                    throw new IndexOutOfRangeException($"cannot handle object type: {objectTypeTag.Type}");
            }

            newInstance.Type = objectTypeTag.Type;

            return newInstance;
        }

        private void ProcessImageInfoTag(FrontendObject frontendObject, ImageInfoTag imageInfoTag)
        {
            Debug.WriteLine("FEObject {0:X8} has ImageInfo: value={1}", frontendObject.NameHash, imageInfoTag.Value);
        }

        private void ProcessObjectReferenceTag(FrontendObject frontendObject, ObjectReferenceTag objectReferenceTag)
        {
            Debug.WriteLine("FEObject {0:X8} references object {1:X8}; flags={2}", frontendObject.NameHash,
                objectReferenceTag.ReferencedObjectGuid, objectReferenceTag.Flags);
        }

        public override FrontendChunkType GetChunkType()
        {
            return FrontendChunkType.ObjectData;
        }

        public ObjectDataChunk(FrontendObject frontendObject) : base(frontendObject)
        {
        }
    }
}