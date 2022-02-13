using System.IO;
using FEngLib.Chunks;
using FEngLib.Objects;
using FEngLib.Scripts.Tags;
using FEngLib.Tags;
using static FEngLib.FrontendTagType;

namespace FEngLib.Scripts;

public class ScriptTagStream : TagStream
{
    public ScriptTagStream(BinaryReader reader, long length, IObject<ObjectData> frontendObject,
        ScriptProcessingContext scriptProcessingContext) : base(
        reader, length)
    {
        FrontendObject = frontendObject;
        ScriptProcessingContext = scriptProcessingContext;
    }

    private IObject<ObjectData> FrontendObject { get; }
    private ScriptProcessingContext ScriptProcessingContext { get; }

    public override Tag NextTag()
    {
        var (id, size) = (Reader.ReadUInt16(), Reader.ReadUInt16());
        var pos = Reader.BaseStream.Position;
        Tag tag = (FrontendTagType)id switch
        {
            ScriptHeader => new ScriptHeaderTag(FrontendObject, ScriptProcessingContext),
            ScriptChain => new ScriptChainTag(FrontendObject, ScriptProcessingContext),
            ScriptKeyTrack => new ScriptKeyTrackTag(FrontendObject, ScriptProcessingContext),
            ScriptTrackOffset => new ScriptTrackOffsetTag(FrontendObject, ScriptProcessingContext),
            ScriptKeyNode => new ScriptKeyNodeTag(FrontendObject, ScriptProcessingContext),
            ScriptEvents => new ScriptEventsTag(FrontendObject, ScriptProcessingContext),
            ScriptName => new ScriptNameTag(FrontendObject, ScriptProcessingContext),
            _ => throw new ChunkReadingException($"Unrecognized tag: 0x{id:X4}")
        };

        tag.Read(Reader, id, size);

        if (Reader.BaseStream.Position - pos != size)
            throw new ChunkReadingException(
                $"Expected {size} bytes to be read by {tag.GetType()} but {Reader.BaseStream.Position - pos} bytes were read");

        return tag;
    }
}