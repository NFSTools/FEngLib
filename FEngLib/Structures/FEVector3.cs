using System.IO;
using CoreLibraries.IO;

namespace FEngLib.Structures
{
    public class FEVector3 : IBinaryAccess
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public void Read(BinaryReader br)
        {
            X = br.ReadSingle();
            Y = br.ReadSingle();
            Z = br.ReadSingle();
        }

        public void Write(BinaryWriter bw)
        {
            bw.Write(X);
            bw.Write(Y);
            bw.Write(Z);
        }

        public override string ToString()
        {
            return $"({X}, {Y}, {Z})";
        }
    }
}