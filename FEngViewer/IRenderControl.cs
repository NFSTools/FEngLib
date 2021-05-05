using FEngRender.Data;

namespace FEngViewer
{
    public interface IRenderControl
    {
        RenderTreeNode SelectedNode { get; set; }

        void Init(string textureDir);

        void Render(RenderTree renderTree);
    }
}