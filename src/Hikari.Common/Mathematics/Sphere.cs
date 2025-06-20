namespace Hikari.Common.Mathematics;

/// <summary>
/// 球体
/// </summary>
public class Sphere
{
    /// <summary>
    /// 地球
    /// </summary>
    public static Sphere Earth => new(new Point3D(0, 0, 0), 6371.393);

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="center">球心坐标</param>
    /// <param name="radius">半径</param>
    public Sphere(Point3D center, double radius)
    {
        if (radius < 0)
        {
            throw new ArgumentException("半径不能为负数", nameof(radius));
        }
        Center = center;
        Radius = radius;
    }
    /// <summary>
    /// 重载构造函数 - 只传入半径，中心默认为原点
    /// </summary>
    /// <param name="radius">半径</param>
    public Sphere(double radius) : this(new Point3D(0, 0, 0), radius) { }
    /// <summary>
    /// 球心坐标
    /// </summary>
    public Point3D Center { get; }
    /// <summary>
    /// 半径
    /// </summary>
    public double Radius { get; }

    /// <summary>
    /// 角度转弧度
    /// </summary>
    /// <param name="d">角度</param>
    /// <returns>弧度</returns>
    private static double Angle2Radian(double d)
    {
        return d / 180.0 * Math.PI;
    }

    /// <summary>
    /// 计算球体上两点的弧长
    /// </summary>
    /// <param name="lat1"></param>
    /// <param name="lng1"></param>
    /// <param name="lat2"></param>
    /// <param name="lng2"></param>
    /// <returns></returns>
    public double GetDistance(double lat1, double lng1, double lat2, double lng2)
    {
        var rad1 = Angle2Radian(lat1);
        var rad2 = Angle2Radian(lat2);
        return 2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin((rad1 - rad2) / 2), 2) + Math.Cos(rad1) * Math.Cos(rad2) * Math.Pow(Math.Sin((Angle2Radian(lng1) - Angle2Radian(lng2)) / 2), 2))) * Radius;
    }

    /// <summary>
    /// 是否相交
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool IsIntersectWith(Sphere other)
    {
        return !IsExternallyTangentWith(other) && !IsInternallyTangentWith(other) && !IsSeparateWith(other) && !IsContainWith(other);
    }

    /// <summary>
    /// 是否外切
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool IsExternallyTangentWith(Sphere other)
    {
        double distance = Center.DistanceTo(other.Center);
        double sumOfRadii = Radius + other.Radius;
        return Math.Abs(distance - sumOfRadii) < double.Epsilon;
    }
    /// <summary>
    /// 是否内切
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool IsInternallyTangentWith(Sphere other)
    {
        double distance = Center.DistanceTo(other.Center);
        double diffOfRadii = Math.Abs(Radius - other.Radius);
        return Math.Abs(distance - diffOfRadii) < double.Epsilon;
    }
    /// <summary>
    /// 是否相离
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool IsSeparateWith(Sphere other)
    {
        double distance = Center.DistanceTo(other.Center);
        double sumOfRadii = Radius + other.Radius;
        return distance > sumOfRadii;
    }
    /// <summary>
    /// 是否包含
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool IsContainWith(Sphere other)
    {
        double distance = Center.DistanceTo(other.Center);
        double diffOfRadii = Math.Abs(Radius - other.Radius);
        return distance < diffOfRadii;
    }
}
