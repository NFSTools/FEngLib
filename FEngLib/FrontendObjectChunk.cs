using System.IO;

namespace FEngLib
{
    public abstract class FrontendObjectChunk : FrontendChunk
    {
        public FrontendObject FrontendObject { get; }

        protected FrontendObjectChunk(FrontendObject frontendObject)
        {
            FrontendObject = frontendObject;
        }
    }
}