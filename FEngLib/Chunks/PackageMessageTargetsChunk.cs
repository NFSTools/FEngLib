using System;
using System.IO;
using FEngLib.Packages;
using FEngLib.Scripts;
using FEngLib.Scripts.Tags;
using FEngLib.Tags;

namespace FEngLib.Chunks
{
    public class PackageMessageTargetsChunk : FrontendChunk
    {
        public override void Read(Package package, FrontendChunkBlock chunkBlock,
            FrontendChunkReader chunkReader, BinaryReader reader)
        {
            TagStream tagStream = new MessageTagStream(reader, package, chunkBlock,
                chunkBlock.Size);

            while (tagStream.HasTag())
            {
                var tag = tagStream.NextTag(null);
                package = ProcessTag(package, tag);
            }
        }

        private Package ProcessTag(Package package, Tag tag)
        {
            switch (tag)
            {
                case MessageResponseInfoTag messageResponseInfoTag:
                    ProcessMessageResponseInfoTag(package, messageResponseInfoTag);
                    break;
                case ResponseIdTag responseIdTag:
                    ProcessResponseIdTag(package, responseIdTag);
                    break;
                case ResponseIntParamTag responseParamTag:
                    ProcessResponseParamTag(package, responseParamTag);
                    break;
                case ResponseTargetTag responseTargetTag:
                    ProcessResponseTargetTag(package, responseTargetTag);
                    break;
            }

            return package;
        }

        private void ProcessResponseParamTag(Package package,
            ResponseIntParamTag responseIntParamTag)
        {
            package.MessageResponses[^1].Responses[^1].Param = responseIntParamTag.Param;
        }

        private void ProcessResponseTargetTag(Package package,
            ResponseTargetTag responseTargetTag)
        {
            package.MessageResponses[^1].Responses[^1].Target = responseTargetTag.Target;
        }

        private void ProcessResponseIdTag(Package package,
            ResponseIdTag responseIdTag)
        {
            var response = new Response {Id = responseIdTag.Id};
            package.MessageResponses[^1].Responses.Add(response);
        }

        private void ProcessMessageResponseInfoTag(Package package,
            MessageResponseInfoTag tag)
        {
            if (package.MessageResponses.Find(r => r.Id == tag.Hash) != null)
            {
                throw new Exception(
                    $"This is supposed to be impossible! Duplicate MessageResponse (0x{tag.Hash:X}) in package {package.Name}");
            }

            var response = new MessageResponse {Id = tag.Hash};

            package.MessageResponses.Add(response);
        }

        public override FrontendChunkType GetChunkType()
        {
            return FrontendChunkType.PackageResponses;
        }
    }
}