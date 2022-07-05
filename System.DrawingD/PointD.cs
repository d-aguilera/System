// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable enable

using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;

// ReSharper disable CompareOfFloatsByEqualityOperator
// ReSharper disable once CheckNamespace
namespace System.Drawing
{
    /// <summary>
    /// Represents an ordered pair of x and y coordinates that define a point in a two-dimensional plane.
    /// </summary>
    [Serializable]
    [TypeForwardedFrom("System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
    public struct PointD : IEquatable<PointD>
    {
        /// <summary>
        /// Creates a new instance of the <see cref='PointD'/> class with member data left uninitialized.
        /// </summary>
        // ReSharper disable once UnassignedReadonlyField
        public static readonly PointD Empty;

        private double _x; // Do not rename (binary serialization)
        private double _y; // Do not rename (binary serialization)

        /// <summary>
        /// Initializes a new instance of the <see cref='PointD'/> class with the specified coordinates.
        /// </summary>
        public PointD(double x, double y)
        {
            _x = x;
            _y = y;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref='PointD'/> struct from the specified
        /// <see cref="Vector2D"/>.
        /// </summary>
        public PointD(Vector2D vector)
        {
            _x = vector.X;
            _y = vector.Y;
        }

        /// <summary>
        /// Creates a new <see cref="Vector2D"/> from this <see cref="PointD"/>.
        /// </summary>
        public Vector2D ToVector2D() => new Vector2D(_x, _y);

        /// <summary>
        /// Gets a value indicating whether this <see cref='PointD'/> is empty.
        /// </summary>
        [Browsable(false)]
        public readonly bool IsEmpty => _x == 0.0 && _y == 0.0;

        /// <summary>
        /// Gets the x-coordinate of this <see cref='PointD'/>.
        /// </summary>
        public double X
        {
            readonly get => _x;
            set => _x = value;
        }

        /// <summary>
        /// Gets the y-coordinate of this <see cref='PointD'/>.
        /// </summary>
        public double Y
        {
            readonly get => _y;
            set => _y = value;
        }

        /// <summary>
        /// Converts the specified <see cref="PointD"/> to a <see cref="Vector2D"/>.
        /// </summary>
        public static explicit operator Vector2D(PointD point) => point.ToVector2D();

        /// <summary>
        /// Converts the specified <see cref="Vector2D"/> to a <see cref="PointD"/>.
        /// </summary>
        public static explicit operator PointD(Vector2D vector) => new PointD(vector);

        /// <summary>
        /// Translates a <see cref='PointD'/> by a given <see cref='Size'/> .
        /// </summary>
        public static PointD operator +(PointD pt, Size sz) => Add(pt, sz);

        /// <summary>
        /// Translates a <see cref='PointD'/> by the negative of a given <see cref='Size'/> .
        /// </summary>
        public static PointD operator -(PointD pt, Size sz) => Subtract(pt, sz);

        /// <summary>
        /// Translates a <see cref='PointD'/> by a given <see cref='SizeD'/> .
        /// </summary>
        public static PointD operator +(PointD pt, SizeD sz) => Add(pt, sz);

        /// <summary>
        /// Translates a <see cref='PointD'/> by the negative of a given <see cref='SizeD'/> .
        /// </summary>
        public static PointD operator -(PointD pt, SizeD sz) => Subtract(pt, sz);

        /// <summary>
        /// Compares two <see cref='PointD'/> objects. The result specifies whether the values of the
        /// <see cref='PointD.X'/> and <see cref='PointD.Y'/> properties of the two
        /// <see cref='PointD'/> objects are equal.
        /// </summary>
        public static bool operator ==(PointD left, PointD right) => left.X == right.X && left.Y == right.Y;

        /// <summary>
        /// Compares two <see cref='PointD'/> objects. The result specifies whether the values of the
        /// <see cref='PointD.X'/> or <see cref='PointD.Y'/> properties of the two
        /// <see cref='PointD'/> objects are unequal.
        /// </summary>
        public static bool operator !=(PointD left, PointD right) => !(left == right);

        /// <summary>
        /// Translates a <see cref='PointD'/> by a given <see cref='Size'/> .
        /// </summary>
        public static PointD Add(PointD pt, Size sz) => new PointD(pt.X + sz.Width, pt.Y + sz.Height);

        /// <summary>
        /// Translates a <see cref='PointD'/> by the negative of a given <see cref='Size'/> .
        /// </summary>
        public static PointD Subtract(PointD pt, Size sz) => new PointD(pt.X - sz.Width, pt.Y - sz.Height);

        /// <summary>
        /// Translates a <see cref='PointD'/> by a given <see cref='SizeD'/> .
        /// </summary>
        public static PointD Add(PointD pt, SizeD sz) => new PointD(pt.X + sz.Width, pt.Y + sz.Height);

        /// <summary>
        /// Translates a <see cref='PointD'/> by the negative of a given <see cref='SizeD'/> .
        /// </summary>
        public static PointD Subtract(PointD pt, SizeD sz) => new PointD(pt.X - sz.Width, pt.Y - sz.Height);

        public readonly override bool Equals([NotNullWhen(true)] object? obj) => obj is PointD point && Equals(point);

        public readonly bool Equals(PointD other) => this == other;

        public readonly override int GetHashCode() => HashCode.Combine(X.GetHashCode(), Y.GetHashCode());

        public readonly override string ToString() => $"{{X={_x}, Y={_y}}}";
    }
}
