using System.ComponentModel;
using System.Drawing.Design;
using System.Numerics;
using FEngLib.Objects;
using FEngLib.Structures;
using FEngViewer.TypeConverters;
// ReSharper disable UnusedMember.Global

namespace FEngViewer
{
    public class ObjectViewWrapper
    {
        private IObject<ObjectData> _obj;

        public ObjectViewWrapper(IObject<ObjectData> obj)
        {
            _obj = obj;
        }

        [Category("Data")]
        [TypeConverter(typeof(Color4TypeConverter))]
        [Editor(typeof(Color4Editor), typeof(UITypeEditor))]
        public Color4 Color
        {
            get => _obj.Data.Color;
            set => _obj.Data.Color = value;
        }
        
        [Category("Data")]
        [TypeConverter(typeof(Vector3TypeConverter))]
        public Vector3 Pivot
        {
            get => _obj.Data.Pivot;
            set => _obj.Data.Pivot = value;
        }
        
        [Category("Data")]
        [TypeConverter(typeof(Vector3TypeConverter))]
        public Vector3 Position
        {
            get => _obj.Data.Position;
            set => _obj.Data.Position = value;
        }
        
        [Category("Data")]
        [TypeConverter(typeof(QuaternionTypeConverter))]
        public Quaternion Rotation
        {
            get => _obj.Data.Rotation;
            set => _obj.Data.Rotation = value;
        }
        
        [Category("Data")]
        [TypeConverter(typeof(Vector3TypeConverter))]
        public Vector3 Size
        {
            get => _obj.Data.Size;
            set => _obj.Data.Size = value;
        }
    }
}