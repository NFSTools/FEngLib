using System.IO;
using FEngLib.Objects;
using FEngLib.Packages;
using FEngLib.Scripts;
using FEngLib.Scripts.Tags;
using FEngLib.Tags;

namespace FEngLib.Chunks;

public class ScriptDataChunk : FrontendObjectChunk
{
    public ScriptDataChunk(IObject<ObjectData> frontendObject) : base(frontendObject)
    {
    }

    public override IObject<ObjectData> Read(Package package, ObjectReaderState readerState, BinaryReader reader)
    {
        var ctx = new ScriptProcessingContext(FrontendObject.CreateScript());
        var tagStream = new ScriptTagStream(reader, readerState.CurrentChunkBlock,
            readerState.CurrentChunkBlock.Size);

        while (tagStream.HasTag())
        {
            var tag = tagStream.NextTag(FrontendObject, ctx);
            ProcessTag(ctx.Script, tag);
        }

        // FrontendObject.Scripts.Add(script);

        return FrontendObject;
    }

    public override FrontendChunkType GetChunkType()
    {
        return FrontendChunkType.ScriptData;
    }

    private void ProcessTag(Script script, Tag tag)
    {
        switch (tag)
        {
            case ScriptHeaderTag scriptHeaderTag:
                ProcessScriptHeaderTag(script, scriptHeaderTag);
                break;
            case ScriptNameTag scriptNameTag:
                script.Name = scriptNameTag.Name;
                script.Id = scriptNameTag.NameHash;
                break;
            case ScriptChainTag scriptChainTag:
                script.ChainedId = scriptChainTag.Id;
                break;
        }
    }

    private void ProcessScriptHeaderTag(Script script,
        ScriptHeaderTag scriptHeaderTag)
    {
        script.Id = scriptHeaderTag.Id;
        script.Flags = scriptHeaderTag.Flags;
        script.Length = scriptHeaderTag.Length;
    }
}