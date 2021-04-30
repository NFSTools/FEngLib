using System.IO;

namespace FEngLib.Structures
{
    public class FEQuaternion : IBinaryAccess
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public float W { get; set; }

        public void Read(BinaryReader br)
        {
            X = br.ReadSingle();
            Y = br.ReadSingle();
            Z = br.ReadSingle();
            W = br.ReadSingle();
        }

        public void Write(BinaryWriter bw)
        {
            bw.Write(X);
            bw.Write(Y);
            bw.Write(Z);
            bw.Write(W);
        }

        public override string ToString()
        {
            return $"({X}, {Y}, {Z}) [{W}]";
        }
    }
}