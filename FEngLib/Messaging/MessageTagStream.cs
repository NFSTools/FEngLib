using System.IO;
using FEngLib.Chunks;
using FEngLib.Messaging.Tags;
using FEngLib.Packages.Tags;
using FEngLib.Tags;
using static FEngLib.FrontendTagType;

namespace FEngLib.Messaging;

public class MessageTagStream : TagStream
{
    public MessageTagStream(BinaryReader reader, long length) :
        base(reader, length)
    {
    }

    public override Tag NextTag()
    {
        var (id, size) = (Reader.ReadUInt16(), Reader.ReadUInt16());
        var pos = Reader.BaseStream.Position;
        Tag tag = (FrontendTagType)id switch
        {
            MessageResponseInfo => new MessageResponseInfoTag(),
            MessageResponseCount => new MessageResponseCountTag(),
            ResponseId => new ResponseIdTag(),
            ResponseIntParam => new ResponseIntParamTag(),
            ResponseStringParam => new ResponseStringParamTag(),
            ResponseTarget => new ResponseTargetTag(),
            MessageTargetCount => new MessageTargetCountTag(),
            MessageTargetList => new MessageTargetListTag(),
            _ => throw new ChunkReadingException($"Unrecognized tag: 0x{id:X4}")
        };

        tag.Read(Reader, id, size);

        if (Reader.BaseStream.Position - pos != size)
            throw new ChunkReadingException(
                $"Expected {size} bytes to be read by {tag.GetType()} but {Reader.BaseStream.Position - pos} bytes were read");

        return tag;
    }
}