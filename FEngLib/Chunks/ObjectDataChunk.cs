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
        public override FrontendObject Read(FrontendPackage package, FrontendChunkBlock chunkBlock, FrontendChunkReader chunkReader, BinaryReader reader)
        {
            FrontendTagReader tagReader = new FrontendTagReader(reader);
            FrontendObject newFrontendObject = FrontendObject;

            foreach (var tag in tagReader.ReadObjectTags(FrontendObject, chunkBlock.Size))
            {
                newFrontendObject = this.ProcessTag(FrontendObject, tag);
            }

            return newFrontendObject;
        }

        private FrontendObject ProcessTag(FrontendObject frontendObject, FrontendTag tag)
        {
            switch (tag)
            {
                case ObjectTypeTag objectTypeTag:
                    return ProcessObjectTypeTag(frontendObject, objectTypeTag);
                case ObjectHashTag objectHashTag:
                    frontendObject.NameHash = objectHashTag.Hash;
                    break;
                case ObjectReferenceTag objectReferenceTag:
                    ProcessObjectReferenceTag(frontendObject, objectReferenceTag);
                    break;
                case ImageInfoTag imageInfoTag:
                    ProcessImageInfoTag(frontendObject, imageInfoTag);
                    break;
            }

            return frontendObject;
        }

        private FrontendObject ProcessObjectTypeTag(FrontendObject frontendObject, ObjectTypeTag objectTypeTag)
        {
            FrontendObject newInstance = frontendObject;

            switch (objectTypeTag.Type)
            {
                case FEObjType.FE_Image:
                    newInstance = new FrontendImage(frontendObject);
                    break;
                case FEObjType.FE_Group:
                    newInstance = new FrontendGroup(frontendObject);
                    break;
                default:
                    throw new IndexOutOfRangeException($"cannot handle object type: {objectTypeTag.Type}");
            }

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