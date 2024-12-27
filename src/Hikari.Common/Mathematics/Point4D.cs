namespace Hikari.Common.Mathematics;
/// <summary>
/// 坐标点
/// </summary>
public class Point4D
{
    public double X { get; set; }
    public double Y { get; set; }
    public double Z { get; set; }
    public double W { get; set; }
    public Point4D() { }

    public Point4D(double x, double y, double z, double w)
    {
        X = x;
        Y = y;
        Z = z;
        W = w;

    }
    public static Point4D operator +(Point4D first, Point4D second)
    {

        return new Point4D(first.X + second.X, first.Y + second.Y, first.Z + second.Z, first.W + second.W);
    }
    public static Point4D operator -(Point4D first, Point4D second)
    {
        return new Point4D(first.X - second.X, first.Y - second.Y, first.Z - second.Z, first.W - second.W);
    }
    public static Point4D operator +(Point4D first, double factor)
    {
        return new Point4D(first.X + factor, first.Y + factor, first.Z + factor, first.W + factor);
    }
    public static Point4D operator -(Point4D first, double factor)
    {
        return first + (-factor);
    }
    public static Point4D operator *(Point4D first, double factor)
    {
        return new Point4D(first.X * factor, first.Y * factor, first.Z * factor, first.W * factor);
    }
    public static Point4D operator /(Point4D first, double factor)
    {
        return first * (1 / factor);
    }
    public override string ToString() => $"({X}, {Y}, {Z}, {W})";
}