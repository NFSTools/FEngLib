using System.Numerics;
using FEngLib;

namespace FEngRender
{
    /// <summary>
    /// Base class for representing an item in a <see cref="RenderTree"/>.
    /// </summary>
    public class RenderTreeNode
    {
        public Matrix4x4 ObjectMatrix { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RenderTreeNode"/> class.
        /// </summary>
        /// <param name="frontendObject">
        ///   The <see cref="FEngLib.FrontendObject"/> instance owned by the <see cref="RenderTreeNode"/> instance.
        /// </param>
        public RenderTreeNode(FrontendObject frontendObject)
        {
            FrontendObject = frontendObject;
        }

        /// <summary>
        /// Computes the object's matrix, given a view matrix.
        /// </summary>
        /// <param name="viewMatrix"></param>
        public void ComputeObjectMatrix(Matrix4x4 viewMatrix)
        {
            ObjectMatrix = Matrix4x4.Multiply(new Matrix4x4(
                FrontendObject.Size.X, 0, 0, 0,
                0, FrontendObject.Size.Y, 0, 0,
                0, 0, FrontendObject.Size.Z, 0,
                FrontendObject.Position.X, FrontendObject.Position.Y, FrontendObject.Position.Z, 1
            ), viewMatrix);
        }

        /// <summary>
        /// Gets the object's Z-coordinate.
        /// </summary>
        /// <returns></returns>
        public float GetZ()
        {
            return ObjectMatrix.M43;
        }

        /// <summary>
        /// The <see cref="FEngLib.FrontendObject"/> instance owned by the node.
        /// </summary>
        public FrontendObject FrontendObject { get; }
    }
}