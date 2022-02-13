using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Globalization;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using FEngLib.Packages;

namespace FEngViewer;

public class ResourceRequestTypeConverter : TypeConverter
{
    public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
    {
        return typeof(string) == destinationType;
    }

    public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value,
        Type destinationType)
    {
        if (typeof(string) == destinationType)
            if (value is ResourceRequest resourceRequest)
                return resourceRequest.Name;

        return "(none)";
    }
}

public class ResourceRequestEditor : UITypeEditor
{
    private IWindowsFormsEditorService _editorService;

    public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
    {
        // drop down mode (we'll host a listbox in the drop down)
        return UITypeEditorEditStyle.DropDown;
    }

    public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
    {
        _editorService = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService)) ??
                         throw new NullReferenceException("Could not get instance of IWindowsFormEditorService!");

        // use a list box
        var lb = new ListBox();
        lb.SelectionMode = SelectionMode.One;
        lb.SelectedValueChanged += OnListBoxSelectedValueChanged;
        lb.DisplayMember = nameof(ResourceRequest.Name);

        foreach (var resourceRequest in AppService.Instance.GetResourceRequests())
        {
            var index = lb.Items.Add(resourceRequest);
            if (resourceRequest.Equals(value)) lb.SelectedIndex = index;
        }

        // show this model stuff
        _editorService.DropDownControl(lb);
        if (lb.SelectedItem == null) // no selection, return the passed-in value as is
            return value;

        return lb.SelectedItem;
    }

    private void OnListBoxSelectedValueChanged(object sender, EventArgs e)
    {
        // close the drop down as soon as something is clicked
        _editorService.CloseDropDown();
    }
}