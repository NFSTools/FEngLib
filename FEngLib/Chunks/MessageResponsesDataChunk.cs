﻿using System.IO;
using FEngLib.Objects;
using FEngLib.Packages;
using FEngLib.Scripts;
using FEngLib.Scripts.Tags;
using FEngLib.Tags;

namespace FEngLib.Chunks;

public class MessageResponsesDataChunk : FrontendObjectChunk
{
    public MessageResponsesDataChunk(IObject<ObjectData> frontendObject) : base(frontendObject)
    {
    }

    public override IObject<ObjectData> Read(Package package, ObjectReaderState readerState, BinaryReader reader)
    {
        var newFrontendObject = FrontendObject;
        TagStream tagStream = new MessageTagStream(reader, package,
            readerState.CurrentChunkBlock,
            readerState.CurrentChunkBlock.Size);

        while (tagStream.HasTag())
        {
            var tag = tagStream.NextTag(newFrontendObject);
            newFrontendObject = ProcessTag(newFrontendObject, tag);
        }

        return newFrontendObject;
    }

    private IObject<ObjectData> ProcessTag(IObject<ObjectData> frontendObject,
        Tag tag)
    {
        switch (tag)
        {
            case MessageResponseInfoTag messageResponseInfoTag:
                ProcessMessageResponseInfoTag(frontendObject, messageResponseInfoTag);
                break;
            case ResponseIdTag responseIdTag:
                ProcessResponseIdTag(frontendObject, responseIdTag);
                break;
            case ResponseIntParamTag responseParamTag:
                ProcessResponseIntParamTag(frontendObject, responseParamTag);
                break;
            case ResponseStringParamTag responseParamTag:
                ProcessResponseStringParamTag(frontendObject, responseParamTag);
                break;
            case ResponseTargetTag responseTargetTag:
                ProcessResponseTargetTag(frontendObject, responseTargetTag);
                break;
        }

        return frontendObject;
    }

    private void ProcessResponseIntParamTag(IObject<ObjectData> frontendObject,
        ResponseIntParamTag responseIntParamTag)
    {
        frontendObject.MessageResponses[^1].Responses[^1].Param = responseIntParamTag.Param;
    }

    private void ProcessResponseStringParamTag(IObject<ObjectData> frontendObject,
        ResponseStringParamTag responseStringParamTag)
    {
        frontendObject.MessageResponses[^1].Responses[^1].Param = responseStringParamTag.Param;
    }

    private void ProcessResponseTargetTag(IObject<ObjectData> frontendObject,
        ResponseTargetTag responseTargetTag)
    {
        frontendObject.MessageResponses[^1].Responses[^1].Target = responseTargetTag.Target;
    }

    private void ProcessResponseIdTag(IObject<ObjectData> frontendObject,
        ResponseIdTag responseIdTag)
    {
        var response = new Response {Id = responseIdTag.Id};
        frontendObject.MessageResponses[^1].Responses.Add(response);
    }

    private void ProcessMessageResponseInfoTag(IObject<ObjectData> frontendObject,
        MessageResponseInfoTag tag)
    {
        if (frontendObject.MessageResponses.Find(r => r.Id == tag.Hash) != null)
        {
            throw new ChunkReadingException($"Encountered a duplicate MessageResponse with ID 0x{tag.Hash:X}. This should not be possible.");
        }

        var response = new MessageResponse {Id = tag.Hash};

        frontendObject.MessageResponses.Add(response);
    }

    public override FrontendChunkType GetChunkType()
    {
        return FrontendChunkType.MessageResponses;
    }
}