using System.IO;
using FEngLib.Messaging;
using FEngLib.Objects;
using FEngLib.Packages;
using FEngLib.Tags;

namespace FEngLib.Chunks;

public class MessageResponsesDataChunk : FrontendObjectChunk
{
    public MessageResponsesDataChunk(IObject<ObjectData> frontendObject) : base(frontendObject)
    {
    }

    public override IObject<ObjectData> Read(Package package, ObjectReaderState readerState, BinaryReader reader)
    {
        var tagProcessor = new MessageResponseTagProcessor();
        TagStream tagStream = new MessageTagStream(reader,
            readerState.CurrentChunkBlock.Size);

        while (tagStream.HasTag())
        {
            var tag = tagStream.NextTag();
            tagProcessor.ProcessTag(tag);
        }

        ResponseHelpers.PopulateMessageResponseList(tagProcessor.MessageResponseEntryList, FrontendObject);

        return FrontendObject;
    }

    public override FrontendChunkType GetChunkType()
    {
        return FrontendChunkType.MessageResponses;
    }
}