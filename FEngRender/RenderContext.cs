using System.Numerics;
using FEngRender.Data;

namespace FEngRender;

public record RenderContext(Matrix4x4 ViewMatrix, RenderTreeNode Parent);