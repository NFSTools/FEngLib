using System.Windows.Forms;
using FEngRender.Data;

namespace FEngViewer;

public partial class ObjectDetailsControl : UserControl
{
    public ObjectDetailsControl()
    {
        InitializeComponent();
    }

    public void UpdateObjectDetails(RenderTreeNode nodeTag)
    {
        var obj = nodeTag.GetObject();
        labelObjType.Text = obj.Type.ToString();
        labelObjHash.Text = $"{obj.NameHash:X}";
        labelObjGUID.Text = $"{obj.Guid:X}";
        labelObjFlags.Text = $"{obj.Flags}";

        if (obj.ResourceRequest is {} resourceRequest)
        {
            // labelObjResID.Text = obj.ResourceIndex.ToString();
            labelObjResID.Text = $"{resourceRequest.Name} ({resourceRequest.Type})";
        }
        else
        {
            labelObjResID.Text = "<n/a>";
        }
    }
}