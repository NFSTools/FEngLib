using System.IO;
using FEngLib.Data;
using FEngLib.Object;

namespace FEngLib.Tags
{
    public class ObjectDataTag : FrontendTag
    {
        public ObjectData Data { get; set; }

        public override void Read(BinaryReader br, FrontendChunkBlock chunkBlock, FrontendPackage package,
            ushort id,
            ushort length)
        {
            Data = FrontendObject.Type switch
            {
                ObjectType.Image => new ImageData(),
                ObjectType.MultiImage => new MultiImageData(),
                ObjectType.ColoredImage => new ColoredImageData(),
                _ => new ObjectData()
            };

            Data.Read(br);
        }

        public ObjectDataTag(IObject<ObjectData> frontendObject) : base(frontendObject)
        {
        }
    }
}