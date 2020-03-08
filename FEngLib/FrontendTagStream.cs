using System.IO;

namespace FEngLib
{
    public abstract class FrontendTagStream
    {
        protected readonly BinaryReader Reader;
        private readonly long _endPosition;

        protected FrontendTagStream(BinaryReader reader, long length)
        {
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