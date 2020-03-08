using System.IO;

namespace FEngLib
{
    public abstract class FrontendTagStream
    {
        protected readonly FrontendChunkBlock FrontendChunkBlock;
        protected readonly BinaryReader Reader;
        private readonly long _endPosition;

        protected FrontendTagStream(BinaryReader reader, FrontendChunkBlock frontendChunkBlock, long length)
        {
            FrontendChunkBlock = frontendChunkBlock;
            Reader = reader;
            _endPosition = reader.BaseStream.Position + length;
        }

        public bool HasTag()
        {
            return Reader.BaseStream.Position < _endPosition;
        }

        public abstract FrontendTag NextTag(FrontendObject frontendObject);
    }
}