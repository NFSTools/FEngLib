using System.Collections.Generic;
using FEngLib.Data;

namespace FEngLib
{
    public class FrontendObject
    {
        public FEObjType Type { get; set; }
        public FE_ObjectFlags Flags { get; set; }
        public uint NameHash { get; set; }
        public List<FrontendScript> Scripts { get; set; }

        public FrontendObject()
        {
            Scripts = new List<FrontendScript>();
        }

        public FrontendObject(FrontendObject original) : this()
        {
            Type = original.Type;
            NameHash = original.NameHash;
            Flags = original.Flags;
        }
    }
}