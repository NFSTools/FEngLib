using System.IO;
using System.Numerics;
using FEngLib.Structures;

namespace FEngLib.Data
{
    public class FEMultiImageData : FEImageData
    {
        public Vector2[] TopLeftUV { get; set; }
        public Vector2[] BottomRightUV { get; set; }
        public Vector3 PivotRotation { get; set; }

        public override void Read(BinaryReader br)
        {
            base.Read(br);

            TopLeftUV = new Vector2[3];
            BottomRightUV = new Vector2[3];
            PivotRotation = new Vector3();

            for (int i = 0; i < TopLeftUV.Length; i++)
            {
                TopLeftUV[i] = new Vector2(br.ReadSingle(), br.ReadSingle());
            }

            for (int i = 0; i < BottomRightUV.Length; i++)
            {
                BottomRightUV[i] = new Vector2(br.ReadSingle(), br.ReadSingle());
            }

            PivotRotation = new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
        }

        public override void Write(BinaryWriter bw)
        {
            base.Write(bw);

            foreach (var t in TopLeftUV)
            {
                bw.Write(t.X);
                bw.Write(t.Y);
            }

            foreach (var t in BottomRightUV)
            {
                bw.Write(t.X);
                bw.Write(t.Y);
            }

            bw.Write(PivotRotation.X);
            bw.Write(PivotRotation.Y);
            bw.Write(PivotRotation.Z);
        }
    }
}