using System.IO;
using FEngLib.Structures;

namespace FEngLib.Data
{
    public class FEImageData : FEObjData
    {
        public FEVector2 UpperLeft { get; set; }
        public FEVector2 LowerRight { get; set; }

        public override void Read(BinaryReader br)
        {
            base.Read(br);

            UpperLeft = new FEVector2();
            LowerRight = new FEVector2();
            UpperLeft.Read(br);
            LowerRight.Read(br);
        }

        public override void Write(BinaryWriter bw)
        {
            base.Write(bw);

            UpperLeft.Write(bw);
            LowerRight.Write(bw);
        }
    }
}