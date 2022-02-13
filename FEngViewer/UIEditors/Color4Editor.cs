using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using FEngLib.Structures;

namespace FEngViewer.UIEditors;

/// <summary>
/// Wraps ColorDialog for a PropertyGrid
/// </summary>
public class Color4Editor : UITypeEditor
{
    private IWindowsFormsEditorService _service;

    public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
    {
        return UITypeEditorEditStyle.Modal;
    }

    public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
    {
        if (provider != null)
            _service = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

        if (_service == null)
            return value;
            
        var color = (Color4)value;

        var colorDialog = new ColorDialog();

        // Set the selected color in the dialog.
        colorDialog.Color = Color.FromArgb(color.Alpha, color.Red, color.Green, color.Blue);

        // Show the dialog.
        colorDialog.ShowDialog();

        var sysColor = colorDialog.Color;
        return new Color4(sysColor.B, sysColor.G, sysColor.R, sysColor.A);;
    }
}