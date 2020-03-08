namespace FEngLib.Objects
{
    public class FrontendImage : FrontendObject
    {
        public uint ImageFlags { get; set; }

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