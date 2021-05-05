using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using FEngRender;
using FEngRender.Data;
using static SixLabors.ImageSharp.ImageExtensions;

namespace FEngViewer
{
    public partial class ImageSharpRenderControl : UserControl, IRenderControl
    {
        private RenderTreeRenderer _renderer;
        
        public ImageSharpRenderControl()
        {
            InitializeComponent();
        }

        public void Init(string textureDir)
        {
            _renderer = new RenderTreeRenderer();
            _renderer.LoadTextures(textureDir);
        }

        public void Render(RenderTree renderTree)
        {
            var image = _renderer.Render(renderTree);
            var stream = new MemoryStream();
            image.SaveAsBmp(stream);
            viewOutput.Image = Image.FromStream(stream);
        }

        public RenderTreeNode SelectedNode
        {
            get => _renderer.SelectedNode;
            set => _renderer.SelectedNode = value;
        }
    }
}
