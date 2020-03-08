using FEngLib.Data;

namespace FEngLib
{
    public class FrontendObject
    {
        public FEObjType Type { get; set; }
        public FE_ObjectFlags Flags { get; set; }
        public uint NameHash { get; set; }

        public FrontendObject() { }

        public FrontendObject(FrontendObject original)
        {
            Type = original.Type;
            NameHash = original.NameHash;
            Flags = original.Flags;
        }
    }
}