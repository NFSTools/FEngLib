using FEngLib.Structures;

namespace FEngLib.Objects
{
    public class FrontendColoredImage : FrontendImage
    {
        public FEColor[] VertexColors { get; set; }

        public FrontendColoredImage()
        {
            VertexColors = new FEColor[4];
        }

        public FrontendColoredImage(FrontendObject original) : base(original)
        {
            VertexColors = new FEColor[4];
        }

        public FrontendColoredImage(FrontendColoredImage original) : this(original as FrontendObject)
        {
            VertexColors = original.VertexColors;
        }
    }
}