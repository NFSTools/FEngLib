namespace FEngLib.Objects
{
    public class FrontendImage : FrontendObject
    {
        public FrontendImage()
        {
        }

        public FrontendImage(FrontendObject original) : base(original)
        {
        }
        public FrontendImage(FrontendImage original) : this(original as FrontendObject)
        {
        }
    }
}