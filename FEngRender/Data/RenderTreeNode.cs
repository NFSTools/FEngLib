using System;
using System.Diagnostics;
using System.Numerics;
using FEngLib;
using FEngLib.Data;
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

        public FEColor ObjectColor { get; private set; }

        /// <summary>
        /// The <see cref="FEngLib.FrontendObject"/> instance owned by the node.
        /// </summary>
        public FrontendObject FrontendObject { get; }

        /// <summary>
        /// The <see cref="FrontendScript"/> that is currently running.
        /// </summary>
        public FrontendScript CurrentScript { get; private set; }

        /// <summary>
        /// The current time offset of the current script.
        /// </summary>
        public int CurrentScriptTime { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RenderTreeNode"/> class.
        /// </summary>
        /// <param name="frontendObject">
        ///   The <see cref="FEngLib.FrontendObject"/> instance owned by the <see cref="RenderTreeNode"/> instance.
        /// </param>
        public RenderTreeNode(FrontendObject frontendObject)
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
            var size = FrontendObject.Size;
            var position = FrontendObject.Position;
            var rotation = FrontendObject.Rotation;
            var color = FrontendObject.Color;

            // test
            //deltaTime = 20;

            //deltaTime = Math.Max(0, Math.Min(60, deltaTime));

            if (CurrentScript != null
                && CurrentScript.Length > 0
                && CurrentScriptTime >= 0)
            {
                var canRunScript = true;

                if (CurrentScriptTime > CurrentScript.Length)
                {
                    if ((CurrentScript.Flags & 1) == 1)
                    {
                        Debug.WriteLine("looping");
                        CurrentScriptTime = 0;
                    }
                    else
                    {
                        Debug.WriteLine("done");
                        SetScript(null);
                        canRunScript = false;
                    }
                }

                if (canRunScript)
                {
                    var colorTrack = GetKeyTrack(CurrentScript, KeyTrackType.Color);
                    var posTrack = GetKeyTrack(CurrentScript, KeyTrackType.Position);
                    var sizeTrack = GetKeyTrack(CurrentScript, KeyTrackType.Size);
                    //var rotTrack = GetKeyTrack(CurrentScript, KeyTrackType.Color);
                    if (colorTrack != null)
                        color = TrackInterpolation.Interpolate<FEColor>(colorTrack, CurrentScriptTime);
                    if (posTrack != null)
                        position = TrackInterpolation.Interpolate<FEVector3>(posTrack, CurrentScriptTime);
                    if (sizeTrack != null)
                        size = TrackInterpolation.Interpolate<FEVector3>(sizeTrack, CurrentScriptTime);

                    Debug.WriteLine("T={0} L={1}", CurrentScriptTime, CurrentScript.Length);
                    CurrentScriptTime += deltaTime;
                }
            }

            var scaleMatrix = Matrix4x4.CreateScale(size.X, size.Y, size.Z);
            var rotateMatrix = Matrix4x4.CreateFromQuaternion(rotation.ToQuaternion());
            var transMatrix = Matrix4x4.CreateTranslation(position.X, position.Y, position.Z);

            if (matrixRotate)
                ObjectMatrix = scaleMatrix * rotateMatrix * transMatrix * viewMatrix;
            else
                ObjectMatrix = scaleMatrix * transMatrix * viewMatrix;
            ObjectRotation = rotation.ToQuaternion();
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
        public void SetScript(FrontendScript script)
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

        private FEKeyTrack GetKeyTrack(FrontendScript script, KeyTrackType trackType)
        {
            uint offset = (uint) trackType;

            return script.Tracks.Find(e => e.Offset == offset);
        }

        private enum KeyTrackType
        {
            Color = 0,
            Pivot = 4,
            Position = 7,
            Rotation = 10,
            Size = 14
        }
    }
}