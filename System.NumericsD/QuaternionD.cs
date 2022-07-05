// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Globalization;

// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable CompareOfFloatsByEqualityOperator
// ReSharper disable once CheckNamespace
namespace System.Numerics
{
    /// <summary>
    /// A structure encapsulating a four-dimensional vector (x,y,z,w), 
    /// which is used to efficiently rotate an object about the (x,y,z) vector by the angle theta, where w = cos(theta/2).
    /// </summary>
    public struct QuaternionD : IEquatable<QuaternionD>
    {
        /// <summary>
        /// Specifies the X-value of the vector component of the QuaternionD.
        /// </summary>
        public double X;
        /// <summary>
        /// Specifies the Y-value of the vector component of the QuaternionD.
        /// </summary>
        public double Y;
        /// <summary>
        /// Specifies the Z-value of the vector component of the QuaternionD.
        /// </summary>
        public double Z;
        /// <summary>
        /// Specifies the rotation component of the QuaternionD.
        /// </summary>
        public double W;

        /// <summary>
        /// Returns a QuaternionD representing no rotation. 
        /// </summary>
        public static QuaternionD Identity => new QuaternionD(0.0, 0.0, 0.0, 1.0);

        /// <summary>
        /// Returns whether the QuaternionD is the identity QuaternionD.
        /// </summary>
        public bool IsIdentity => X == 0.0 && Y == 0.0 && Z == 0.0 && W == 1.0;

        /// <summary>
        /// Constructs a QuaternionD from the given components.
        /// </summary>
        /// <param name="x">The X component of the QuaternionD.</param>
        /// <param name="y">The Y component of the QuaternionD.</param>
        /// <param name="z">The Z component of the QuaternionD.</param>
        /// <param name="w">The W component of the QuaternionD.</param>
        public QuaternionD(double x, double y, double z, double w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        /// <summary>
        /// Constructs a QuaternionD from the given vector and rotation parts.
        /// </summary>
        /// <param name="vectorPart">The vector part of the QuaternionD.</param>
        /// <param name="scalarPart">The rotation part of the QuaternionD.</param>
        public QuaternionD(Vector3D vectorPart, double scalarPart)
        {
            X = vectorPart.X;
            Y = vectorPart.Y;
            Z = vectorPart.Z;
            W = scalarPart;
        }

        /// <summary>
        /// Calculates the length of the QuaternionD.
        /// </summary>
        /// <returns>The computed length of the QuaternionD.</returns>
        public double Length()
        {
            var ls = X * X + Y * Y + Z * Z + W * W;

            return Math.Sqrt(ls);
        }

        /// <summary>
        /// Calculates the length squared of the QuaternionD. This operation is cheaper than Length().
        /// </summary>
        /// <returns>The length squared of the QuaternionD.</returns>
        public double LengthSquared()
        {
            return X * X + Y * Y + Z * Z + W * W;
        }

        /// <summary>
        /// Divides each component of the QuaternionD by the length of the QuaternionD.
        /// </summary>
        /// <param name="value">The source QuaternionD.</param>
        /// <returns>The normalized QuaternionD.</returns>
        public static QuaternionD Normalize(QuaternionD value)
        {
            QuaternionD ans;

            var ls = value.X * value.X + value.Y * value.Y + value.Z * value.Z + value.W * value.W;

            var invNorm = 1.0 / Math.Sqrt(ls);

            ans.X = value.X * invNorm;
            ans.Y = value.Y * invNorm;
            ans.Z = value.Z * invNorm;
            ans.W = value.W * invNorm;

            return ans;
        }

        /// <summary>
        /// Creates the conjugate of a specified QuaternionD.
        /// </summary>
        /// <param name="value">The QuaternionD of which to return the conjugate.</param>
        /// <returns>A new QuaternionD that is the conjugate of the specified one.</returns>
        public static QuaternionD Conjugate(QuaternionD value)
        {
            QuaternionD ans;

            ans.X = -value.X;
            ans.Y = -value.Y;
            ans.Z = -value.Z;
            ans.W = value.W;

            return ans;
        }

        /// <summary>
        /// Returns the inverse of a QuaternionD.
        /// </summary>
        /// <param name="value">The source QuaternionD.</param>
        /// <returns>The inverted QuaternionD.</returns>
        public static QuaternionD Inverse(QuaternionD value)
        {
            //  -1   (       a              -v       )
            // q   = ( -------------   ------------- )
            //       (  a^2 + |v|^2  ,  a^2 + |v|^2  )

            QuaternionD ans;

            var ls = value.X * value.X + value.Y * value.Y + value.Z * value.Z + value.W * value.W;
            var invNorm = 1.0 / ls;

            ans.X = -value.X * invNorm;
            ans.Y = -value.Y * invNorm;
            ans.Z = -value.Z * invNorm;
            ans.W = value.W * invNorm;

            return ans;
        }

        /// <summary>
        /// Creates a QuaternionD from a vector and an angle to rotate about the vector.
        /// </summary>
        /// <param name="axis">The vector to rotate around.</param>
        /// <param name="angle">The angle, in radians, to rotate around the vector.</param>
        /// <returns>The created QuaternionD.</returns>
        public static QuaternionD CreateFromAxisAngle(Vector3D axis, double angle)
        {
            QuaternionD ans;

            var halfAngle = angle * 0.5;
            var s = Math.Sin(halfAngle);
            var c = Math.Cos(halfAngle);

            ans.X = axis.X * s;
            ans.Y = axis.Y * s;
            ans.Z = axis.Z * s;
            ans.W = c;

            return ans;
        }

        /// <summary>
        /// Creates a new QuaternionD from the given yaw, pitch, and roll, in radians.
        /// </summary>
        /// <param name="yaw">The yaw angle, in radians, around the Y-axis.</param>
        /// <param name="pitch">The pitch angle, in radians, around the X-axis.</param>
        /// <param name="roll">The roll angle, in radians, around the Z-axis.</param>
        /// <returns></returns>
        public static QuaternionD CreateFromYawPitchRoll(double yaw, double pitch, double roll)
        {
            //  Roll first, about axis the object is facing, then
            //  pitch upward, then yaw to face into the new heading

            var halfRoll = roll * 0.5;
            var sr = Math.Sin(halfRoll);
            var cr = Math.Cos(halfRoll);

            var halfPitch = pitch * 0.5;
            var sp = Math.Sin(halfPitch);
            var cp = Math.Cos(halfPitch);

            var halfYaw = yaw * 0.5;
            var sy = Math.Sin(halfYaw);
            var cy = Math.Cos(halfYaw);

            QuaternionD result;

            result.X = cy * sp * cr + sy * cp * sr;
            result.Y = sy * cp * cr - cy * sp * sr;
            result.Z = cy * cp * sr - sy * sp * cr;
            result.W = cy * cp * cr + sy * sp * sr;

            return result;
        }

        /// <summary>
        /// Creates a QuaternionD from the given rotation matrix.
        /// </summary>
        /// <param name="matrix">The rotation matrix.</param>
        /// <returns>The created QuaternionD.</returns>
        public static QuaternionD CreateFromRotationMatrix(Matrix4x4D matrix)
        {
            var trace = matrix.M11 + matrix.M22 + matrix.M33;

            var q = new QuaternionD();

            if (trace > 0.0)
            {
                var s = Math.Sqrt(trace + 1.0);
                q.W = s * 0.5;
                s = 0.5 / s;
                q.X = (matrix.M23 - matrix.M32) * s;
                q.Y = (matrix.M31 - matrix.M13) * s;
                q.Z = (matrix.M12 - matrix.M21) * s;
            }
            else
            {
                if (matrix.M11 >= matrix.M22 && matrix.M11 >= matrix.M33)
                {
                    var s = Math.Sqrt(1.0 + matrix.M11 - matrix.M22 - matrix.M33);
                    var invS = 0.5 / s;
                    q.X = 0.5 * s;
                    q.Y = (matrix.M12 + matrix.M21) * invS;
                    q.Z = (matrix.M13 + matrix.M31) * invS;
                    q.W = (matrix.M23 - matrix.M32) * invS;
                }
                else if (matrix.M22 > matrix.M33)
                {
                    var s = Math.Sqrt(1.0 + matrix.M22 - matrix.M11 - matrix.M33);
                    var invS = 0.5 / s;
                    q.X = (matrix.M21 + matrix.M12) * invS;
                    q.Y = 0.5 * s;
                    q.Z = (matrix.M32 + matrix.M23) * invS;
                    q.W = (matrix.M31 - matrix.M13) * invS;
                }
                else
                {
                    var s = Math.Sqrt(1.0 + matrix.M33 - matrix.M11 - matrix.M22);
                    var invS = 0.5 / s;
                    q.X = (matrix.M31 + matrix.M13) * invS;
                    q.Y = (matrix.M32 + matrix.M23) * invS;
                    q.Z = 0.5 * s;
                    q.W = (matrix.M12 - matrix.M21) * invS;
                }
            }

            return q;
        }

        /// <summary>
        /// Calculates the dot product of two QuaternionDs.
        /// </summary>
        /// <param name="quaternion1">The first source QuaternionD.</param>
        /// <param name="quaternion2">The second source QuaternionD.</param>
        /// <returns>The dot product of the QuaternionDs.</returns>
        public static double Dot(QuaternionD quaternion1, QuaternionD quaternion2)
        {
            return quaternion1.X * quaternion2.X +
                   quaternion1.Y * quaternion2.Y +
                   quaternion1.Z * quaternion2.Z +
                   quaternion1.W * quaternion2.W;
        }

        /// <summary>
        /// Interpolates between two quaternions, using spherical linear interpolation.
        /// </summary>
        /// <param name="quaternion1">The first source QuaternionD.</param>
        /// <param name="quaternion2">The second source QuaternionD.</param>
        /// <param name="amount">The relative weight of the second source QuaternionD in the interpolation.</param>
        /// <returns>The interpolated QuaternionD.</returns>
        public static QuaternionD Slerp(QuaternionD quaternion1, QuaternionD quaternion2, double amount)
        {
            const double epsilon = 1e-6;

            var t = amount;

            var cosOmega = quaternion1.X * quaternion2.X + quaternion1.Y * quaternion2.Y +
                           quaternion1.Z * quaternion2.Z + quaternion1.W * quaternion2.W;

            var flip = false;

            if (cosOmega < 0.0)
            {
                flip = true;
                cosOmega = -cosOmega;
            }

            double s1, s2;

            if (cosOmega > (1.0 - epsilon))
            {
                // Too close, do straight linear interpolation.
                s1 = 1.0 - t;
                s2 = flip ? -t : t;
            }
            else
            {
                var omega = Math.Acos(cosOmega);
                var invSinOmega = (1.0 / Math.Sin(omega));

                s1 = Math.Sin((1.0 - t) * omega) * invSinOmega;
                s2 = flip
                    ? -Math.Sin(t * omega) * invSinOmega
                    : Math.Sin(t * omega) * invSinOmega;
            }

            QuaternionD ans;

            ans.X = s1 * quaternion1.X + s2 * quaternion2.X;
            ans.Y = s1 * quaternion1.Y + s2 * quaternion2.Y;
            ans.Z = s1 * quaternion1.Z + s2 * quaternion2.Z;
            ans.W = s1 * quaternion1.W + s2 * quaternion2.W;

            return ans;
        }

        /// <summary>
        ///  Linearly interpolates between two quaternions.
        /// </summary>
        /// <param name="quaternion1">The first source QuaternionD.</param>
        /// <param name="quaternion2">The second source QuaternionD.</param>
        /// <param name="amount">The relative weight of the second source QuaternionD in the interpolation.</param>
        /// <returns>The interpolated QuaternionD.</returns>
        public static QuaternionD Lerp(QuaternionD quaternion1, QuaternionD quaternion2, double amount)
        {
            var t = amount;
            var t1 = 1.0 - t;

            var r = new QuaternionD();

            var dot = quaternion1.X * quaternion2.X + quaternion1.Y * quaternion2.Y +
                      quaternion1.Z * quaternion2.Z + quaternion1.W * quaternion2.W;

            if (dot >= 0.0)
            {
                r.X = t1 * quaternion1.X + t * quaternion2.X;
                r.Y = t1 * quaternion1.Y + t * quaternion2.Y;
                r.Z = t1 * quaternion1.Z + t * quaternion2.Z;
                r.W = t1 * quaternion1.W + t * quaternion2.W;
            }
            else
            {
                r.X = t1 * quaternion1.X - t * quaternion2.X;
                r.Y = t1 * quaternion1.Y - t * quaternion2.Y;
                r.Z = t1 * quaternion1.Z - t * quaternion2.Z;
                r.W = t1 * quaternion1.W - t * quaternion2.W;
            }

            // Normalize it.
            var ls = r.X * r.X + r.Y * r.Y + r.Z * r.Z + r.W * r.W;
            var invNorm = 1.0 / Math.Sqrt(ls);

            r.X *= invNorm;
            r.Y *= invNorm;
            r.Z *= invNorm;
            r.W *= invNorm;

            return r;
        }

        /// <summary>
        /// Concatenates two QuaternionDs; the result represents the value1 rotation followed by the value2 rotation.
        /// </summary>
        /// <param name="value1">The first QuaternionD rotation in the series.</param>
        /// <param name="value2">The second QuaternionD rotation in the series.</param>
        /// <returns>A new QuaternionD representing the concatenation of the value1 rotation followed by the value2 rotation.</returns>
        public static QuaternionD Concatenate(QuaternionD value1, QuaternionD value2)
        {
            QuaternionD ans;

            // Concatenate rotation is actually q2 * q1 instead of q1 * q2.
            // So that's why value2 goes q1 and value1 goes q2.
            var q1x = value2.X;
            var q1y = value2.Y;
            var q1z = value2.Z;
            var q1w = value2.W;

            var q2x = value1.X;
            var q2y = value1.Y;
            var q2z = value1.Z;
            var q2w = value1.W;

            // cross(av, bv)
            var cx = q1y * q2z - q1z * q2y;
            var cy = q1z * q2x - q1x * q2z;
            var cz = q1x * q2y - q1y * q2x;

            var dot = q1x * q2x + q1y * q2y + q1z * q2z;

            ans.X = q1x * q2w + q2x * q1w + cx;
            ans.Y = q1y * q2w + q2y * q1w + cy;
            ans.Z = q1z * q2w + q2z * q1w + cz;
            ans.W = q1w * q2w - dot;

            return ans;
        }

        /// <summary>
        /// Flips the sign of each component of the quaternion.
        /// </summary>
        /// <param name="value">The source QuaternionD.</param>
        /// <returns>The negated QuaternionD.</returns>
        public static QuaternionD Negate(QuaternionD value)
        {
            QuaternionD ans;

            ans.X = -value.X;
            ans.Y = -value.Y;
            ans.Z = -value.Z;
            ans.W = -value.W;

            return ans;
        }

        /// <summary>
        /// Adds two QuaternionDs element-by-element.
        /// </summary>
        /// <param name="value1">The first source QuaternionD.</param>
        /// <param name="value2">The second source QuaternionD.</param>
        /// <returns>The result of adding the QuaternionDs.</returns>
        public static QuaternionD Add(QuaternionD value1, QuaternionD value2)
        {
            QuaternionD ans;

            ans.X = value1.X + value2.X;
            ans.Y = value1.Y + value2.Y;
            ans.Z = value1.Z + value2.Z;
            ans.W = value1.W + value2.W;

            return ans;
        }

        /// <summary>
        /// Subtracts one QuaternionD from another.
        /// </summary>
        /// <param name="value1">The first source QuaternionD.</param>
        /// <param name="value2">The second QuaternionD, to be subtracted from the first.</param>
        /// <returns>The result of the subtraction.</returns>
        public static QuaternionD Subtract(QuaternionD value1, QuaternionD value2)
        {
            QuaternionD ans;

            ans.X = value1.X - value2.X;
            ans.Y = value1.Y - value2.Y;
            ans.Z = value1.Z - value2.Z;
            ans.W = value1.W - value2.W;

            return ans;
        }

        /// <summary>
        /// Multiplies two QuaternionDs together.
        /// </summary>
        /// <param name="value1">The QuaternionD on the left side of the multiplication.</param>
        /// <param name="value2">The QuaternionD on the right side of the multiplication.</param>
        /// <returns>The result of the multiplication.</returns>
        public static QuaternionD Multiply(QuaternionD value1, QuaternionD value2)
        {
            QuaternionD ans;

            var q1x = value1.X;
            var q1y = value1.Y;
            var q1z = value1.Z;
            var q1w = value1.W;

            var q2x = value2.X;
            var q2y = value2.Y;
            var q2z = value2.Z;
            var q2w = value2.W;

            // cross(av, bv)
            var cx = q1y * q2z - q1z * q2y;
            var cy = q1z * q2x - q1x * q2z;
            var cz = q1x * q2y - q1y * q2x;

            var dot = q1x * q2x + q1y * q2y + q1z * q2z;

            ans.X = q1x * q2w + q2x * q1w + cx;
            ans.Y = q1y * q2w + q2y * q1w + cy;
            ans.Z = q1z * q2w + q2z * q1w + cz;
            ans.W = q1w * q2w - dot;

            return ans;
        }

        /// <summary>
        /// Multiplies a QuaternionD by a scalar value.
        /// </summary>
        /// <param name="value1">The source QuaternionD.</param>
        /// <param name="value2">The scalar value.</param>
        /// <returns>The result of the multiplication.</returns>
        public static QuaternionD Multiply(QuaternionD value1, double value2)
        {
            QuaternionD ans;

            ans.X = value1.X * value2;
            ans.Y = value1.Y * value2;
            ans.Z = value1.Z * value2;
            ans.W = value1.W * value2;

            return ans;
        }

        /// <summary>
        /// Divides a QuaternionD by another QuaternionD.
        /// </summary>
        /// <param name="value1">The source QuaternionD.</param>
        /// <param name="value2">The divisor.</param>
        /// <returns>The result of the division.</returns>
        public static QuaternionD Divide(QuaternionD value1, QuaternionD value2)
        {
            QuaternionD ans;

            var q1x = value1.X;
            var q1y = value1.Y;
            var q1z = value1.Z;
            var q1w = value1.W;

            //-------------------------------------
            // Inverse part.
            var ls = value2.X * value2.X + value2.Y * value2.Y +
                     value2.Z * value2.Z + value2.W * value2.W;
            var invNorm = 1.0 / ls;

            var q2x = -value2.X * invNorm;
            var q2y = -value2.Y * invNorm;
            var q2z = -value2.Z * invNorm;
            var q2w = value2.W * invNorm;

            //-------------------------------------
            // Multiply part.

            // cross(av, bv)
            var cx = q1y * q2z - q1z * q2y;
            var cy = q1z * q2x - q1x * q2z;
            var cz = q1x * q2y - q1y * q2x;

            var dot = q1x * q2x + q1y * q2y + q1z * q2z;

            ans.X = q1x * q2w + q2x * q1w + cx;
            ans.Y = q1y * q2w + q2y * q1w + cy;
            ans.Z = q1z * q2w + q2z * q1w + cz;
            ans.W = q1w * q2w - dot;

            return ans;
        }

        /// <summary>
        /// Flips the sign of each component of the quaternion.
        /// </summary>
        /// <param name="value">The source QuaternionD.</param>
        /// <returns>The negated QuaternionD.</returns>
        public static QuaternionD operator -(QuaternionD value)
        {
            QuaternionD ans;

            ans.X = -value.X;
            ans.Y = -value.Y;
            ans.Z = -value.Z;
            ans.W = -value.W;

            return ans;
        }

        /// <summary>
        /// Adds two QuaternionDs element-by-element.
        /// </summary>
        /// <param name="value1">The first source QuaternionD.</param>
        /// <param name="value2">The second source QuaternionD.</param>
        /// <returns>The result of adding the QuaternionDs.</returns>
        public static QuaternionD operator +(QuaternionD value1, QuaternionD value2)
        {
            QuaternionD ans;

            ans.X = value1.X + value2.X;
            ans.Y = value1.Y + value2.Y;
            ans.Z = value1.Z + value2.Z;
            ans.W = value1.W + value2.W;

            return ans;
        }

        /// <summary>
        /// Subtracts one QuaternionD from another.
        /// </summary>
        /// <param name="value1">The first source QuaternionD.</param>
        /// <param name="value2">The second QuaternionD, to be subtracted from the first.</param>
        /// <returns>The result of the subtraction.</returns>
        public static QuaternionD operator -(QuaternionD value1, QuaternionD value2)
        {
            QuaternionD ans;

            ans.X = value1.X - value2.X;
            ans.Y = value1.Y - value2.Y;
            ans.Z = value1.Z - value2.Z;
            ans.W = value1.W - value2.W;

            return ans;
        }

        /// <summary>
        /// Multiplies two QuaternionDs together.
        /// </summary>
        /// <param name="value1">The QuaternionD on the left side of the multiplication.</param>
        /// <param name="value2">The QuaternionD on the right side of the multiplication.</param>
        /// <returns>The result of the multiplication.</returns>
        public static QuaternionD operator *(QuaternionD value1, QuaternionD value2)
        {
            QuaternionD ans;

            var q1x = value1.X;
            var q1y = value1.Y;
            var q1z = value1.Z;
            var q1w = value1.W;

            var q2x = value2.X;
            var q2y = value2.Y;
            var q2z = value2.Z;
            var q2w = value2.W;

            // cross(av, bv)
            var cx = q1y * q2z - q1z * q2y;
            var cy = q1z * q2x - q1x * q2z;
            var cz = q1x * q2y - q1y * q2x;

            var dot = q1x * q2x + q1y * q2y + q1z * q2z;

            ans.X = q1x * q2w + q2x * q1w + cx;
            ans.Y = q1y * q2w + q2y * q1w + cy;
            ans.Z = q1z * q2w + q2z * q1w + cz;
            ans.W = q1w * q2w - dot;

            return ans;
        }

        /// <summary>
        /// Multiplies a QuaternionD by a scalar value.
        /// </summary>
        /// <param name="value1">The source QuaternionD.</param>
        /// <param name="value2">The scalar value.</param>
        /// <returns>The result of the multiplication.</returns>
        public static QuaternionD operator *(QuaternionD value1, double value2)
        {
            QuaternionD ans;

            ans.X = value1.X * value2;
            ans.Y = value1.Y * value2;
            ans.Z = value1.Z * value2;
            ans.W = value1.W * value2;

            return ans;
        }

        /// <summary>
        /// Divides a QuaternionD by another QuaternionD.
        /// </summary>
        /// <param name="value1">The source QuaternionD.</param>
        /// <param name="value2">The divisor.</param>
        /// <returns>The result of the division.</returns>
        public static QuaternionD operator /(QuaternionD value1, QuaternionD value2)
        {
            QuaternionD ans;

            var q1x = value1.X;
            var q1y = value1.Y;
            var q1z = value1.Z;
            var q1w = value1.W;

            //-------------------------------------
            // Inverse part.
            var ls = value2.X * value2.X + value2.Y * value2.Y +
                     value2.Z * value2.Z + value2.W * value2.W;
            var invNorm = 1.0 / ls;

            var q2x = -value2.X * invNorm;
            var q2y = -value2.Y * invNorm;
            var q2z = -value2.Z * invNorm;
            var q2w = value2.W * invNorm;

            //-------------------------------------
            // Multiply part.

            // cross(av, bv)
            var cx = q1y * q2z - q1z * q2y;
            var cy = q1z * q2x - q1x * q2z;
            var cz = q1x * q2y - q1y * q2x;

            var dot = q1x * q2x + q1y * q2y + q1z * q2z;

            ans.X = q1x * q2w + q2x * q1w + cx;
            ans.Y = q1y * q2w + q2y * q1w + cy;
            ans.Z = q1z * q2w + q2z * q1w + cz;
            ans.W = q1w * q2w - dot;

            return ans;
        }

        /// <summary>
        /// Returns a boolean indicating whether the two given QuaternionDs are equal.
        /// </summary>
        /// <param name="value1">The first QuaternionD to compare.</param>
        /// <param name="value2">The second QuaternionD to compare.</param>
        /// <returns>True if the QuaternionDs are equal; False otherwise.</returns>
        public static bool operator ==(QuaternionD value1, QuaternionD value2)
        {
            return (value1.X == value2.X &&
                    value1.Y == value2.Y &&
                    value1.Z == value2.Z &&
                    value1.W == value2.W);
        }

        /// <summary>
        /// Returns a boolean indicating whether the two given QuaternionDs are not equal.
        /// </summary>
        /// <param name="value1">The first QuaternionD to compare.</param>
        /// <param name="value2">The second QuaternionD to compare.</param>
        /// <returns>True if the QuaternionDs are not equal; False if they are equal.</returns>
        public static bool operator !=(QuaternionD value1, QuaternionD value2)
        {
            return (value1.X != value2.X ||
                    value1.Y != value2.Y ||
                    value1.Z != value2.Z ||
                    value1.W != value2.W);
        }

        /// <summary>
        /// Returns a boolean indicating whether the given QuaternionD is equal to this QuaternionD instance.
        /// </summary>
        /// <param name="other">The QuaternionD to compare this instance to.</param>
        /// <returns>True if the other QuaternionD is equal to this instance; False otherwise.</returns>
        public bool Equals(QuaternionD other)
        {
            return (X == other.X &&
                    Y == other.Y &&
                    Z == other.Z &&
                    W == other.W);
        }

        /// <summary>
        /// Returns a boolean indicating whether the given Object is equal to this QuaternionD instance.
        /// </summary>
        /// <param name="obj">The Object to compare against.</param>
        /// <returns>True if the Object is equal to this QuaternionD; False otherwise.</returns>
        public override bool Equals(object obj)
        {
            return obj is QuaternionD quat && Equals(quat);
        }

        /// <summary>
        /// Returns a String representing this QuaternionD instance.
        /// </summary>
        /// <returns>The string representation.</returns>
        public override string ToString()
        {
            var ci = CultureInfo.CurrentCulture;

            return string.Format(ci, "{{X:{0} Y:{1} Z:{2} W:{3}}}", X.ToString(ci), Y.ToString(ci), Z.ToString(ci), W.ToString(ci));
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>The hash code.</returns>
        public override int GetHashCode()
        {
            return X.GetHashCode() + Y.GetHashCode() + Z.GetHashCode() + W.GetHashCode();
        }
    }
}
