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
    /// Stores the location and size of a rectangular region.
    /// </summary>
    [Serializable]
    [TypeForwardedFrom("System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
    public struct RectangleD : IEquatable<RectangleD>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref='RectangleD'/> class.
        /// </summary>
        // ReSharper disable once UnassignedReadonlyField
        public static readonly RectangleD Empty;

        private double _x; // Do not rename (binary serialization)
        private double _y; // Do not rename (binary serialization)
        private double _width; // Do not rename (binary serialization)
        private double _height; // Do not rename (binary serialization)

        /// <summary>
        /// Initializes a new instance of the <see cref='RectangleD'/> class with the specified location
        /// and size.
        /// </summary>
        public RectangleD(double x, double y, double width, double height)
        {
            _x = x;
            _y = y;
            _width = width;
            _height = height;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref='RectangleD'/> class with the specified location
        /// and size.
        /// </summary>
        public RectangleD(PointD location, SizeD size)
        {
            _x = location.X;
            _y = location.Y;
            _width = size.Width;
            _height = size.Height;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref='RectangleD'/> struct from the specified
        /// <see cref="Vector4D"/>.
        /// </summary>
        public RectangleD(Vector4D vector)
        {
            _x = vector.X;
            _y = vector.Y;
            _width = vector.Z;
            _height = vector.W;
        }

        /// <summary>
        /// Creates a new <see cref="Vector4D"/> from this <see cref="RectangleD"/>.
        /// </summary>
        public Vector4D ToVector4D() => new Vector4D(_x, _y, _width, _height);

        /// <summary>
        /// Converts the specified <see cref="RectangleD"/> to a <see cref="Vector4D"/>.
        /// </summary>
        public static explicit operator Vector4D(RectangleD rectangle) => rectangle.ToVector4D();

        /// <summary>
        /// Converts the specified <see cref="Vector2"/> to a <see cref="RectangleD"/>.
        /// </summary>
        public static explicit operator RectangleD(Vector4D vector) => new RectangleD(vector);

        /// <summary>
        /// Creates a new <see cref='RectangleD'/> with the specified location and size.
        /// </summary>
        public static RectangleD FromLtrb(double left, double top, double right, double bottom) =>
            new RectangleD(left, top, right - left, bottom - top);

        /// <summary>
        /// Gets or sets the coordinates of the upper-left corner of the rectangular region represented by this
        /// <see cref='RectangleD'/>.
        /// </summary>
        [Browsable(false)]
        public PointD Location
        {
            readonly get => new PointD(X, Y);
            set
            {
                X = value.X;
                Y = value.Y;
            }
        }

        /// <summary>
        /// Gets or sets the size of this <see cref='RectangleD'/>.
        /// </summary>
        [Browsable(false)]
        public SizeD Size
        {
            readonly get => new SizeD(Width, Height);
            set
            {
                Width = value.Width;
                Height = value.Height;
            }
        }

        /// <summary>
        /// Gets or sets the x-coordinate of the upper-left corner of the rectangular region defined by this
        /// <see cref='RectangleD'/>.
        /// </summary>
        public double X
        {
            readonly get => _x;
            set => _x = value;
        }

        /// <summary>
        /// Gets or sets the y-coordinate of the upper-left corner of the rectangular region defined by this
        /// <see cref='RectangleD'/>.
        /// </summary>
        public double Y
        {
            readonly get => _y;
            set => _y = value;
        }

        /// <summary>
        /// Gets or sets the width of the rectangular region defined by this <see cref='RectangleD'/>.
        /// </summary>
        public double Width
        {
            readonly get => _width;
            set => _width = value;
        }

        /// <summary>
        /// Gets or sets the height of the rectangular region defined by this <see cref='RectangleD'/>.
        /// </summary>
        public double Height
        {
            readonly get => _height;
            set => _height = value;
        }

        /// <summary>
        /// Gets the x-coordinate of the upper-left corner of the rectangular region defined by this
        /// <see cref='RectangleD'/> .
        /// </summary>
        [Browsable(false)]
        public readonly double Left => X;

        /// <summary>
        /// Gets the y-coordinate of the upper-left corner of the rectangular region defined by this
        /// <see cref='RectangleD'/>.
        /// </summary>
        [Browsable(false)]
        public readonly double Top => Y;

        /// <summary>
        /// Gets the x-coordinate of the lower-right corner of the rectangular region defined by this
        /// <see cref='RectangleD'/>.
        /// </summary>
        [Browsable(false)]
        public readonly double Right => X + Width;

        /// <summary>
        /// Gets the y-coordinate of the lower-right corner of the rectangular region defined by this
        /// <see cref='RectangleD'/>.
        /// </summary>
        [Browsable(false)]
        public readonly double Bottom => Y + Height;

        /// <summary>
        /// Tests whether this <see cref='RectangleD'/> has a <see cref='RectangleD.Width'/> or a <see cref='RectangleD.Height'/> of 0.
        /// </summary>
        [Browsable(false)]
        public readonly bool IsEmpty => (Width <= 0) || (Height <= 0);

        /// <summary>
        /// Tests whether <paramref name="obj"/> is a <see cref='RectangleD'/> with the same location and
        /// size of this <see cref='RectangleD'/>.
        /// </summary>
        public readonly override bool Equals([NotNullWhen(true)] object? obj) => obj is RectangleD rect && Equals(rect);

        public readonly bool Equals(RectangleD other) => this == other;

        /// <summary>
        /// Tests whether two <see cref='RectangleD'/> objects have equal location and size.
        /// </summary>
        public static bool operator ==(RectangleD left, RectangleD right) =>
            left.X == right.X && left.Y == right.Y && left.Width == right.Width && left.Height == right.Height;

        /// <summary>
        /// Tests whether two <see cref='RectangleD'/> objects differ in location or size.
        /// </summary>
        public static bool operator !=(RectangleD left, RectangleD right) => !(left == right);

        /// <summary>
        /// Determines if the specified point is contained within the rectangular region defined by this
        /// <see cref='Rectangle'/> .
        /// </summary>
        public readonly bool Contains(double x, double y) => X <= x && x < X + Width && Y <= y && y < Y + Height;

        /// <summary>
        /// Determines if the specified point is contained within the rectangular region defined by this
        /// <see cref='Rectangle'/> .
        /// </summary>
        public readonly bool Contains(PointD pt) => Contains(pt.X, pt.Y);

        /// <summary>
        /// Determines if the rectangular region represented by <paramref name="rect"/> is entirely contained within
        /// the rectangular region represented by this <see cref='Rectangle'/> .
        /// </summary>
        public readonly bool Contains(RectangleD rect) =>
            (X <= rect.X) && (rect.X + rect.Width <= X + Width) && (Y <= rect.Y) && (rect.Y + rect.Height <= Y + Height);

        /// <summary>
        /// Gets the hash code for this <see cref='RectangleD'/>.
        /// </summary>
        public readonly override int GetHashCode() => HashCode.Combine(X, Y, Width, Height);

        /// <summary>
        /// Inflates this <see cref='Rectangle'/> by the specified amount.
        /// </summary>
        public void Inflate(double x, double y)
        {
            X -= x;
            Y -= y;
            Width += 2.0 * x;
            Height += 2.0 * y;
        }

        /// <summary>
        /// Inflates this <see cref='Rectangle'/> by the specified amount.
        /// </summary>
        public void Inflate(SizeD size) => Inflate(size.Width, size.Height);

        /// <summary>
        /// Creates a <see cref='Rectangle'/> that is inflated by the specified amount.
        /// </summary>
        public static RectangleD Inflate(RectangleD rect, double x, double y)
        {
            RectangleD r = rect;
            r.Inflate(x, y);
            return r;
        }

        /// <summary>
        /// Creates a Rectangle that represents the intersection between this Rectangle and rect.
        /// </summary>
        public void Intersect(RectangleD rect)
        {
            RectangleD result = Intersect(rect, this);

            X = result.X;
            Y = result.Y;
            Width = result.Width;
            Height = result.Height;
        }

        /// <summary>
        /// Creates a rectangle that represents the intersection between a and b. If there is no intersection, an
        /// empty rectangle is returned.
        /// </summary>
        public static RectangleD Intersect(RectangleD a, RectangleD b)
        {
            double x1 = Math.Max(a.X, b.X);
            double x2 = Math.Min(a.X + a.Width, b.X + b.Width);
            double y1 = Math.Max(a.Y, b.Y);
            double y2 = Math.Min(a.Y + a.Height, b.Y + b.Height);

            if (x2 >= x1 && y2 >= y1)
            {
                return new RectangleD(x1, y1, x2 - x1, y2 - y1);
            }

            return Empty;
        }

        /// <summary>
        /// Determines if this rectangle intersects with rect.
        /// </summary>
        public readonly bool IntersectsWith(RectangleD rect) =>
            (rect.X < X + Width) && (X < rect.X + rect.Width) && (rect.Y < Y + Height) && (Y < rect.Y + rect.Height);

        /// <summary>
        /// Creates a rectangle that represents the union between a and b.
        /// </summary>
        public static RectangleD Union(RectangleD a, RectangleD b)
        {
            double x1 = Math.Min(a.X, b.X);
            double x2 = Math.Max(a.X + a.Width, b.X + b.Width);
            double y1 = Math.Min(a.Y, b.Y);
            double y2 = Math.Max(a.Y + a.Height, b.Y + b.Height);

            return new RectangleD(x1, y1, x2 - x1, y2 - y1);
        }

        /// <summary>
        /// Adjusts the location of this rectangle by the specified amount.
        /// </summary>
        public void Offset(PointD pos) => Offset(pos.X, pos.Y);

        /// <summary>
        /// Adjusts the location of this rectangle by the specified amount.
        /// </summary>
        public void Offset(double x, double y)
        {
            X += x;
            Y += y;
        }

        public readonly RectangleF ToRectangleF()
        {
            return RectangleF.FromLTRB((float)Left, (float)Top, (float)Right, (float)Bottom);
        }

        /// <summary>
        /// Converts the <see cref='RectangleD.Location'/> and <see cref='RectangleD.Size'/>
        /// of this <see cref='RectangleD'/> to a human-readable string.
        /// </summary>
        public readonly override string ToString() => $"{{X={X},Y={Y},Width={Width},Height={Height}}}";

        /// <summary>
        /// Converts the specified <see cref='Rectangle'/> to a
        /// <see cref='RectangleD'/>.
        /// </summary>
        public static implicit operator RectangleD(Rectangle r) => new RectangleD(r.X, r.Y, r.Width, r.Height);
    }
}
