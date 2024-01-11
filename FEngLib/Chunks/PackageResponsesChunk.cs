using System.IO;
using FEngLib.Messaging;
using FEngLib.Packages;
using FEngLib.Tags;

namespace FEngLib.Chunks;

public class PackageResponsesChunk : FrontendChunk
{
    public override void Read(Package package, FrontendChunkBlock chunkBlock,
        FrontendChunkReader chunkReader, BinaryReader reader)
    {
        var tagProcessor = new MessageResponseTagProcessor();
        TagStream tagStream = new MessageTagStream(reader,
            chunkBlock.Size);

        while (tagStream.HasTag())
        {
            var tag = tagStream.NextTag();
            tagProcessor.ProcessTag(tag);
        }

        ResponseHelpers.PopulateMessageResponseList(tagProcessor.MessageResponseEntryList, package);
    }

    public override FrontendChunkType GetChunkType()
    {
        return FrontendChunkType.PackageResponses;
    }
}