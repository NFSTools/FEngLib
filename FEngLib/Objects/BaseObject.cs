using System.Collections.Generic;
using System.IO;
using System.Numerics;
using FEngLib.Packages;
using FEngLib.Scripts;
using FEngLib.Structures;
using FEngLib.Utils;

namespace FEngLib.Objects
{
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

    public interface IObject<out TData> where TData : ObjectData
    {
        TData Data { get; }
        ObjectType Type { get; set; }
        ObjectFlags Flags { get; set; }
        ResourceRequest ResourceRequest { get; set; }
        string Name { get; set; }
        uint NameHash { get; set; }
        uint Guid { get; set; }
        IObject<ObjectData> Parent { get; set; }
        List<Script> Scripts { get; }
        List<MessageResponse> MessageResponses { get; }

        void InitializeData();
    }

    public abstract class BaseObject : BaseObject<ObjectData>
    {
        protected BaseObject(ObjectData data) : base(data)
        {
        }
    }

    public abstract class BaseObject<TData> : IObject<TData> where TData : ObjectData
    {
        protected BaseObject(TData data)
        {
            Scripts = new List<Script>();
            MessageResponses = new List<MessageResponse>();
            Data = data;
        }

        
        public TData Data { get; protected set; }
        public ObjectType Type { get; set; }
        public ObjectFlags Flags { get; set; }
        public ResourceRequest ResourceRequest { get; set; }
        public string Name { get; set; }
        public uint NameHash { get; set; }
        public uint Guid { get; set; }
        public IObject<ObjectData> Parent { get; set; }
        public List<Script> Scripts { get; }
        public List<MessageResponse> MessageResponses { get; }

        public abstract void InitializeData();
    }
}