using System;
using System.IO;
using FEngLib.Chunks;
using FEngLib.Object;
using FEngLib.Objects;
using FEngLib.Scripts.Tags;
using FEngLib.Tags;

namespace FEngLib.Scripts
{
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

        public Tag NextTag(IObject<ObjectData> frontendObject, Script script)
        {
            var (id, size) = (Reader.ReadUInt16(), Reader.ReadUInt16());
            var pos = Reader.BaseStream.Position;
            Tag tag = id switch
            {
                0x6853 => new ScriptHeaderTag(frontendObject, script),
                0x6353 => new ScriptChainTag(frontendObject, script),
                0x4946 => new ScriptKeyTrackTag(frontendObject, script),
                0x6F54 => new ScriptTrackOffsetTag(frontendObject, script),
                0x644B => new ScriptKeyNodeTag(frontendObject, script),
                0x5645 => new ScriptEventsTag(frontendObject, script),
                0x6E53 => new ScriptNameTag(frontendObject, script),
                _ => throw new ChunkReadingException($"Unrecognized tag: 0x{id:X4}")
            };

            tag.Read(Reader, FrontendChunkBlock, null, id, size);

            if (Reader.BaseStream.Position - pos != size)
                throw new ChunkReadingException(
                    $"Expected {size} bytes to be read by {tag.GetType()} but {Reader.BaseStream.Position - pos} bytes were read");

            return tag;
        }
    }
}