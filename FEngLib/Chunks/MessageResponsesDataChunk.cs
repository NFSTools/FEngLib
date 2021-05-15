using System.IO;
using FEngLib.Data;
using FEngLib.Object;
using FEngLib.Tags;

namespace FEngLib.Chunks
{
    public class MessageResponsesDataChunk : FrontendObjectChunk
    {
        public MessageResponsesDataChunk(IObject<ObjectData> frontendObject) : base(frontendObject)
        {
        }

        public override IObject<ObjectData> Read(FrontendPackage package, ObjectReaderState readerState, BinaryReader reader)
        {
            var newFrontendObject = FrontendObject;
            FrontendTagStream tagStream = new FrontendMessagesTagStream(reader, package,
                readerState.CurrentChunkBlock,
                readerState.CurrentChunkBlock.Size);

            while (tagStream.HasTag())
            {
                var tag = tagStream.NextTag(newFrontendObject);
                newFrontendObject = ProcessTag(newFrontendObject, readerState.CurrentChunkBlock, tag);
            }

            return newFrontendObject;
        }

        private IObject<ObjectData> ProcessTag(IObject<ObjectData> frontendObject, FrontendChunkBlock frontendChunkBlock,
            FrontendTag tag)
        {
            switch (tag)
            {
                case MessageResponseInfoTag messageResponseInfoTag:
                    ProcessMessageResponseInfoTag(frontendObject, frontendChunkBlock, messageResponseInfoTag);
                    break;
                case ResponseIdTag responseIdTag:
                    ProcessResponseIdTag(frontendObject, frontendChunkBlock, responseIdTag);
                    break;
                case ResponseIntParamTag responseParamTag:
                    ProcessResponseParamTag(frontendObject, frontendChunkBlock, responseParamTag);
                    break;
                case ResponseTargetTag responseTargetTag:
                    ProcessResponseTargetTag(frontendObject, frontendChunkBlock, responseTargetTag);
                    break;
            }

            return frontendObject;
        }

        private void ProcessResponseParamTag(IObject<ObjectData> frontendObject, FrontendChunkBlock frontendChunkBlock,
            ResponseIntParamTag responseIntParamTag)
        {
            frontendObject.MessageResponses[^1].Responses[^1].Param = responseIntParamTag.Param;
        }

        private void ProcessResponseTargetTag(IObject<ObjectData> frontendObject, FrontendChunkBlock frontendChunkBlock,
            ResponseTargetTag responseTargetTag)
        {
            frontendObject.MessageResponses[^1].Responses[^1].Target = responseTargetTag.Target;
        }

        private void ProcessResponseIdTag(IObject<ObjectData> frontendObject, FrontendChunkBlock frontendChunkBlock,
            ResponseIdTag responseIdTag)
        {
            var response = new FEResponse {Id = responseIdTag.Id};
            frontendObject.MessageResponses[^1].Responses.Add(response);
        }

        private void ProcessMessageResponseInfoTag(IObject<ObjectData> frontendObject, FrontendChunkBlock frontendChunkBlock,
            MessageResponseInfoTag tag)
        {
            FEMessageResponse foundResponse;

            if ((foundResponse = frontendObject.MessageResponses.Find(r => r.Id == tag.Hash)) != null)
            {
                foundResponse.Responses.Clear();
            }
            else
            {
                var response = new FEMessageResponse {Id = tag.Hash};

                frontendObject.MessageResponses.Add(response);
            }
        }

        public override FrontendChunkType GetChunkType()
        {
            return FrontendChunkType.MessageResponses;
        }
    }
}