using System.IO;
using FEngLib.Structures;

namespace FEngLib.Data
{
    public class FEColoredImageData : FEImageData
    {
        public FEColor[] VertexColors { get; set; }

        public override void Read(BinaryReader br)
        {
            base.Read(br);

            VertexColors = new FEColor[4];

            for (int i = 0; i < 4; i++)
            {
                VertexColors[i] = new FEColor();
                VertexColors[i].Read(br);
            }
        }

        public override void Write(BinaryWriter bw)
        {
            base.Write(bw);

            foreach (var vertexColor in VertexColors)
            {
                vertexColor.Write(bw);
            }
        }
    }
}