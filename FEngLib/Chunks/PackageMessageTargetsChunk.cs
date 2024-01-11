using System;
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
        TagStream tagStream = new MessageTagStream(reader,
            chunkBlock.Size);

        while (tagStream.HasTag())
        {
            var tag = tagStream.NextTag();
            ProcessTag(tag);
        }
    }

    private void ProcessTag(Tag tag)
    {
        switch (tag)
        {
            // These are not really important to us. They are also trivial to regenerate.
            case MessageTargetCountTag:
            case MessageTargetListTag:
                break;
            default:
                throw new Exception($"Unexpected tag in MessageTargets: {tag.GetType()}");
        }
    }

    public override FrontendChunkType GetChunkType()
    {
        return FrontendChunkType.Targets;
    }
}