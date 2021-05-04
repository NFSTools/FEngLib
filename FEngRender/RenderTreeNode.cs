using System.Numerics;
using FEngLib;
using FEngLib.Structures;

namespace FEngRender
{
    /// <summary>
    /// Base class for representing an item in a <see cref="RenderTree"/>.
    /// </summary>
    public class RenderTreeNode
    {
        public Matrix4x4 ObjectMatrix { get; private set; }

        public Quaternion ObjectRotation { get; private set; }

        public FEColor ObjectColor { get; private set; }

        /// <summary>
        /// The <see cref="FEngLib.FrontendObject"/> instance owned by the node.
        /// </summary>
        public FrontendObject FrontendObject { get; }

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
        /// Computes transformations based on context.
        /// </summary>
        /// <param name="viewMatrix"></param>
        /// <param name="parentNode"></param>
        public void ApplyContext(Matrix4x4 viewMatrix, RenderTreeNode parentNode)
        {
            var scaleMatrix = Matrix4x4.CreateScale(FrontendObject.Size.X, FrontendObject.Size.Y, FrontendObject.Size.Z);
            var transMatrix = Matrix4x4.CreateTranslation(FrontendObject.Position.X, FrontendObject.Position.Y,
                FrontendObject.Position.Z);

            ObjectMatrix = scaleMatrix * transMatrix * viewMatrix;
            ObjectRotation = FrontendObject.Rotation.ToQuaternion();
            ObjectColor = FrontendObject.Color;

            if (parentNode != null)
            {
                ObjectRotation *= parentNode.ObjectRotation;
                ObjectColor = ColorHelpers.BlendColors(ObjectColor, parentNode.ObjectColor);
            }
        }

        /// <summary>
        /// Gets the object's Z-coordinate.
        /// </summary>
        /// <returns></returns>
        public float GetZ()
        {
            return ObjectMatrix.M43;
        }
    }
}