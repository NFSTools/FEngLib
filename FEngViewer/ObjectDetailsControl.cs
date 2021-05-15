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

            if (obj.Data is {} data)
            {
                labelObjDataColor.Text = data.Color.ToString();
                labelObjDataPivot.Text = data.Pivot.ToString();
                labelObjDataPosition.Text = data.Position.ToString();
                labelObjDataRotation.Text = data.Rotation.ToString();
                labelObjDataSize.Text = data.Size.ToString();
            }
            else
            {
                labelObjDataColor.Text = labelObjDataPivot.Text = labelObjDataPosition.Text =
                    labelObjDataRotation.Text = labelObjDataSize.Text = "<n/a>";
            }
        }
    }
}