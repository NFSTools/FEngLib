using System;
using FEngLib.Chunks;
using FEngLib.Messaging.Tags;
using FEngLib.Tags;

namespace FEngLib.Messaging;

public class MessageResponseTagProcessor<TEntity> where TEntity : IHaveMessageResponses
{
    public void ProcessTag(TEntity targetEntity, Tag tag)
    {
        switch (tag)
        {
            case MessageResponseCountTag: // TODO: do we want to use this for data validation?
                break;
            case MessageResponseInfoTag messageResponseInfoTag:
                ProcessMessageResponseInfoTag(targetEntity, messageResponseInfoTag);
                break;
            case ResponseIdTag responseIdTag:
                ProcessResponseIdTag(targetEntity, responseIdTag);
                break;
            case ResponseIntParamTag responseParamTag:
                ProcessResponseIntParamTag(targetEntity, responseParamTag);
                break;
            case ResponseStringParamTag responseParamTag:
                ProcessResponseStringParamTag(targetEntity, responseParamTag);
                break;
            case ResponseTargetTag responseTargetTag:
                ProcessResponseTargetTag(targetEntity, responseTargetTag);
                break;
            default:
                throw new NotImplementedException($"Unsupported tag type: {tag.GetType()}");
        }
    }

    private void ProcessResponseIntParamTag(TEntity targetEntity,
        ResponseIntParamTag responseIntParamTag)
    {
        targetEntity.MessageResponses[^1].Responses[^1].IntParam = responseIntParamTag.Param;
    }

    private void ProcessResponseStringParamTag(TEntity targetEntity,
        ResponseStringParamTag responseStringParamTag)
    {
        targetEntity.MessageResponses[^1].Responses[^1].StringParam = responseStringParamTag.Param;
    }

    private void ProcessResponseTargetTag(TEntity targetEntity,
        ResponseTargetTag responseTargetTag)
    {
        targetEntity.MessageResponses[^1].Responses[^1].Target = responseTargetTag.Target;
    }

    private void ProcessResponseIdTag(TEntity targetEntity,
        ResponseIdTag responseIdTag)
    {
        var response = new Response { Id = responseIdTag.Id };
        targetEntity.MessageResponses[^1].Responses.Add(response);
    }

    private void ProcessMessageResponseInfoTag(TEntity targetEntity,
        MessageResponseInfoTag tag)
    {
        if (targetEntity.MessageResponses.Find(r => r.Id == tag.Hash) != null)
            throw new ChunkReadingException(
                $"Encountered a duplicate MessageResponse with ID 0x{tag.Hash:X}. This should not be possible.");

        var response = new MessageResponse { Id = tag.Hash };

        targetEntity.MessageResponses.Add(response);
    }
}