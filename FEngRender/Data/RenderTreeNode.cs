using System;
using System.Diagnostics;
using System.Numerics;
using FEngLib;
using FEngLib.Object;
using FEngLib.Objects;
using FEngLib.Scripts;
using FEngLib.Structures;
using FEngRender.Script;
using FEngRender.Utils;

namespace FEngRender.Data
{
    /// <summary>
    /// Base class for representing an item in a <see cref="RenderTree"/>.
    /// </summary>
    public class RenderTreeNode
    {
        public Matrix4x4 ObjectMatrix { get; private set; }

        public Quaternion ObjectRotation { get; private set; }

        public Color4 ObjectColor { get; private set; }

        // TODO: THESE SHOULD *NOT* BE HERE!!!
        // TODO cont: REFACTORING IS NECESSARY!!!!!!!!!!!
        public Vector2 UpperLeft { get; private set; }
        public Vector2 LowerRight { get; private set; }

        /// <summary>
        /// The <see cref="IObject{TData}"/> instance owned by the node.
        /// </summary>
        public IObject<ObjectData> FrontendObject { get; }

        /// <summary>
        /// The <see cref="Script"/> that is currently running.
        /// </summary>
        public FEngLib.Scripts.Script CurrentScript { get; private set; }

        /// <summary>
        /// The current time offset of the current script.
        /// </summary>
        public int CurrentScriptTime { get; private set; }

        public bool Hidden { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RenderTreeNode"/> class.
        /// </summary>
        /// <param name="frontendObject">
        ///   The <see cref="IObject{TData}"/> instance owned by the <see cref="RenderTreeNode"/> instance.
        /// </param>
        public RenderTreeNode(IObject<ObjectData> frontendObject)
        {
            FrontendObject = frontendObject;
            SetScript(frontendObject.Scripts.Find(s => s.Id == 0x001744B3));
        }

        /// <summary>
        /// Applies script state and computes transformations.
        /// </summary>
        /// <param name="viewMatrix"></param>
        /// <param name="parentNode"></param>
        /// <param name="deltaTime"></param>
        /// <param name="matrixRotate"></param>
        public void PrepareForRender(Matrix4x4 viewMatrix, RenderTreeNode parentNode, int deltaTime,
            bool matrixRotate = false)
        {
            var frontendObjectData = FrontendObject.Data;
            var size = frontendObjectData.Size;
            var position = frontendObjectData.Position;
            var rotation = frontendObjectData.Rotation;
            var color = frontendObjectData.Color;

            if (frontendObjectData is ImageData imageData)
            {
                UpperLeft = imageData.UpperLeft;
                LowerRight = imageData.LowerRight;
            }

            if (CurrentScript != null
                && (CurrentScript.Length > 0 || CurrentScript.ChainedId != 0xFFFFFFFF)
                && CurrentScriptTime >= 0)
            {
                if (CurrentScriptTime >= CurrentScript.Length)
                {
                    if ((CurrentScript.Flags & 1) == 1)
                    {
                        Debug.WriteLine("looping script {0:X} for object {1:X}", CurrentScript.Id, FrontendObject.NameHash);
                        CurrentScriptTime = 0;
                    }
                    else if (CurrentScript.ChainedId != 0xFFFFFFFF)
                    {
                        var nextScript = FrontendObject.Scripts.Find(s => s.Id == CurrentScript.ChainedId) ??
                                         throw new Exception(
                                             $"Cannot find chained script (object {FrontendObject.NameHash:X}, base script {CurrentScript.Id:X}): {CurrentScript.ChainedId:X}");
                        Debug.WriteLine("activating chained script for object {1:X}: {0}",
                            nextScript.Name ?? nextScript.Id.ToString("X"), FrontendObject.NameHash);

                        SetScript(nextScript);
                    }
                }

                var colorTrack = GetKeyTrack(CurrentScript, KeyTrackType.Color);
                var posTrack = GetKeyTrack(CurrentScript, KeyTrackType.Position);
                var sizeTrack = GetKeyTrack(CurrentScript, KeyTrackType.Size);
                var rotTrack = GetKeyTrack(CurrentScript, KeyTrackType.Rotation);
                if (colorTrack != null)
                    color = TrackInterpolation.Interpolate<Color4>(colorTrack, CurrentScriptTime);
                if (posTrack != null)
                    position = TrackInterpolation.Interpolate<Vector3>(posTrack, CurrentScriptTime);
                if (sizeTrack != null)
                    size = TrackInterpolation.Interpolate<Vector3>(sizeTrack, CurrentScriptTime);
                if (rotTrack != null)
                    rotation = TrackInterpolation.Interpolate<Quaternion>(rotTrack, CurrentScriptTime);

                if (FrontendObject is Image)
                {
                    var upperLeftTrack = GetKeyTrack(CurrentScript, KeyTrackType.UpperLeft);
                    var lowerRightTrack = GetKeyTrack(CurrentScript, KeyTrackType.LowerRight);

                    if (upperLeftTrack != null)
                        UpperLeft = TrackInterpolation.Interpolate<Vector2>(upperLeftTrack, CurrentScriptTime);
                    if (lowerRightTrack != null)
                        LowerRight = TrackInterpolation.Interpolate<Vector2>(lowerRightTrack, CurrentScriptTime);
                }

                CurrentScriptTime += deltaTime;
            }

            var scaleMatrix = Matrix4x4.CreateScale(size.X, size.Y, size.Z);
            var rotateMatrix = Matrix4x4.CreateFromQuaternion(rotation);
            var transMatrix = Matrix4x4.CreateTranslation(position.X, position.Y, position.Z);

            if (matrixRotate)
                ObjectMatrix = scaleMatrix * rotateMatrix * transMatrix * viewMatrix;
            else
                ObjectMatrix = scaleMatrix * transMatrix * viewMatrix;
            ObjectRotation = rotation;
            ObjectColor = color;

            if (parentNode != null)
            {
                ObjectRotation *= parentNode.ObjectRotation;
                ObjectColor = ColorHelpers.BlendColors(ObjectColor, parentNode.ObjectColor);
            }
        }

        /// <summary>
        /// Sets the currently running script and resets the script time.
        /// </summary>
        /// <param name="script">The script to run.</param>
        public void SetScript(FEngLib.Scripts.Script script)
        {
            this.CurrentScript = script;
            this.CurrentScriptTime = script == null ? -1 : 0;
        }

        /// <summary>
        /// Gets the object's Z-coordinate.
        /// </summary>
        /// <returns></returns>
        public float GetZ()
        {
            return ObjectMatrix.M43;
        }

        private Track GetKeyTrack(FEngLib.Scripts.Script script, KeyTrackType trackType)
        {
            uint offset = (uint)trackType;

            return script.Tracks.Find(e => e.Offset == offset);
        }

        private enum KeyTrackType
        {
            Color = 0,
            Pivot = 4,
            Position = 7,
            Rotation = 10,
            Size = 14,
            UpperLeft = 17,
            LowerRight = 19
        }
    }
}