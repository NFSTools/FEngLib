using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using FEngLib.Structures;
using FEngLib.Utils;

namespace FEngLib.Scripts;

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

public interface ITrack
{
    public TrackInterpolationMethod InterpType { get; set; }
    public byte InterpAction { get; set; }
    public uint Length { get; set; }
}

public interface ITrack<TValue> : ITrack where TValue : struct
{
    TValue BaseKey { get; set; }
    LinkedList<TrackNode<TValue>> DeltaKeys { get; set; }

    LinkedListNode<TrackNode<TValue>> GetDeltaKeyAt(int time);
}

public abstract class Track : ITrack
{
    public TrackInterpolationMethod InterpType { get; set; }
    public byte InterpAction { get; set; }
    public uint Length { get; set; }

    public abstract TrackParamType GetParamType();
    public abstract byte GetParamSize();
    public abstract void ReadKeys(BinaryReader binaryReader, uint numKeys);
    public abstract void WriteKeys(BinaryWriter binaryWriter);
}

public abstract class Track<TValue> : Track, ITrack<TValue> where TValue : struct
{
    protected Track()
    {
        DeltaKeys = new LinkedList<TrackNode<TValue>>();
    }

    public TValue BaseKey { get; set; }
    public LinkedList<TrackNode<TValue>> DeltaKeys { get; set; }

    public LinkedListNode<TrackNode<TValue>> GetDeltaKeyAt(int time)
    {
        LinkedListNode<TrackNode<TValue>> node;
        for (node = DeltaKeys.First;
             node?.Next != null && node.Value.Time < time;
             node = node.Next)
        {
            //
        }

        return node;
    }

    public override void ReadKeys(BinaryReader binaryReader, uint numKeys)
    {
        for (var i = 0; i < numKeys; i++)
        {
            var time = binaryReader.ReadInt32();

            if (i == 0)
            {
                Debug.Assert(time == -1, "time == -1");
                BaseKey = ReadKey(binaryReader);
            }
            else
            {
                Debug.Assert(time >= 0, "time >= 0");
                DeltaKeys.AddLast(new TrackNode<TValue>
                {
                    Time = time,
                    Val = ReadKey(binaryReader)
                });
            }
        }
    }

    public override void WriteKeys(BinaryWriter binaryWriter)
    {
        // Write base key
        binaryWriter.Write(-1);
        WriteKey(binaryWriter, BaseKey);

        // Write delta keys
        foreach (var deltaKey in DeltaKeys.OrderBy(k => k.Time))
        {
            binaryWriter.Write(deltaKey.Time);
            WriteKey(binaryWriter, deltaKey.Val);
        }
    }

    protected abstract TValue ReadKey(BinaryReader binaryReader);
    protected abstract void WriteKey(BinaryWriter binaryWriter, TValue value);
}

public class Vector2Track : Track<Vector2>
{
    public override TrackParamType GetParamType()
    {
        return TrackParamType.Vector2;
    }

    public override byte GetParamSize()
    {
        return 8;
    }

    protected override Vector2 ReadKey(BinaryReader binaryReader)
    {
        return binaryReader.ReadVector2();
    }

    protected override void WriteKey(BinaryWriter binaryWriter, Vector2 value)
    {
        binaryWriter.Write(value);
    }
}

public class Vector3Track : Track<Vector3>
{
    public override TrackParamType GetParamType()
    {
        return TrackParamType.Vector3;
    }

    public override byte GetParamSize()
    {
        return 12;
    }

    protected override Vector3 ReadKey(BinaryReader binaryReader)
    {
        return binaryReader.ReadVector3();
    }

    protected override void WriteKey(BinaryWriter binaryWriter, Vector3 value)
    {
        binaryWriter.Write(value);
    }
}

public class QuaternionTrack : Track<Quaternion>
{
    public override TrackParamType GetParamType()
    {
        return TrackParamType.Quaternion;
    }

    public override byte GetParamSize()
    {
        return 16;
    }

    protected override Quaternion ReadKey(BinaryReader binaryReader)
    {
        return binaryReader.ReadQuaternion();
    }

    protected override void WriteKey(BinaryWriter binaryWriter, Quaternion value)
    {
        binaryWriter.Write(value);
    }
}

public class ColorTrack : Track<Color4>
{
    public override TrackParamType GetParamType()
    {
        return TrackParamType.Color;
    }

    public override byte GetParamSize()
    {
        return 16;
    }

    protected override Color4 ReadKey(BinaryReader binaryReader)
    {
        return binaryReader.ReadColor();
    }

    protected override void WriteKey(BinaryWriter binaryWriter, Color4 value)
    {
        binaryWriter.Write(value);
    }
}