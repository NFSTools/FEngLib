using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using FEngLib.Chunks;
using FEngLib.Messaging.Tags;
using FEngLib.Tags;

namespace FEngLib.Messaging;

public class MessageResponseTagProcessor
{
    private List<MessageResponseEntry> _messageResponseEntryList = new();

    public IReadOnlyList<IMessageResponseEntry> MessageResponseEntryList => _messageResponseEntryList;

    // This is a pretty dumb hack, but it gets the job done...
    private interface IResponseCommandEntry
    {
        void SetTarget(uint target);
        void SetIntParam(uint param);
        void SetStringParam(string param);
    }

    public interface IMessageResponseEntry : IEnumerable<ResponseCommandEntry>
    {
        uint ID { get; }
    }

    public class ResponseCommandEntry : IResponseCommandEntry
    {
        public ResponseCommandEntry(uint ID)
        {
            this.ID = ID;
        }

        public uint ID { get; }
        public uint? Target { get; private set; }
        public uint? IParam { get; private set; }
        public string SParam { get; private set; }

        void IResponseCommandEntry.SetTarget(uint target)
        {
            Target = target;
        }

        void IResponseCommandEntry.SetIntParam(uint param)
        {
            Debug.Assert(SParam == null);
            IParam = param;
        }

        void IResponseCommandEntry.SetStringParam(string param)
        {
            Debug.Assert(IParam == null);
            SParam = param;
        }
    }

    private class MessageResponseEntry : IMessageResponseEntry
    {
        public List<ResponseCommandEntry> Commands { get; }
        public uint ID { get; }

        public MessageResponseEntry(uint id)
        {
            ID = id;
            Commands = new List<ResponseCommandEntry>();
        }

        public IEnumerator<ResponseCommandEntry> GetEnumerator()
        {
            return Commands.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    private readonly HashSet<uint> _seenMessageHashes = new();

    public void ProcessTag(Tag tag)
    {
        switch (tag)
        {
            case MessageResponseCountTag: // TODO: do we want to use this for data validation?
                break;
            case MessageResponseInfoTag messageResponseInfoTag:
                ProcessMessageResponseInfoTag(messageResponseInfoTag);
                break;
            case ResponseIdTag responseIdTag:
                ProcessResponseIdTag(responseIdTag);
                break;
            case ResponseIntParamTag responseParamTag:
                ProcessResponseIntParamTag(responseParamTag);
                break;
            case ResponseStringParamTag responseParamTag:
                ProcessResponseStringParamTag(responseParamTag);
                break;
            case ResponseTargetTag responseTargetTag:
                ProcessResponseTargetTag(responseTargetTag);
                break;
            default:
                throw new NotImplementedException($"Unsupported tag type: {tag.GetType()}");
        }
    }

    private void ProcessResponseIntParamTag(ResponseIntParamTag responseIntParamTag)
    {
        // C# makes us do this stupid dance to access an interface method. I don't like it, but it is what it is.
        ((IResponseCommandEntry) _messageResponseEntryList[^1].Commands[^1]).SetIntParam(responseIntParamTag.Param);
    }

    private void ProcessResponseStringParamTag(ResponseStringParamTag responseStringParamTag)
    {
        ((IResponseCommandEntry)_messageResponseEntryList[^1].Commands[^1]).SetStringParam(responseStringParamTag.Param);
    }

    private void ProcessResponseTargetTag(ResponseTargetTag responseTargetTag)
    {
        ((IResponseCommandEntry)_messageResponseEntryList[^1].Commands[^1]).SetTarget(responseTargetTag.Target);
    }

    private void ProcessResponseIdTag(ResponseIdTag responseIdTag)
    {
        Debug.Assert(responseIdTag.Id <= 0x501);

        _messageResponseEntryList[^1].Commands.Add(new ResponseCommandEntry(responseIdTag.Id));
        //ResponseCommand response = responseIdTag.Id switch
        //{
        //    var id => throw new ChunkReadingException($"Unsupported ResponseId: 0x{id:X}")
        //};
        //var response = new ResponseCommand { Id = responseIdTag.Id };
        //targetEntity.MessageResponses[^1].Responses.Add(response);
    }

    private void ProcessMessageResponseInfoTag(MessageResponseInfoTag tag)
    {
        if (!_seenMessageHashes.Add(tag.Hash))
            throw new ChunkReadingException(
                $"Encountered a duplicate MessageResponse with ID 0x{tag.Hash:X}. This should not be possible.");

        _messageResponseEntryList.Add(new MessageResponseEntry(tag.Hash));
    }
}