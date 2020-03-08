using System.IO;
using FEngLib.Data;

namespace FEngLib.Tags
{
    public class ObjectDataTag : FrontendTag
    {
        public FEObjData Data { get; set; }

        public override void Read(BinaryReader br)
        {
            Data = FrontendObject.Type switch
            {
                _ => new FEObjData()
            };

            Data.Read(br);
        }

        public ObjectDataTag(FrontendObject frontendObject) : base(frontendObject)
        {
        }
    }
}