using FEngLib.Object;

namespace FEngLib
{
    public abstract class FrontendScriptTag : FrontendTag
    {
        public FrontendScript FrontendScript { get; }

        protected FrontendScriptTag(IObject<ObjectData> frontendObject, FrontendScript frontendScript) : base(frontendObject)
        {
            FrontendScript = frontendScript;
        }
    }
}