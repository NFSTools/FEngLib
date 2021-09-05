using System.ComponentModel;
using System.Numerics;

namespace FEngViewer.TypeConverters
{
    public class Vector3TypeConverter : FieldReflectingTypeConverter<Vector3, float>
    {
        protected override object CreateInstanceImpl(ITypeDescriptorContext context,
            System.Collections.IDictionary propertyValues)
        {
            return new Vector3((float)propertyValues["X"], (float)propertyValues["Y"], (float)propertyValues["Z"]);
        }

        protected override string ToStringRepresentation(object value)
        {
            var vec = (Vector3)value;
            return $"X: {vec.X} Y: {vec.Y} Z: {vec.Z}";
        }
    }
}