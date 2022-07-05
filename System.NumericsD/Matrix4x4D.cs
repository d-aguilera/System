// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Globalization;
using System.Security;

#pragma warning disable CS0649

// ReSharper disable IdentifierTypo
// ReSharper disable NotAccessedField.Local
// ReSharper disable InconsistentNaming
// ReSharper disable CompareOfFloatsByEqualityOperator
// ReSharper disable once CheckNamespace
namespace System.Numerics
{
    /// <summary>
    /// A structure encapsulating a 4x4 matrix.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public struct Matrix4x4D : IEquatable<Matrix4x4D>
    {
        #region Public Fields
        /// <summary>
        /// Value at row 1, column 1 of the matrix.
        /// </summary>
        public double M11;
        /// <summary>
        /// Value at row 1, column 2 of the matrix.
        /// </summary>
        public double M12;
        /// <summary>
        /// Value at row 1, column 3 of the matrix.
        /// </summary>
        public double M13;
        /// <summary>
        /// Value at row 1, column 4 of the matrix.
        /// </summary>
        public double M14;

        /// <summary>
        /// Value at row 2, column 1 of the matrix.
        /// </summary>
        public double M21;
        /// <summary>
        /// Value at row 2, column 2 of the matrix.
        /// </summary>
        public double M22;
        /// <summary>
        /// Value at row 2, column 3 of the matrix.
        /// </summary>
        public double M23;
        /// <summary>
        /// Value at row 2, column 4 of the matrix.
        /// </summary>
        public double M24;

        /// <summary>
        /// Value at row 3, column 1 of the matrix.
        /// </summary>
        public double M31;
        /// <summary>
        /// Value at row 3, column 2 of the matrix.
        /// </summary>
        public double M32;
        /// <summary>
        /// Value at row 3, column 3 of the matrix.
        /// </summary>
        public double M33;
        /// <summary>
        /// Value at row 3, column 4 of the matrix.
        /// </summary>
        public double M34;

        /// <summary>
        /// Value at row 4, column 1 of the matrix.
        /// </summary>
        public double M41;
        /// <summary>
        /// Value at row 4, column 2 of the matrix.
        /// </summary>
        public double M42;
        /// <summary>
        /// Value at row 4, column 3 of the matrix.
        /// </summary>
        public double M43;
        /// <summary>
        /// Value at row 4, column 4 of the matrix.
        /// </summary>
        public double M44;
        #endregion Public Fields

        /// <summary>
        /// Returns the multiplicative identity matrix.
        /// </summary>
        public static Matrix4x4D Identity { get; } = new Matrix4x4D
        (
            1.0, 0.0, 0.0, 0.0,
            0.0, 1.0, 0.0, 0.0,
            0.0, 0.0, 1.0, 0.0,
            0.0, 0.0, 0.0, 1.0
        );

        /// <summary>
        /// Returns whether the matrix is the identity matrix.
        /// </summary>
        public bool IsIdentity =>
            M11 == 1.0 && M22 == 1.0 && M33 == 1.0 && M44 == 1.0 && // Check diagonal element first for early out.
            M12 == 0.0 && M13 == 0.0 && M14 == 0.0 &&
            M21 == 0.0 && M23 == 0.0 && M24 == 0.0 &&
            M31 == 0.0 && M32 == 0.0 && M34 == 0.0 &&
            M41 == 0.0 && M42 == 0.0 && M43 == 0.0;

        /// <summary>
        /// Gets or sets the translation component of this matrix.
        /// </summary>
        public Vector3D Translation
        {
            get => new Vector3D(M41, M42, M43);
            set
            {
                M41 = value.X;
                M42 = value.Y;
                M43 = value.Z;
            }
        }

        /// <summary>
        /// Constructs a Matrix4x4D from the given components.
        /// </summary>
        public Matrix4x4D(
            double m11, double m12, double m13, double m14,
            double m21, double m22, double m23, double m24,
            double m31, double m32, double m33, double m34,
            double m41, double m42, double m43, double m44)
        {
            M11 = m11;
            M12 = m12;
            M13 = m13;
            M14 = m14;

            M21 = m21;
            M22 = m22;
            M23 = m23;
            M24 = m24;

            M31 = m31;
            M32 = m32;
            M33 = m33;
            M34 = m34;

            M41 = m41;
            M42 = m42;
            M43 = m43;
            M44 = m44;
        }

        /// <summary>
        /// Constructs a Matrix4x4D from the given Matrix3x2.
        /// </summary>
        /// <param name="value">The source Matrix3x2.</param>
        public Matrix4x4D(Matrix3x2 value)
        {
            M11 = value.M11;
            M12 = value.M12;
            M13 = 0.0;
            M14 = 0.0;
            M21 = value.M21;
            M22 = value.M22;
            M23 = 0.0;
            M24 = 0.0;
            M31 = 0.0;
            M32 = 0.0;
            M33 = 1.0;
            M34 = 0.0;
            M41 = value.M31;
            M42 = value.M32;
            M43 = 0.0;
            M44 = 1.0;
        }

        /// <summary>
        /// Creates a spherical billboard that rotates around a specified object position.
        /// </summary>
        /// <param name="objectPosition">Position of the object the billboard will rotate around.</param>
        /// <param name="cameraPosition">Position of the camera.</param>
        /// <param name="cameraUpVector">The up vector of the camera.</param>
        /// <param name="cameraForwardVector">The forward vector of the camera.</param>
        /// <returns>The created billboard matrix</returns>
        public static Matrix4x4D CreateBillboard(
            Vector3D objectPosition, Vector3D cameraPosition, Vector3D cameraUpVector, Vector3D cameraForwardVector)
        {
            const double epsilon = 1e-4;

            var zaxis = new Vector3D(
                objectPosition.X - cameraPosition.X,
                objectPosition.Y - cameraPosition.Y,
                objectPosition.Z - cameraPosition.Z);

            var norm = zaxis.LengthSquared();

            if (norm < epsilon)
            {
                zaxis = -cameraForwardVector;
            }
            else
            {
                zaxis = Vector3D.Multiply(zaxis, 1.0 / Math.Sqrt(norm));
            }

            var xaxis = Vector3D.Normalize(Vector3D.Cross(cameraUpVector, zaxis));

            var yaxis = Vector3D.Cross(zaxis, xaxis);

            Matrix4x4D result;

            result.M11 = xaxis.X;
            result.M12 = xaxis.Y;
            result.M13 = xaxis.Z;
            result.M14 = 0.0;
            result.M21 = yaxis.X;
            result.M22 = yaxis.Y;
            result.M23 = yaxis.Z;
            result.M24 = 0.0;
            result.M31 = zaxis.X;
            result.M32 = zaxis.Y;
            result.M33 = zaxis.Z;
            result.M34 = 0.0;

            result.M41 = objectPosition.X;
            result.M42 = objectPosition.Y;
            result.M43 = objectPosition.Z;
            result.M44 = 1.0;

            return result;
        }

        /// <summary>
        /// Creates a cylindrical billboard that rotates around a specified axis.
        /// </summary>
        /// <param name="objectPosition">Position of the object the billboard will rotate around.</param>
        /// <param name="cameraPosition">Position of the camera.</param>
        /// <param name="rotateAxis">Axis to rotate the billboard around.</param>
        /// <param name="cameraForwardVector">Forward vector of the camera.</param>
        /// <param name="objectForwardVector">Forward vector of the object.</param>
        /// <returns>The created billboard matrix.</returns>
        public static Matrix4x4D CreateConstrainedBillboard(
            Vector3D objectPosition, Vector3D cameraPosition, Vector3D rotateAxis, Vector3D cameraForwardVector, Vector3D objectForwardVector)
        {
            const double epsilon = 1e-4;
            const double minAngle = 1.0 - 0.1 * (Math.PI / 180.0); // 0.1 degrees

            // Treat the case when object and camera positions are too close.
            var faceDir = new Vector3D(
                objectPosition.X - cameraPosition.X,
                objectPosition.Y - cameraPosition.Y,
                objectPosition.Z - cameraPosition.Z);

            var norm = faceDir.LengthSquared();

            if (norm < epsilon)
            {
                faceDir = -cameraForwardVector;
            }
            else
            {
                faceDir = Vector3D.Multiply(faceDir, (1.0 / Math.Sqrt(norm)));
            }

            var yaxis = rotateAxis;
            Vector3D xaxis;
            Vector3D zaxis;

            // Treat the case when angle between faceDir and rotateAxis is too close to 0.
            var dot = Vector3D.Dot(rotateAxis, faceDir);

            if (Math.Abs(dot) > minAngle)
            {
                zaxis = objectForwardVector;

                // Make sure passed values are useful for compute.
                dot = Vector3D.Dot(rotateAxis, zaxis);

                if (Math.Abs(dot) > minAngle)
                {
                    zaxis = (Math.Abs(rotateAxis.Z) > minAngle) ? new Vector3D(1.0, 0.0, 0.0) : new Vector3D(0.0, 0.0, -1.0);
                }

                xaxis = Vector3D.Normalize(Vector3D.Cross(rotateAxis, zaxis));
                zaxis = Vector3D.Normalize(Vector3D.Cross(xaxis, rotateAxis));
            }
            else
            {
                xaxis = Vector3D.Normalize(Vector3D.Cross(rotateAxis, faceDir));
                zaxis = Vector3D.Normalize(Vector3D.Cross(xaxis, yaxis));
            }

            Matrix4x4D result;

            result.M11 = xaxis.X;
            result.M12 = xaxis.Y;
            result.M13 = xaxis.Z;
            result.M14 = 0.0;
            result.M21 = yaxis.X;
            result.M22 = yaxis.Y;
            result.M23 = yaxis.Z;
            result.M24 = 0.0;
            result.M31 = zaxis.X;
            result.M32 = zaxis.Y;
            result.M33 = zaxis.Z;
            result.M34 = 0.0;

            result.M41 = objectPosition.X;
            result.M42 = objectPosition.Y;
            result.M43 = objectPosition.Z;
            result.M44 = 1.0;

            return result;
        }

        /// <summary>
        /// Creates a translation matrix.
        /// </summary>
        /// <param name="position">The amount to translate in each axis.</param>
        /// <returns>The translation matrix.</returns>
        public static Matrix4x4D CreateTranslation(Vector3D position)
        {
            Matrix4x4D result;

            result.M11 = 1.0;
            result.M12 = 0.0;
            result.M13 = 0.0;
            result.M14 = 0.0;
            result.M21 = 0.0;
            result.M22 = 1.0;
            result.M23 = 0.0;
            result.M24 = 0.0;
            result.M31 = 0.0;
            result.M32 = 0.0;
            result.M33 = 1.0;
            result.M34 = 0.0;

            result.M41 = position.X;
            result.M42 = position.Y;
            result.M43 = position.Z;
            result.M44 = 1.0;

            return result;
        }

        /// <summary>
        /// Creates a translation matrix.
        /// </summary>
        /// <param name="xPosition">The amount to translate on the X-axis.</param>
        /// <param name="yPosition">The amount to translate on the Y-axis.</param>
        /// <param name="zPosition">The amount to translate on the Z-axis.</param>
        /// <returns>The translation matrix.</returns>
        public static Matrix4x4D CreateTranslation(double xPosition, double yPosition, double zPosition)
        {
            Matrix4x4D result;

            result.M11 = 1.0;
            result.M12 = 0.0;
            result.M13 = 0.0;
            result.M14 = 0.0;
            result.M21 = 0.0;
            result.M22 = 1.0;
            result.M23 = 0.0;
            result.M24 = 0.0;
            result.M31 = 0.0;
            result.M32 = 0.0;
            result.M33 = 1.0;
            result.M34 = 0.0;

            result.M41 = xPosition;
            result.M42 = yPosition;
            result.M43 = zPosition;
            result.M44 = 1.0;

            return result;
        }

        /// <summary>
        /// Creates a scaling matrix.
        /// </summary>
        /// <param name="xScale">Value to scale by on the X-axis.</param>
        /// <param name="yScale">Value to scale by on the Y-axis.</param>
        /// <param name="zScale">Value to scale by on the Z-axis.</param>
        /// <returns>The scaling matrix.</returns>
        public static Matrix4x4D CreateScale(double xScale, double yScale, double zScale)
        {
            Matrix4x4D result;

            result.M11 = xScale;
            result.M12 = 0.0;
            result.M13 = 0.0;
            result.M14 = 0.0;
            result.M21 = 0.0;
            result.M22 = yScale;
            result.M23 = 0.0;
            result.M24 = 0.0;
            result.M31 = 0.0;
            result.M32 = 0.0;
            result.M33 = zScale;
            result.M34 = 0.0;
            result.M41 = 0.0;
            result.M42 = 0.0;
            result.M43 = 0.0;
            result.M44 = 1.0;

            return result;
        }

        /// <summary>
        /// Creates a scaling matrix with a center point.
        /// </summary>
        /// <param name="xScale">Value to scale by on the X-axis.</param>
        /// <param name="yScale">Value to scale by on the Y-axis.</param>
        /// <param name="zScale">Value to scale by on the Z-axis.</param>
        /// <param name="centerPoint">The center point.</param>
        /// <returns>The scaling matrix.</returns>
        public static Matrix4x4D CreateScale(double xScale, double yScale, double zScale, Vector3D centerPoint)
        {
            Matrix4x4D result;

            var tx = centerPoint.X * (1.0 - xScale);
            var ty = centerPoint.Y * (1.0 - yScale);
            var tz = centerPoint.Z * (1.0 - zScale);

            result.M11 = xScale;
            result.M12 = 0.0;
            result.M13 = 0.0;
            result.M14 = 0.0;
            result.M21 = 0.0;
            result.M22 = yScale;
            result.M23 = 0.0;
            result.M24 = 0.0;
            result.M31 = 0.0;
            result.M32 = 0.0;
            result.M33 = zScale;
            result.M34 = 0.0;
            result.M41 = tx;
            result.M42 = ty;
            result.M43 = tz;
            result.M44 = 1.0;

            return result;
        }

        /// <summary>
        /// Creates a scaling matrix.
        /// </summary>
        /// <param name="scales">The vector containing the amount to scale by on each axis.</param>
        /// <returns>The scaling matrix.</returns>
        public static Matrix4x4D CreateScale(Vector3D scales)
        {
            Matrix4x4D result;

            result.M11 = scales.X;
            result.M12 = 0.0;
            result.M13 = 0.0;
            result.M14 = 0.0;
            result.M21 = 0.0;
            result.M22 = scales.Y;
            result.M23 = 0.0;
            result.M24 = 0.0;
            result.M31 = 0.0;
            result.M32 = 0.0;
            result.M33 = scales.Z;
            result.M34 = 0.0;
            result.M41 = 0.0;
            result.M42 = 0.0;
            result.M43 = 0.0;
            result.M44 = 1.0;

            return result;
        }

        /// <summary>
        /// Creates a scaling matrix with a center point.
        /// </summary>
        /// <param name="scales">The vector containing the amount to scale by on each axis.</param>
        /// <param name="centerPoint">The center point.</param>
        /// <returns>The scaling matrix.</returns>
        public static Matrix4x4D CreateScale(Vector3D scales, Vector3D centerPoint)
        {
            Matrix4x4D result;

            var tx = centerPoint.X * (1.0 - scales.X);
            var ty = centerPoint.Y * (1.0 - scales.Y);
            var tz = centerPoint.Z * (1.0 - scales.Z);

            result.M11 = scales.X;
            result.M12 = 0.0;
            result.M13 = 0.0;
            result.M14 = 0.0;
            result.M21 = 0.0;
            result.M22 = scales.Y;
            result.M23 = 0.0;
            result.M24 = 0.0;
            result.M31 = 0.0;
            result.M32 = 0.0;
            result.M33 = scales.Z;
            result.M34 = 0.0;
            result.M41 = tx;
            result.M42 = ty;
            result.M43 = tz;
            result.M44 = 1.0;

            return result;
        }

        /// <summary>
        /// Creates a uniform scaling matrix that scales equally on each axis.
        /// </summary>
        /// <param name="scale">The uniform scaling factor.</param>
        /// <returns>The scaling matrix.</returns>
        public static Matrix4x4D CreateScale(double scale)
        {
            Matrix4x4D result;

            result.M11 = scale;
            result.M12 = 0.0;
            result.M13 = 0.0;
            result.M14 = 0.0;
            result.M21 = 0.0;
            result.M22 = scale;
            result.M23 = 0.0;
            result.M24 = 0.0;
            result.M31 = 0.0;
            result.M32 = 0.0;
            result.M33 = scale;
            result.M34 = 0.0;
            result.M41 = 0.0;
            result.M42 = 0.0;
            result.M43 = 0.0;
            result.M44 = 1.0;

            return result;
        }

        /// <summary>
        /// Creates a uniform scaling matrix that scales equally on each axis with a center point.
        /// </summary>
        /// <param name="scale">The uniform scaling factor.</param>
        /// <param name="centerPoint">The center point.</param>
        /// <returns>The scaling matrix.</returns>
        public static Matrix4x4D CreateScale(double scale, Vector3D centerPoint)
        {
            Matrix4x4D result;

            var tx = centerPoint.X * (1.0 - scale);
            var ty = centerPoint.Y * (1.0 - scale);
            var tz = centerPoint.Z * (1.0 - scale);

            result.M11 = scale;
            result.M12 = 0.0;
            result.M13 = 0.0;
            result.M14 = 0.0;
            result.M21 = 0.0;
            result.M22 = scale;
            result.M23 = 0.0;
            result.M24 = 0.0;
            result.M31 = 0.0;
            result.M32 = 0.0;
            result.M33 = scale;
            result.M34 = 0.0;
            result.M41 = tx;
            result.M42 = ty;
            result.M43 = tz;
            result.M44 = 1.0;

            return result;
        }

        /// <summary>
        /// Creates a matrix for rotating points around the X-axis.
        /// </summary>
        /// <param name="radians">The amount, in radians, by which to rotate around the X-axis.</param>
        /// <returns>The rotation matrix.</returns>
        public static Matrix4x4D CreateRotationX(double radians)
        {
            Matrix4x4D result;

            var c = Math.Cos(radians);
            var s = Math.Sin(radians);

            // [  1  0  0  0 ]
            // [  0  c  s  0 ]
            // [  0 -s  c  0 ]
            // [  0  0  0  1 ]
            result.M11 = 1.0;
            result.M12 = 0.0;
            result.M13 = 0.0;
            result.M14 = 0.0;
            result.M21 = 0.0;
            result.M22 = c;
            result.M23 = s;
            result.M24 = 0.0;
            result.M31 = 0.0;
            result.M32 = -s;
            result.M33 = c;
            result.M34 = 0.0;
            result.M41 = 0.0;
            result.M42 = 0.0;
            result.M43 = 0.0;
            result.M44 = 1.0;

            return result;
        }

        /// <summary>
        /// Creates a matrix for rotating points around the X-axis, from a center point.
        /// </summary>
        /// <param name="radians">The amount, in radians, by which to rotate around the X-axis.</param>
        /// <param name="centerPoint">The center point.</param>
        /// <returns>The rotation matrix.</returns>
        public static Matrix4x4D CreateRotationX(double radians, Vector3D centerPoint)
        {
            Matrix4x4D result;

            var c = Math.Cos(radians);
            var s = Math.Sin(radians);

            var y = centerPoint.Y * (1 - c) + centerPoint.Z * s;
            var z = centerPoint.Z * (1 - c) - centerPoint.Y * s;

            // [  1  0  0  0 ]
            // [  0  c  s  0 ]
            // [  0 -s  c  0 ]
            // [  0  y  z  1 ]
            result.M11 = 1.0;
            result.M12 = 0.0;
            result.M13 = 0.0;
            result.M14 = 0.0;
            result.M21 = 0.0;
            result.M22 = c;
            result.M23 = s;
            result.M24 = 0.0;
            result.M31 = 0.0;
            result.M32 = -s;
            result.M33 = c;
            result.M34 = 0.0;
            result.M41 = 0.0;
            result.M42 = y;
            result.M43 = z;
            result.M44 = 1.0;

            return result;
        }

        /// <summary>
        /// Creates a matrix for rotating points around the Y-axis.
        /// </summary>
        /// <param name="radians">The amount, in radians, by which to rotate around the Y-axis.</param>
        /// <returns>The rotation matrix.</returns>
        public static Matrix4x4D CreateRotationY(double radians)
        {
            Matrix4x4D result;

            var c = Math.Cos(radians);
            var s = Math.Sin(radians);

            // [  c  0 -s  0 ]
            // [  0  1  0  0 ]
            // [  s  0  c  0 ]
            // [  0  0  0  1 ]
            result.M11 = c;
            result.M12 = 0.0;
            result.M13 = -s;
            result.M14 = 0.0;
            result.M21 = 0.0;
            result.M22 = 1.0;
            result.M23 = 0.0;
            result.M24 = 0.0;
            result.M31 = s;
            result.M32 = 0.0;
            result.M33 = c;
            result.M34 = 0.0;
            result.M41 = 0.0;
            result.M42 = 0.0;
            result.M43 = 0.0;
            result.M44 = 1.0;

            return result;
        }

        /// <summary>
        /// Creates a matrix for rotating points around the Y-axis, from a center point.
        /// </summary>
        /// <param name="radians">The amount, in radians, by which to rotate around the Y-axis.</param>
        /// <param name="centerPoint">The center point.</param>
        /// <returns>The rotation matrix.</returns>
        public static Matrix4x4D CreateRotationY(double radians, Vector3D centerPoint)
        {
            Matrix4x4D result;

            var c = Math.Cos(radians);
            var s = Math.Sin(radians);

            var x = centerPoint.X * (1 - c) - centerPoint.Z * s;
            var z = centerPoint.Z * (1 - c) + centerPoint.X * s;

            // [  c  0 -s  0 ]
            // [  0  1  0  0 ]
            // [  s  0  c  0 ]
            // [  x  0  z  1 ]
            result.M11 = c;
            result.M12 = 0.0;
            result.M13 = -s;
            result.M14 = 0.0;
            result.M21 = 0.0;
            result.M22 = 1.0;
            result.M23 = 0.0;
            result.M24 = 0.0;
            result.M31 = s;
            result.M32 = 0.0;
            result.M33 = c;
            result.M34 = 0.0;
            result.M41 = x;
            result.M42 = 0.0;
            result.M43 = z;
            result.M44 = 1.0;

            return result;
        }

        /// <summary>
        /// Creates a matrix for rotating points around the Z-axis.
        /// </summary>
        /// <param name="radians">The amount, in radians, by which to rotate around the Z-axis.</param>
        /// <returns>The rotation matrix.</returns>
        public static Matrix4x4D CreateRotationZ(double radians)
        {
            Matrix4x4D result;

            var c = Math.Cos(radians);
            var s = Math.Sin(radians);

            // [  c  s  0  0 ]
            // [ -s  c  0  0 ]
            // [  0  0  1  0 ]
            // [  0  0  0  1 ]
            result.M11 = c;
            result.M12 = s;
            result.M13 = 0.0;
            result.M14 = 0.0;
            result.M21 = -s;
            result.M22 = c;
            result.M23 = 0.0;
            result.M24 = 0.0;
            result.M31 = 0.0;
            result.M32 = 0.0;
            result.M33 = 1.0;
            result.M34 = 0.0;
            result.M41 = 0.0;
            result.M42 = 0.0;
            result.M43 = 0.0;
            result.M44 = 1.0;

            return result;
        }

        /// <summary>
        /// Creates a matrix for rotating points around the Z-axis, from a center point.
        /// </summary>
        /// <param name="radians">The amount, in radians, by which to rotate around the Z-axis.</param>
        /// <param name="centerPoint">The center point.</param>
        /// <returns>The rotation matrix.</returns>
        public static Matrix4x4D CreateRotationZ(double radians, Vector3D centerPoint)
        {
            Matrix4x4D result;

            var c = Math.Cos(radians);
            var s = Math.Sin(radians);

            var x = centerPoint.X * (1 - c) + centerPoint.Y * s;
            var y = centerPoint.Y * (1 - c) - centerPoint.X * s;

            // [  c  s  0  0 ]
            // [ -s  c  0  0 ]
            // [  0  0  1  0 ]
            // [  x  y  0  1 ]
            result.M11 = c;
            result.M12 = s;
            result.M13 = 0.0;
            result.M14 = 0.0;
            result.M21 = -s;
            result.M22 = c;
            result.M23 = 0.0;
            result.M24 = 0.0;
            result.M31 = 0.0;
            result.M32 = 0.0;
            result.M33 = 1.0;
            result.M34 = 0.0;
            result.M41 = x;
            result.M42 = y;
            result.M43 = 0.0;
            result.M44 = 1.0;

            return result;
        }

        /// <summary>
        /// Creates a matrix that rotates around an arbitrary vector.
        /// </summary>
        /// <param name="axis">The axis to rotate around.</param>
        /// <param name="angle">The angle to rotate around the given axis, in radians.</param>
        /// <returns>The rotation matrix.</returns>
        public static Matrix4x4D CreateFromAxisAngle(Vector3D axis, double angle)
        {
            // a: angle
            // x, y, z: unit vector for axis.
            //
            // Rotation matrix M can compute by using below equation.
            //
            //        T               T
            //  M = uu + (cos a)( I-uu ) + (sin a)S
            //
            // Where:
            //
            //  u = ( x, y, z )
            //
            //      [  0 -z  y ]
            //  S = [  z  0 -x ]
            //      [ -y  x  0 ]
            //
            //      [ 1 0 0 ]
            //  I = [ 0 1 0 ]
            //      [ 0 0 1 ]
            //
            //
            //     [  xx+cosa*(1-xx)   yx-cosa*yx-sina*z zx-cosa*xz+sina*y ]
            // M = [ xy-cosa*yx+sina*z    yy+cosa(1-yy)  yz-cosa*yz-sina*x ]
            //     [ zx-cosa*zx-sina*y zy-cosa*zy+sina*x   zz+cosa*(1-zz)  ]
            //
            double x = axis.X, y = axis.Y, z = axis.Z;
            double sa = Math.Sin(angle), ca = Math.Cos(angle);
            double xx = x * x, yy = y * y, zz = z * z;
            double xy = x * y, xz = x * z, yz = y * z;

            Matrix4x4D result;

            result.M11 = xx + ca * (1.0 - xx);
            result.M12 = xy - ca * xy + sa * z;
            result.M13 = xz - ca * xz - sa * y;
            result.M14 = 0.0;
            result.M21 = xy - ca * xy - sa * z;
            result.M22 = yy + ca * (1.0 - yy);
            result.M23 = yz - ca * yz + sa * x;
            result.M24 = 0.0;
            result.M31 = xz - ca * xz + sa * y;
            result.M32 = yz - ca * yz - sa * x;
            result.M33 = zz + ca * (1.0 - zz);
            result.M34 = 0.0;
            result.M41 = 0.0;
            result.M42 = 0.0;
            result.M43 = 0.0;
            result.M44 = 1.0;

            return result;
        }

        /// <summary>
        /// Creates a perspective projection matrix based on a field of view, aspect ratio, and near and far view plane distances. 
        /// </summary>
        /// <param name="fieldOfView">Field of view in the y direction, in radians.</param>
        /// <param name="aspectRatio">Aspect ratio, defined as view space width divided by height.</param>
        /// <param name="nearPlaneDistance">Distance to the near view plane.</param>
        /// <param name="farPlaneDistance">Distance to the far view plane.</param>
        /// <returns>The perspective projection matrix.</returns>
        public static Matrix4x4D CreatePerspectiveFieldOfView(
            double fieldOfView, double aspectRatio, double nearPlaneDistance, double farPlaneDistance)
        {
            if (fieldOfView <= 0.0 || fieldOfView >= Math.PI)
                throw new ArgumentOutOfRangeException("fieldOfView");

            if (nearPlaneDistance <= 0.0)
                throw new ArgumentOutOfRangeException("nearPlaneDistance");

            if (farPlaneDistance <= 0.0)
                throw new ArgumentOutOfRangeException("farPlaneDistance");

            if (nearPlaneDistance >= farPlaneDistance)
                throw new ArgumentOutOfRangeException("nearPlaneDistance");

            var yScale = 1.0 / Math.Tan(fieldOfView * 0.5);
            var xScale = yScale / aspectRatio;

            Matrix4x4D result;

            result.M11 = xScale;
            result.M12 = result.M13 = result.M14 = 0.0;

            result.M22 = yScale;
            result.M21 = result.M23 = result.M24 = 0.0;

            result.M31 = result.M32 = 0.0;
            result.M33 = farPlaneDistance / (nearPlaneDistance - farPlaneDistance);
            result.M34 = -1.0;

            result.M41 = result.M42 = result.M44 = 0.0;
            result.M43 = nearPlaneDistance * farPlaneDistance / (nearPlaneDistance - farPlaneDistance);

            return result;
        }

        /// <summary>
        /// Creates a perspective projection matrix from the given view volume dimensions.
        /// </summary>
        /// <param name="width">Width of the view volume at the near view plane.</param>
        /// <param name="height">Height of the view volume at the near view plane.</param>
        /// <param name="nearPlaneDistance">Distance to the near view plane.</param>
        /// <param name="farPlaneDistance">Distance to the far view plane.</param>
        /// <returns>The perspective projection matrix.</returns>
        public static Matrix4x4D CreatePerspective(double width, double height, double nearPlaneDistance, double farPlaneDistance)
        {
            if (nearPlaneDistance <= 0.0)
                throw new ArgumentOutOfRangeException("nearPlaneDistance");

            if (farPlaneDistance <= 0.0)
                throw new ArgumentOutOfRangeException("farPlaneDistance");

            if (nearPlaneDistance >= farPlaneDistance)
                throw new ArgumentOutOfRangeException("nearPlaneDistance");

            Matrix4x4D result;

            result.M11 = 2.0 * nearPlaneDistance / width;
            result.M12 = result.M13 = result.M14 = 0.0;

            result.M22 = 2.0 * nearPlaneDistance / height;
            result.M21 = result.M23 = result.M24 = 0.0;

            result.M33 = farPlaneDistance / (nearPlaneDistance - farPlaneDistance);
            result.M31 = result.M32 = 0.0;
            result.M34 = -1.0;

            result.M41 = result.M42 = result.M44 = 0.0;
            result.M43 = nearPlaneDistance * farPlaneDistance / (nearPlaneDistance - farPlaneDistance);

            return result;
        }

        /// <summary>
        /// Creates a customized, perspective projection matrix.
        /// </summary>
        /// <param name="left">Minimum x-value of the view volume at the near view plane.</param>
        /// <param name="right">Maximum x-value of the view volume at the near view plane.</param>
        /// <param name="bottom">Minimum y-value of the view volume at the near view plane.</param>
        /// <param name="top">Maximum y-value of the view volume at the near view plane.</param>
        /// <param name="nearPlaneDistance">Distance to the near view plane.</param>
        /// <param name="farPlaneDistance">Distance to of the far view plane.</param>
        /// <returns>The perspective projection matrix.</returns>
        public static Matrix4x4D CreatePerspectiveOffCenter(
            double left, double right, double bottom, double top, double nearPlaneDistance, double farPlaneDistance)
        {
            if (nearPlaneDistance <= 0.0)
                throw new ArgumentOutOfRangeException("nearPlaneDistance");

            if (farPlaneDistance <= 0.0)
                throw new ArgumentOutOfRangeException("farPlaneDistance");

            if (nearPlaneDistance >= farPlaneDistance)
                throw new ArgumentOutOfRangeException("nearPlaneDistance");

            Matrix4x4D result;

            result.M11 = 2.0 * nearPlaneDistance / (right - left);
            result.M12 = result.M13 = result.M14 = 0.0;

            result.M22 = 2.0 * nearPlaneDistance / (top - bottom);
            result.M21 = result.M23 = result.M24 = 0.0;

            result.M31 = (left + right) / (right - left);
            result.M32 = (top + bottom) / (top - bottom);
            result.M33 = farPlaneDistance / (nearPlaneDistance - farPlaneDistance);
            result.M34 = -1.0;

            result.M43 = nearPlaneDistance * farPlaneDistance / (nearPlaneDistance - farPlaneDistance);
            result.M41 = result.M42 = result.M44 = 0.0;

            return result;
        }

        /// <summary>
        /// Creates an orthographic perspective matrix from the given view volume dimensions.
        /// </summary>
        /// <param name="width">Width of the view volume.</param>
        /// <param name="height">Height of the view volume.</param>
        /// <param name="zNearPlane">Minimum Z-value of the view volume.</param>
        /// <param name="zFarPlane">Maximum Z-value of the view volume.</param>
        /// <returns>The orthographic projection matrix.</returns>
        public static Matrix4x4D CreateOrthographic(double width, double height, double zNearPlane, double zFarPlane)
        {
            Matrix4x4D result;

            result.M11 = 2.0 / width;
            result.M12 = result.M13 = result.M14 = 0.0;

            result.M22 = 2.0 / height;
            result.M21 = result.M23 = result.M24 = 0.0;

            result.M33 = 1.0 / (zNearPlane - zFarPlane);
            result.M31 = result.M32 = result.M34 = 0.0;

            result.M41 = result.M42 = 0.0;
            result.M43 = zNearPlane / (zNearPlane - zFarPlane);
            result.M44 = 1.0;

            return result;
        }

        /// <summary>
        /// Builds a customized, orthographic projection matrix.
        /// </summary>
        /// <param name="left">Minimum X-value of the view volume.</param>
        /// <param name="right">Maximum X-value of the view volume.</param>
        /// <param name="bottom">Minimum Y-value of the view volume.</param>
        /// <param name="top">Maximum Y-value of the view volume.</param>
        /// <param name="zNearPlane">Minimum Z-value of the view volume.</param>
        /// <param name="zFarPlane">Maximum Z-value of the view volume.</param>
        /// <returns>The orthographic projection matrix.</returns>
        public static Matrix4x4D CreateOrthographicOffCenter(
            double left, double right, double bottom, double top, double zNearPlane, double zFarPlane)
        {
            Matrix4x4D result;

            result.M11 = 2.0 / (right - left);
            result.M12 = result.M13 = result.M14 = 0.0;

            result.M22 = 2.0 / (top - bottom);
            result.M21 = result.M23 = result.M24 = 0.0;

            result.M33 = 1.0 / (zNearPlane - zFarPlane);
            result.M31 = result.M32 = result.M34 = 0.0;

            result.M41 = (left + right) / (left - right);
            result.M42 = (top + bottom) / (bottom - top);
            result.M43 = zNearPlane / (zNearPlane - zFarPlane);
            result.M44 = 1.0;

            return result;
        }

        /// <summary>
        /// Creates a view matrix.
        /// </summary>
        /// <param name="cameraPosition">The position of the camera.</param>
        /// <param name="cameraTarget">The target towards which the camera is pointing.</param>
        /// <param name="cameraUpVector">The direction that is "up" from the camera's point of view.</param>
        /// <returns>The view matrix.</returns>
        public static Matrix4x4D CreateLookAt(Vector3D cameraPosition, Vector3D cameraTarget, Vector3D cameraUpVector)
        {
            var zaxis = Vector3D.Normalize(cameraPosition - cameraTarget);
            var xaxis = Vector3D.Normalize(Vector3D.Cross(cameraUpVector, zaxis));
            var yaxis = Vector3D.Cross(zaxis, xaxis);

            Matrix4x4D result;

            result.M11 = xaxis.X;
            result.M12 = yaxis.X;
            result.M13 = zaxis.X;
            result.M14 = 0.0;
            result.M21 = xaxis.Y;
            result.M22 = yaxis.Y;
            result.M23 = zaxis.Y;
            result.M24 = 0.0;
            result.M31 = xaxis.Z;
            result.M32 = yaxis.Z;
            result.M33 = zaxis.Z;
            result.M34 = 0.0;
            result.M41 = -Vector3D.Dot(xaxis, cameraPosition);
            result.M42 = -Vector3D.Dot(yaxis, cameraPosition);
            result.M43 = -Vector3D.Dot(zaxis, cameraPosition);
            result.M44 = 1.0;

            return result;
        }

        /// <summary>
        /// Creates a world matrix with the specified parameters.
        /// </summary>
        /// <param name="position">The position of the object; used in translation operations.</param>
        /// <param name="forward">Forward direction of the object.</param>
        /// <param name="up">Upward direction of the object; usually [0, 1, 0].</param>
        /// <returns>The world matrix.</returns>
        public static Matrix4x4D CreateWorld(Vector3D position, Vector3D forward, Vector3D up)
        {
            var zaxis = Vector3D.Normalize(-forward);
            var xaxis = Vector3D.Normalize(Vector3D.Cross(up, zaxis));
            var yaxis = Vector3D.Cross(zaxis, xaxis);

            Matrix4x4D result;

            result.M11 = xaxis.X;
            result.M12 = xaxis.Y;
            result.M13 = xaxis.Z;
            result.M14 = 0.0;
            result.M21 = yaxis.X;
            result.M22 = yaxis.Y;
            result.M23 = yaxis.Z;
            result.M24 = 0.0;
            result.M31 = zaxis.X;
            result.M32 = zaxis.Y;
            result.M33 = zaxis.Z;
            result.M34 = 0.0;
            result.M41 = position.X;
            result.M42 = position.Y;
            result.M43 = position.Z;
            result.M44 = 1.0;

            return result;
        }

        /// <summary>
        /// Creates a rotation matrix from the given QuaternionD rotation value.
        /// </summary>
        /// <param name="quaternion">The source QuaternionD.</param>
        /// <returns>The rotation matrix.</returns>
        public static Matrix4x4D CreateFromQuaternion(QuaternionD quaternion)
        {
            Matrix4x4D result;

            var xx = quaternion.X * quaternion.X;
            var yy = quaternion.Y * quaternion.Y;
            var zz = quaternion.Z * quaternion.Z;

            var xy = quaternion.X * quaternion.Y;
            var wz = quaternion.Z * quaternion.W;
            var xz = quaternion.Z * quaternion.X;
            var wy = quaternion.Y * quaternion.W;
            var yz = quaternion.Y * quaternion.Z;
            var wx = quaternion.X * quaternion.W;

            result.M11 = 1.0 - 2.0 * (yy + zz);
            result.M12 = 2.0 * (xy + wz);
            result.M13 = 2.0 * (xz - wy);
            result.M14 = 0.0;
            result.M21 = 2.0 * (xy - wz);
            result.M22 = 1.0 - 2.0 * (zz + xx);
            result.M23 = 2.0 * (yz + wx);
            result.M24 = 0.0;
            result.M31 = 2.0 * (xz + wy);
            result.M32 = 2.0 * (yz - wx);
            result.M33 = 1.0 - 2.0 * (yy + xx);
            result.M34 = 0.0;
            result.M41 = 0.0;
            result.M42 = 0.0;
            result.M43 = 0.0;
            result.M44 = 1.0;

            return result;
        }

        /// <summary>
        /// Creates a rotation matrix from the specified yaw, pitch, and roll.
        /// </summary>
        /// <param name="yaw">Angle of rotation, in radians, around the Y-axis.</param>
        /// <param name="pitch">Angle of rotation, in radians, around the X-axis.</param>
        /// <param name="roll">Angle of rotation, in radians, around the Z-axis.</param>
        /// <returns>The rotation matrix.</returns>
        public static Matrix4x4D CreateFromYawPitchRoll(double yaw, double pitch, double roll)
        {
            var q = QuaternionD.CreateFromYawPitchRoll(yaw, pitch, roll);

            return CreateFromQuaternion(q);
        }

        /// <summary>
        /// Creates a Matrix that flattens geometry into a specified Plane as if casting a shadow from a specified light source.
        /// </summary>
        /// <param name="lightDirection">The direction from which the light that will cast the shadow is coming.</param>
        /// <param name="plane">The Plane onto which the new matrix should flatten geometry so as to cast a shadow.</param>
        /// <returns>A new Matrix that can be used to flatten geometry onto the specified plane from the specified direction.</returns>
        public static Matrix4x4D CreateShadow(Vector3D lightDirection, Plane plane)
        {
            var p = Plane.Normalize(plane);

            var dot = p.Normal.X * lightDirection.X + p.Normal.Y * lightDirection.Y + p.Normal.Z * lightDirection.Z;
            double a = -p.Normal.X;
            double b = -p.Normal.Y;
            double c = -p.Normal.Z;
            double d = -p.D;

            Matrix4x4D result;

            result.M11 = a * lightDirection.X + dot;
            result.M21 = b * lightDirection.X;
            result.M31 = c * lightDirection.X;
            result.M41 = d * lightDirection.X;

            result.M12 = a * lightDirection.Y;
            result.M22 = b * lightDirection.Y + dot;
            result.M32 = c * lightDirection.Y;
            result.M42 = d * lightDirection.Y;

            result.M13 = a * lightDirection.Z;
            result.M23 = b * lightDirection.Z;
            result.M33 = c * lightDirection.Z + dot;
            result.M43 = d * lightDirection.Z;

            result.M14 = 0.0;
            result.M24 = 0.0;
            result.M34 = 0.0;
            result.M44 = dot;

            return result;
        }

        /// <summary>
        /// Creates a Matrix that reflects the coordinate system about a specified Plane.
        /// </summary>
        /// <param name="value">The Plane about which to create a reflection.</param>
        /// <returns>A new matrix expressing the reflection.</returns>
        public static Matrix4x4D CreateReflection(Plane value)
        {
            value = Plane.Normalize(value);

            double a = value.Normal.X;
            double b = value.Normal.Y;
            double c = value.Normal.Z;

            var fa = -2.0 * a;
            var fb = -2.0 * b;
            var fc = -2.0 * c;

            Matrix4x4D result;

            result.M11 = fa * a + 1.0;
            result.M12 = fb * a;
            result.M13 = fc * a;
            result.M14 = 0.0;

            result.M21 = fa * b;
            result.M22 = fb * b + 1.0;
            result.M23 = fc * b;
            result.M24 = 0.0;

            result.M31 = fa * c;
            result.M32 = fb * c;
            result.M33 = fc * c + 1.0;
            result.M34 = 0.0;

            result.M41 = fa * value.D;
            result.M42 = fb * value.D;
            result.M43 = fc * value.D;
            result.M44 = 1.0;

            return result;
        }

        /// <summary>
        /// Calculates the determinant of the matrix.
        /// </summary>
        /// <returns>The determinant of the matrix.</returns>
        public double GetDeterminant()
        {
            // | a b c d |     | f g h |     | e g h |     | e f h |     | e f g |
            // | e f g h | = a | j k l | - b | i k l | + c | i j l | - d | i j k |
            // | i j k l |     | n o p |     | m o p |     | m n p |     | m n o |
            // | m n o p |
            //
            //   | f g h |
            // a | j k l | = a ( f ( kp - lo ) - g ( jp - ln ) + h ( jo - kn ) )
            //   | n o p |
            //
            //   | e g h |     
            // b | i k l | = b ( e ( kp - lo ) - g ( ip - lm ) + h ( io - km ) )
            //   | m o p |     
            //
            //   | e f h |
            // c | i j l | = c ( e ( jp - ln ) - f ( ip - lm ) + h ( in - jm ) )
            //   | m n p |
            //
            //   | e f g |
            // d | i j k | = d ( e ( jo - kn ) - f ( io - km ) + g ( in - jm ) )
            //   | m n o |
            //
            // Cost of operation
            // 17 adds and 28 muls.
            //
            // add: 6 + 8 + 3 = 17
            // mul: 12 + 16 = 28

            double a = M11, b = M12, c = M13, d = M14;
            double e = M21, f = M22, g = M23, h = M24;
            double i = M31, j = M32, k = M33, l = M34;
            double m = M41, n = M42, o = M43, p = M44;

            var kp_lo = k * p - l * o;
            var jp_ln = j * p - l * n;
            var jo_kn = j * o - k * n;
            var ip_lm = i * p - l * m;
            var io_km = i * o - k * m;
            var in_jm = i * n - j * m;

            return a * (f * kp_lo - g * jp_ln + h * jo_kn) -
                   b * (e * kp_lo - g * ip_lm + h * io_km) +
                   c * (e * jp_ln - f * ip_lm + h * in_jm) -
                   d * (e * jo_kn - f * io_km + g * in_jm);
        }

        /// <summary>
        /// Attempts to calculate the inverse of the given matrix. If successful, result will contain the inverted matrix.
        /// </summary>
        /// <param name="matrix">The source matrix to invert.</param>
        /// <param name="result">If successful, contains the inverted matrix.</param>
        /// <returns>True if the source matrix could be inverted; False otherwise.</returns>
        public static bool Invert(Matrix4x4D matrix, out Matrix4x4D result)
        {
            //                                       -1
            // If you have matrix M, inverse Matrix M   can compute
            //
            //     -1       1      
            //    M   = --------- A
            //            det(M)
            //
            // A is adjugate (adjoint) of M, where,
            //
            //      T
            // A = C
            //
            // C is Cofactor matrix of M, where,
            //           i + j
            // C   = (-1)      * det(M  )
            //  ij                    ij
            //
            //     [ a b c d ]
            // M = [ e f g h ]
            //     [ i j k l ]
            //     [ m n o p ]
            //
            // First Row
            //           2 | f g h |
            // C   = (-1)  | j k l | = + ( f ( kp - lo ) - g ( jp - ln ) + h ( jo - kn ) )
            //  11         | n o p |
            //
            //           3 | e g h |
            // C   = (-1)  | i k l | = - ( e ( kp - lo ) - g ( ip - lm ) + h ( io - km ) )
            //  12         | m o p |
            //
            //           4 | e f h |
            // C   = (-1)  | i j l | = + ( e ( jp - ln ) - f ( ip - lm ) + h ( in - jm ) )
            //  13         | m n p |
            //
            //           5 | e f g |
            // C   = (-1)  | i j k | = - ( e ( jo - kn ) - f ( io - km ) + g ( in - jm ) )
            //  14         | m n o |
            //
            // Second Row
            //           3 | b c d |
            // C   = (-1)  | j k l | = - ( b ( kp - lo ) - c ( jp - ln ) + d ( jo - kn ) )
            //  21         | n o p |
            //
            //           4 | a c d |
            // C   = (-1)  | i k l | = + ( a ( kp - lo ) - c ( ip - lm ) + d ( io - km ) )
            //  22         | m o p |
            //
            //           5 | a b d |
            // C   = (-1)  | i j l | = - ( a ( jp - ln ) - b ( ip - lm ) + d ( in - jm ) )
            //  23         | m n p |
            //
            //           6 | a b c |
            // C   = (-1)  | i j k | = + ( a ( jo - kn ) - b ( io - km ) + c ( in - jm ) )
            //  24         | m n o |
            //
            // Third Row
            //           4 | b c d |
            // C   = (-1)  | f g h | = + ( b ( gp - ho ) - c ( fp - hn ) + d ( fo - gn ) )
            //  31         | n o p |
            //
            //           5 | a c d |
            // C   = (-1)  | e g h | = - ( a ( gp - ho ) - c ( ep - hm ) + d ( eo - gm ) )
            //  32         | m o p |
            //
            //           6 | a b d |
            // C   = (-1)  | e f h | = + ( a ( fp - hn ) - b ( ep - hm ) + d ( en - fm ) )
            //  33         | m n p |
            //
            //           7 | a b c |
            // C   = (-1)  | e f g | = - ( a ( fo - gn ) - b ( eo - gm ) + c ( en - fm ) )
            //  34         | m n o |
            //
            // Fourth Row
            //           5 | b c d |
            // C   = (-1)  | f g h | = - ( b ( gl - hk ) - c ( fl - hj ) + d ( fk - gj ) )
            //  41         | j k l |
            //
            //           6 | a c d |
            // C   = (-1)  | e g h | = + ( a ( gl - hk ) - c ( el - hi ) + d ( ek - gi ) )
            //  42         | i k l |
            //
            //           7 | a b d |
            // C   = (-1)  | e f h | = - ( a ( fl - hj ) - b ( el - hi ) + d ( ej - fi ) )
            //  43         | i j l |
            //
            //           8 | a b c |
            // C   = (-1)  | e f g | = + ( a ( fk - gj ) - b ( ek - gi ) + c ( ej - fi ) )
            //  44         | i j k |
            //
            // Cost of operation
            // 53 adds, 104 muls, and 1 div.
            double a = matrix.M11, b = matrix.M12, c = matrix.M13, d = matrix.M14;
            double e = matrix.M21, f = matrix.M22, g = matrix.M23, h = matrix.M24;
            double i = matrix.M31, j = matrix.M32, k = matrix.M33, l = matrix.M34;
            double m = matrix.M41, n = matrix.M42, o = matrix.M43, p = matrix.M44;

            var kp_lo = k * p - l * o;
            var jp_ln = j * p - l * n;
            var jo_kn = j * o - k * n;
            var ip_lm = i * p - l * m;
            var io_km = i * o - k * m;
            var in_jm = i * n - j * m;

            var a11 = +(f * kp_lo - g * jp_ln + h * jo_kn);
            var a12 = -(e * kp_lo - g * ip_lm + h * io_km);
            var a13 = +(e * jp_ln - f * ip_lm + h * in_jm);
            var a14 = -(e * jo_kn - f * io_km + g * in_jm);

            var det = a * a11 + b * a12 + c * a13 + d * a14;

            if (Math.Abs(det) < double.Epsilon)
            {
                result = new Matrix4x4D(double.NaN, double.NaN, double.NaN, double.NaN,
                                       double.NaN, double.NaN, double.NaN, double.NaN,
                                       double.NaN, double.NaN, double.NaN, double.NaN,
                                       double.NaN, double.NaN, double.NaN, double.NaN);
                return false;
            }

            var invDet = 1.0 / det;

            result.M11 = a11 * invDet;
            result.M21 = a12 * invDet;
            result.M31 = a13 * invDet;
            result.M41 = a14 * invDet;

            result.M12 = -(b * kp_lo - c * jp_ln + d * jo_kn) * invDet;
            result.M22 = +(a * kp_lo - c * ip_lm + d * io_km) * invDet;
            result.M32 = -(a * jp_ln - b * ip_lm + d * in_jm) * invDet;
            result.M42 = +(a * jo_kn - b * io_km + c * in_jm) * invDet;

            var gp_ho = g * p - h * o;
            var fp_hn = f * p - h * n;
            var fo_gn = f * o - g * n;
            var ep_hm = e * p - h * m;
            var eo_gm = e * o - g * m;
            var en_fm = e * n - f * m;

            result.M13 = +(b * gp_ho - c * fp_hn + d * fo_gn) * invDet;
            result.M23 = -(a * gp_ho - c * ep_hm + d * eo_gm) * invDet;
            result.M33 = +(a * fp_hn - b * ep_hm + d * en_fm) * invDet;
            result.M43 = -(a * fo_gn - b * eo_gm + c * en_fm) * invDet;

            var gl_hk = g * l - h * k;
            var fl_hj = f * l - h * j;
            var fk_gj = f * k - g * j;
            var el_hi = e * l - h * i;
            var ek_gi = e * k - g * i;
            var ej_fi = e * j - f * i;

            result.M14 = -(b * gl_hk - c * fl_hj + d * fk_gj) * invDet;
            result.M24 = +(a * gl_hk - c * el_hi + d * ek_gi) * invDet;
            result.M34 = -(a * fl_hj - b * el_hi + d * ej_fi) * invDet;
            result.M44 = +(a * fk_gj - b * ek_gi + c * ej_fi) * invDet;

            return true;
        }

        private struct CanonicalBasis
        {
            public Vector3D Row0;
            public Vector3D Row1;
            public Vector3D Row2;
        };

        [SecuritySafeCritical]
        private struct VectorBasis
        {
            public unsafe Vector3D* Element0;
            public unsafe Vector3D* Element1;
            public unsafe Vector3D* Element2;
        }

        /// <summary>
        /// Attempts to extract the scale, translation, and rotation components from the given scale/rotation/translation matrix.
        /// If successful, the out parameters will contained the extracted values.
        /// </summary>
        /// <param name="matrix">The source matrix.</param>
        /// <param name="scale">The scaling component of the transformation matrix.</param>
        /// <param name="rotation">The rotation component of the transformation matrix.</param>
        /// <param name="translation">The translation component of the transformation matrix</param>
        /// <returns>True if the source matrix was successfully decomposed; False otherwise.</returns>
        [SecuritySafeCritical]
        public static bool Decompose(Matrix4x4D matrix, out Vector3D scale, out QuaternionD rotation, out Vector3D translation)
        {
            var result = true;

            unsafe
            {
                fixed (Vector3D* scaleBase = &scale)
                {
                    var pfScales = (double*)scaleBase;
                    const double EPSILON = 0.0001;

                    VectorBasis vectorBasis;
                    var pVectorBasis = (Vector3D**)&vectorBasis;

                    var matTemp = Identity;
                    var canonicalBasis = new CanonicalBasis();
                    var pCanonicalBasis = &canonicalBasis.Row0;

                    canonicalBasis.Row0 = new Vector3D(1.0, 0.0, 0.0);
                    canonicalBasis.Row1 = new Vector3D(0.0, 1.0, 0.0);
                    canonicalBasis.Row2 = new Vector3D(0.0, 0.0, 1.0);

                    translation = new Vector3D(
                        matrix.M41,
                        matrix.M42,
                        matrix.M43);

                    pVectorBasis[0] = (Vector3D*)&matTemp.M11;
                    pVectorBasis[1] = (Vector3D*)&matTemp.M21;
                    pVectorBasis[2] = (Vector3D*)&matTemp.M31;

                    *(pVectorBasis[0]) = new Vector3D(matrix.M11, matrix.M12, matrix.M13);
                    *(pVectorBasis[1]) = new Vector3D(matrix.M21, matrix.M22, matrix.M23);
                    *(pVectorBasis[2]) = new Vector3D(matrix.M31, matrix.M32, matrix.M33);

                    scale.X = pVectorBasis[0]->Length();
                    scale.Y = pVectorBasis[1]->Length();
                    scale.Z = pVectorBasis[2]->Length();

                    uint a, b, c;
                    #region Ranking
                    double x = pfScales[0], y = pfScales[1], z = pfScales[2];
                    if (x < y)
                    {
                        if (y < z)
                        {
                            a = 2;
                            b = 1;
                            c = 0;
                        }
                        else
                        {
                            a = 1;

                            if (x < z)
                            {
                                b = 2;
                                c = 0;
                            }
                            else
                            {
                                b = 0;
                                c = 2;
                            }
                        }
                    }
                    else
                    {
                        if (x < z)
                        {
                            a = 2;
                            b = 0;
                            c = 1;
                        }
                        else
                        {
                            a = 0;

                            if (y < z)
                            {
                                b = 2;
                                c = 1;
                            }
                            else
                            {
                                b = 1;
                                c = 2;
                            }
                        }
                    }
                    #endregion

                    if (pfScales[a] < EPSILON)
                    {
                        *(pVectorBasis[a]) = pCanonicalBasis[a];
                    }

                    *pVectorBasis[a] = Vector3D.Normalize(*pVectorBasis[a]);

                    if (pfScales[b] < EPSILON)
                    {
                        uint cc;

                        var fAbsX = Math.Abs(pVectorBasis[a]->X);
                        var fAbsY = Math.Abs(pVectorBasis[a]->Y);
                        var fAbsZ = Math.Abs(pVectorBasis[a]->Z);

                        #region Ranking
                        if (fAbsX < fAbsY)
                        {
                            if (fAbsY < fAbsZ)
                            {
                                cc = 0;
                            }
                            else
                            {
                                cc = fAbsX < fAbsZ ? 0U : 2U;
                            }
                        }
                        else
                        {
                            if (fAbsX < fAbsZ)
                            {
                                cc = 1;
                            }
                            else
                            {
                                cc = fAbsY < fAbsZ ? 1U : 2U;
                            }
                        }
                        #endregion

                        *pVectorBasis[b] = Vector3D.Cross(*pVectorBasis[a], *(pCanonicalBasis + cc));
                    }

                    *pVectorBasis[b] = Vector3D.Normalize(*pVectorBasis[b]);

                    if (pfScales[c] < EPSILON)
                    {
                        *pVectorBasis[c] = Vector3D.Cross(*pVectorBasis[a], *pVectorBasis[b]);
                    }

                    *pVectorBasis[c] = Vector3D.Normalize(*pVectorBasis[c]);

                    var det = matTemp.GetDeterminant();

                    // use Kramer's rule to check for handedness of coordinate system
                    if (det < 0.0)
                    {
                        // switch coordinate system by negating the scale and inverting the basis vector on the x-axis
                        pfScales[a] = -pfScales[a];
                        *pVectorBasis[a] = -(*pVectorBasis[a]);

                        det = -det;
                    }

                    det -= 1.0;
                    det *= det;

                    if ((EPSILON < det))
                    {
                        // Non-SRT matrix encountered
                        rotation = QuaternionD.Identity;
                        result = false;
                    }
                    else
                    {
                        // generate the quaternion from the matrix
                        rotation = QuaternionD.CreateFromRotationMatrix(matTemp);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Transforms the given matrix by applying the given QuaternionD rotation.
        /// </summary>
        /// <param name="value">The source matrix to transform.</param>
        /// <param name="rotation">The rotation to apply.</param>
        /// <returns>The transformed matrix.</returns>
        public static Matrix4x4D Transform(Matrix4x4D value, QuaternionD rotation)
        {
            // Compute rotation matrix.
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

            var q11 = 1.0 - yy2 - zz2;
            var q21 = xy2 - wz2;
            var q31 = xz2 + wy2;

            var q12 = xy2 + wz2;
            var q22 = 1.0 - xx2 - zz2;
            var q32 = yz2 - wx2;

            var q13 = xz2 - wy2;
            var q23 = yz2 + wx2;
            var q33 = 1.0 - xx2 - yy2;

            Matrix4x4D result;

            // First row
            result.M11 = value.M11 * q11 + value.M12 * q21 + value.M13 * q31;
            result.M12 = value.M11 * q12 + value.M12 * q22 + value.M13 * q32;
            result.M13 = value.M11 * q13 + value.M12 * q23 + value.M13 * q33;
            result.M14 = value.M14;

            // Second row
            result.M21 = value.M21 * q11 + value.M22 * q21 + value.M23 * q31;
            result.M22 = value.M21 * q12 + value.M22 * q22 + value.M23 * q32;
            result.M23 = value.M21 * q13 + value.M22 * q23 + value.M23 * q33;
            result.M24 = value.M24;

            // Third row
            result.M31 = value.M31 * q11 + value.M32 * q21 + value.M33 * q31;
            result.M32 = value.M31 * q12 + value.M32 * q22 + value.M33 * q32;
            result.M33 = value.M31 * q13 + value.M32 * q23 + value.M33 * q33;
            result.M34 = value.M34;

            // Fourth row
            result.M41 = value.M41 * q11 + value.M42 * q21 + value.M43 * q31;
            result.M42 = value.M41 * q12 + value.M42 * q22 + value.M43 * q32;
            result.M43 = value.M41 * q13 + value.M42 * q23 + value.M43 * q33;
            result.M44 = value.M44;

            return result;
        }

        /// <summary>
        /// Transposes the rows and columns of a matrix.
        /// </summary>
        /// <param name="matrix">The source matrix.</param>
        /// <returns>The transposed matrix.</returns>
        public static Matrix4x4D Transpose(Matrix4x4D matrix)
        {
            Matrix4x4D result;

            result.M11 = matrix.M11;
            result.M12 = matrix.M21;
            result.M13 = matrix.M31;
            result.M14 = matrix.M41;
            result.M21 = matrix.M12;
            result.M22 = matrix.M22;
            result.M23 = matrix.M32;
            result.M24 = matrix.M42;
            result.M31 = matrix.M13;
            result.M32 = matrix.M23;
            result.M33 = matrix.M33;
            result.M34 = matrix.M43;
            result.M41 = matrix.M14;
            result.M42 = matrix.M24;
            result.M43 = matrix.M34;
            result.M44 = matrix.M44;

            return result;
        }

        /// <summary>
        /// Linearly interpolates between the corresponding values of two matrices.
        /// </summary>
        /// <param name="matrix1">The first source matrix.</param>
        /// <param name="matrix2">The second source matrix.</param>
        /// <param name="amount">The relative weight of the second source matrix.</param>
        /// <returns>The interpolated matrix.</returns>
        public static Matrix4x4D Lerp(Matrix4x4D matrix1, Matrix4x4D matrix2, double amount)
        {
            Matrix4x4D result;

            // First row
            result.M11 = matrix1.M11 + (matrix2.M11 - matrix1.M11) * amount;
            result.M12 = matrix1.M12 + (matrix2.M12 - matrix1.M12) * amount;
            result.M13 = matrix1.M13 + (matrix2.M13 - matrix1.M13) * amount;
            result.M14 = matrix1.M14 + (matrix2.M14 - matrix1.M14) * amount;

            // Second row
            result.M21 = matrix1.M21 + (matrix2.M21 - matrix1.M21) * amount;
            result.M22 = matrix1.M22 + (matrix2.M22 - matrix1.M22) * amount;
            result.M23 = matrix1.M23 + (matrix2.M23 - matrix1.M23) * amount;
            result.M24 = matrix1.M24 + (matrix2.M24 - matrix1.M24) * amount;

            // Third row
            result.M31 = matrix1.M31 + (matrix2.M31 - matrix1.M31) * amount;
            result.M32 = matrix1.M32 + (matrix2.M32 - matrix1.M32) * amount;
            result.M33 = matrix1.M33 + (matrix2.M33 - matrix1.M33) * amount;
            result.M34 = matrix1.M34 + (matrix2.M34 - matrix1.M34) * amount;

            // Fourth row
            result.M41 = matrix1.M41 + (matrix2.M41 - matrix1.M41) * amount;
            result.M42 = matrix1.M42 + (matrix2.M42 - matrix1.M42) * amount;
            result.M43 = matrix1.M43 + (matrix2.M43 - matrix1.M43) * amount;
            result.M44 = matrix1.M44 + (matrix2.M44 - matrix1.M44) * amount;

            return result;
        }

        /// <summary>
        /// Returns a new matrix with the negated elements of the given matrix.
        /// </summary>
        /// <param name="value">The source matrix.</param>
        /// <returns>The negated matrix.</returns>
        public static Matrix4x4D Negate(Matrix4x4D value)
        {
            Matrix4x4D result;

            result.M11 = -value.M11;
            result.M12 = -value.M12;
            result.M13 = -value.M13;
            result.M14 = -value.M14;
            result.M21 = -value.M21;
            result.M22 = -value.M22;
            result.M23 = -value.M23;
            result.M24 = -value.M24;
            result.M31 = -value.M31;
            result.M32 = -value.M32;
            result.M33 = -value.M33;
            result.M34 = -value.M34;
            result.M41 = -value.M41;
            result.M42 = -value.M42;
            result.M43 = -value.M43;
            result.M44 = -value.M44;

            return result;
        }

        /// <summary>
        /// Adds two matrices together.
        /// </summary>
        /// <param name="value1">The first source matrix.</param>
        /// <param name="value2">The second source matrix.</param>
        /// <returns>The resulting matrix.</returns>
        public static Matrix4x4D Add(Matrix4x4D value1, Matrix4x4D value2)
        {
            Matrix4x4D result;

            result.M11 = value1.M11 + value2.M11;
            result.M12 = value1.M12 + value2.M12;
            result.M13 = value1.M13 + value2.M13;
            result.M14 = value1.M14 + value2.M14;
            result.M21 = value1.M21 + value2.M21;
            result.M22 = value1.M22 + value2.M22;
            result.M23 = value1.M23 + value2.M23;
            result.M24 = value1.M24 + value2.M24;
            result.M31 = value1.M31 + value2.M31;
            result.M32 = value1.M32 + value2.M32;
            result.M33 = value1.M33 + value2.M33;
            result.M34 = value1.M34 + value2.M34;
            result.M41 = value1.M41 + value2.M41;
            result.M42 = value1.M42 + value2.M42;
            result.M43 = value1.M43 + value2.M43;
            result.M44 = value1.M44 + value2.M44;

            return result;
        }

        /// <summary>
        /// Subtracts the second matrix from the first.
        /// </summary>
        /// <param name="value1">The first source matrix.</param>
        /// <param name="value2">The second source matrix.</param>
        /// <returns>The result of the subtraction.</returns>
        public static Matrix4x4D Subtract(Matrix4x4D value1, Matrix4x4D value2)
        {
            Matrix4x4D result;

            result.M11 = value1.M11 - value2.M11;
            result.M12 = value1.M12 - value2.M12;
            result.M13 = value1.M13 - value2.M13;
            result.M14 = value1.M14 - value2.M14;
            result.M21 = value1.M21 - value2.M21;
            result.M22 = value1.M22 - value2.M22;
            result.M23 = value1.M23 - value2.M23;
            result.M24 = value1.M24 - value2.M24;
            result.M31 = value1.M31 - value2.M31;
            result.M32 = value1.M32 - value2.M32;
            result.M33 = value1.M33 - value2.M33;
            result.M34 = value1.M34 - value2.M34;
            result.M41 = value1.M41 - value2.M41;
            result.M42 = value1.M42 - value2.M42;
            result.M43 = value1.M43 - value2.M43;
            result.M44 = value1.M44 - value2.M44;

            return result;
        }

        /// <summary>
        /// Multiplies a matrix by another matrix.
        /// </summary>
        /// <param name="value1">The first source matrix.</param>
        /// <param name="value2">The second source matrix.</param>
        /// <returns>The result of the multiplication.</returns>
        public static Matrix4x4D Multiply(Matrix4x4D value1, Matrix4x4D value2)
        {
            Matrix4x4D result;

            // First row
            result.M11 = value1.M11 * value2.M11 + value1.M12 * value2.M21 + value1.M13 * value2.M31 + value1.M14 * value2.M41;
            result.M12 = value1.M11 * value2.M12 + value1.M12 * value2.M22 + value1.M13 * value2.M32 + value1.M14 * value2.M42;
            result.M13 = value1.M11 * value2.M13 + value1.M12 * value2.M23 + value1.M13 * value2.M33 + value1.M14 * value2.M43;
            result.M14 = value1.M11 * value2.M14 + value1.M12 * value2.M24 + value1.M13 * value2.M34 + value1.M14 * value2.M44;

            // Second row
            result.M21 = value1.M21 * value2.M11 + value1.M22 * value2.M21 + value1.M23 * value2.M31 + value1.M24 * value2.M41;
            result.M22 = value1.M21 * value2.M12 + value1.M22 * value2.M22 + value1.M23 * value2.M32 + value1.M24 * value2.M42;
            result.M23 = value1.M21 * value2.M13 + value1.M22 * value2.M23 + value1.M23 * value2.M33 + value1.M24 * value2.M43;
            result.M24 = value1.M21 * value2.M14 + value1.M22 * value2.M24 + value1.M23 * value2.M34 + value1.M24 * value2.M44;

            // Third row
            result.M31 = value1.M31 * value2.M11 + value1.M32 * value2.M21 + value1.M33 * value2.M31 + value1.M34 * value2.M41;
            result.M32 = value1.M31 * value2.M12 + value1.M32 * value2.M22 + value1.M33 * value2.M32 + value1.M34 * value2.M42;
            result.M33 = value1.M31 * value2.M13 + value1.M32 * value2.M23 + value1.M33 * value2.M33 + value1.M34 * value2.M43;
            result.M34 = value1.M31 * value2.M14 + value1.M32 * value2.M24 + value1.M33 * value2.M34 + value1.M34 * value2.M44;

            // Fourth row
            result.M41 = value1.M41 * value2.M11 + value1.M42 * value2.M21 + value1.M43 * value2.M31 + value1.M44 * value2.M41;
            result.M42 = value1.M41 * value2.M12 + value1.M42 * value2.M22 + value1.M43 * value2.M32 + value1.M44 * value2.M42;
            result.M43 = value1.M41 * value2.M13 + value1.M42 * value2.M23 + value1.M43 * value2.M33 + value1.M44 * value2.M43;
            result.M44 = value1.M41 * value2.M14 + value1.M42 * value2.M24 + value1.M43 * value2.M34 + value1.M44 * value2.M44;

            return result;
        }

        /// <summary>
        /// Multiplies a matrix by a scalar value.
        /// </summary>
        /// <param name="value1">The source matrix.</param>
        /// <param name="value2">The scaling factor.</param>
        /// <returns>The scaled matrix.</returns>
        public static Matrix4x4D Multiply(Matrix4x4D value1, double value2)
        {
            Matrix4x4D result;

            result.M11 = value1.M11 * value2;
            result.M12 = value1.M12 * value2;
            result.M13 = value1.M13 * value2;
            result.M14 = value1.M14 * value2;
            result.M21 = value1.M21 * value2;
            result.M22 = value1.M22 * value2;
            result.M23 = value1.M23 * value2;
            result.M24 = value1.M24 * value2;
            result.M31 = value1.M31 * value2;
            result.M32 = value1.M32 * value2;
            result.M33 = value1.M33 * value2;
            result.M34 = value1.M34 * value2;
            result.M41 = value1.M41 * value2;
            result.M42 = value1.M42 * value2;
            result.M43 = value1.M43 * value2;
            result.M44 = value1.M44 * value2;

            return result;
        }

        /// <summary>
        /// Returns a new matrix with the negated elements of the given matrix.
        /// </summary>
        /// <param name="value">The source matrix.</param>
        /// <returns>The negated matrix.</returns>
        public static Matrix4x4D operator -(Matrix4x4D value)
        {
            Matrix4x4D m;

            m.M11 = -value.M11;
            m.M12 = -value.M12;
            m.M13 = -value.M13;
            m.M14 = -value.M14;
            m.M21 = -value.M21;
            m.M22 = -value.M22;
            m.M23 = -value.M23;
            m.M24 = -value.M24;
            m.M31 = -value.M31;
            m.M32 = -value.M32;
            m.M33 = -value.M33;
            m.M34 = -value.M34;
            m.M41 = -value.M41;
            m.M42 = -value.M42;
            m.M43 = -value.M43;
            m.M44 = -value.M44;

            return m;
        }

        /// <summary>
        /// Adds two matrices together.
        /// </summary>
        /// <param name="value1">The first source matrix.</param>
        /// <param name="value2">The second source matrix.</param>
        /// <returns>The resulting matrix.</returns>
        public static Matrix4x4D operator +(Matrix4x4D value1, Matrix4x4D value2)
        {
            Matrix4x4D m;

            m.M11 = value1.M11 + value2.M11;
            m.M12 = value1.M12 + value2.M12;
            m.M13 = value1.M13 + value2.M13;
            m.M14 = value1.M14 + value2.M14;
            m.M21 = value1.M21 + value2.M21;
            m.M22 = value1.M22 + value2.M22;
            m.M23 = value1.M23 + value2.M23;
            m.M24 = value1.M24 + value2.M24;
            m.M31 = value1.M31 + value2.M31;
            m.M32 = value1.M32 + value2.M32;
            m.M33 = value1.M33 + value2.M33;
            m.M34 = value1.M34 + value2.M34;
            m.M41 = value1.M41 + value2.M41;
            m.M42 = value1.M42 + value2.M42;
            m.M43 = value1.M43 + value2.M43;
            m.M44 = value1.M44 + value2.M44;

            return m;
        }

        /// <summary>
        /// Subtracts the second matrix from the first.
        /// </summary>
        /// <param name="value1">The first source matrix.</param>
        /// <param name="value2">The second source matrix.</param>
        /// <returns>The result of the subtraction.</returns>
        public static Matrix4x4D operator -(Matrix4x4D value1, Matrix4x4D value2)
        {
            Matrix4x4D m;

            m.M11 = value1.M11 - value2.M11;
            m.M12 = value1.M12 - value2.M12;
            m.M13 = value1.M13 - value2.M13;
            m.M14 = value1.M14 - value2.M14;
            m.M21 = value1.M21 - value2.M21;
            m.M22 = value1.M22 - value2.M22;
            m.M23 = value1.M23 - value2.M23;
            m.M24 = value1.M24 - value2.M24;
            m.M31 = value1.M31 - value2.M31;
            m.M32 = value1.M32 - value2.M32;
            m.M33 = value1.M33 - value2.M33;
            m.M34 = value1.M34 - value2.M34;
            m.M41 = value1.M41 - value2.M41;
            m.M42 = value1.M42 - value2.M42;
            m.M43 = value1.M43 - value2.M43;
            m.M44 = value1.M44 - value2.M44;

            return m;
        }

        /// <summary>
        /// Multiplies a matrix by another matrix.
        /// </summary>
        /// <param name="value1">The first source matrix.</param>
        /// <param name="value2">The second source matrix.</param>
        /// <returns>The result of the multiplication.</returns>
        public static Matrix4x4D operator *(Matrix4x4D value1, Matrix4x4D value2)
        {
            Matrix4x4D m;

            // First row
            m.M11 = value1.M11 * value2.M11 + value1.M12 * value2.M21 + value1.M13 * value2.M31 + value1.M14 * value2.M41;
            m.M12 = value1.M11 * value2.M12 + value1.M12 * value2.M22 + value1.M13 * value2.M32 + value1.M14 * value2.M42;
            m.M13 = value1.M11 * value2.M13 + value1.M12 * value2.M23 + value1.M13 * value2.M33 + value1.M14 * value2.M43;
            m.M14 = value1.M11 * value2.M14 + value1.M12 * value2.M24 + value1.M13 * value2.M34 + value1.M14 * value2.M44;

            // Second row
            m.M21 = value1.M21 * value2.M11 + value1.M22 * value2.M21 + value1.M23 * value2.M31 + value1.M24 * value2.M41;
            m.M22 = value1.M21 * value2.M12 + value1.M22 * value2.M22 + value1.M23 * value2.M32 + value1.M24 * value2.M42;
            m.M23 = value1.M21 * value2.M13 + value1.M22 * value2.M23 + value1.M23 * value2.M33 + value1.M24 * value2.M43;
            m.M24 = value1.M21 * value2.M14 + value1.M22 * value2.M24 + value1.M23 * value2.M34 + value1.M24 * value2.M44;

            // Third row
            m.M31 = value1.M31 * value2.M11 + value1.M32 * value2.M21 + value1.M33 * value2.M31 + value1.M34 * value2.M41;
            m.M32 = value1.M31 * value2.M12 + value1.M32 * value2.M22 + value1.M33 * value2.M32 + value1.M34 * value2.M42;
            m.M33 = value1.M31 * value2.M13 + value1.M32 * value2.M23 + value1.M33 * value2.M33 + value1.M34 * value2.M43;
            m.M34 = value1.M31 * value2.M14 + value1.M32 * value2.M24 + value1.M33 * value2.M34 + value1.M34 * value2.M44;

            // Fourth row
            m.M41 = value1.M41 * value2.M11 + value1.M42 * value2.M21 + value1.M43 * value2.M31 + value1.M44 * value2.M41;
            m.M42 = value1.M41 * value2.M12 + value1.M42 * value2.M22 + value1.M43 * value2.M32 + value1.M44 * value2.M42;
            m.M43 = value1.M41 * value2.M13 + value1.M42 * value2.M23 + value1.M43 * value2.M33 + value1.M44 * value2.M43;
            m.M44 = value1.M41 * value2.M14 + value1.M42 * value2.M24 + value1.M43 * value2.M34 + value1.M44 * value2.M44;

            return m;
        }

        /// <summary>
        /// Multiplies a matrix by a scalar value.
        /// </summary>
        /// <param name="value1">The source matrix.</param>
        /// <param name="value2">The scaling factor.</param>
        /// <returns>The scaled matrix.</returns>
        public static Matrix4x4D operator *(Matrix4x4D value1, double value2)
        {
            Matrix4x4D m;

            m.M11 = value1.M11 * value2;
            m.M12 = value1.M12 * value2;
            m.M13 = value1.M13 * value2;
            m.M14 = value1.M14 * value2;
            m.M21 = value1.M21 * value2;
            m.M22 = value1.M22 * value2;
            m.M23 = value1.M23 * value2;
            m.M24 = value1.M24 * value2;
            m.M31 = value1.M31 * value2;
            m.M32 = value1.M32 * value2;
            m.M33 = value1.M33 * value2;
            m.M34 = value1.M34 * value2;
            m.M41 = value1.M41 * value2;
            m.M42 = value1.M42 * value2;
            m.M43 = value1.M43 * value2;
            m.M44 = value1.M44 * value2;
            return m;
        }

        /// <summary>
        /// Returns a boolean indicating whether the given two matrices are equal.
        /// </summary>
        /// <param name="value1">The first matrix to compare.</param>
        /// <param name="value2">The second matrix to compare.</param>
        /// <returns>True if the given matrices are equal; False otherwise.</returns>
        public static bool operator ==(Matrix4x4D value1, Matrix4x4D value2)
        {
            return (value1.M11 == value2.M11 && value1.M22 == value2.M22 && value1.M33 == value2.M33 && value1.M44 == value2.M44 && // Check diagonal element first for early out.
                                                value1.M12 == value2.M12 && value1.M13 == value2.M13 && value1.M14 == value2.M14 &&
                    value1.M21 == value2.M21 && value1.M23 == value2.M23 && value1.M24 == value2.M24 &&
                    value1.M31 == value2.M31 && value1.M32 == value2.M32 && value1.M34 == value2.M34 &&
                    value1.M41 == value2.M41 && value1.M42 == value2.M42 && value1.M43 == value2.M43);
        }

        /// <summary>
        /// Returns a boolean indicating whether the given two matrices are not equal.
        /// </summary>
        /// <param name="value1">The first matrix to compare.</param>
        /// <param name="value2">The second matrix to compare.</param>
        /// <returns>True if the given matrices are not equal; False if they are equal.</returns>
        public static bool operator !=(Matrix4x4D value1, Matrix4x4D value2)
        {
            return (value1.M11 != value2.M11 || value1.M12 != value2.M12 || value1.M13 != value2.M13 || value1.M14 != value2.M14 ||
                    value1.M21 != value2.M21 || value1.M22 != value2.M22 || value1.M23 != value2.M23 || value1.M24 != value2.M24 ||
                    value1.M31 != value2.M31 || value1.M32 != value2.M32 || value1.M33 != value2.M33 || value1.M34 != value2.M34 ||
                    value1.M41 != value2.M41 || value1.M42 != value2.M42 || value1.M43 != value2.M43 || value1.M44 != value2.M44);
        }

        /// <summary>
        /// Returns a boolean indicating whether this matrix instance is equal to the other given matrix.
        /// </summary>
        /// <param name="other">The matrix to compare this instance to.</param>
        /// <returns>True if the matrices are equal; False otherwise.</returns>
        public bool Equals(Matrix4x4D other)
        {
            return (M11 == other.M11 && M22 == other.M22 && M33 == other.M33 && M44 == other.M44 && // Check diagonal element first for early out.
                                        M12 == other.M12 && M13 == other.M13 && M14 == other.M14 &&
                    M21 == other.M21 && M23 == other.M23 && M24 == other.M24 &&
                    M31 == other.M31 && M32 == other.M32 && M34 == other.M34 &&
                    M41 == other.M41 && M42 == other.M42 && M43 == other.M43);
        }

        /// <summary>
        /// Returns a boolean indicating whether the given Object is equal to this matrix instance.
        /// </summary>
        /// <param name="obj">The Object to compare against.</param>
        /// <returns>True if the Object is equal to this matrix; False otherwise.</returns>
        public override bool Equals(object obj)
        {
            return obj is Matrix4x4D matrix && Equals(matrix);
        }

        /// <summary>
        /// Returns a String representing this matrix instance.
        /// </summary>
        /// <returns>The string representation.</returns>
        public override string ToString()
        {
            var ci = CultureInfo.CurrentCulture;

            return string.Format(ci, "{{ {{M11:{0} M12:{1} M13:{2} M14:{3}}} {{M21:{4} M22:{5} M23:{6} M24:{7}}} {{M31:{8} M32:{9} M33:{10} M34:{11}}} {{M41:{12} M42:{13} M43:{14} M44:{15}}} }}",
                                 M11.ToString(ci), M12.ToString(ci), M13.ToString(ci), M14.ToString(ci),
                                 M21.ToString(ci), M22.ToString(ci), M23.ToString(ci), M24.ToString(ci),
                                 M31.ToString(ci), M32.ToString(ci), M33.ToString(ci), M34.ToString(ci),
                                 M41.ToString(ci), M42.ToString(ci), M43.ToString(ci), M44.ToString(ci));
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>The hash code.</returns>
        public override int GetHashCode()
        {
            return M11.GetHashCode() + M12.GetHashCode() + M13.GetHashCode() + M14.GetHashCode() +
                   M21.GetHashCode() + M22.GetHashCode() + M23.GetHashCode() + M24.GetHashCode() +
                   M31.GetHashCode() + M32.GetHashCode() + M33.GetHashCode() + M34.GetHashCode() +
                   M41.GetHashCode() + M42.GetHashCode() + M43.GetHashCode() + M44.GetHashCode();
        }
    }
}
