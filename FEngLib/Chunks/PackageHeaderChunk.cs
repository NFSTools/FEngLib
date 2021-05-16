using System.IO;
using FEngLib.Packages;
using FEngLib.Utils;

namespace FEngLib.Chunks
{
    public class PackageHeaderChunk : FrontendChunk
    {
        /// <summary>
        ///     The number of entries in the RsNm chunk
        /// </summary>
        public int NumResourceNames { get; set; }

        /// <summary>
        ///     The name of the package
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     The filename of the package
        /// </summary>
        public string Filename { get; set; }

        public override void Read(Package package, FrontendChunkBlock chunkBlock,
            FrontendChunkReader chunkReader, BinaryReader reader)
        {
            if (reader.ReadUInt32() != 0x20000) throw new InvalidDataException("Invalid header constant");

            if (reader.ReadUInt32() != 0) throw new InvalidDataException("Expected null after constant");

            NumResourceNames = reader.ReadInt32();

            reader.ReadUInt32();
            //if (reader.ReadUInt32() != 3)
            //{
            //    throw new InvalidDataException("Invalid header version");
            //}

            var nameLength = reader.ReadInt32();

            if (nameLength < 1) throw new InvalidDataException("Invalid name length");

            var filenameLength = reader.ReadInt32();

            if (filenameLength < 1) throw new InvalidDataException("Invalid filename length");

            Name = new string(reader.ReadChars(nameLength)).Trim('\0');
            Filename = new string(reader.ReadChars(filenameLength)).Trim('\0');

            reader.AlignReader(4);
        }

        public override FrontendChunkType GetChunkType()
        {
            return FrontendChunkType.PackageHeader;
        }
    }
}