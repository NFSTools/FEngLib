using System.Collections.Generic;
using FEngLib.Data;

namespace FEngLib
{
    /// <summary>
    ///     Stores frontend objects, resource names, etc.
    /// </summary>
    public class FrontendPackage
    {
        public FrontendPackage()
        {
            Objects = new List<FrontendObject>();
            MessageDefinitions = new List<MessageDefinition>();
            MessageResponses = new List<FEMessageResponse>();
            MessageTargetLists = new List<FEMessageTargetList>();
            ResourceRequests = new List<FEResourceRequest>();
        }

        public string Name { get; set; }
        public string Filename { get; set; }
        public List<FrontendObject> Objects { get; set; }
        public List<MessageDefinition> MessageDefinitions { get; set; }
        public List<FEMessageResponse> MessageResponses { get; set; }
        public List<FEMessageTargetList> MessageTargetLists { get; set; }
        public List<FEResourceRequest> ResourceRequests { get; set; }

        public FrontendObject FindObjectByGuid(uint guid)
        {
            return Objects.Find(o => o.Guid == guid) ??
                   throw new KeyNotFoundException($"Could not find object with GUID: 0x{guid:X8}");
        }

        public FrontendObject FindObjectByHash(uint hash)
        {
            return Objects.Find(o => o.NameHash == hash) ??
                   throw new KeyNotFoundException($"Could not find object with hash: 0x{hash:X8}");
        }

        public class MessageDefinition
        {
            public string Name { get; set; }
            public string Category { get; set; }
        }
    }
}