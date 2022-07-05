using System.Diagnostics;

// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable once CheckNamespace
namespace System.Numerics
{
    [DebuggerDisplay("{DebuggerDisplay}")]
    public struct Angle
    {
        private const double PI2 = Math.PI * 2.0;
        private const double PI180 = 180.0 / Math.PI;

        public static Angle Zero { get; } = FromDegrees(0.0);

        public double Degrees { get; private set; }

        public double Radians { get; private set; }

        private string DebuggerDisplay => $"{Radians} ({Degrees:F}º)";

        public Vector2D ToVector()
        {
            return new Vector2D(Math.Cos(Radians), Math.Sin(Radians));
        }

        public static Angle FromDegrees(double degrees)
        {
            degrees %= 360.0;

            if (degrees < 0.0)
            {
                degrees += 360.0;
            }

            return new Angle
            {
                Degrees = degrees,
                Radians = degrees / PI180,
            };
        }

        public static Angle FromRadians(double radians)
        {
            radians %= PI2;

            if (radians < 0.0)
            {
                radians += PI2;
            }

            return new Angle
            {
                Radians = radians,
                Degrees = radians * PI180,
            };
        }

        public static Angle FromVector(Vector2D vector)
        {
            return FromRadians(Math.Atan2(vector.Y, vector.X));
        }

        public static Angle operator +(Angle left, Angle right)
        {
            return FromDegrees(left.Degrees + right.Degrees);
        }

        public static Angle operator -(Angle left, Angle right)
        {
            return FromDegrees(left.Degrees - right.Degrees);
        }

        public static Angle operator *(Angle left, double right)
        {
            return FromDegrees(left.Degrees * right);
        }

        public static Angle operator *(double left, Angle right)
        {
            return FromDegrees(right.Degrees * left);
        }

        public static Angle operator /(Angle left, double right)
        {
            return FromDegrees(left.Degrees / right);
        }
    }
}
