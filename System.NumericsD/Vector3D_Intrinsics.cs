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
    // is actually recognized by the JIT, but both are here for simplicity.

    public partial struct Vector3D
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

        #region Constructors
        /// <summary>
        /// Constructs a vector whose elements are all the single specified value.
        /// </summary>
        /// <param name="value">The element to fill the vector with.</param>
        [JitIntrinsic]
        public Vector3D(double value) : this(value, value, value) { }

        /// <summary>
        /// Constructs a Vector3D from the given Vector2D and a third value.
        /// </summary>
        /// <param name="value">The Vector to extract X and Y components from.</param>
        /// <param name="z">The Z component.</param>
        public Vector3D(Vector2D value, double z) : this(value.X, value.Y, z) { }

        /// <summary>
        /// Constructs a vector with the given individual elements.
        /// </summary>
        /// <param name="x">The X component.</param>
        /// <param name="y">The Y component.</param>
        /// <param name="z">The Z component.</param>
        [JitIntrinsic]
        public Vector3D(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
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
            if ((array.Length - index) < 3)
            {
                throw new ArgumentException("Arg_ElementsInSourceIsGreaterThanDestination");
            }
            array[index] = X;
            array[index + 1] = Y;
            array[index + 2] = Z;
        }

        /// <summary>
        /// Returns a boolean indicating whether the given Vector3D is equal to this Vector3D instance.
        /// </summary>
        /// <param name="other">The Vector3D to compare this instance to.</param>
        /// <returns>True if the other Vector3D is equal to this instance; False otherwise.</returns>
        [JitIntrinsic]
        public bool Equals(Vector3D other)
        {
            return X == other.X &&
                   Y == other.Y &&
                   Z == other.Z;
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
        public static double Dot(Vector3D vector1, Vector3D vector2)
        {
            return vector1.X * vector2.X +
                   vector1.Y * vector2.Y +
                   vector1.Z * vector2.Z;
        }

        /// <summary>
        /// Returns a vector whose elements are the minimum of each of the pairs of elements in the two source vectors.
        /// </summary>
        /// <param name="value1">The first source vector.</param>
        /// <param name="value2">The second source vector.</param>
        /// <returns>The minimized vector.</returns>
        [JitIntrinsic]
        public static Vector3D Min(Vector3D value1, Vector3D value2)
        {
            return new Vector3D(
                (value1.X < value2.X) ? value1.X : value2.X,
                (value1.Y < value2.Y) ? value1.Y : value2.Y,
                (value1.Z < value2.Z) ? value1.Z : value2.Z);
        }

        /// <summary>
        /// Returns a vector whose elements are the maximum of each of the pairs of elements in the two source vectors.
        /// </summary>
        /// <param name="value1">The first source vector.</param>
        /// <param name="value2">The second source vector.</param>
        /// <returns>The maximized vector.</returns>
        [JitIntrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D Max(Vector3D value1, Vector3D value2)
        {
            return new Vector3D(
                (value1.X > value2.X) ? value1.X : value2.X,
                (value1.Y > value2.Y) ? value1.Y : value2.Y,
                (value1.Z > value2.Z) ? value1.Z : value2.Z);
        }

        /// <summary>
        /// Returns a vector whose elements are the absolute values of each of the source vector's elements.
        /// </summary>
        /// <param name="value">The source vector.</param>
        /// <returns>The absolute value vector.</returns>
        [JitIntrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D Abs(Vector3D value)
        {
            return new Vector3D(Math.Abs(value.X), Math.Abs(value.Y), Math.Abs(value.Z));
        }

        /// <summary>
        /// Returns a vector whose elements are the square root of each of the source vector's elements.
        /// </summary>
        /// <param name="value">The source vector.</param>
        /// <returns>The square root vector.</returns>
        [JitIntrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D SquareRoot(Vector3D value)
        {
            return new Vector3D(Math.Sqrt(value.X), Math.Sqrt(value.Y), Math.Sqrt(value.Z));
        }
        #endregion Public Static Methods

        #region Public Static Operators
        /// <summary>
        /// Adds two vectors together.
        /// </summary>
        /// <param name="left">The first source vector.</param>
        /// <param name="right">The second source vector.</param>
        /// <returns>The summed vector.</returns>
        [JitIntrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D operator +(Vector3D left, Vector3D right)
        {
            return new Vector3D(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
        }

        /// <summary>
        /// Subtracts the second vector from the first.
        /// </summary>
        /// <param name="left">The first source vector.</param>
        /// <param name="right">The second source vector.</param>
        /// <returns>The difference vector.</returns>
        [JitIntrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D operator -(Vector3D left, Vector3D right)
        {
            return new Vector3D(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
        }

        /// <summary>
        /// Multiplies two vectors together.
        /// </summary>
        /// <param name="left">The first source vector.</param>
        /// <param name="right">The second source vector.</param>
        /// <returns>The product vector.</returns>
        [JitIntrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D operator *(Vector3D left, Vector3D right)
        {
            return new Vector3D(left.X * right.X, left.Y * right.Y, left.Z * right.Z);
        }

        /// <summary>
        /// Multiplies a vector by the given scalar.
        /// </summary>
        /// <param name="left">The source vector.</param>
        /// <param name="right">The scalar value.</param>
        /// <returns>The scaled vector.</returns>
        [JitIntrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D operator *(Vector3D left, double right)
        {
            return left * new Vector3D(right);
        }

        /// <summary>
        /// Multiplies a vector by the given scalar.
        /// </summary>
        /// <param name="left">The scalar value.</param>
        /// <param name="right">The source vector.</param>
        /// <returns>The scaled vector.</returns>
        [JitIntrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D operator *(double left, Vector3D right)
        {
            return new Vector3D(left) * right;
        }

        /// <summary>
        /// Divides the first vector by the second.
        /// </summary>
        /// <param name="left">The first source vector.</param>
        /// <param name="right">The second source vector.</param>
        /// <returns>The vector resulting from the division.</returns>
        [JitIntrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D operator /(Vector3D left, Vector3D right)
        {
            return new Vector3D(left.X / right.X, left.Y / right.Y, left.Z / right.Z);
        }

        /// <summary>
        /// Divides the vector by the given scalar.
        /// </summary>
        /// <param name="value1">The source vector.</param>
        /// <param name="value2">The scalar value.</param>
        /// <returns>The result of the division.</returns>
        [JitIntrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D operator /(Vector3D value1, double value2)
        {
            double invDiv = 1.0 / value2;

            return new Vector3D(
                value1.X * invDiv,
                value1.Y * invDiv,
                value1.Z * invDiv);
        }

        /// <summary>
        /// Negates a given vector.
        /// </summary>
        /// <param name="value">The source vector.</param>
        /// <returns>The negated vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D operator -(Vector3D value)
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
        public static bool operator ==(Vector3D left, Vector3D right)
        {
            return left.X == right.X &&
                   left.Y == right.Y &&
                   left.Z == right.Z;
        }

        /// <summary>
        /// Returns a boolean indicating whether the two given vectors are not equal.
        /// </summary>
        /// <param name="left">The first vector to compare.</param>
        /// <param name="right">The second vector to compare.</param>
        /// <returns>True if the vectors are not equal; False if they are equal.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Vector3D left, Vector3D right)
        {
            return left.X != right.X ||
                   left.Y != right.Y ||
                   left.Z != right.Z;
        }
        #endregion Public Static Operators
    }
}
