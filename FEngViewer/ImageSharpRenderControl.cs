using System.Drawing;
using System.IO;
using System.Windows.Forms;
using FEngLib.Structures;
using FEngRender;
using FEngRender.Data;
using static SixLabors.ImageSharp.ImageExtensions;

namespace FEngViewer
{
    public partial class ImageSharpRenderControl : UserControl, IRenderControl
    {
        private ImageRenderTreeRenderer _renderer;
        
        public ImageSharpRenderControl()
        {
            InitializeComponent();
        }

        public void Init(string textureDir)
        {
            _renderer = new ImageRenderTreeRenderer();
            _renderer.LoadTextures(textureDir);
        }

        public void Render(RenderTree renderTree)
        {
            var image = _renderer.Render(renderTree);
            var stream = new MemoryStream();
            image.SaveAsBmp(stream);
            viewOutput.Image = Image.FromStream(stream);
        }

        public Color4 BackgroundColor
        {
            set => viewOutput.BackColor = Color.FromArgb(value.Alpha, value.Red, value.Green, value.Blue);
        }

        public RenderTreeNode SelectedNode
        {
            get => _renderer.SelectedNode;
            set => _renderer.SelectedNode = value;
        }
    }
}
