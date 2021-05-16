using FEngLib.Object;
using FEngLib.Objects;
using FEngLib.Tags;

namespace FEngLib.Scripts
{
    public abstract class ScriptTag : Tag
    {
        protected Script Script { get; }

        protected ScriptTag(IObject<ObjectData> frontendObject, Script script) : base(frontendObject)
        {
            Script = script;
        }
    }
}