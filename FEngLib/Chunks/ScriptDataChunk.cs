using System.IO;
using FEngLib.Tags;

namespace FEngLib.Chunks
{
    public class ScriptDataChunk : FrontendObjectChunk
    {
        public override FrontendObject Read(FrontendPackage package, FrontendChunkBlock chunkBlock, FrontendChunkReader chunkReader, BinaryReader reader)
        {
            FrontendTagReader tagReader = new FrontendTagReader(reader);
            FrontendScript script = new FrontendScript();

            foreach (var tag in tagReader.ReadScriptTags(FrontendObject, script, chunkBlock.Size))
            {
                this.ProcessTag(FrontendObject, script, tag);
            }

            return FrontendObject;
        }

        public override FrontendChunkType GetChunkType()
        {
            return FrontendChunkType.ScriptData;
        }

        private void ProcessTag(FrontendObject frontendObject, FrontendScript frontendScript, FrontendTag tag)
        {
            switch (tag)
            {
                case ScriptHeaderTag scriptHeaderTag:
                    ProcessScriptHeaderTag(frontendObject, frontendScript, scriptHeaderTag);
                    break;
            }
        }

        private void ProcessScriptHeaderTag(FrontendObject frontendObject, FrontendScript frontendScript, ScriptHeaderTag scriptHeaderTag)
        {
            frontendScript.Id = scriptHeaderTag.Id;
            frontendScript.TrackCount = scriptHeaderTag.TrackCount;
            frontendScript.Flags = scriptHeaderTag.Flags;
            frontendScript.Length = scriptHeaderTag.Length;
        }

        public ScriptDataChunk(FrontendObject frontendObject) : base(frontendObject)
        {
        }
    }
}