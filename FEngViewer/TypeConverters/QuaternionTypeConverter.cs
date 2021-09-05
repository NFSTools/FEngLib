using System.Collections;
using System.ComponentModel;
using System.Numerics;

namespace FEngViewer.TypeConverters
{
    public class QuaternionTypeConverter : FieldReflectingTypeConverter<Quaternion, float>
    {
        protected override object CreateInstanceImpl(ITypeDescriptorContext context, IDictionary propertyValues)
        {
            return new Quaternion((float)propertyValues["X"], (float)propertyValues["Y"], (float)propertyValues["Z"],
                (float)propertyValues["W"]);
        }

        protected override string ToStringRepresentation(object value)
        {
            var quat = (Quaternion)value;
            return $"X: {quat.X} Y: {quat.Y} Z: {quat.Z} W: {quat.W}";
        }
    }
}