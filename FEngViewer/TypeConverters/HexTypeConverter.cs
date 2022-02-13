using System;
using System.ComponentModel;
using System.Globalization;
using FEngLib.Structures;

namespace FEngViewer.TypeConverters;

public class HexTypeConverter : UInt32Converter
{
    public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
    {
        if (destinationType == typeof(string))
        {
            NumberFormatInfo? formatInfo = (NumberFormatInfo?)culture.GetFormat(typeof(NumberFormatInfo));
            return "0x" + ((uint)value).ToString("X", formatInfo);
        } else
            return base.ConvertTo(context, culture, value, destinationType);
    }
}