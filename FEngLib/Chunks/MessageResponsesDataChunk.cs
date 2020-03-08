using System.Diagnostics;
using System.IO;
using FEngLib.Data;
using FEngLib.Tags;

namespace FEngLib.Chunks
{
    public class MessageResponsesDataChunk : FrontendObjectChunk
    {
        public override FrontendObject Read(FrontendPackage package, ObjectReaderState readerState, BinaryReader reader)
        {
            FrontendObject newFrontendObject = FrontendObject;
            FrontendTagStream tagStream = new FrontendMessagesTagStream(reader, readerState.CurrentChunkBlock,
                readerState.CurrentChunkBlock.Size);

            while (tagStream.HasTag())
            {
                FrontendTag tag = tagStream.NextTag(newFrontendObject);
                Debug.WriteLine("MESSAGES TAG {0}", tag);
                newFrontendObject = ProcessTag(newFrontendObject, readerState.CurrentChunkBlock, tag);
            }

            return newFrontendObject;
        }

        private FrontendObject ProcessTag(FrontendObject frontendObject, FrontendChunkBlock frontendChunkBlock, FrontendTag tag)
        {
            switch (tag)
            {
                case MessageResponseInfoTag messageResponseInfoTag:
                    ProcessMessageResponseInfoTag(frontendObject, frontendChunkBlock, messageResponseInfoTag);
                    break;
            }

            return frontendObject;
        }

        private void ProcessMessageResponseInfoTag(FrontendObject frontendObject, FrontendChunkBlock frontendChunkBlock,
            MessageResponseInfoTag tag)
        {
            var response = new FEMessageResponse { Id = tag.Hash };

            frontendObject.Responses.Add(response);
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