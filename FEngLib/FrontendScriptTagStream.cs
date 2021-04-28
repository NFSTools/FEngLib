using System;
using System.IO;
using FEngLib.Tags;

namespace FEngLib
{
    public class FrontendScriptTagStream : FrontendTagStream
    {
        public FrontendScriptTagStream(BinaryReader reader, FrontendChunkBlock frontendChunkBlock, long length) : base(
            reader, frontendChunkBlock, length)
        {
        }

        public override FrontendTag NextTag(FrontendObject frontendObject)
        {
            throw new NotImplementedException("Use NextTag(FrontendObject, FrontendScript) instead");
        }

        public FrontendTag NextTag(FrontendObject frontendObject, FrontendScript frontendScript)
        {
            var (id, size) = (Reader.ReadUInt16(), Reader.ReadUInt16());
            var pos = Reader.BaseStream.Position;
            FrontendTag tag = id switch
            {
                0x6853 => new ScriptHeaderTag(frontendObject, frontendScript),
                0x6353 => new ScriptChainTag(frontendObject, frontendScript),
                0x4946 => new ScriptKeyTrackTag(frontendObject, frontendScript),
                0x6F54 => new ScriptTrackOffsetTag(frontendObject, frontendScript),
                0x644B => new ScriptKeyNodeTag(frontendObject, frontendScript),
                0x5645 => new ScriptEventsTag(frontendObject, frontendScript),
                0x6E53 => new ScriptNameTag(frontendObject, frontendScript),
                _ => throw new ChunkReadingException($"Unrecognized tag: 0x{id:X4}")
            };

            tag.Read(Reader, FrontendChunkBlock, frontendObject.Package, id, size);

            if (Reader.BaseStream.Position - pos != size)
                throw new ChunkReadingException(
                    $"Expected {size} bytes to be read by {tag.GetType()} but {Reader.BaseStream.Position - pos} bytes were read");

            return tag;
        }
    }
}