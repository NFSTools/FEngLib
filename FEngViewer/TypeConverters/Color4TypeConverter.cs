using System.ComponentModel;
using FEngLib.Structures;

namespace FEngViewer.TypeConverters
{
    public class Color4TypeConverter : FieldReflectingTypeConverter<Color4, int>
    {
        protected override object CreateInstanceImpl(ITypeDescriptorContext context, System.Collections.IDictionary propertyValues)
        {
            return new Color4((int)propertyValues["Blue"], (int)propertyValues["Green"], (int)propertyValues["Red"],
                (int)propertyValues["Alpha"]);
        }

        protected override string ToStringRepresentation(object value)
        {
            var col = (Color4)value;
            return col.ToString();
        }
    }
}