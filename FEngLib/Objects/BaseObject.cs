using System.Collections.Generic;
using System.IO;
using System.Numerics;
using FEngLib.Messaging;
using FEngLib.Packages;
using FEngLib.Scripts;
using FEngLib.Structures;
using FEngLib.Utils;

namespace FEngLib.Objects;

/// <summary>
/// This represents all common data found in an object's 'ObjD' chunk.
/// For objects where the ObjD chunk contains extra data (e.g. images),
/// inherit from this class to represent the extra values in that chunk. 
/// </summary>
public class ObjectData : IBinaryAccess
{
    public Color4 Color { get; set; }
    public Vector3 Pivot { get; set; }
    public Vector3 Position { get; set; }
    public Quaternion Rotation { get; set; }
    public Vector3 Size { get; set; }

    public virtual void Read(BinaryReader br)
    {
        Color = br.ReadColor();
        Pivot = br.ReadVector3();
        Position = br.ReadVector3();
        Rotation = br.ReadQuaternion();
        Size = br.ReadVector3();
    }

    public virtual void Write(BinaryWriter bw)
    {
        bw.Write(Color);
        bw.Write(Pivot);
        bw.Write(Position);
        bw.Write(Rotation);
        bw.Write(Size);
    }
}

/// <summary>
/// Common interface for all object types and their properties.
/// Extend this if your object type has extra ObjD attributes,
/// and if there are other object types that inherit from your object type (to ensure type safety).
/// </summary>
/// <typeparam name="TData">
/// A type inheriting from ObjectData,
/// representing the contents of an ObjD chunk for this object.
/// </typeparam>
public interface IObject<out TData> : IScriptedObject, IHaveMessageResponses where TData : ObjectData
{
    TData Data { get; }
    ObjectType Type { get; set; }
    ObjectFlags Flags { get; set; }
    ResourceRequest ResourceRequest { get; set; }
    string Name { get; set; }
    uint NameHash { get; set; }
    uint Guid { get; set; }
    IObject<ObjectData> Parent { get; set; }

    void InitializeData();
}

public interface IScriptedObject
{
    IEnumerable<Script> GetScripts();

    Script CreateScript();

    Script FindScript(uint id);
}

public interface IScriptedObject<out TScript> : IScriptedObject where TScript : Script
{
    new IEnumerable<TScript> GetScripts();

    new TScript CreateScript();

    new TScript FindScript(uint id);
}

public class BaseObjectScript : Script<ScriptTracks>
{
}

/// <summary>
/// All objects that don't have any extra attributes in their ObjD chunk's SA tag should inherit from this.
/// </summary>
public abstract class BaseObject : BaseObject<ObjectData, BaseObjectScript>
{
    protected BaseObject(ObjectData data) : base(data)
    {
    }
}

/// <summary>
/// All objects that have extra data in their ObjD chunk's SA tag should inherit from this base class.
/// </summary>
/// <typeparam name="TData">
/// A type inheriting from ObjectData that represents any additional attributes relevant for this object type.
/// </typeparam>
/// <typeparam name="TScript"></typeparam>
public abstract class BaseObject<TData, TScript> : IObject<TData>, IScriptedObject<TScript>
    where TData : ObjectData where TScript : Script, new()
{
    protected BaseObject(TData data)
    {
        Scripts = new List<TScript>();
        MessageResponses = new List<MessageResponse>();
        Data = data;
    }

    public List<TScript> Scripts { get; }

    public TData Data { get; protected set; }
    public ObjectType Type { get; set; }
    public ObjectFlags Flags { get; set; }
    public ResourceRequest ResourceRequest { get; set; }
    public string Name { get; set; }
    public uint NameHash { get; set; }
    public uint Guid { get; set; }
    public IObject<ObjectData> Parent { get; set; }
    public List<MessageResponse> MessageResponses { get; }

    public abstract void InitializeData();

    Script IScriptedObject.FindScript(uint id)
    {
        return FindScript(id);
    }

    Script IScriptedObject.CreateScript()
    {
        return CreateScript();
    }

    IEnumerable<Script> IScriptedObject.GetScripts()
    {
        return GetScripts();
    }

    public IEnumerable<TScript> GetScripts()
    {
        return Scripts;
    }

    public TScript CreateScript()
    {
        var script = new TScript();
        Scripts.Add(script);
        return script;
    }

    public TScript FindScript(uint id)
    {
        return Scripts.Find(s => s.Id == id);
    }
}