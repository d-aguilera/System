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
    /// A structure encapsulating two single precision floating point values and provides hardware accelerated methods.
    /// </summary>
    public partial struct Vector2D : IEquatable<Vector2D>, IFormattable
    {
        #region Public Static Properties
        /// <summary>
        /// Returns the vector (0,0).
        /// </summary>
        public static Vector2D Zero => new Vector2D();

        /// <summary>
        /// Returns the vector (1,1).
        /// </summary>
        public static Vector2D One => new Vector2D(1.0, 1.0);

        /// <summary>
        /// Returns the vector (1,0).
        /// </summary>
        public static Vector2D UnitX => new Vector2D(1.0f, 0.0f);

        /// <summary>
        /// Returns the vector (0,1).
        /// </summary>
        public static Vector2D UnitY => new Vector2D(0.0f, 1.0f);

        #endregion Public Static Properties

        #region Public instance methods
        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>The hash code.</returns>
        public override int GetHashCode()
        {
            var hash = X.GetHashCode();
            hash = HashCodeHelper.CombineHashCodes(hash, Y.GetHashCode());
            return hash;
        }

        /// <summary>
        /// Returns a boolean indicating whether the given Object is equal to this Vector2D instance.
        /// </summary>
        /// <param name="obj">The Object to compare against.</param>
        /// <returns>True if the Object is equal to this Vector2D; False otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj)
        {
            return obj is Vector2D vector && Equals(vector);
        }

        /// <summary>
        /// Returns a String representing this Vector2D instance.
        /// </summary>
        /// <returns>The string representation.</returns>
        public override string ToString()
        {
            return ToString("G", CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Returns a String representing this Vector2D instance, using the specified format to format individual elements.
        /// </summary>
        /// <param name="format">The format of individual elements.</param>
        /// <returns>The string representation.</returns>
        public string ToString(string format)
        {
            return ToString(format, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Returns a String representing this Vector2D instance, using the specified format to format individual elements 
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
                var ls = X * X + Y * Y;
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
            else
            {
                return X * X + Y * Y;
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
        public static double Distance(Vector2D value1, Vector2D value2)
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

                var ls = dx * dx + dy * dy;

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
        public static double DistanceSquared(Vector2D value1, Vector2D value2)
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

                return dx * dx + dy * dy;
            }
        }

        /// <summary>
        /// Returns a vector with the same direction as the given vector, but with a length of 1.
        /// </summary>
        /// <param name="value">The vector to normalize.</param>
        /// <returns>The normalized vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2D Normalize(Vector2D value)
        {
            if (Vector.IsHardwareAccelerated)
            {
                var length = value.Length();
                return value / length;
            }
            else
            {
                var ls = value.X * value.X + value.Y * value.Y;
                var invNorm = 1.0f / Math.Sqrt(ls);

                return new Vector2D(
                    value.X * invNorm,
                    value.Y * invNorm);
            }
        }

        /// <summary>
        /// Returns the reflection of a vector off a surface that has the specified normal.
        /// </summary>
        /// <param name="vector">The source vector.</param>
        /// <param name="normal">The normal of the surface being reflected off.</param>
        /// <returns>The reflected vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2D Reflect(Vector2D vector, Vector2D normal)
        {
            if (Vector.IsHardwareAccelerated)
            {
                var dot = Dot(vector, normal);
                return vector - (2 * dot * normal);
            }
            else
            {
                var dot = vector.X * normal.X + vector.Y * normal.Y;

                return new Vector2D(
                    vector.X - 2.0f * dot * normal.X,
                    vector.Y - 2.0f * dot * normal.Y);
            }
        }

        /// <summary>
        /// Restricts a vector between a min and max value.
        /// </summary>
        /// <param name="value1">The source vector.</param>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2D Clamp(Vector2D value1, Vector2D min, Vector2D max)
        {
            // This compare order is very important!!!
            // We must follow HLSL behavior in the case user specified min value is bigger than max value.
            var x = value1.X;
            x = (x > max.X) ? max.X : x;
            x = (x < min.X) ? min.X : x;

            var y = value1.Y;
            y = (y > max.Y) ? max.Y : y;
            y = (y < min.Y) ? min.Y : y;

            return new Vector2D(x, y);
        }

        /// <summary>
        /// Linearly interpolates between two vectors based on the given weighting.
        /// </summary>
        /// <param name="value1">The first source vector.</param>
        /// <param name="value2">The second source vector.</param>
        /// <param name="amount">Value between 0 and 1 indicating the weight of the second source vector.</param>
        /// <returns>The interpolated vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2D Lerp(Vector2D value1, Vector2D value2, double amount)
        {
            return new Vector2D(
                value1.X + (value2.X - value1.X) * amount,
                value1.Y + (value2.Y - value1.Y) * amount);
        }

        /// <summary>
        /// Transforms a vector by the given matrix.
        /// </summary>
        /// <param name="position">The source vector.</param>
        /// <param name="matrix">The transformation matrix.</param>
        /// <returns>The transformed vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2D Transform(Vector2D position, Matrix3x2D matrix)
        {
            return new Vector2D(
                position.X * matrix.M11 + position.Y * matrix.M21 + matrix.M31,
                position.X * matrix.M12 + position.Y * matrix.M22 + matrix.M32);
        }

        /// <summary>
        /// Transforms a vector by the given matrix.
        /// </summary>
        /// <param name="position">The source vector.</param>
        /// <param name="matrix">The transformation matrix.</param>
        /// <returns>The transformed vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2D Transform(Vector2D position, Matrix4x4 matrix)
        {
            return new Vector2D(
                position.X * matrix.M11 + position.Y * matrix.M21 + matrix.M41,
                position.X * matrix.M12 + position.Y * matrix.M22 + matrix.M42);
        }

        /// <summary>
        /// Transforms a vector normal by the given matrix.
        /// </summary>
        /// <param name="normal">The source vector.</param>
        /// <param name="matrix">The transformation matrix.</param>
        /// <returns>The transformed vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2D TransformNormal(Vector2D normal, Matrix3x2D matrix)
        {
            return new Vector2D(
                normal.X * matrix.M11 + normal.Y * matrix.M21,
                normal.X * matrix.M12 + normal.Y * matrix.M22);
        }

        /// <summary>
        /// Transforms a vector normal by the given matrix.
        /// </summary>
        /// <param name="normal">The source vector.</param>
        /// <param name="matrix">The transformation matrix.</param>
        /// <returns>The transformed vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2D TransformNormal(Vector2D normal, Matrix4x4 matrix)
        {
            return new Vector2D(
                normal.X * matrix.M11 + normal.Y * matrix.M21,
                normal.X * matrix.M12 + normal.Y * matrix.M22);
        }

        /// <summary>
        /// Transforms a vector by the given Quaternion rotation value.
        /// </summary>
        /// <param name="value">The source vector to be rotated.</param>
        /// <param name="rotation">The rotation to apply.</param>
        /// <returns>The transformed vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2D Transform(Vector2D value, Quaternion rotation)
        {
            double x2 = rotation.X + rotation.X;
            double y2 = rotation.Y + rotation.Y;
            double z2 = rotation.Z + rotation.Z;

            var wz2 = rotation.W * z2;
            var xx2 = rotation.X * x2;
            var xy2 = rotation.X * y2;
            var yy2 = rotation.Y * y2;
            var zz2 = rotation.Z * z2;

            return new Vector2D(
                value.X * (1.0f - yy2 - zz2) + value.Y * (xy2 - wz2),
                value.X * (xy2 + wz2) + value.Y * (1.0f - xx2 - zz2));
        }
        #endregion Public Static Methods

        #region Public operator methods
        // all the below methods should be inlined as they are 
        // implemented over JIT intrinsics

        /// <summary>
        /// Adds two vectors together.
        /// </summary>
        /// <param name="left">The first source vector.</param>
        /// <param name="right">The second source vector.</param>
        /// <returns>The summed vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2D Add(Vector2D left, Vector2D right)
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
        public static Vector2D Subtract(Vector2D left, Vector2D right)
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
        public static Vector2D Multiply(Vector2D left, Vector2D right)
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
        public static Vector2D Multiply(Vector2D left, Single right)
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
        public static Vector2D Multiply(Single left, Vector2D right)
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
        public static Vector2D Divide(Vector2D left, Vector2D right)
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
        public static Vector2D Divide(Vector2D left, Single divisor)
        {
            return left / divisor;
        }

        /// <summary>
        /// Negates a given vector.
        /// </summary>
        /// <param name="value">The source vector.</param>
        /// <returns>The negated vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2D Negate(Vector2D value)
        {
            return -value;
        }
        #endregion Public operator methods
    }
}
