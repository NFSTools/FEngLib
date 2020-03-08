using System.IO;
using CoreLibraries.IO;
using FEngLib.Structures;

namespace FEngLib.Data
{
    public class FEObjData : IBinaryAccess
    {
        public FEColor Color { get; set; }
        public FEVector3 Pivot { get; set; }
        public FEVector3 Position { get; set; }
        public FEQuaternion Rotation { get; set; }
        public FEVector3 Size { get; set; }

        public virtual void Read(BinaryReader br)
        {
            Color = new FEColor();
            Pivot = new FEVector3();
            Position = new FEVector3();
            Rotation = new FEQuaternion();
            Size = new FEVector3();

            Color.Read(br);
            Pivot.Read(br);
            Position.Read(br);
            Rotation.Read(br);
            Size.Read(br);
        }

        public virtual void Write(BinaryWriter bw)
        {
            Color.Write(bw);
            Pivot.Write(bw);
            Position.Write(bw);
            Rotation.Write(bw);
            Size.Write(bw);
        }
    }
}