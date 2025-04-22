namespace Hikari.Common.Mathematics;
/// <summary>
/// 坐标点
/// </summary>
public class Point3D
{
    public double X { get; set; }
    public double Y { get; set; }
    public double Z { get; set; }
    public Point3D() { }

    public Point3D(double x, double y, double z)
    {
        X = x;
        Y = y;
        Z = z;
    }
    public static Point3D operator +(Point3D first, Point3D second)
    {

        return new Point3D(first.X + second.X, first.Y + second.Y, first.Z + second.Z);
    }
    public static Point3D operator -(Point3D first, Point3D second)
    {
        return new Point3D(first.X - second.X, first.Y - second.Y, first.Z - second.Z);
    }
    public static Point3D operator +(Point3D first, double factor)
    {
        return new Point3D(first.X + factor, first.Y + factor, first.Z + factor);
    }
    public static Point3D operator -(Point3D first, double factor)
    {
        return first + (-factor);
    }
    public static Point3D operator *(Point3D first, double factor)
    {
        return new Point3D(first.X * factor, first.Y * factor, first.Z * factor);
    }
    public static Point3D operator /(Point3D first, double factor)
    {
        return first * (1 / factor);
    }
    public override string ToString() => $"({X}, {Y}, {Z})";
}