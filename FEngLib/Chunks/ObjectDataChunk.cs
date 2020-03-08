using System.Diagnostics;
using System.IO;
using FEngLib.Tags;

namespace FEngLib.Chunks
{
    public class ObjectDataChunk : FrontendObjectChunk
    {
        public override void Read(FrontendPackage package, FrontendChunkBlock chunkBlock, FrontendChunkReader chunkReader, BinaryReader reader)
        {
            FrontendTagReader tagReader = new FrontendTagReader(reader);

            foreach (var tag in tagReader.ReadObjectTags(FrontendObject, chunkBlock.Size))
            {
                this.ProcessTag(FrontendObject, tag);
            }
        }

        private void ProcessTag(FrontendObject frontendObject, FrontendTag tag)
        {
            switch (tag)
            {
                case ObjectTypeTag objectTypeTag:
                    frontendObject.Type = objectTypeTag.Type;
                    break;
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