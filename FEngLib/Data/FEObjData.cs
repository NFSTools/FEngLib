using System.IO;
using System.Numerics;
using FEngLib.Structures;

namespace FEngLib.Data
{
    public class FEObjData : IBinaryAccess
    {
        public FEColor Color { get; set; }
        public Vector3 Pivot { get; set; }
        public Vector3 Position { get; set; }
        public Quaternion Rotation { get; set; }
        public Vector3 Size { get; set; }

        public virtual void Read(BinaryReader br)
        {
            Color = new FEColor();
            Color.Read(br);
            Pivot = new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
            Position = new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
            Rotation = new Quaternion(br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
            Size = new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
        }

        public virtual void Write(BinaryWriter bw)
        {
            Color.Write(bw);
            
            bw.Write(Pivot.X);
            bw.Write(Pivot.Y);
            bw.Write(Pivot.Z);
            
            bw.Write(Position.X);
            bw.Write(Position.Y);
            bw.Write(Position.Z);
            
            bw.Write(Rotation.X);
            bw.Write(Rotation.Y);
            bw.Write(Rotation.Z);
            bw.Write(Rotation.W);

            bw.Write(Size.X);
            bw.Write(Size.Y);
            bw.Write(Size.Z);
        }
    }
}