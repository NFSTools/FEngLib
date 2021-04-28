using System.IO;
using FEngLib.Data;
using FEngLib.Tags;

namespace FEngLib.Chunks
{
    public class PackageMessageTargetsChunk : FrontendChunk
    {
        public override void Read(FrontendPackage package, FrontendChunkBlock chunkBlock,
            FrontendChunkReader chunkReader, BinaryReader reader)
        {
            FrontendTagStream tagStream = new FrontendMessagesTagStream(reader, package, chunkBlock,
                chunkBlock.Size);

            while (tagStream.HasTag())
            {
                var tag = tagStream.NextTag(null);
                package = ProcessTag(package, tag);
            }
        }

        private FrontendPackage ProcessTag(FrontendPackage frontendPackage, FrontendTag tag)
        {
            switch (tag)
            {
                case MessageResponseInfoTag messageResponseInfoTag:
                    ProcessMessageResponseInfoTag(frontendPackage, messageResponseInfoTag);
                    break;
                case ResponseIdTag responseIdTag:
                    ProcessResponseIdTag(frontendPackage, responseIdTag);
                    break;
                case ResponseIntParamTag responseParamTag:
                    ProcessResponseParamTag(frontendPackage, responseParamTag);
                    break;
                case ResponseTargetTag responseTargetTag:
                    ProcessResponseTargetTag(frontendPackage, responseTargetTag);
                    break;
            }

            return frontendPackage;
        }

        private void ProcessResponseParamTag(FrontendPackage frontendPackage,
            ResponseIntParamTag responseIntParamTag)
        {
            frontendPackage.MessageResponses[^1].Responses[^1].Param = responseIntParamTag.Param;
        }

        private void ProcessResponseTargetTag(FrontendPackage frontendPackage,
            ResponseTargetTag responseTargetTag)
        {
            frontendPackage.MessageResponses[^1].Responses[^1].Target = responseTargetTag.Target;
        }

        private void ProcessResponseIdTag(FrontendPackage frontendPackage,
            ResponseIdTag responseIdTag)
        {
            var response = new FEResponse {Id = responseIdTag.Id};
            frontendPackage.MessageResponses[^1].Responses.Add(response);
        }

        private void ProcessMessageResponseInfoTag(FrontendPackage frontendPackage,
            MessageResponseInfoTag tag)
        {
            FEMessageResponse foundResponse;

            if ((foundResponse = frontendPackage.MessageResponses.Find(r => r.Id == tag.Hash)) != null)
            {
                foundResponse.Responses.Clear();
            }
            else
            {
                var response = new FEMessageResponse {Id = tag.Hash};

                frontendPackage.MessageResponses.Add(response);
            }
        }

        public override FrontendChunkType GetChunkType()
        {
            return FrontendChunkType.PackageResponses;
        }
    }
}