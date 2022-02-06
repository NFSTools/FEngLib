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

    // TODO extract all this into a separate "Writer" thing or something.
    public void Write(BinaryWriter writer)
    {
        WritePkHd(writer);
        WriteTypS(writer);
        WriteResCC(writer);
        WriteObjCC(writer);
    }
    
    private void WritePkHd(BinaryWriter writer)
    {
        using var ms = new MemoryStream();
        using var mw = new BinaryWriter(ms);
        mw.Write(0x20000);
        mw.Write(0);
        mw.Write(ResourceRequests.Count);
        var rootObjCount = Objects.Count(o => o.Parent == null);
        mw.Write(rootObjCount);
        mw.Write(Name.Length + 1);
        mw.Write(Filename.Length + 1);
        mw.Write(Name.ToCharArray());
        mw.Write('\0');
        mw.Write(Filename.ToCharArray());
        mw.Write('\0');
        mw.AlignWriter(4);

        writer.Write(new[] { 'P', 'k', 'H', 'd' });
        writer.Write((int) ms.Length);
        ms.WriteTo(writer.BaseStream);
    }

    private void WriteTypS(BinaryWriter writer)
    {
        // TODO these are from UG2 UI_QRTrackSelect.fng.
        //  they should be the same, at the very least per game.
        // TODO: check if these are always the same in every fng, or if the type sizes present vary.
        var sizes = new Dictionary<int, int>
        {
            { 5, 0x44 },
            { 2, 0x44 },
            { 1, 0x54 }
        };
        
        writer.Write(new []{ 'T', 'y', 'p', 'S' });
        writer.Write(8 * sizes.Count);
        foreach (var (key, value) in sizes)
        {
            writer.Write(key);
            writer.Write(value);
        }
    }

    private void WriteResCC(BinaryWriter writer)
    {
        writer.Write(new []{ 'R', 'e', 's' });
        writer.Write((byte) 0xCC);
        
        using var resms = new MemoryStream();
        using var resbw = new BinaryWriter(resms);
        
        using var nmms = new MemoryStream();
        using var nmbw = new BinaryWriter(nmms);

        using var rqms = new MemoryStream();
        using var rqbw = new BinaryWriter(rqms);

        rqbw.Write(ResourceRequests.Count);
        foreach (var resrq in ResourceRequests)
        {
            rqbw.Write(resrq.ID);
            
            rqbw.Write(resrq.NameOffset);
            nmbw.Write(resrq.Name.ToCharArray());
            nmbw.Write('\0');

            rqbw.WriteEnum(resrq.Type);
            rqbw.Write(resrq.Flags);
            rqbw.Write(resrq.Handle);
            rqbw.Write(resrq.UserParam);
        }

        rqbw.AlignWriter(4);
        nmbw.AlignWriter(4);
        
        resbw.Write(new []{ 'R', 's', 'N', 'm' });
        resbw.Write((int) nmms.Length);
        nmms.WriteTo(resms);
        
        resbw.Write(new []{ 'R', 's', 'R', 'q' });
        resbw.Write((int) rqms.Length);
        rqms.WriteTo(resms);

        writer.Write((int) resms.Length);
        resms.WriteTo(writer.BaseStream);
    }

    private void WriteObjCC(BinaryWriter writer)
    {
        // TODO Butn chunk
        
        writer.Write(new []{ 'O', 'b', 'j' });
        writer.Write((byte) 0xCC);
        writer.Write(0);
    }

    public class MessageDefinition
    {
        public string Name { get; set; }
        public string Category { get; set; }
    }
}