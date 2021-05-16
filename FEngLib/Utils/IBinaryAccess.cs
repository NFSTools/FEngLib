using System.IO;

namespace FEngLib.Utils
{
    public interface IBinaryAccess
    {
        void Read(BinaryReader br);
        void Write(BinaryWriter bw);
    }
}