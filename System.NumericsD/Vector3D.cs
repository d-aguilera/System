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
    /// A structure encapsulating three single precision floating point values and provides hardware accelerated methods.
    /// </summary>
    public partial struct Vector3D : IEquatable<Vector3D>, IFormattable
    {
        #region Public Static Properties
        /// <summary>
        /// Returns the vector (0,0,0).
        /// </summary>
        public static Vector3D Zero => new Vector3D();

        /// <summary>
        /// Returns the vector (1,1,1).
        /// </summary>
        public static Vector3D One => new Vector3D(1.0, 1.0, 1.0);

        /// <summary>
        /// Returns the vector (1,0,0).
        /// </summary>
        public static Vector3D UnitX => new Vector3D(1.0, 0.0, 0.0);

        /// <summary>
        /// Returns the vector (0,1,0).
        /// </summary>
        public static Vector3D UnitY => new Vector3D(0.0, 1.0, 0.0);

        /// <summary>
        /// Returns the vector (0,0,1).
        /// </summary>
        public static Vector3D UnitZ => new Vector3D(0.0, 0.0, 1.0);

        #endregion Public Static Properties

        #region Public Instance Methods

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>The hash code.</returns>
        public override int GetHashCode()
        {
            var hash = this.X.GetHashCode();
            hash = HashCodeHelper.CombineHashCodes(hash, this.Y.GetHashCode());
            hash = HashCodeHelper.CombineHashCodes(hash, this.Z.GetHashCode());
            return hash;
        }

        /// <summary>
        /// Returns a boolean indicating whether the given Object is equal to this Vector3D instance.
        /// </summary>
        /// <param name="obj">The Object to compare against.</param>
        /// <returns>True if the Object is equal to this Vector3D; False otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj)
        {
            return obj is Vector3D vector && Equals(vector);
        }

        /// <summary>
        /// Returns a String representing this Vector3D instance.
        /// </summary>
        /// <returns>The string representation.</returns>
        public override string ToString()
        {
            return ToString("G", CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Returns a String representing this Vector3D instance, using the specified format to format individual elements.
        /// </summary>
        /// <param name="format">The format of individual elements.</param>
        /// <returns>The string representation.</returns>
        public string ToString(string format)
        {
            return ToString(format, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Returns a String representing this Vector3D instance, using the specified format to format individual elements 
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
            sb.Append(X.ToString(format, formatProvider));
            sb.Append(separator);
            sb.Append(' ');
            sb.Append(Y.ToString(format, formatProvider));
            sb.Append(separator);
            sb.Append(' ');
            sb.Append(Z.ToString(format, formatProvider));
            sb.Append('>');
            return sb.ToString();
        }

        /// <summary>
        /// Returns the length of the vector.
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
                var ls = X * X + Y * Y + Z * Z;
                return Math.Sqrt(ls);
            }
        }

        /// <summary>
        /// Returns the length of the vector squared. This operation is cheaper than Length().
        /// </summary>
        /// <returns>The vector's length squared.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double LengthSquared()
        {
            if (Vector.IsHardwareAccelerated)
            {
                return Dot(this, this);
            }

            return X * X + Y * Y + Z * Z;
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
        public static double Distance(Vector3D value1, Vector3D value2)
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

                var ls = dx * dx + dy * dy + dz * dz;

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
        public static double DistanceSquared(Vector3D value1, Vector3D value2)
        {
            if (Vector.IsHardwareAccelerated)
            {
                var difference = value1 - value2;
                return Dot(difference, difference);
            }

            var dx = value1.X - value2.X;
            var dy = value1.Y - value2.Y;
            var dz = value1.Z - value2.Z;

            return dx * dx + dy * dy + dz * dz;
        }

        /// <summary>
        /// Returns a vector with the same direction as the given vector, but with a length of 1.
        /// </summary>
        /// <param name="value">The vector to normalize.</param>
        /// <returns>The normalized vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D Normalize(Vector3D value)
        {
            if (Vector.IsHardwareAccelerated)
            {
                var length = value.Length();
                return value / length;
            }
            else
            {
                var ls = value.X * value.X + value.Y * value.Y + value.Z * value.Z;
                var length = Math.Sqrt(ls);
                return new Vector3D(value.X / length, value.Y / length, value.Z / length);
            }
        }

        /// <summary>
        /// Computes the cross product of two vectors.
        /// </summary>
        /// <param name="vector1">The first vector.</param>
        /// <param name="vector2">The second vector.</param>
        /// <returns>The cross product.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D Cross(Vector3D vector1, Vector3D vector2)
        {
            return new Vector3D(
                vector1.Y * vector2.Z - vector1.Z * vector2.Y,
                vector1.Z * vector2.X - vector1.X * vector2.Z,
                vector1.X * vector2.Y - vector1.Y * vector2.X);
        }

        /// <summary>
        /// Returns the reflection of a vector off a surface that has the specified normal.
        /// </summary>
        /// <param name="vector">The source vector.</param>
        /// <param name="normal">The normal of the surface being reflected off.</param>
        /// <returns>The reflected vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D Reflect(Vector3D vector, Vector3D normal)
        {
            if (Vector.IsHardwareAccelerated)
            {
                var dot = Dot(vector, normal);
                var temp = normal * dot * 2f;
                return vector - temp;
            }
            else
            {
                var dot = vector.X * normal.X + vector.Y * normal.Y + vector.Z * normal.Z;
                var tempX = normal.X * dot * 2f;
                var tempY = normal.Y * dot * 2f;
                var tempZ = normal.Z * dot * 2f;
                return new Vector3D(vector.X - tempX, vector.Y - tempY, vector.Z - tempZ);
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
        public static Vector3D Clamp(Vector3D value1, Vector3D min, Vector3D max)
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

            return new Vector3D(x, y, z);
        }

        /// <summary>
        /// Linearly interpolates between two vectors based on the given weighting.
        /// </summary>
        /// <param name="value1">The first source vector.</param>
        /// <param name="value2">The second source vector.</param>
        /// <param name="amount">Value between 0 and 1 indicating the weight of the second source vector.</param>
        /// <returns>The interpolated vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D Lerp(Vector3D value1, Vector3D value2, double amount)
        {
            if (Vector.IsHardwareAccelerated)
            {
                var firstInfluence = value1 * (1.0 - amount);
                var secondInfluence = value2 * amount;
                return firstInfluence + secondInfluence;
            }

            return new Vector3D(
                value1.X + (value2.X - value1.X) * amount,
                value1.Y + (value2.Y - value1.Y) * amount,
                value1.Z + (value2.Z - value1.Z) * amount);
        }

        /// <summary>
        /// Transforms a vector by the given matrix.
        /// </summary>
        /// <param name="position">The source vector.</param>
        /// <param name="matrix">The transformation matrix.</param>
        /// <returns>The transformed vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D Transform(Vector3D position, Matrix4x4 matrix)
        {
            return new Vector3D(
                position.X * matrix.M11 + position.Y * matrix.M21 + position.Z * matrix.M31 + matrix.M41,
                position.X * matrix.M12 + position.Y * matrix.M22 + position.Z * matrix.M32 + matrix.M42,
                position.X * matrix.M13 + position.Y * matrix.M23 + position.Z * matrix.M33 + matrix.M43);
        }

        /// <summary>
        /// Transforms a vector normal by the given matrix.
        /// </summary>
        /// <param name="normal">The source vector.</param>
        /// <param name="matrix">The transformation matrix.</param>
        /// <returns>The transformed vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D TransformNormal(Vector3D normal, Matrix4x4 matrix)
        {
            return new Vector3D(
                normal.X * matrix.M11 + normal.Y * matrix.M21 + normal.Z * matrix.M31,
                normal.X * matrix.M12 + normal.Y * matrix.M22 + normal.Z * matrix.M32,
                normal.X * matrix.M13 + normal.Y * matrix.M23 + normal.Z * matrix.M33);
        }

        /// <summary>
        /// Transforms a vector by the given Quaternion rotation value.
        /// </summary>
        /// <param name="value">The source vector to be rotated.</param>
        /// <param name="rotation">The rotation to apply.</param>
        /// <returns>The transformed vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D Transform(Vector3D value, Quaternion rotation)
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

            return new Vector3D(
                value.X * (1.0 - yy2 - zz2) + value.Y * (xy2 - wz2) + value.Z * (xz2 + wy2),
                value.X * (xy2 + wz2) + value.Y * (1.0 - xx2 - zz2) + value.Z * (yz2 - wx2),
                value.X * (xz2 - wy2) + value.Y * (yz2 + wx2) + value.Z * (1.0 - xx2 - yy2));
        }
        #endregion Public Static Methods

        #region Public operator methods

        // All these methods should be inlined as they are implemented
        // over JIT intrinsics

        /// <summary>
        /// Adds two vectors together.
        /// </summary>
        /// <param name="left">The first source vector.</param>
        /// <param name="right">The second source vector.</param>
        /// <returns>The summed vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D Add(Vector3D left, Vector3D right)
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
        public static Vector3D Subtract(Vector3D left, Vector3D right)
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
        public static Vector3D Multiply(Vector3D left, Vector3D right)
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
        public static Vector3D Multiply(Vector3D left, double right)
        {
            return left * right;
        }

        /// <summary>
        /// Multiplies a vector by the given scalar.
        /// </summary>
        /// <param name="left">The scalar value.</param>
        /// <param name="right">The source vector.</param>
        /// <returns>The scaled vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D Multiply(double left, Vector3D right)
        {
            return left * right;
        }

        /// <summary>
        /// Divides the first vector by the second.
        /// </summary>
        /// <param name="left">The first source vector.</param>
        /// <param name="right">The second source vector.</param>
        /// <returns>The vector resulting from the division.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D Divide(Vector3D left, Vector3D right)
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
        public static Vector3D Divide(Vector3D left, double divisor)
        {
            return left / divisor;
        }

        /// <summary>
        /// Negates a given vector.
        /// </summary>
        /// <param name="value">The source vector.</param>
        /// <returns>The negated vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D Negate(Vector3D value)
        {
            return -value;
        }
        #endregion Public operator methods
    }
}
