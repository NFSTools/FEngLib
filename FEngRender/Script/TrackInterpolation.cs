using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using FEngLib.Data;
using FEngLib.Structures;

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
                        var c1 = node1.GetValue<FEColor>();
                        var c2 = node2.GetValue<FEColor>();
                        return new FEColor
                        {
                            Alpha = c1.Alpha + c2.Alpha,
                            Blue = c1.Blue + c2.Blue,
                            Green = c1.Green + c2.Green,
                            Red = c1.Red + c2.Red
                        };
                    }
                case FEParamType.PT_Vector2:
                    {
                        var v1 = node1.GetValue<FEVector2>();
                        var v2 = node2.GetValue<FEVector2>();

                        return new FEVector2
                        {
                            X = v1.X + v2.X,
                            Y = v1.Y + v2.Y
                        };
                    }
                case FEParamType.PT_Vector3:
                    {
                        var v1 = node1.GetValue<FEVector3>();
                        var v2 = node2.GetValue<FEVector3>();

                        return new FEVector3
                        {
                            X = v1.X + v2.X,
                            Y = v1.Y + v2.Y,
                            Z = v1.Z + v2.Z,
                        };
                    }
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
                    return LerpColor(node1.GetValue<FEColor>(), node2.GetValue<FEColor>(), t,
                        offset.GetValue<FEColor>());
                case FEParamType.PT_Vector2:
                    return LerpVector2(node1.GetValue<FEVector2>(), node2.GetValue<FEVector2>(), t,
                        offset.GetValue<FEVector2>());
                case FEParamType.PT_Vector3:
                    return LerpVector3(node1.GetValue<FEVector3>(), node2.GetValue<FEVector3>(), t,
                        offset.GetValue<FEVector3>());
                default:
                    throw new IndexOutOfRangeException($"Unsupported ParamType for LERP: {track.ParamType}");
            }
        }

        private static FEVector3 LerpVector3(FEVector3 v1, FEVector3 v2, float t, FEVector3 offset)
        {
            return new FEVector3
            {
                X = (v2.X - v1.X) * t + offset.X + v1.X,
                Y = (v2.Y - v1.Y) * t + offset.Y + v1.Y,
                Z = (v2.Z - v1.Z) * t + offset.Z + v1.Z,
            };
        }

        private static FEVector2 LerpVector2(FEVector2 v1, FEVector2 v2, float t, FEVector2 offset)
        {
            return new FEVector2
            {
                X = (v2.X - v1.X) * t + offset.X + v1.X,
                Y = (v2.Y - v1.Y) * t + offset.Y + v1.Y
            };
        }

        private static FEColor LerpColor(FEColor c1, FEColor c2, float t, FEColor offset)
        {
            return new FEColor
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