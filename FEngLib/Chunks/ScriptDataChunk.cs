using System.IO;
using FEngLib.Tags;

namespace FEngLib.Chunks
{
    public class ScriptDataChunk : FrontendObjectChunk
    {
        public ScriptDataChunk(FrontendObject frontendObject) : base(frontendObject)
        {
        }

        public override FrontendObject Read(FrontendPackage package, ObjectReaderState readerState, BinaryReader reader)
        {
            var tagStream = new FrontendScriptTagStream(reader, readerState.CurrentChunkBlock,
                readerState.CurrentChunkBlock.Size);
            var script = new FrontendScript();

            while (tagStream.HasTag())
            {
                var tag = tagStream.NextTag(FrontendObject, script);
                ProcessTag(FrontendObject, script, tag);
            }

            FrontendObject.Scripts.Add(script);

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

        private void ProcessScriptHeaderTag(FrontendObject frontendObject, FrontendScript frontendScript,
            ScriptHeaderTag scriptHeaderTag)
        {
            frontendScript.Id = scriptHeaderTag.Id;
            frontendScript.Flags = scriptHeaderTag.Flags;
            frontendScript.Length = scriptHeaderTag.Length;
        }
    }
}