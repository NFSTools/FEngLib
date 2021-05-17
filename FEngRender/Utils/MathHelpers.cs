using System;
using System.Numerics;

namespace FEngRender.Utils
{
    /// <summary>
    /// Useful mathematical functions
    /// </summary>
    public static class MathHelpers
    {
        /// <summary>
        /// Computes Euler angles from a <see cref="Quaternion"/>.
        /// </summary>
        /// <param name="q"></param>
        /// <returns></returns>
        public static EulerAngles QuaternionToEuler(Quaternion q)
        {
            var sinr_cosp = 2 * (q.W * q.X + q.Y * q.Z);
            var cosr_cosp = 1 - 2 * (q.X * q.X + q.Y * q.Y);
            var roll = Math.Atan2(sinr_cosp, cosr_cosp);

            var sinp = 2 * (q.W * q.Y - q.Z * q.X);
            var pitch = Math.Abs(sinp) >= 1 ? Math.CopySign(Math.PI / 2, sinp) : Math.Asin(sinp);

            var siny_cosp = 2 * (q.W * q.Z + q.X * q.Y);
            var cosy_cosp = 1 - 2 * (q.Y * q.Y + q.Z * q.Z);
            var yaw = Math.Atan2(siny_cosp, cosy_cosp);

            return new EulerAngles(roll, pitch, yaw);
        }

        /// <summary>
        /// A structure holding roll, pitch and yaw values.
        /// </summary>
        public readonly struct EulerAngles
        {
            public readonly double Roll;
            public readonly double Pitch;
            public readonly double Yaw;

            public EulerAngles(double roll, double pitch, double yaw)
            {
                Roll = roll;
                Pitch = pitch;
                Yaw = yaw;
            }
        }
    }
}