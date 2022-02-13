using FEngLib.Objects;

namespace FEngLib.Scripts;

public abstract class ScriptTag : ObjectTag
{
    protected ScriptTag(IObject<ObjectData> frontendObject, ScriptProcessingContext scriptProcessingContext) : base(
        frontendObject)
    {
        ScriptProcessingContext = scriptProcessingContext;
    }

    protected Script Script => ScriptProcessingContext.Script;
    protected ScriptProcessingContext ScriptProcessingContext { get; }
}