using System.IO;
using CoreLibraries.IO;

namespace FEngLib.Structures
{
    public class FEColor : IBinaryAccess
    {
        public int Blue { get; set; }
        public int Green { get; set; }
        public int Red { get; set; }
        public int Alpha { get; set; }
        
        public void Read(BinaryReader br)
        {
            Blue = br.ReadInt32();
            Green = br.ReadInt32();
            Red = br.ReadInt32();
            Alpha = br.ReadInt32();
        }

        public void Write(BinaryWriter bw)
        {
            bw.Write(Blue);
            bw.Write(Green);
            bw.Write(Red);
            bw.Write(Alpha);
        }
    }
}