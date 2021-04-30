using System.IO;

namespace FEngLib
{
    public interface IBinaryAccess
    {
        void Read(BinaryReader br);
        void Write(BinaryWriter bw);
    }
}