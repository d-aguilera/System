using System.Drawing.Drawing2D;
using System.Numerics;

// ReSharper disable once CheckNamespace
namespace System.Drawing
{
    public class GraphicsD
    {
        public double DpiX => Graphics.DpiX;
        public double DpiY => Graphics.DpiY;
        public Graphics Graphics { get; }

        public Matrix Transform
        {
            get => Graphics.Transform;
            set => Graphics.Transform = value;
        }
        public Region Clip
        {
            get => Graphics.Clip;
            set => Graphics.Clip = value;
        }

        public GraphicsD(Graphics g)
        {
            Graphics = g;
        }

        public void Clear(Color color)
        {
            Graphics.Clear(color);
        }

        public void DrawEllipse(Pen pen, double x, double y, double width, double height)
        {
            Graphics.DrawEllipse(pen, (float)x, (float)y, (float)width, (float)height);
        }

        public void DrawEllipse(Pen pen, RectangleD rect)
        {
            DrawEllipse(pen, rect.X, rect.Y, rect.Width, rect.Height);
        }

        public void DrawImage(Image image, double x, double y)
        {
            Graphics.DrawImage(image, (float)x, (float)y);
        }

        public void DrawLines(Pen pen, Vector2D[] vectors)
        {
            var points = new PointF[vectors.Length];
            for (var i = 0; i < vectors.Length; i++)
            {
                var v = vectors[i];
                points[i] = new PointF((float)v.X, (float)v.Y);
            }

            Graphics.DrawLines(pen, points);
        }

        public void DrawPath(Pen pen, GraphicsPath path)
        {
            Graphics.DrawPath(pen, path);
        }

        public void FillRectangle(SolidBrush brush, double x, double y, double width, double height)
        {
            Graphics.FillRectangle(brush, (float)x, (float)y, (float)width, (float)height);
        }

        public void ResetClip()
        {
            Graphics.ResetClip();
        }

        public void ResetTransform()
        {
            Graphics.ResetTransform();
        }

        public void RotateTransform(double angle)
        {
            Graphics.RotateTransform((float)angle);
        }

        public void ScaleTransform(double sx, double sy)
        {
            Graphics.ScaleTransform((float)sx, (float)sy);
        }

        public void TranslateTransform(double dx, double dy)
        {
            Graphics.TranslateTransform((float)dx, (float)dy);
        }
    }
}
