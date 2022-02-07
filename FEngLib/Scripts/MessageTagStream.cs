using System.IO;
using FEngLib.Chunks;
using FEngLib.Objects;
using FEngLib.Packages;
using FEngLib.Packages.Tags;
using FEngLib.Scripts.Tags;
using FEngLib.Tags;
using static FEngLib.FrontendTagType;

namespace FEngLib.Scripts;

public class MessageTagStream : TagStream
{
    public MessageTagStream(BinaryReader reader, Package package,
        FrontendChunkBlock frontendChunkBlock, long length) :
        base(reader, frontendChunkBlock, length)
    {
        Package = package;
    }

    public Package Package { get; }

    public override Tag NextTag(IObject<ObjectData> frontendObject)
    {
        var (id, size) = (Reader.ReadUInt16(), Reader.ReadUInt16());
        var pos = Reader.BaseStream.Position;
        Tag tag = (FrontendTagType)id switch
        {
            MessageResponseInfo => new MessageResponseInfoTag(frontendObject),
            MessageResponseCount => new MessageResponseCountTag(frontendObject),
            ResponseId => new ResponseIdTag(frontendObject),
            ResponseIntParam => new ResponseIntParamTag(frontendObject),
            ResponseStringParam => new ResponseStringParamTag(frontendObject),
            ResponseTarget => new ResponseTargetTag(frontendObject),
            MessageTargetCount => new MessageTargetCountTag(frontendObject),
            FrontendTagType.MessageTargetList => new MessageTargetListTag(frontendObject),
            _ => throw new ChunkReadingException($"Unrecognized tag: 0x{id:X4}")
        };

        tag.Read(Reader, FrontendChunkBlock, Package, id, size);

        if (Reader.BaseStream.Position - pos != size)
            throw new ChunkReadingException(
                $"Expected {size} bytes to be read by {tag.GetType()} but {Reader.BaseStream.Position - pos} bytes were read");

        return tag;
    }
}