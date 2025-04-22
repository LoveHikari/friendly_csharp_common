namespace Hikari.Common.Mathematics;
/// <summary>
/// 坐标点
/// </summary>
public class Point2D
{
    public double X { get; set; }
    public double Y { get; set; }
    public Point2D() { }

    public Point2D(double x, double y)
    {
        X = x;
        Y = y;
    }
    public static Point2D operator +(Point2D first, Point2D second)
    {

        return new Point2D(first.X + second.X, first.Y + second.Y);
    }
    public static Point2D operator -(Point2D first, Point2D second)
    {
        return new Point2D(first.X - second.X, first.Y - second.Y);
    }
    public static Point2D operator +(Point2D first, double factor)
    {
        return new Point2D(first.X + factor, first.Y + factor);
    }
    public static Point2D operator -(Point2D first, double factor)
    {
        return first + (-factor);
    }
    public static Point2D operator *(Point2D first, double factor)
    {
        return new Point2D(first.X * factor, first.Y * factor);
    }
    public static Point2D operator /(Point2D first, double factor)
    {
        return first * (1 / factor);
    }
    public override string ToString() => $"({X}, {Y})";
}