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
            FrontendTagStream tagStream = new FrontendMessagesTagStream(reader, newFrontendObject.Package, readerState.CurrentChunkBlock,
                readerState.CurrentChunkBlock.Size);

            while (tagStream.HasTag())
            {
                FrontendTag tag = tagStream.NextTag(newFrontendObject);
                //Debug.WriteLine("MESSAGES TAG {0}", tag);
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
                case ResponseIdTag responseIdTag:
                    ProcessResponseIdTag(frontendObject, frontendChunkBlock, responseIdTag);
                    break;
                case ResponseParamTag responseParamTag:
                    ProcessResponseParamTag(frontendObject, frontendChunkBlock, responseParamTag);
                    break;
                case ResponseTargetTag responseTargetTag:
                    ProcessResponseTargetTag(frontendObject, frontendChunkBlock, responseTargetTag);
                    break;
            }

            return frontendObject;
        }

        private void ProcessResponseParamTag(FrontendObject frontendObject, FrontendChunkBlock frontendChunkBlock,
            ResponseParamTag responseParamTag)
        {
            frontendObject.MessageResponses[^1].Responses[^1].Param = responseParamTag.Param;
        }

        private void ProcessResponseTargetTag(FrontendObject frontendObject, FrontendChunkBlock frontendChunkBlock,
            ResponseTargetTag responseTargetTag)
        {
            frontendObject.MessageResponses[^1].Responses[^1].Target = responseTargetTag.Target;
        }

        private void ProcessResponseIdTag(FrontendObject frontendObject, FrontendChunkBlock frontendChunkBlock,
            ResponseIdTag responseIdTag)
        {
            FEResponse response = new FEResponse { Id = responseIdTag.Id };
            frontendObject.MessageResponses[^1].Responses.Add(response);
        }

        private void ProcessMessageResponseInfoTag(FrontendObject frontendObject, FrontendChunkBlock frontendChunkBlock,
            MessageResponseInfoTag tag)
        {
            FEMessageResponse foundResponse;

            if ((foundResponse = frontendObject.MessageResponses.Find(r => r.Id == tag.Hash)) != null)
            {
                foundResponse.Responses.Clear();
            }
            else
            {
                var response = new FEMessageResponse { Id = tag.Hash };

                frontendObject.MessageResponses.Add(response);
            }
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