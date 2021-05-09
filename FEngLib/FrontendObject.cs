using System.Collections.Generic;
using System.Numerics;
using FEngLib.Data;
using FEngLib.Structures;

namespace FEngLib
{
    public class FrontendObject
    {
        public FrontendObject()
        {
            Scripts = new List<FrontendScript>();
            MessageResponses = new List<FEMessageResponse>();
        }

        public FrontendObject(FrontendObject original) : this()
        {
            Type = original.Type;
            NameHash = original.NameHash;
            Flags = original.Flags;
            Package = original.Package;
            Name = original.Name;
        }

        public FEObjType Type { get; set; }
        public FE_ObjectFlags Flags { get; set; }

        public FEResourceRequest ResourceRequest { get; set; }
        public string Name { get; set; }
        public uint NameHash { get; set; }
        public uint Guid { get; set; }
        public FrontendObject Parent { get; set; }
        public List<FrontendScript> Scripts { get; set; }
        public List<FEMessageResponse> MessageResponses { get; set; }
        public FrontendPackage Package { get; set; }

        public FEColor Color { get; set; }
        public Vector3 Pivot { get; set; }
        public Vector3 Position { get; set; }
        public Quaternion Rotation { get; set; }
        public Vector3 Size { get; set; }
    }
}