namespace Hikari.Common.Mathematics;
/// <summary>
/// 坐标点
/// </summary>
public class Point4D
{
    /// <summary>
    /// x坐标
    /// </summary>
    public double X { get; set; }
    /// <summary>
    /// y坐标
    /// </summary>
    public double Y { get; set; }
    /// <summary>
    /// z坐标
    /// </summary>
    public double Z { get; set; }
    /// <summary>
    /// w坐标
    /// </summary>
    public double W { get; set; }
    /// <summary>
    /// 构造函数
    /// </summary>
    public Point4D() { }
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="x">x坐标</param>
    /// <param name="y">y坐标</param>
    /// <param name="z">z坐标</param>
    /// <param name="w">w坐标</param>
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
    public override bool Equals(object? obj)
    {
        if (obj is Point4D point)
        {
            return X == point.X && Y == point.Y && Z == point.Z && W == point.W;
        }

        return false;
    }
}