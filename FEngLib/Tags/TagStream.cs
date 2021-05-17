using System.IO;
using FEngLib.Objects;

namespace FEngLib.Tags
{
    public abstract class TagStream
    {
        protected readonly FrontendChunkBlock FrontendChunkBlock;
        protected readonly BinaryReader Reader;
        private readonly long _endPosition;

        protected TagStream(BinaryReader reader, FrontendChunkBlock frontendChunkBlock, long length)
        {
            FrontendChunkBlock = frontendChunkBlock;
            Reader = reader;
            _endPosition = reader.BaseStream.Position + length;
        }

        public bool HasTag()
        {
            return Reader.BaseStream.Position < _endPosition;
        }

        public abstract Tag NextTag(IObject<ObjectData> frontendObject);
    }
}