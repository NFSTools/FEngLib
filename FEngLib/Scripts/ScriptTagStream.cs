using System;
using System.IO;
using FEngLib.Chunks;
using FEngLib.Objects;
using FEngLib.Scripts.Tags;
using FEngLib.Tags;
using static FEngLib.FrontendTagType;

namespace FEngLib.Scripts;

public class ScriptTagStream : TagStream
{
    public ScriptTagStream(BinaryReader reader, FrontendChunkBlock frontendChunkBlock, long length) : base(
        reader, frontendChunkBlock, length)
    {
    }

    public override Tag NextTag(IObject<ObjectData> frontendObject)
    {
        throw new NotImplementedException("Use NextTag(FrontendObject, Script) instead");
    }

    public Tag NextTag(IObject<ObjectData> frontendObject, ScriptProcessingContext scriptProcessingContext)
    {
        var (id, size) = (Reader.ReadUInt16(), Reader.ReadUInt16());
        var pos = Reader.BaseStream.Position;
        Tag tag = (FrontendTagType)id switch
        {
            ScriptHeader => new ScriptHeaderTag(frontendObject, scriptProcessingContext),
            ScriptChain => new ScriptChainTag(frontendObject, scriptProcessingContext),
            ScriptKeyTrack => new ScriptKeyTrackTag(frontendObject, scriptProcessingContext),
            ScriptTrackOffset => new ScriptTrackOffsetTag(frontendObject, scriptProcessingContext),
            ScriptKeyNode => new ScriptKeyNodeTag(frontendObject, scriptProcessingContext),
            ScriptEvents => new ScriptEventsTag(frontendObject, scriptProcessingContext),
            ScriptName => new ScriptNameTag(frontendObject, scriptProcessingContext),
            _ => throw new ChunkReadingException($"Unrecognized tag: 0x{id:X4}")
        };

        tag.Read(Reader, FrontendChunkBlock, null, id, size);

        if (Reader.BaseStream.Position - pos != size)
            throw new ChunkReadingException(
                $"Expected {size} bytes to be read by {tag.GetType()} but {Reader.BaseStream.Position - pos} bytes were read");

        return tag;
    }
}