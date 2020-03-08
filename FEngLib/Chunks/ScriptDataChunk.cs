using System.Diagnostics;
using System.IO;
using FEngLib.Tags;

namespace FEngLib.Chunks
{
    public class ScriptDataChunk : FrontendObjectChunk
    {
        public override FrontendObject Read(FrontendPackage package, ObjectReaderState readerState, BinaryReader reader)
        {
            FrontendScriptTagStream tagStream = new FrontendScriptTagStream(reader, readerState.CurrentChunkBlock, readerState.CurrentChunkBlock.Size);
            FrontendScript script = new FrontendScript();

            while (tagStream.HasTag())
            {
                FrontendTag tag = tagStream.NextTag(FrontendObject, script);
                Debug.WriteLine("SCRIPT TAG {0}", tag);
                ProcessTag(FrontendObject, script, tag);
            }

            Debug.WriteLine("ADD SCRIPT: id={0:X8}", script.Id);
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

        private void ProcessScriptHeaderTag(FrontendObject frontendObject, FrontendScript frontendScript, ScriptHeaderTag scriptHeaderTag)
        {
            frontendScript.Id = scriptHeaderTag.Id;
            frontendScript.Flags = scriptHeaderTag.Flags;
            frontendScript.Length = scriptHeaderTag.Length;
        }

        public ScriptDataChunk(FrontendObject frontendObject) : base(frontendObject)
        {
        }
    }
}