namespace Hikari.Common.Mathematics;

/// <summary>
/// 椭圆
/// </summary>
public class Ellipse
{
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="center">椭圆中心坐标</param>
    /// <param name="semiMajorAxis">长半轴</param>
    /// <param name="semiMinorAxis">短半轴</param>
    public Ellipse(Point2D center, double semiMajorAxis, double semiMinorAxis)
    {
        Center = center;
        SemiMajorAxis = semiMajorAxis;
        SemiMinorAxis = semiMinorAxis;

        if (semiMajorAxis < 0 || semiMinorAxis < 0)
        {
            throw new ArgumentException("长半轴和短半轴不能为负数");
        }
    }

    /// <summary>
    /// 椭圆中心坐标
    /// </summary>
    public Point2D Center { get; }

    /// <summary>
    /// 长半轴
    /// </summary>
    public double SemiMajorAxis { get; }

    /// <summary>
    /// 短半轴
    /// </summary>
    public double SemiMinorAxis { get; }

    /// <summary>
    /// 是否相交
    /// </summary>
    /// <param name="that">另一个椭圆</param>
    /// <returns></returns>
    public bool IsCrossWith(Ellipse that)
    {
        //判断相交的步骤
        //将椭圆方程代入：将两个椭圆的方程代入，形成一个方程组。
        //求解方程组：通过代数方法求解这个方程组，得到交点。
        //检查交点：如果有实数解，则说明两个椭圆相交。
        //说明
        //参数：h1, k1, a1, b1 是第一个椭圆的中心和半轴，h2, k2, a2, b2 是第二个椭圆的参数。
        //相交条件：通过计算椭圆的参数，判断是否满足相交条件。
        //返回值：如果满足条件，则返回 true，表示两个椭圆相交。
        //注意事项
        //这个方法是一个近似判断，适用于大多数情况，但在某些特殊情况下可能需要更复杂的几何算法。
        //实际应用中，可能需要考虑椭圆的旋转、平移等变换，这会使得判断相交的算法更加复杂。对于旋转的椭圆，通常需要将其转换为标准位置（未旋转）进行计算。

        // 椭圆1的参数
        double h1 = this.Center.X;
        double k1 = this.Center.Y;
        double a1 = this.SemiMajorAxis;
        double b1 = this.SemiMinorAxis;

        // 椭圆2的参数
        double h2 = that.Center.X;
        double k2 = that.Center.Y;
        double a2 = that.SemiMajorAxis;
        double b2 = that.SemiMinorAxis;

        // 计算椭圆的相交条件
        double A = (b1 * b1 * (h2 - h1) * (h2 - h1)) + (a1 * a1 * (k2 - k1) * (k2 - k1));
        double B = (b2 * b2 * (h2 - h1) * (h2 - h1)) + (a2 * a2 * (k2 - k1) * (k2 - k1));
        double C = (a1 * a1 * b2 * b2) - (a2 * a2 * b1 * b1);

        // 判断是否相交
        return (A <= C && B <= C);
    }

    /// <summary>
    /// 是否相切
    /// </summary>
    /// <param name="that">另一个椭圆</param>
    /// <returns></returns>
    public bool IsIntersectWith(Ellipse that)
    {
        //判断相切的条件
        //两个椭圆相切的条件是它们的交点数量为1。可以通过以下步骤来判断：
        //计算椭圆的交点：将两个椭圆的方程代入，形成一个方程组。
        //求解方程组：通过代数方法求解这个方程组，得到交点。
        //检查交点：如果有一个交点且该交点满足两个椭圆的方程，则说明两个椭圆相切。
        //说明
        //参数：h1, k1, a1, b1 是第一个椭圆的中心和半轴，h2, k2, a2, b2 是第二个椭圆的参数。
        //相切条件：通过计算两个椭圆中心之间的距离，判断是否满足相切条件。
        //相切的条件是两个椭圆的中心距离等于它们的长半轴之和或差，或者短半轴之和或差。
        //返回值：如果满足条件，则返回 true，表示两个椭圆相切。
        //注意事项
        //这个方法是一个近似判断，适用于大多数情况，但在某些特殊情况下可能需要更复杂的几何算法。
        //实际应用中，可能需要考虑椭圆的旋转、平移等变换，这会使得判断相切的算法更加复杂。对于旋转的椭圆，通常需要将其转换为标准位置（未旋转）进行计算。
        // 椭圆1的参数
        double h1 = this.Center.X;
        double k1 = this.Center.Y;
        double a1 = this.SemiMajorAxis;
        double b1 = this.SemiMinorAxis;

        // 椭圆2的参数
        double h2 = that.Center.X;
        double k2 = that.Center.Y;
        double a2 = that.SemiMajorAxis;
        double b2 = that.SemiMinorAxis;

        // 计算椭圆的相切条件
        double distance = Math.Sqrt(Math.Pow(h2 - h1, 2) + Math.Pow(k2 - k1, 2));

        // 判断相切条件
        return Math.Abs(distance - (a1 + a2)) < 1e-7 || Math.Abs(distance - Math.Abs(a1 - a2)) < 1e-7 ||
               Math.Abs(distance - (b1 + b2)) < 1e-7 || Math.Abs(distance - Math.Abs(b1 - b2)) < 1e-7;
    }

    /// <summary>
    /// 是否相离
    /// </summary>
    /// <param name="that">另一个椭圆</param>
    /// <returns></returns>
    public bool IsSeparateWith(Ellipse that)
    {
        return !IsCrossWith(that) && !IsIntersectWith(that);
    }
}