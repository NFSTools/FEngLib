using System.IO;
using System.Numerics;
using FEngLib.Structures;

namespace FEngLib.Data
{
    public class FEImageData : FEObjData
    {
        public Vector2 UpperLeft { get; set; }
        public Vector2 LowerRight { get; set; }

        public override void Read(BinaryReader br)
        {
            base.Read(br);

            UpperLeft = new Vector2(br.ReadSingle(), br.ReadSingle());
            LowerRight = new Vector2(br.ReadSingle(), br.ReadSingle());
        }

        public override void Write(BinaryWriter bw)
        {
            base.Write(bw);

            bw.Write(UpperLeft.X);
            bw.Write(UpperLeft.Y);
            bw.Write(LowerRight.X);
            bw.Write(LowerRight.Y);
        }
    }
}