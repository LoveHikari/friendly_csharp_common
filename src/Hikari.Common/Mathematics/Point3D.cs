namespace Hikari.Common.Mathematics;
/// <summary>
/// 坐标点
/// </summary>
public class Point3D
{
    public double X { get; set; }
    public double Y { get; set; }
    public double Z { get; set; }
    /// <summary>
    /// 构造函数
    /// </summary>
    public Point3D() { }
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="x">x坐标</param>
    /// <param name="y">y坐标</param>
    /// <param name="z">z坐标</param>
    public Point3D(double x, double y, double z)
    {
        X = x;
        Y = y;
        Z = z;
    }
    /// <summary>
    /// 计算到另一点的距离
    /// </summary>
    /// <param name="other">另一点</param>
    /// <returns>距离</returns>
    public double DistanceTo(Point3D other)
    {
        double dx = X - other.X;
        double dy = Y - other.Y;
        double dz = Z - other.Z;

        return Math.Sqrt(dx * dx + dy * dy + dz * dz);
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
    public override bool Equals(object? obj)
    {
        if (obj is Point3D point)
        {
            return X == point.X && Y == point.Y && Z == point.Z;
        }

        return false;
    }
}