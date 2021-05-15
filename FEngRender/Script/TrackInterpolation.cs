using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Numerics;
using FEngLib.Data;
using FEngLib.Structures;
using FEngRender.Utils;

namespace FEngRender.Script
{
    /// <summary>
    /// Implements interpolation of keys in <see cref="FEKeyTrack"/> objects
    /// </summary>
    public static class TrackInterpolation
    {
        public static T Interpolate<T>(FEKeyTrack track, int time)
        {
            var nextDeltaKeyNode = track.GetDeltaKeyAt(time);

            if (nextDeltaKeyNode == null || track.DeltaKeys.Count == 0)
            {
                return (T)track.BaseKey.Val;
            }

            var nextDeltaKey = nextDeltaKeyNode.Value;
            LinkedListNode<FEKeyNode> prevDeltaKeyNode;
            float lerpFactor;

            if (track.InterpAction == 0)
            {
                prevDeltaKeyNode = nextDeltaKeyNode.Previous;

                if (prevDeltaKeyNode != null && time < nextDeltaKey.Time)
                {
                    var prevDeltaKey = prevDeltaKeyNode.Value;
                    lerpFactor = (time - prevDeltaKey.Time) / (float)(nextDeltaKey.Time - prevDeltaKey.Time);
                }
                else
                {
                    lerpFactor = 1;
                }
            }
            else if (track.InterpAction == 1)
            {
                if (time == nextDeltaKey.Time)
                {
                    prevDeltaKeyNode = nextDeltaKeyNode;
                    lerpFactor = 0;
                }
                else if (time < nextDeltaKey.Time)
                {
                    prevDeltaKeyNode = nextDeltaKeyNode.Previous;
                    if (prevDeltaKeyNode != null)
                    {
                        float divisor = nextDeltaKey.Time - prevDeltaKeyNode.Value.Time;
                        lerpFactor = (time - prevDeltaKeyNode.Value.Time) / divisor;
                    }
                    else
                    {
                        prevDeltaKeyNode = track.GetDeltaKeyAt((int)track.Length);
                        int distanceToTrackEnd = (int)track.Length - prevDeltaKeyNode.Value.Time;
                        float divisor = distanceToTrackEnd + nextDeltaKey.Time;
                        lerpFactor = (time + distanceToTrackEnd) / divisor;
                    }
                }
                else
                {
                    prevDeltaKeyNode = nextDeltaKeyNode;
                    float divisor = nextDeltaKey.Time + track.Length - prevDeltaKeyNode.Value.Time;
                    lerpFactor = (time - prevDeltaKeyNode.Value.Time) / divisor;
                }
            }
            else
            {
                throw new InvalidOperationException($"Unsupported InterpAction: {track.InterpAction}");
            }

            if (lerpFactor == 0)
            {
                return (T)AddKeys(track, track.BaseKey, prevDeltaKeyNode!.Value);
            }

            if (Math.Abs(lerpFactor - 1) < 0.000000000001f)
            {
                return (T)AddKeys(track, track.BaseKey, nextDeltaKey);
            }

            return (T)LerpKeys(track, lerpFactor, prevDeltaKeyNode!.Value, nextDeltaKey, track.BaseKey);
        }

        private static object AddKeys(FEKeyTrack track, FEKeyNode node1, FEKeyNode node2)
        {
            switch (track.ParamType)
            {
                case FEParamType.PT_Int:
                    return node1.GetValue<int>() + node2.GetValue<int>();
                case FEParamType.PT_Color:
                    {
                        var c1 = node1.GetValue<Color4>();
                        var c2 = node2.GetValue<Color4>();
                        return new Color4
                        {
                            Alpha = c1.Alpha + c2.Alpha,
                            Blue = c1.Blue + c2.Blue,
                            Green = c1.Green + c2.Green,
                            Red = c1.Red + c2.Red
                        };
                    }
                case FEParamType.PT_Vector2:
                    return node1.GetValue<Vector2>() + node2.GetValue<Vector2>();
                case FEParamType.PT_Vector3:
                    return node1.GetValue<Vector3>() + node2.GetValue<Vector3>();
                case FEParamType.PT_Quaternion:
                    return node1.GetValue<Quaternion>() * node2.GetValue<Quaternion>();
                default:
                    throw new IndexOutOfRangeException($"Unsupported ParamType for ADD: {track.ParamType}");
            }
        }

        private static object LerpKeys(FEKeyTrack track, float t, FEKeyNode node1, FEKeyNode node2, FEKeyNode offset)
        {
            switch (track.ParamType)
            {
                case FEParamType.PT_Int:
                    return LerpInteger(node1.GetValue<int>(), node2.GetValue<int>(), t, offset.GetValue<int>());
                case FEParamType.PT_Color:
                    return LerpColor(node1.GetValue<Color4>(), node2.GetValue<Color4>(), t,
                        offset.GetValue<Color4>());
                case FEParamType.PT_Vector2:
                    return Vector2.Lerp(node1.GetValue<Vector2>(), node2.GetValue<Vector2>(), t) +
                           offset.GetValue<Vector2>();
                case FEParamType.PT_Vector3:
                    return Vector3.Lerp(node1.GetValue<Vector3>(), node2.GetValue<Vector3>(), t) +
                           offset.GetValue<Vector3>();
                case FEParamType.PT_Quaternion:
                    return Quaternion.Lerp(node1.GetValue<Quaternion>(), node2.GetValue<Quaternion>(), t) *
                           offset.GetValue<Quaternion>();
                default:
                    throw new IndexOutOfRangeException($"Unsupported ParamType for LERP: {track.ParamType}");
            }
        }

        private static Color4 LerpColor(Color4 c1, Color4 c2, float t, Color4 offset)
        {
            return new Color4
            {
                Blue = offset.Blue + c1.Blue + (int)((c2.Blue - c1.Blue) * t + 0.5f),
                Green = offset.Green + c1.Green + (int)((c2.Green - c1.Green) * t + 0.5f),
                Red = offset.Red + c1.Red + (int)((c2.Red - c1.Red) * t + 0.5f),
                Alpha = offset.Alpha + c1.Alpha + (int)((c2.Alpha - c1.Alpha) * t + 0.5f),
            };
        }

        private static int LerpInteger(int n1, int n2, float t, int offset)
        {
            return (int)(n2 + offset + ((n1 - n2) * t + 0.5f));
        }
    }
}