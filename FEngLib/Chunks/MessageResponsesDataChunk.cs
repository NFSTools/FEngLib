using System.Diagnostics;
using System.IO;

namespace FEngLib.Chunks
{
    public class MessageResponsesDataChunk : FrontendObjectChunk
    {
        public override FrontendObject Read(FrontendPackage package, ObjectReaderState readerState, BinaryReader reader)
        {
            FrontendObject newFrontendObject = FrontendObject;
            FrontendTagStream tagStream = new FrontendMessagesTagStream(reader, readerState.CurrentChunkBlock.Size);

            while (tagStream.HasTag())
            {
                FrontendTag tag = tagStream.NextTag(newFrontendObject);
                Debug.WriteLine("MESSAGES TAG {0}", tag);
                newFrontendObject = ProcessTag(newFrontendObject, tag);
            }

            return newFrontendObject;
        }

        private FrontendObject ProcessTag(FrontendObject frontendObject, FrontendTag tag)
        {
            return frontendObject;
        }

        public override FrontendChunkType GetChunkType()
        {
            return FrontendChunkType.MessageResponses;
        }

        public MessageResponsesDataChunk(FrontendObject frontendObject) : base(frontendObject)
        {
        }
    }
}