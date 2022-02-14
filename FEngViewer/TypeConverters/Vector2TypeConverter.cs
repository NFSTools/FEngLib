using System.Collections;
using System.ComponentModel;
using System.Numerics;

namespace FEngViewer.TypeConverters;

public class Vector2TypeConverter : FieldReflectingTypeConverter<Vector2, float>
{
    protected override object CreateInstanceImpl(ITypeDescriptorContext context,
        IDictionary propertyValues)
    {
        return new Vector2((float)propertyValues["X"], (float)propertyValues["Y"]);
    }

    protected override string ToStringRepresentation(object value)
    {
        var vec = (Vector2)value;
        return $"X: {vec.X} Y: {vec.Y}";
    }
}