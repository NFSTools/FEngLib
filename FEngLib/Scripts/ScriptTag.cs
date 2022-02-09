using FEngLib.Objects;
using FEngLib.Tags;

namespace FEngLib.Scripts;

public abstract class ScriptTag : Tag
{
    protected ScriptTag(IObject<ObjectData> frontendObject, ScriptProcessingContext scriptProcessingContext) : base(
        frontendObject)
    {
        ScriptProcessingContext = scriptProcessingContext;
    }

    protected Script Script => ScriptProcessingContext.Script;
    protected ScriptProcessingContext ScriptProcessingContext { get; }
}