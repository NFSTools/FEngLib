using System.IO;
using CoreLibraries.IO;

namespace FEngLib.Structures
{
    public class FEVector2 : IBinaryAccess
    {
        public float X { get; set; }
        public float Y { get; set; }

        public void Read(BinaryReader br)
        {
            X = br.ReadSingle();
            Y = br.ReadSingle();
        }

        public void Write(BinaryWriter bw)
        {
            bw.Write(X);
            bw.Write(Y);
        }
    }
}