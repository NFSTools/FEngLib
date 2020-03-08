using System.Diagnostics;
using System.IO;

namespace FEngLib.Tags
{
    public class ObjectParentTag : FrontendTag
    {
        public uint ParentId { get; set; }

        public ObjectParentTag(FrontendObject frontendObject) : base(frontendObject)
        {
        }

        public override void Read(BinaryReader br, ushort length)
        {
            ParentId = br.ReadUInt32();
            Debug.WriteLine("FEObject {0:X8} has parent {1:X8}", FrontendObject.NameHash, ParentId);
        }
    }
}