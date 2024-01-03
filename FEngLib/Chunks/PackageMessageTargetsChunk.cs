using System.IO;
using FEngLib.Messaging;
using FEngLib.Packages;
using FEngLib.Packages.Tags;
using FEngLib.Tags;

namespace FEngLib.Chunks;

public class PackageMessageTargetsChunk : FrontendChunk
{
    public override void Read(Package package, FrontendChunkBlock chunkBlock,
        FrontendChunkReader chunkReader, BinaryReader reader)
    {
        var tagProcessor = new MessageResponseTagProcessor<Package>();
        TagStream tagStream = new MessageTagStream(reader,
            chunkBlock.Size);

        while (tagStream.HasTag())
        {
            var tag = tagStream.NextTag();
            ProcessTag(tagProcessor, package, tag);
        }
    }

    private void ProcessTag(MessageResponseTagProcessor<Package> messageResponseTagProcessor, Package package, Tag tag)
    {
        switch (tag)
        {
            // These are not really important to us. They are also trivial to regenerate.
            case MessageTargetCountTag:
            case MessageTargetListTag:
                break;
            default:
                messageResponseTagProcessor.ProcessTag(package, tag);
                break;
        }
    }

    public override FrontendChunkType GetChunkType()
    {
        return FrontendChunkType.PackageResponses;
    }
}