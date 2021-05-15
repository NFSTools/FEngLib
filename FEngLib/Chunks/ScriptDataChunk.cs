using System.IO;
using FEngLib.Object;
using FEngLib.Tags;

namespace FEngLib.Chunks
{
    public class ScriptDataChunk : FrontendObjectChunk
    {
        public ScriptDataChunk(IObject<ObjectData> frontendObject) : base(frontendObject)
        {
        }

        public override IObject<ObjectData> Read(FrontendPackage package, ObjectReaderState readerState, BinaryReader reader)
        {
            var tagStream = new FrontendScriptTagStream(reader, readerState.CurrentChunkBlock,
                readerState.CurrentChunkBlock.Size);
            var script = new FrontendScript();

            while (tagStream.HasTag())
            {
                var tag = tagStream.NextTag(FrontendObject, script);
                ProcessTag(script, tag);
            }

            FrontendObject.Scripts.Add(script);

            return FrontendObject;
        }

        public override FrontendChunkType GetChunkType()
        {
            return FrontendChunkType.ScriptData;
        }

        private void ProcessTag(FrontendScript frontendScript, FrontendTag tag)
        {
            switch (tag)
            {
                case ScriptHeaderTag scriptHeaderTag:
                    ProcessScriptHeaderTag(frontendScript, scriptHeaderTag);
                    break;
                case ScriptNameTag scriptNameTag:
                    frontendScript.Name = scriptNameTag.Name;
                    frontendScript.Id = scriptNameTag.NameHash;
                    break;
                case ScriptChainTag scriptChainTag:
                    frontendScript.ChainedId = scriptChainTag.Id;
                    break;
            }
        }

        private void ProcessScriptHeaderTag(FrontendScript frontendScript,
            ScriptHeaderTag scriptHeaderTag)
        {
            frontendScript.Id = scriptHeaderTag.Id;
            frontendScript.Flags = scriptHeaderTag.Flags;
            frontendScript.Length = scriptHeaderTag.Length;
        }
    }
}