using System.IO;
using System.Text;

namespace FEngLib.Utils
{
    public static class NullTerminatedString
    {
        public static string Read(BinaryReader br)
        {
            return Read(br.BaseStream);
        }

        public static string Read(Stream stream)
        {
            var sb = new StringBuilder();
            byte b;
            do
            {
                var i = stream.ReadByte();

                if (i == -1) break;

                b = (byte) i;

                //b = br.ReadByte();
                if (b != 0) sb.Append((char) b);
            } while (b != 0);

            return sb.ToString();
        }

        public static void Write(BinaryWriter bw, string value)
        {
            var str = Encoding.ASCII.GetBytes(value);
            bw.Write(str);
            bw.Write((byte) 0);
        }
    }
}