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
            labelObjFlags.Text = $"{obj.Flags:X}";
            labelObjResID.Text = obj.ResourceIndex.ToString();

            labelObjDataColor.Text = obj.Color.ToString();
            labelObjDataPivot.Text = obj.Pivot.ToString();
            labelObjDataPosition.Text = obj.Position.ToString();
            labelObjDataRotation.Text = obj.Rotation.ToString();
            labelObjDataSize.Text = obj.Size.ToString();
        }
    }
}