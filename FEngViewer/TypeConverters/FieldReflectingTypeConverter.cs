using System;
using System.ComponentModel;
using System.Globalization;

namespace FEngViewer.TypeConverters;

/// <summary>
/// Implements everything needed to edit a type in a PropertyGrid, given the following constraints are fulfilled:
/// - All fields in the type MUST have the same type
/// This is well suited for adapting for example System.Numerics.Vector3, see Vector3TypeConverter.
/// </summary>
/// <typeparam name="TNativeType">Type of the object the converter is for</typeparam>
/// <typeparam name="TFieldTypeA">Type of the fields in the object</typeparam>
public abstract class FieldReflectingTypeConverter<TNativeType, TFieldTypeA> : TypeConverter
{
    protected abstract string ToStringRepresentation(object value);
        
    public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value,
        Type destinationType)
    {
        if (destinationType != typeof(string)) return base.ConvertTo(context, culture, value, destinationType);

        return ToStringRepresentation(value);
    }

    // This exists just to force deriving classes to implement this.
    protected abstract object CreateInstanceImpl(ITypeDescriptorContext context,
        System.Collections.IDictionary propertyValues);

    public override object CreateInstance(ITypeDescriptorContext context,
        System.Collections.IDictionary propertyValues)
    {
        return CreateInstanceImpl(context, propertyValues);
    }

    public override bool GetCreateInstanceSupported(ITypeDescriptorContext context) => true;

    // TODO support mixed field types when necessary
    public override PropertyDescriptorCollection GetProperties(
        ITypeDescriptorContext context,
        object value,
        Attribute[] attributes)
    {
        if (value is not TNativeType)
            return base.GetProperties(context, value, attributes);

        var fieldInfos = value.GetType().GetFields();

        var propDescs = new PropertyDescriptor[fieldInfos.Length];

        for (var i = 0; i < fieldInfos.Length; i++)
        {
            propDescs[i] = new FieldPropertyDescriptor<TNativeType, TFieldTypeA>(fieldInfos[i].Name, null);
        }

        return new PropertyDescriptorCollection(propDescs);
    }
        
    public override bool GetPropertiesSupported(ITypeDescriptorContext context) => true;
}

/// <summary>
/// A synthetic property that proxies an object's field.
/// </summary>
/// <typeparam name="T">The struct's type</typeparam>
/// <typeparam name="TFieldType">The field's type</typeparam>
internal class FieldPropertyDescriptor<T, TFieldType> : PropertyDescriptor
{
    public FieldPropertyDescriptor(string name, Attribute[] attrs) : base(name, attrs)
    {
    }

    public override bool CanResetValue(object component) => false;

    public override object GetValue(object component)
    {
        if (component is T obj)
        {
            return obj.GetType().GetField(Name)?.GetValue(obj);                
        }

        throw new Exception();
    }

    public override void ResetValue(object component)
    {
        throw new NotSupportedException();
    }

    public override void SetValue(object component, object value)
    {
        if (component is T obj)
        {
            obj.GetType().GetField(Name)?.SetValue(obj, value);                
        }
        else
        {
            throw new Exception();
        }
    }

    public override bool ShouldSerializeValue(object component) => false;

    public override Type ComponentType => typeof(T);
    public override bool IsReadOnly => false;
    public override Type PropertyType => typeof(TFieldType);
}