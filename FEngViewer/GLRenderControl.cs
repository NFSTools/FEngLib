using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using FEngRender.Data;
using FEngRender.OpenGL;
using JetBrains.Annotations;

namespace FEngViewer
{
    public partial class GLRenderControl : UserControl, IRenderControl
    {
        private GLRenderTreeRenderer _renderer;
        [CanBeNull] private RenderTree _renderTree;
        private Timer _timer = null!;

        public GLRenderControl()
        {
            InitializeComponent();
        }

        private void glControl_Load(object sender, EventArgs e)
        {
            // Redraw the screen every 1/20 of a second.
            _timer = new Timer();
            _timer.Tick += (tsender, te) =>
            {
                RenderInternal();
            };
            _timer.Interval = 50;   // 1000 ms per sec / 50 ms per frame = 20 FPS

            glControl.MakeCurrent();
            _renderer = new GLRenderTreeRenderer();
        }

        private void glControl_Paint(object sender, PaintEventArgs e)
        {
            // render once initially
            RenderInternal();
        }

        private void RenderInternal()
        {
            glControl.MakeCurrent();
            _renderer.Render(_renderTree);
        }

        public RenderTreeNode SelectedNode
        {
            get => _renderer.SelectedNode;
            set => _renderer.SelectedNode = value;
        }

        public void Init(string textureDir)
        {
            _renderer.LoadTextures(textureDir);
        }

        public void Render(RenderTree renderTree)
        {
            _renderTree = renderTree;
            _timer.Start();
        }
    }
}