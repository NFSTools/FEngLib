using System.Windows.Forms;

namespace FEngViewer
{
    public partial class ObjectDetailsView : UserControl
    {
        public ObjectDetailsView()
        {
            InitializeComponent();
        }

        public void UpdateObjectDetails(FEObjectViewNode nodeTag)
        {
            var obj = nodeTag.Obj;
            labelObjType.Text = obj.Type.ToString();
            labelObjHash.Text = $"{obj.NameHash:X}";
            labelObjGUID.Text = $"{obj.Guid:X}";
            labelObjFlags.Text = $"{obj.Flags}";

            if (obj.ResourceIndex > -1)
            {
                // labelObjResID.Text = obj.ResourceIndex.ToString();
                var resourceRequest = obj.Package.ResourceRequests[obj.ResourceIndex];
                labelObjResID.Text = $"{obj.ResourceIndex} - {resourceRequest.Name} ({resourceRequest.Type})";
            }
            else
            {
                labelObjResID.Text = "<n/a>";
            }

            labelObjDataColor.Text = obj.Color?.ToString() ?? "<n/a>";
            labelObjDataPivot.Text = obj.Pivot?.ToString() ?? "<n/a>";
            labelObjDataPosition.Text = obj.Position?.ToString() ?? "<n/a>";
            labelObjDataRotation.Text = obj.Rotation?.ToString() ?? "<n/a>";
            labelObjDataSize.Text = obj.Size?.ToString() ?? "<n/a>";
        }
    }
}