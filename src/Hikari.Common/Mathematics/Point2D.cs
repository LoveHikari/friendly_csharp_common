namespace Hikari.Common.Mathematics;
/// <summary>
/// 坐标点
/// </summary>
public class Point2D
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
    /// 构造函数
    /// </summary>
    public Point2D() { }
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="x">x坐标</param>
    /// <param name="y">y坐标</param>
    public Point2D(double x, double y)
    {
        X = x;
        Y = y;
    }
    /// <summary>
    /// 两个点相加
    /// </summary>
    /// <param name="first">第一个点</param>
    /// <param name="second">第二个点</param>
    /// <returns></returns>
    public static Point2D operator +(Point2D first, Point2D second)
    {

        return new Point2D(first.X + second.X, first.Y + second.Y);
    }
    /// <summary>
    /// 两个点相减
    /// </summary>
    /// <param name="first">第一个点</param>
    /// <param name="second">第二个点</param>
    /// <returns></returns>
    public static Point2D operator -(Point2D first, Point2D second)
    {
        return new Point2D(first.X - second.X, first.Y - second.Y);
    }
    /// <summary>
    /// 位移
    /// </summary>
    /// <param name="first">点坐标</param>
    /// <param name="factor">位移量</param>
    /// <returns></returns>
    public static Point2D operator +(Point2D first, double factor)
    {
        return new Point2D(first.X + factor, first.Y + factor);
    }
    /// <summary>
    /// 位移
    /// </summary>
    /// <param name="first">点坐标</param>
    /// <param name="factor">位移量</param>
    /// <returns></returns>
    public static Point2D operator -(Point2D first, double factor)
    {
        return first + (-factor);
    }
    /// <summary>
    /// 点乘
    /// </summary>
    /// <param name="first">点坐标</param>
    /// <param name="factor">位移量</param>
    /// <returns></returns>
    public static Point2D operator *(Point2D first, double factor)
    {
        return new Point2D(first.X * factor, first.Y * factor);
    }
    /// <summary>
    /// 点除
    /// </summary>
    /// <param name="first">点坐标</param>
    /// <param name="factor">位移量</param>
    /// <returns></returns>
    public static Point2D operator /(Point2D first, double factor)
    {
        return first * (1 / factor);
    }
    /// <summary>
    /// ToString
    /// </summary>
    /// <returns></returns>
    public override string ToString() => $"({X}, {Y})";

    public override bool Equals(object? obj)
    {
        if (obj is Point2D point)
        {
            return X == point.X && Y == point.Y;
        }

        return false;
    }
}