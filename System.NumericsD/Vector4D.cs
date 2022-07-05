// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;

// ReSharper disable IdentifierTypo
// ReSharper disable once CheckNamespace
namespace System.Numerics
{
    /// <summary>
    /// A structure encapsulating four single precision floating point values and provides hardware accelerated methods.
    /// </summary>
    public partial struct Vector4D : IEquatable<Vector4D>, IFormattable
    {
        #region Public Static Properties
        /// <summary>
        /// Returns the vector (0,0,0,0).
        /// </summary>
        public static Vector4D Zero => new Vector4D();

        /// <summary>
        /// Returns the vector (1,1,1,1).
        /// </summary>
        public static Vector4D One => new Vector4D(1.0, 1.0, 1.0, 1.0);

        /// <summary>
        /// Returns the vector (1,0,0,0).
        /// </summary>
        public static Vector4D UnitX => new Vector4D(1.0, 0.0, 0.0, 0.0);

        /// <summary>
        /// Returns the vector (0,1,0,0).
        /// </summary>
        public static Vector4D UnitY => new Vector4D(0.0, 1.0, 0.0, 0.0);

        /// <summary>
        /// Returns the vector (0,0,1,0).
        /// </summary>
        public static Vector4D UnitZ => new Vector4D(0.0, 0.0, 1.0, 0.0);

        /// <summary>
        /// Returns the vector (0,0,0,1).
        /// </summary>
        public static Vector4D UnitW => new Vector4D(0.0, 0.0, 0.0, 1.0);

        #endregion Public Static Properties

        #region Public instance methods
        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>The hash code.</returns>
        public override int GetHashCode()
        {
            var hash = this.X.GetHashCode();
            hash = HashCodeHelper.CombineHashCodes(hash, this.Y.GetHashCode());
            hash = HashCodeHelper.CombineHashCodes(hash, this.Z.GetHashCode());
            hash = HashCodeHelper.CombineHashCodes(hash, this.W.GetHashCode());
            return hash;
        }

        /// <summary>
        /// Returns a boolean indicating whether the given Object is equal to this Vector4D instance.
        /// </summary>
        /// <param name="obj">The Object to compare against.</param>
        /// <returns>True if the Object is equal to this Vector4D; False otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj)
        {
            return obj is Vector4D vector && Equals(vector);
        }

        /// <summary>
        /// Returns a String representing this Vector4D instance.
        /// </summary>
        /// <returns>The string representation.</returns>
        public override string ToString()
        {
            return ToString("G", CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Returns a String representing this Vector4D instance, using the specified format to format individual elements.
        /// </summary>
        /// <param name="format">The format of individual elements.</param>
        /// <returns>The string representation.</returns>
        public string ToString(string format)
        {
            return ToString(format, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Returns a String representing this Vector4D instance, using the specified format to format individual elements 
        /// and the given IFormatProvider.
        /// </summary>
        /// <param name="format">The format of individual elements.</param>
        /// <param name="formatProvider">The format provider to use when formatting elements.</param>
        /// <returns>The string representation.</returns>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            var sb = new StringBuilder();
            var separator = NumberFormatInfo.GetInstance(formatProvider).NumberGroupSeparator;
            sb.Append('<');
            sb.Append(this.X.ToString(format, formatProvider));
            sb.Append(separator);
            sb.Append(' ');
            sb.Append(this.Y.ToString(format, formatProvider));
            sb.Append(separator);
            sb.Append(' ');
            sb.Append(this.Z.ToString(format, formatProvider));
            sb.Append(separator);
            sb.Append(' ');
            sb.Append(this.W.ToString(format, formatProvider));
            sb.Append('>');
            return sb.ToString();
        }

        /// <summary>
        /// Returns the length of the vector. This operation is cheaper than Length().
        /// </summary>
        /// <returns>The vector's length.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double Length()
        {
            if (Vector.IsHardwareAccelerated)
            {
                var ls = Dot(this, this);
                return Math.Sqrt(ls);
            }
            else
            {
                var ls = X * X + Y * Y + Z * Z + W * W;

                return Math.Sqrt(ls);
            }
        }

        /// <summary>
        /// Returns the length of the vector squared.
        /// </summary>
        /// <returns>The vector's length squared.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double LengthSquared()
        {
            if (Vector.IsHardwareAccelerated)
            {
                return Dot(this, this);
            }
            else
            {
                return X * X + Y * Y + Z * Z + W * W;
            }
        }
        #endregion Public Instance Methods

        #region Public Static Methods
        /// <summary>
        /// Returns the Euclidean distance between the two given points.
        /// </summary>
        /// <param name="value1">The first point.</param>
        /// <param name="value2">The second point.</param>
        /// <returns>The distance.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Distance(Vector4D value1, Vector4D value2)
        {
            if (Vector.IsHardwareAccelerated)
            {
                var difference = value1 - value2;
                var ls = Dot(difference, difference);
                return Math.Sqrt(ls);
            }
            else
            {
                var dx = value1.X - value2.X;
                var dy = value1.Y - value2.Y;
                var dz = value1.Z - value2.Z;
                var dw = value1.W - value2.W;

                var ls = dx * dx + dy * dy + dz * dz + dw * dw;

                return Math.Sqrt(ls);
            }
        }

        /// <summary>
        /// Returns the Euclidean distance squared between the two given points.
        /// </summary>
        /// <param name="value1">The first point.</param>
        /// <param name="value2">The second point.</param>
        /// <returns>The distance squared.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double DistanceSquared(Vector4D value1, Vector4D value2)
        {
            if (Vector.IsHardwareAccelerated)
            {
                var difference = value1 - value2;
                return Dot(difference, difference);
            }
            else
            {
                var dx = value1.X - value2.X;
                var dy = value1.Y - value2.Y;
                var dz = value1.Z - value2.Z;
                var dw = value1.W - value2.W;

                return dx * dx + dy * dy + dz * dz + dw * dw;
            }
        }

        /// <summary>
        /// Returns a vector with the same direction as the given vector, but with a length of 1.
        /// </summary>
        /// <param name="vector">The vector to normalize.</param>
        /// <returns>The normalized vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4D Normalize(Vector4D vector)
        {
            if (Vector.IsHardwareAccelerated)
            {
                var length = vector.Length();
                return vector / length;
            }
            else
            {
                var ls = vector.X * vector.X + vector.Y * vector.Y + vector.Z * vector.Z + vector.W * vector.W;
                var invNorm = 1.0 / Math.Sqrt(ls);

                return new Vector4D(
                    vector.X * invNorm,
                    vector.Y * invNorm,
                    vector.Z * invNorm,
                    vector.W * invNorm);
            }
        }

        /// <summary>
        /// Restricts a vector between a min and max value.
        /// </summary>
        /// <param name="value1">The source vector.</param>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        /// <returns>The restricted vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4D Clamp(Vector4D value1, Vector4D min, Vector4D max)
        {
            // This compare order is very important!!!
            // We must follow HLSL behavior in the case user specified min value is bigger than max value.

            var x = value1.X;
            x = (x > max.X) ? max.X : x;
            x = (x < min.X) ? min.X : x;

            var y = value1.Y;
            y = (y > max.Y) ? max.Y : y;
            y = (y < min.Y) ? min.Y : y;

            var z = value1.Z;
            z = (z > max.Z) ? max.Z : z;
            z = (z < min.Z) ? min.Z : z;

            var w = value1.W;
            w = (w > max.W) ? max.W : w;
            w = (w < min.W) ? min.W : w;

            return new Vector4D(x, y, z, w);
        }

        /// <summary>
        /// Linearly interpolates between two vectors based on the given weighting.
        /// </summary>
        /// <param name="value1">The first source vector.</param>
        /// <param name="value2">The second source vector.</param>
        /// <param name="amount">Value between 0 and 1 indicating the weight of the second source vector.</param>
        /// <returns>The interpolated vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4D Lerp(Vector4D value1, Vector4D value2, double amount)
        {
            return new Vector4D(
                value1.X + (value2.X - value1.X) * amount,
                value1.Y + (value2.Y - value1.Y) * amount,
                value1.Z + (value2.Z - value1.Z) * amount,
                value1.W + (value2.W - value1.W) * amount);
        }

        /// <summary>
        /// Transforms a vector by the given matrix.
        /// </summary>
        /// <param name="position">The source vector.</param>
        /// <param name="matrix">The transformation matrix.</param>
        /// <returns>The transformed vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4D Transform(Vector2D position, Matrix4x4D matrix)
        {
            return new Vector4D(
                position.X * matrix.M11 + position.Y * matrix.M21 + matrix.M41,
                position.X * matrix.M12 + position.Y * matrix.M22 + matrix.M42,
                position.X * matrix.M13 + position.Y * matrix.M23 + matrix.M43,
                position.X * matrix.M14 + position.Y * matrix.M24 + matrix.M44);
        }

        /// <summary>
        /// Transforms a vector by the given matrix.
        /// </summary>
        /// <param name="position">The source vector.</param>
        /// <param name="matrix">The transformation matrix.</param>
        /// <returns>The transformed vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4D Transform(Vector3D position, Matrix4x4D matrix)
        {
            return new Vector4D(
                position.X * matrix.M11 + position.Y * matrix.M21 + position.Z * matrix.M31 + matrix.M41,
                position.X * matrix.M12 + position.Y * matrix.M22 + position.Z * matrix.M32 + matrix.M42,
                position.X * matrix.M13 + position.Y * matrix.M23 + position.Z * matrix.M33 + matrix.M43,
                position.X * matrix.M14 + position.Y * matrix.M24 + position.Z * matrix.M34 + matrix.M44);
        }

        /// <summary>
        /// Transforms a vector by the given matrix.
        /// </summary>
        /// <param name="vector">The source vector.</param>
        /// <param name="matrix">The transformation matrix.</param>
        /// <returns>The transformed vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4D Transform(Vector4D vector, Matrix4x4 matrix)
        {
            return new Vector4D(
                vector.X * matrix.M11 + vector.Y * matrix.M21 + vector.Z * matrix.M31 + vector.W * matrix.M41,
                vector.X * matrix.M12 + vector.Y * matrix.M22 + vector.Z * matrix.M32 + vector.W * matrix.M42,
                vector.X * matrix.M13 + vector.Y * matrix.M23 + vector.Z * matrix.M33 + vector.W * matrix.M43,
                vector.X * matrix.M14 + vector.Y * matrix.M24 + vector.Z * matrix.M34 + vector.W * matrix.M44);
        }

        /// <summary>
        /// Transforms a vector by the given Quaternion rotation value.
        /// </summary>
        /// <param name="value">The source vector to be rotated.</param>
        /// <param name="rotation">The rotation to apply.</param>
        /// <returns>The transformed vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4D Transform(Vector2D value, QuaternionD rotation)
        {
            var x2 = rotation.X + rotation.X;
            var y2 = rotation.Y + rotation.Y;
            var z2 = rotation.Z + rotation.Z;

            var wx2 = rotation.W * x2;
            var wy2 = rotation.W * y2;
            var wz2 = rotation.W * z2;
            var xx2 = rotation.X * x2;
            var xy2 = rotation.X * y2;
            var xz2 = rotation.X * z2;
            var yy2 = rotation.Y * y2;
            var yz2 = rotation.Y * z2;
            var zz2 = rotation.Z * z2;

            return new Vector4D(
                value.X * (1.0 - yy2 - zz2) + value.Y * (xy2 - wz2),
                value.X * (xy2 + wz2) + value.Y * (1.0 - xx2 - zz2),
                value.X * (xz2 - wy2) + value.Y * (yz2 + wx2),
                1.0);
        }

        /// <summary>
        /// Transforms a vector by the given Quaternion rotation value.
        /// </summary>
        /// <param name="value">The source vector to be rotated.</param>
        /// <param name="rotation">The rotation to apply.</param>
        /// <returns>The transformed vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4D Transform(Vector3D value, QuaternionD rotation)
        {
            var x2 = rotation.X + rotation.X;
            var y2 = rotation.Y + rotation.Y;
            var z2 = rotation.Z + rotation.Z;

            var wx2 = rotation.W * x2;
            var wy2 = rotation.W * y2;
            var wz2 = rotation.W * z2;
            var xx2 = rotation.X * x2;
            var xy2 = rotation.X * y2;
            var xz2 = rotation.X * z2;
            var yy2 = rotation.Y * y2;
            var yz2 = rotation.Y * z2;
            var zz2 = rotation.Z * z2;

            return new Vector4D(
                value.X * (1.0 - yy2 - zz2) + value.Y * (xy2 - wz2) + value.Z * (xz2 + wy2),
                value.X * (xy2 + wz2) + value.Y * (1.0 - xx2 - zz2) + value.Z * (yz2 - wx2),
                value.X * (xz2 - wy2) + value.Y * (yz2 + wx2) + value.Z * (1.0 - xx2 - yy2),
                1.0);
        }

        /// <summary>
        /// Transforms a vector by the given Quaternion rotation value.
        /// </summary>
        /// <param name="value">The source vector to be rotated.</param>
        /// <param name="rotation">The rotation to apply.</param>
        /// <returns>The transformed vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4D Transform(Vector4D value, Quaternion rotation)
        {
            double x2 = rotation.X + rotation.X;
            double y2 = rotation.Y + rotation.Y;
            double z2 = rotation.Z + rotation.Z;

            var wx2 = rotation.W * x2;
            var wy2 = rotation.W * y2;
            var wz2 = rotation.W * z2;
            var xx2 = rotation.X * x2;
            var xy2 = rotation.X * y2;
            var xz2 = rotation.X * z2;
            var yy2 = rotation.Y * y2;
            var yz2 = rotation.Y * z2;
            var zz2 = rotation.Z * z2;

            return new Vector4D(
                value.X * (1.0 - yy2 - zz2) + value.Y * (xy2 - wz2) + value.Z * (xz2 + wy2),
                value.X * (xy2 + wz2) + value.Y * (1.0 - xx2 - zz2) + value.Z * (yz2 - wx2),
                value.X * (xz2 - wy2) + value.Y * (yz2 + wx2) + value.Z * (1.0 - xx2 - yy2),
                value.W);
        }
        #endregion Public Static Methods

        #region Public operator methods
        // All these methods should be inlines as they are implemented
        // over JIT intrinsics

        /// <summary>
        /// Adds two vectors together.
        /// </summary>
        /// <param name="left">The first source vector.</param>
        /// <param name="right">The second source vector.</param>
        /// <returns>The summed vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4D Add(Vector4D left, Vector4D right)
        {
            return left + right;
        }

        /// <summary>
        /// Subtracts the second vector from the first.
        /// </summary>
        /// <param name="left">The first source vector.</param>
        /// <param name="right">The second source vector.</param>
        /// <returns>The difference vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4D Subtract(Vector4D left, Vector4D right)
        {
            return left - right;
        }

        /// <summary>
        /// Multiplies two vectors together.
        /// </summary>
        /// <param name="left">The first source vector.</param>
        /// <param name="right">The second source vector.</param>
        /// <returns>The product vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4D Multiply(Vector4D left, Vector4D right)
        {
            return left * right;
        }

        /// <summary>
        /// Multiplies a vector by the given scalar.
        /// </summary>
        /// <param name="left">The source vector.</param>
        /// <param name="right">The scalar value.</param>
        /// <returns>The scaled vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4D Multiply(Vector4D left, double right)
        {
            return left * new Vector4D(right, right, right, right);
        }

        /// <summary>
        /// Multiplies a vector by the given scalar.
        /// </summary>
        /// <param name="left">The scalar value.</param>
        /// <param name="right">The source vector.</param>
        /// <returns>The scaled vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4D Multiply(double left, Vector4D right)
        {
            return new Vector4D(left, left, left, left) * right;
        }

        /// <summary>
        /// Divides the first vector by the second.
        /// </summary>
        /// <param name="left">The first source vector.</param>
        /// <param name="right">The second source vector.</param>
        /// <returns>The vector resulting from the division.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4D Divide(Vector4D left, Vector4D right)
        {
            return left / right;
        }

        /// <summary>
        /// Divides the vector by the given scalar.
        /// </summary>
        /// <param name="left">The source vector.</param>
        /// <param name="divisor">The scalar value.</param>
        /// <returns>The result of the division.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4D Divide(Vector4D left, double divisor)
        {
            return left / divisor;
        }

        /// <summary>
        /// Negates a given vector.
        /// </summary>
        /// <param name="value">The source vector.</param>
        /// <returns>The negated vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4D Negate(Vector4D value)
        {
            return -value;
        }
        #endregion Public operator methods
    }
}
