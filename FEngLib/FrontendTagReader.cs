using System.IO;

namespace FEngLib
{
    public class FrontendTagReader
    {
        public BinaryReader Reader { get; }

        public FrontendTagReader(BinaryReader reader)
        {
            Reader = reader;
        }

        //public IEnumerable<FrontendTag> ReadScriptTags(FrontendObject frontendObject, FrontendScript frontendScript, long length)
        //{
        //    var endPos = Reader.BaseStream.Position + length;

        //    while (Reader.BaseStream.Position < endPos)
        //    {
        //        var (id, size) = (Reader.ReadUInt16(), Reader.ReadUInt16());
        //        var pos = Reader.BaseStream.Position;
        //        FrontendScriptTag tag = id switch
        //        {
        //            0x6853 => new ScriptHeaderTag(frontendObject, frontendScript),
        //            _ => throw new ChunkReadingException($"Unrecognized tag: 0x{id:X4}")
        //        };

        //        tag.Read(Reader, size);

        //        if (Reader.BaseStream.Position - pos != size)
        //        {
        //            throw new ChunkReadingException($"Expected {size} bytes to be read by {tag.GetType()} but {Reader.BaseStream.Position - pos} bytes were read");
        //        }

        //        yield return tag;
        //    }
        //}
    }
}