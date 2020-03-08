namespace FEngLib
{
    public abstract class FrontendScriptTag : FrontendTag
    {
        public FrontendScript FrontendScript { get; }

        protected FrontendScriptTag(FrontendObject frontendObject, FrontendScript frontendScript) : base(frontendObject)
        {
            FrontendScript = frontendScript;
        }
    }
}