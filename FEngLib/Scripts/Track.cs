using System.Collections.Generic;

namespace FEngLib.Scripts
{
    public enum TrackParamType : byte
    {
        Boolean = 0x0,
        Integer = 0x1,
        Float = 0x2,
        Vector2 = 0x3,
        Vector3 = 0x4,
        Quaternion = 0x5,
        Color = 0x6,
    }

    public enum TrackInterpolationMethod : byte
    {
        None = 0x0,
        Linear = 0x1,
        Spline = 0x2,
        MoveToLinear = 0x3,
        MoveToSpline = 0x4,
    }

    public class Track
    {
        public Track()
        {
            DeltaKeys = new LinkedList<TrackNode>();
        }

        public TrackParamType ParamType { get; set; }
        public byte ParamSize { get; set; }
        public TrackInterpolationMethod InterpType { get; set; }
        public byte InterpAction { get; set; }
        public uint Length { get; set; }
        public uint Offset { get; set; }
        public TrackNode BaseKey { get; set; }
        public LinkedList<TrackNode> DeltaKeys { get; set; }

        public LinkedListNode<TrackNode> GetDeltaKeyAt(int time)
        {
            LinkedListNode<TrackNode> node;
            for (node = DeltaKeys.First;
                node?.Next != null && node.Value.Time < time;
                node = node.Next)
            {
                //
            }

            return node;
        }
    }
}