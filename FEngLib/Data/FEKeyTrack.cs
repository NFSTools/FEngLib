using System;
using System.Collections.Generic;

namespace FEngLib.Data
{
    public enum FEParamType : byte
    {
        PT_Bool = 0x0,
        PT_Int = 0x1,
        PT_Float = 0x2,
        PT_Vector2 = 0x3,
        PT_Vector3 = 0x4,
        PT_Quaternion = 0x5,
        PT_Color = 0x6,
        PT_ParamTypeCount = 0x7
    }

    public enum FEInterpMethod : byte
    {
        IT_None = 0x0,
        IT_Linear = 0x1,
        IT_Spline = 0x2,
        IT_MoveToLinear = 0x3,
        IT_MoveToSpline = 0x4,
        IT_InterpTypeCount = 0x5
    }

    public enum FEKeyTrackIndices
    {
        FETrack_Color = 0x0,
        FETrack_Pivot = 0x1,
        FETrack_Position = 0x2,
        FETrack_Rotation = 0x3,
        FETrack_Size = 0x4,
        FETrack_UpperLeft = 0x5,
        FETrack_LowerRight = 0x6,
        FETrack_FrameNumber = 0x7,
        FETrack_Color1 = 0x7,
        FETrack_Color2 = 0x8,
        FETrack_Color3 = 0x9,
        FETrack_Color4 = 0xA,
        Num_BaseFETracks = 0xB,
        Undefined = 0xC
    }

    public class FEKeyTrack
    {
        public FEKeyTrack()
        {
            DeltaKeys = new LinkedList<FEKeyNode>();
        }

        public FEParamType ParamType { get; set; }
        public byte ParamSize { get; set; }
        public FEInterpMethod InterpType { get; set; }
        public byte InterpAction { get; set; }
        public uint Length { get; set; }
        public uint Offset { get; set; }
        public FEKeyNode BaseKey { get; set; }
        public LinkedList<FEKeyNode> DeltaKeys { get; set; }

        public LinkedListNode<FEKeyNode> GetDeltaKeyAt(int time)
        {
            LinkedListNode<FEKeyNode> node;
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