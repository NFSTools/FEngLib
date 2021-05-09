using System.Windows.Forms;
using FEngRender;
using FEngRender.Data;

namespace FEngViewer
{
    public partial class ObjectDetailsControl : UserControl
    {
        public ObjectDetailsControl()
        {
            InitializeComponent();
        }

        public void UpdateObjectDetails(RenderTreeNode nodeTag)
        {
            var obj = nodeTag.FrontendObject;
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

            labelObjDataColor.Text = obj.Color?.ToString() ?? "<n/a>";
            labelObjDataPivot.Text = obj.Pivot.ToString();
            labelObjDataPosition.Text = obj.Position.ToString();
            labelObjDataRotation.Text = obj.Rotation.ToString();
            labelObjDataSize.Text = obj.Size.ToString();
        }
    }
}