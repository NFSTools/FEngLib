using System.Collections.Generic;
using System.IO;
using System.Linq;
using FEngLib.Objects;
using FEngLib.Scripts;
using FEngLib.Utils;

namespace FEngLib.Packages;

/// <summary>
///     Stores frontend objects, resource names, etc.
/// </summary>
public class Package
{
    public Package()
    {
        ResourceRequests = new List<ResourceRequest>();
        Objects = new List<IObject<ObjectData>>();
        MessageResponses = new List<MessageResponse>();
        MessageTargetLists = new List<MessageTargetList>();
        MessageDefinitions = new List<MessageDefinition>();
    }

    public string Name { get; set; }
    public string Filename { get; set; }
    public List<ResourceRequest> ResourceRequests { get; }
    public List<IObject<ObjectData>> Objects { get; }
    public List<MessageResponse> MessageResponses { get; }
    public List<MessageTargetList> MessageTargetLists { get; }
    public List<MessageDefinition> MessageDefinitions { get; }

    public int ButtonCount => Objects.Count(o => (o.Flags & ObjectFlags.IsButton) != 0);

    public IObject<ObjectData> FindObjectByGuid(uint guid)
    {
        return Objects.Find(o => o.Guid == guid) ??
               throw new KeyNotFoundException($"Could not find object with GUID: 0x{guid:X8}");
    }

    public IObject<ObjectData> FindObjectByHash(uint hash)
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