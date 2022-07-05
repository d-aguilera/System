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
    /// Represents the size of a rectangular region with an ordered pair of width and height.
    /// </summary>
    [Serializable]
    [TypeForwardedFrom("System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
    [TypeConverter("SizeDConverter, System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
    public struct SizeD : IEquatable<SizeD>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref='SizeD'/> class.
        /// </summary>
        // ReSharper disable once UnassignedReadonlyField
        public static readonly SizeD Empty;

        private double _width; // Do not rename (binary serialization)
        private double _height; // Do not rename (binary serialization)

        /// <summary>
        /// Initializes a new instance of the <see cref='SizeD'/> class from the specified
        /// existing <see cref='SizeD'/>.
        /// </summary>
        public SizeD(SizeD size)
        {
            _width = size._width;
            _height = size._height;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref='SizeD'/> class from the specified
        /// <see cref='PointD'/>.
        /// </summary>
        public SizeD(PointD pt)
        {
            _width = pt.X;
            _height = pt.Y;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref='SizeD'/> struct from the specified
        /// <see cref="Vector2D"/>.
        /// </summary>
        public SizeD(Vector2D vector)
        {
            _width = vector.X;
            _height = vector.Y;
        }

        /// <summary>
        /// Creates a new <see cref="Vector2D"/> from this <see cref="SizeD"/>.
        /// </summary>
        public Vector2D ToVector2D() => new Vector2D(_width, _height);

        /// <summary>
        /// Initializes a new instance of the <see cref='SizeD'/> class from the specified dimensions.
        /// </summary>
        public SizeD(double width, double height)
        {
            this._width = width;
            this._height = height;
        }

        /// <summary>
        /// Converts the specified <see cref="SizeD"/> to a <see cref="Vector2D"/>.
        /// </summary>
        public static explicit operator Vector2D(SizeD size) => size.ToVector2D();

        /// <summary>
        /// Converts the specified <see cref="Vector2D"/> to a <see cref="SizeD"/>.
        /// </summary>
        public static explicit operator SizeD(Vector2D vector) => new SizeD(vector);

        /// <summary>
        /// Performs vector addition of two <see cref='SizeD'/> objects.
        /// </summary>
        public static SizeD operator +(SizeD sz1, SizeD sz2) => Add(sz1, sz2);

        /// <summary>
        /// Contracts a <see cref='SizeD'/> by another <see cref='SizeD'/>
        /// </summary>
        public static SizeD operator -(SizeD sz1, SizeD sz2) => Subtract(sz1, sz2);

        /// <summary>
        /// Multiplies <see cref="SizeD"/> by a <see cref="double"/> producing <see cref="SizeD"/>.
        /// </summary>
        /// <param name="left">Multiplier of type <see cref="double"/>.</param>
        /// <param name="right">Multiplicand of type <see cref="SizeD"/>.</param>
        /// <returns>Product of type <see cref="SizeD"/>.</returns>
        public static SizeD operator *(double left, SizeD right) => Multiply(right, left);

        /// <summary>
        /// Multiplies <see cref="SizeD"/> by a <see cref="double"/> producing <see cref="SizeD"/>.
        /// </summary>
        /// <param name="left">Multiplicand of type <see cref="SizeD"/>.</param>
        /// <param name="right">Multiplier of type <see cref="double"/>.</param>
        /// <returns>Product of type <see cref="SizeD"/>.</returns>
        public static SizeD operator *(SizeD left, double right) => Multiply(left, right);

        /// <summary>
        /// Divides <see cref="SizeD"/> by a <see cref="double"/> producing <see cref="SizeD"/>.
        /// </summary>
        /// <param name="left">Dividend of type <see cref="SizeD"/>.</param>
        /// <param name="right">Divisor of type <see cref="int"/>.</param>
        /// <returns>Result of type <see cref="SizeD"/>.</returns>
        public static SizeD operator /(SizeD left, double right)
            => new SizeD(left._width / right, left._height / right);

        /// <summary>
        /// Tests whether two <see cref='SizeD'/> objects are identical.
        /// </summary>
        public static bool operator ==(SizeD sz1, SizeD sz2) => sz1.Width == sz2.Width && sz1.Height == sz2.Height;

        /// <summary>
        /// Tests whether two <see cref='SizeD'/> objects are different.
        /// </summary>
        public static bool operator !=(SizeD sz1, SizeD sz2) => !(sz1 == sz2);

        /// <summary>
        /// Converts the specified <see cref='SizeD'/> to a <see cref='PointD'/>.
        /// </summary>
        public static explicit operator PointD(SizeD size) => new PointD(size.Width, size.Height);

        /// <summary>
        /// Tests whether this <see cref='SizeD'/> has zero width and height.
        /// </summary>
        [Browsable(false)]
        public readonly bool IsEmpty => _width == 0.0 && _height == 0.0;

        /// <summary>
        /// Represents the horizontal component of this <see cref='SizeD'/>.
        /// </summary>
        public double Width
        {
            readonly get => _width;
            set => _width = value;
        }

        /// <summary>
        /// Represents the vertical component of this <see cref='SizeD'/>.
        /// </summary>
        public double Height
        {
            readonly get => _height;
            set => _height = value;
        }

        /// <summary>
        /// Performs vector addition of two <see cref='SizeD'/> objects.
        /// </summary>
        public static SizeD Add(SizeD sz1, SizeD sz2) => new SizeD(sz1.Width + sz2.Width, sz1.Height + sz2.Height);

        /// <summary>
        /// Contracts a <see cref='SizeD'/> by another <see cref='SizeD'/>.
        /// </summary>
        public static SizeD Subtract(SizeD sz1, SizeD sz2) => new SizeD(sz1.Width - sz2.Width, sz1.Height - sz2.Height);

        /// <summary>
        /// Tests to see whether the specified object is a <see cref='SizeD'/>  with the same dimensions
        /// as this <see cref='SizeD'/>.
        /// </summary>
        public readonly override bool Equals([NotNullWhen(true)] object? obj) => obj is SizeD size && Equals(size);

        public readonly bool Equals(SizeD other) => this == other;

        public readonly override int GetHashCode() => HashCode.Combine(Width, Height);

        public readonly PointD ToPointD() => (PointD)this;

        public readonly Size ToSize() => new Size(unchecked((int)Width), unchecked((int)Height));

        /// <summary>
        /// Creates a human-readable string that represents this <see cref='SizeD'/>.
        /// </summary>
        public readonly override string ToString() => $"{{Width={_width}, Height={_height}}}";

        /// <summary>
        /// Multiplies <see cref="SizeD"/> by a <see cref="double"/> producing <see cref="SizeD"/>.
        /// </summary>
        /// <param name="size">Multiplicand of type <see cref="SizeD"/>.</param>
        /// <param name="multiplier">Multiplier of type <see cref="double"/>.</param>
        /// <returns>Product of type SizeD.</returns>
        private static SizeD Multiply(SizeD size, double multiplier) =>
            new SizeD(size._width * multiplier, size._height * multiplier);
    }
}
