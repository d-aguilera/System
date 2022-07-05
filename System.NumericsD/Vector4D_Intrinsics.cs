// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.CompilerServices;

// ReSharper disable IdentifierTypo
// ReSharper disable CompareOfFloatsByEqualityOperator
// ReSharper disable once CheckNamespace
namespace System.Numerics
{
    // This file contains the definitions for all of the JIT intrinsic methods and properties that are recognized by the current x64 JIT compiler.
    // The implementation defined here is used in any circumstance where the JIT fails to recognize these members as intrinsic.
    // The JIT recognizes these methods and properties by name and signature: if either is changed, the JIT will no longer recognize the member.
    // Some methods declared here are not strictly intrinsic, but delegate to an intrinsic method. For example, only one overload of CopyTo() 

    public partial struct Vector4D
    {
        /// <summary>
        /// The X component of the vector.
        /// </summary>
        public double X;
        /// <summary>
        /// The Y component of the vector.
        /// </summary>
        public double Y;
        /// <summary>
        /// The Z component of the vector.
        /// </summary>
        public double Z;
        /// <summary>
        /// The W component of the vector.
        /// </summary>
        public double W;

        #region Constructors

        /// <summary>
        /// Constructs a vector whose elements are all the single specified value.
        /// </summary>
        /// <param name="value">The element to fill the vector with.</param>
        [JitIntrinsic]
        public Vector4D(double value)
            : this(value, value, value, value)
        {
        }
        /// <summary>
        /// Constructs a vector with the given individual elements.
        /// </summary>
        /// <param name="w">W component.</param>
        /// <param name="x">X component.</param>
        /// <param name="y">Y component.</param>
        /// <param name="z">Z component.</param>
        [JitIntrinsic]
        public Vector4D(double x, double y, double z, double w)
        {
            W = w;
            X = x;
            Y = y;
            Z = z;
        }

        /// <summary>
        /// Constructs a Vector4D from the given Vector2D and a Z and W component.
        /// </summary>
        /// <param name="value">The vector to use as the X and Y components.</param>
        /// <param name="z">The Z component.</param>
        /// <param name="w">The W component.</param>
        public Vector4D(Vector2D value, double z, double w)
        {
            X = value.X;
            Y = value.Y;
            Z = z;
            W = w;
        }

        /// <summary>
        /// Constructs a Vector4D from the given Vector3D and a W component.
        /// </summary>
        /// <param name="value">The vector to use as the X, Y, and Z components.</param>
        /// <param name="w">The W component.</param>
        public Vector4D(Vector3D value, double w)
        {
            X = value.X;
            Y = value.Y;
            Z = value.Z;
            W = w;
        }
        #endregion Constructors

        #region Public Instance Methods
        /// <summary>
        /// Copies the contents of the vector into the given array.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CopyTo(double[] array)
        {
            CopyTo(array, 0);
        }

        /// <summary>
        /// Copies the contents of the vector into the given array, starting from index.
        /// </summary>
        /// <exception cref="ArgumentNullException">If array is null.</exception>
        /// <exception cref="RankException">If array is multidimensional.</exception>
        /// <exception cref="ArgumentOutOfRangeException">If index is greater than end of the array or index is less than zero.</exception>
        /// <exception cref="ArgumentException">If number of elements in source vector is greater than those available in destination array.</exception>
        [JitIntrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CopyTo(double[] array, int index)
        {
            if (array == null)
            {
                // Match the JIT's exception type here. For perf, a NullReference is thrown instead of an ArgumentNull.
                throw new NullReferenceException("Arg_NullArgumentNullRef");
            }
            if (index < 0 || index >= array.Length)
            {
                // ReSharper disable once NotResolvedInText
                throw new ArgumentOutOfRangeException("Arg_ArgumentOutOfRangeException");
            }
            if (array.Length - index < 4)
            {
                throw new ArgumentException("Arg_ElementsInSourceIsGreaterThanDestination");
            }
            array[index] = X;
            array[index + 1] = Y;
            array[index + 2] = Z;
            array[index + 3] = W;
        }

        /// <summary>
        /// Returns a boolean indicating whether the given Vector4D is equal to this Vector4D instance.
        /// </summary>
        /// <param name="other">The Vector4D to compare this instance to.</param>
        /// <returns>True if the other Vector4D is equal to this instance; False otherwise.</returns>
        [JitIntrinsic]
        public bool Equals(Vector4D other)
        {
            return this.X == other.X
                && this.Y == other.Y
                && this.Z == other.Z
                && this.W == other.W;
        }
        #endregion Public Instance Methods

        #region Public Static Methods
        /// <summary>
        /// Returns the dot product of two vectors.
        /// </summary>
        /// <param name="vector1">The first vector.</param>
        /// <param name="vector2">The second vector.</param>
        /// <returns>The dot product.</returns>
        [JitIntrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Dot(Vector4D vector1, Vector4D vector2)
        {
            return vector1.X * vector2.X +
                   vector1.Y * vector2.Y +
                   vector1.Z * vector2.Z +
                   vector1.W * vector2.W;
        }

        /// <summary>
        /// Returns a vector whose elements are the minimum of each of the pairs of elements in the two source vectors.
        /// </summary>
        /// <param name="value1">The first source vector.</param>
        /// <param name="value2">The second source vector.</param>
        /// <returns>The minimized vector.</returns>
        [JitIntrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4D Min(Vector4D value1, Vector4D value2)
        {
            return new Vector4D(
                (value1.X < value2.X) ? value1.X : value2.X,
                (value1.Y < value2.Y) ? value1.Y : value2.Y,
                (value1.Z < value2.Z) ? value1.Z : value2.Z,
                (value1.W < value2.W) ? value1.W : value2.W);
        }

        /// <summary>
        /// Returns a vector whose elements are the maximum of each of the pairs of elements in the two source vectors.
        /// </summary>
        /// <param name="value1">The first source vector.</param>
        /// <param name="value2">The second source vector.</param>
        /// <returns>The maximized vector.</returns>
        [JitIntrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4D Max(Vector4D value1, Vector4D value2)
        {
            return new Vector4D(
                (value1.X > value2.X) ? value1.X : value2.X,
                (value1.Y > value2.Y) ? value1.Y : value2.Y,
                (value1.Z > value2.Z) ? value1.Z : value2.Z,
                (value1.W > value2.W) ? value1.W : value2.W);
        }

        /// <summary>
        /// Returns a vector whose elements are the absolute values of each of the source vector's elements.
        /// </summary>
        /// <param name="value">The source vector.</param>
        /// <returns>The absolute value vector.</returns>
        [JitIntrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4D Abs(Vector4D value)
        {
            return new Vector4D(Math.Abs(value.X), Math.Abs(value.Y), Math.Abs(value.Z), Math.Abs(value.W));
        }

        /// <summary>
        /// Returns a vector whose elements are the square root of each of the source vector's elements.
        /// </summary>
        /// <param name="value">The source vector.</param>
        /// <returns>The square root vector.</returns>
        [JitIntrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4D SquareRoot(Vector4D value)
        {
            return new Vector4D(Math.Sqrt(value.X), Math.Sqrt(value.Y), Math.Sqrt(value.Z), Math.Sqrt(value.W));
        }
        #endregion Public Static Methods

        #region Public static operators
        /// <summary>
        /// Adds two vectors together.
        /// </summary>
        /// <param name="left">The first source vector.</param>
        /// <param name="right">The second source vector.</param>
        /// <returns>The summed vector.</returns>
        [JitIntrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4D operator +(Vector4D left, Vector4D right)
        {
            return new Vector4D(left.X + right.X, left.Y + right.Y, left.Z + right.Z, left.W + right.W);
        }

        /// <summary>
        /// Subtracts the second vector from the first.
        /// </summary>
        /// <param name="left">The first source vector.</param>
        /// <param name="right">The second source vector.</param>
        /// <returns>The difference vector.</returns>
        [JitIntrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4D operator -(Vector4D left, Vector4D right)
        {
            return new Vector4D(left.X - right.X, left.Y - right.Y, left.Z - right.Z, left.W - right.W);
        }

        /// <summary>
        /// Multiplies two vectors together.
        /// </summary>
        /// <param name="left">The first source vector.</param>
        /// <param name="right">The second source vector.</param>
        /// <returns>The product vector.</returns>
        [JitIntrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4D operator *(Vector4D left, Vector4D right)
        {
            return new Vector4D(left.X * right.X, left.Y * right.Y, left.Z * right.Z, left.W * right.W);
        }

        /// <summary>
        /// Multiplies a vector by the given scalar.
        /// </summary>
        /// <param name="left">The source vector.</param>
        /// <param name="right">The scalar value.</param>
        /// <returns>The scaled vector.</returns>
        [JitIntrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4D operator *(Vector4D left, double right)
        {
            return left * new Vector4D(right);
        }

        /// <summary>
        /// Multiplies a vector by the given scalar.
        /// </summary>
        /// <param name="left">The scalar value.</param>
        /// <param name="right">The source vector.</param>
        /// <returns>The scaled vector.</returns>
        [JitIntrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4D operator *(double left, Vector4D right)
        {
            return new Vector4D(left) * right;
        }

        /// <summary>
        /// Divides the first vector by the second.
        /// </summary>
        /// <param name="left">The first source vector.</param>
        /// <param name="right">The second source vector.</param>
        /// <returns>The vector resulting from the division.</returns>
        [JitIntrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4D operator /(Vector4D left, Vector4D right)
        {
            return new Vector4D(left.X / right.X, left.Y / right.Y, left.Z / right.Z, left.W / right.W);
        }

        /// <summary>
        /// Divides the vector by the given scalar.
        /// </summary>
        /// <param name="value1">The source vector.</param>
        /// <param name="value2">The scalar value.</param>
        /// <returns>The result of the division.</returns>
        [JitIntrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4D operator /(Vector4D value1, double value2)
        {
            var invDiv = 1.0 / value2;

            return new Vector4D(
                value1.X * invDiv,
                value1.Y * invDiv,
                value1.Z * invDiv,
                value1.W * invDiv);
        }

        /// <summary>
        /// Negates a given vector.
        /// </summary>
        /// <param name="value">The source vector.</param>
        /// <returns>The negated vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4D operator -(Vector4D value)
        {
            return Zero - value;
        }

        /// <summary>
        /// Returns a boolean indicating whether the two given vectors are equal.
        /// </summary>
        /// <param name="left">The first vector to compare.</param>
        /// <param name="right">The second vector to compare.</param>
        /// <returns>True if the vectors are equal; False otherwise.</returns>
        [JitIntrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Vector4D left, Vector4D right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Returns a boolean indicating whether the two given vectors are not equal.
        /// </summary>
        /// <param name="left">The first vector to compare.</param>
        /// <param name="right">The second vector to compare.</param>
        /// <returns>True if the vectors are not equal; False if they are equal.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Vector4D left, Vector4D right)
        {
            return !(left == right);
        }
        #endregion Public static operators
    }
}
