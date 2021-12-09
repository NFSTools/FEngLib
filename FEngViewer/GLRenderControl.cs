using System.Windows.Forms;
using FEngLib.Structures;
using FEngRender.Data;
using FEngRender.GL;
using JetBrains.Annotations;

namespace FEngViewer
{
    public partial class GLRenderControl : UserControl, IRenderControl
    {
        private GLRenderTreeRenderer _renderer;
        
        [CanBeNull] private RenderTree _renderTree;

        public Color4 BackgroundColor
        {
            set => _renderer.SetBackgroundColor(value);
        }
        
        public GLRenderControl()
        {
            InitializeComponent();
        }

        public RenderTreeNode SelectedNode
        {
            get => _renderer.SelectedNode;
            set => _renderer.SelectedNode = value;
        }
        
        public void Init(string textureDir)
        {
            _renderer?.LoadTextures(textureDir);
        }

        public void Render(RenderTree renderTree)
        {
            _renderTree = renderTree;
        }

        private void openglControl1_OpenGLInitialized(object sender, System.EventArgs e)
        {
            _renderer = new GLRenderTreeRenderer(openglControl1.OpenGL);
            _renderer.PrepareRender();
        }

        private void openglControl1_OpenGLDraw(object sender, SharpGL.RenderEventArgs args)
        {
            if (_renderTree != null)
                _renderer.Render(_renderTree);
        }

        private void openglControl1_MouseMove(object sender, MouseEventArgs e)
        {
            OnMouseMove(e);
        }

        private void openglControl1_MouseClick(object sender, MouseEventArgs e)
        {
            OnMouseClick(e);
        }
    }
}