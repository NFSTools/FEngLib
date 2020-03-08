using System.Diagnostics;
using System.IO;

namespace FEngLib.Tags
{
    public class ImageInfoTag : FrontendTag
    {
        public uint ImageFlags { get; set; }

        public override void Read(BinaryReader br, FrontendChunkBlock chunkBlock, FrontendPackage package,
            ushort id,
            ushort length)
        {
            ImageFlags = br.ReadUInt32();
            //Debug.WriteLine("ImageFlags for {2}[{1:X8}]: {0}", ImageFlags, FrontendObject.NameHash, package.Name);
        }

        public ImageInfoTag(FrontendObject frontendObject) : base(frontendObject)
        {
        }
    }
}