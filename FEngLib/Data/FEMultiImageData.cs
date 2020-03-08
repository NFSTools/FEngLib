using System.IO;
using FEngLib.Structures;

namespace FEngLib.Data
{
    public class FEMultiImageData : FEImageData
    {
        public FEVector2[] TopLeftUV { get; set; }
        public FEVector2[] BottomRightUV { get; set; }
        public FEVector3 PivotRotation { get; set; }

        public override void Read(BinaryReader br)
        {
            base.Read(br);

            TopLeftUV = new FEVector2[3];
            BottomRightUV = new FEVector2[3];
            PivotRotation = new FEVector3();

            for (int i = 0; i < TopLeftUV.Length; i++)
            {
                TopLeftUV[i] = new FEVector2();
                TopLeftUV[i].Read(br);
            }

            for (int i = 0; i < BottomRightUV.Length; i++)
            {
                BottomRightUV[i] = new FEVector2();
                BottomRightUV[i].Read(br);
            }

            PivotRotation.Read(br);
        }

        public override void Write(BinaryWriter bw)
        {
            base.Write(bw);

            foreach (var t in TopLeftUV)
            {
                t.Write(bw);
            }

            foreach (var t in BottomRightUV)
            {
                t.Write(bw);
            }

            PivotRotation.Write(bw);
        }
    }
}