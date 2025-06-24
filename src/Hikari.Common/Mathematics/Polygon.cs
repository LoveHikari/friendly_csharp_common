namespace Hikari.Common.Mathematics;
/// <summary>
/// 多边形
/// </summary>
public class Polygon
{
    /// <summary>
    /// 多边形的顶点列表
    /// </summary>
    public List<Point2D> Vertices { get; set; }
#if NET10_0
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="vertices">多边形的顶点列表</param>
    public Polygon(params List<Point2D> vertices)
    {
        if (vertices == null || vertices.Count < 3)
        {
            throw new ArgumentException("多边形必须至少有三个顶点");
        }
        // 自动按顺时针/逆时针排序顶点
        vertices = ComputeConvexHull(vertices).ToList();
        Vertices = vertices;
    }
#else
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="vertices">多边形的顶点列表</param>
    public Polygon(List<Point2D> vertices)
    {
        if (vertices == null || vertices.Count < 3)
        {
            throw new ArgumentException("多边形必须至少有三个顶点");
        }
        // 自动按顺时针/逆时针排序顶点
        vertices = ComputeConvexHull(vertices).ToList();
        Vertices = vertices;
    }
#endif

    /// <summary>
    /// 按质心角度排序顶点
    /// </summary>
    /// <param name="points"></param>
    /// <returns></returns>
    private IEnumerable<Point2D> SortVertices(List<Point2D> points)
    {
        // 计算质心
        double centerX = points.Average(p => p.X);
        double centerY = points.Average(p => p.Y);
        Point2D center = new Point2D(centerX, centerY);

        // 按相对于质心的极角排序
        return points.OrderBy(p => Math.Atan2(p.Y - center.Y, p.X - centerX));
    }
    /// <summary>
    /// 使用Andrew's monotone chain算法计算凸包
    /// </summary>
    /// <param name="points"></param>
    /// <returns></returns>
    private List<Point2D> ComputeConvexHull(List<Point2D> points)
    {
        if (points.Count <= 1)
            return points;

        points = points.OrderBy(p => p.X).ThenBy(p => p.Y).ToList();

        List<Point2D> hull = new List<Point2D>();

        // 计算下凸包
        foreach (var pt in points)
        {
            while (hull.Count >= 2 && Cross(hull[hull.Count - 2], hull[hull.Count - 1], pt) <= 0)
                hull.RemoveAt(hull.Count - 1);
            hull.Add(pt);
        }

        // 计算上凸包
        int t = hull.Count + 1;
        for (int i = points.Count - 1; i >= 0; i--)
        {
            Point2D pt = points[i];
            while (hull.Count >= t && Cross(hull[hull.Count - 2], hull[hull.Count - 1], pt) <= 0)
                hull.RemoveAt(hull.Count - 1);
            hull.Add(pt);
        }

        hull.RemoveAt(hull.Count - 1);
        return hull;
    }

    private double Cross(Point2D O, Point2D A, Point2D B)
    {
        return (A.X - O.X) * (B.Y - O.Y) - (A.Y - O.Y) * (B.X - O.X);
    }
    /// <summary>
    /// 计算多边形的周长
    /// </summary>
    /// <returns>周长</returns>
    public double CalculatePerimeter()
    {
        double perimeter = 0.0;

        for (int i = 0; i < Vertices.Count; i++)
        {
            var current = Vertices[i];
            var next = Vertices[(i + 1) % Vertices.Count]; // 连接最后一个点和第一个点
            perimeter += Distance(current, next);
        }

        return perimeter;
    }

    /// <summary>
    /// 计算多边形的面积
    /// </summary>
    /// <returns>面积</returns>
    public double CalculateArea()
    {
        double area = 0.0;

        for (int i = 0; i < Vertices.Count; i++)
        {
            var current = Vertices[i];
            var next = Vertices[(i + 1) % Vertices.Count];
            area += (current.X * next.Y) - (next.X * current.Y);
        }

        return Math.Abs(area) / 2.0;
    }

    /// <summary>
    /// 判断一个点是否在多边形内部
    /// </summary>
    /// <param name="point">要判断的点</param>
    /// <returns>如果点在多边形内部返回 true，否则返回 false</returns>
    public bool Contains(Point2D point)
    {
        bool result = false;
        int j = Vertices.Count - 1;

        for (int i = 0; i < Vertices.Count; i++)
        {
            if (Vertices[i].Y < point.Y && Vertices[j].Y >= point.Y || Vertices[j].Y < point.Y && Vertices[i].Y >= point.Y)
            {
                if (Vertices[i].X + (point.Y - Vertices[i].Y) / (double)(Vertices[j].Y - Vertices[i].Y) * (double)(Vertices[j].X - Vertices[i].X) < point.X)
                {
                    result = !result;
                }
            }
            j = i;
        }

        return result;
    }
    /// <summary>
    /// 获取两个多边形的相交区域
    /// </summary>
    /// <param name="other">多边形1</param>
    /// <returns></returns>
    public List<Point2D> ComputeIntersection(Polygon other)
    {
        List<Point2D> intersectionPoints = new List<Point2D>();

        // 检查当前多边形的每条边与另一个多边形的每条边是否相交
        for (int i = 0; i < Vertices.Count; i++)
        {
            Point2D p1 = Vertices[i];
            Point2D p2 = Vertices[(i + 1) % Vertices.Count];

            for (int j = 0; j < other.Vertices.Count; j++)
            {
                Point2D p3 = other.Vertices[j];
                Point2D p4 = other.Vertices[(j + 1) % other.Vertices.Count];

                if (IsIntersect(p1, p2, p3, p4))
                {
                    intersectionPoints.Add(p1); // 这里可以添加交点的计算
                    intersectionPoints.Add(p2); // 这里可以添加交点的计算
                }
            }
        }

        // 这里可以进一步处理交点，确保返回的交点是唯一的
        return intersectionPoints;
    }
    /// <summary>
    /// 计算两点之间的距离
    /// </summary>
    /// <param name="p1">点1</param>
    /// <param name="p2">点2</param>
    /// <returns>两点之间的距离</returns>
    private double Distance(Point2D p1, Point2D p2)
    {
        return Math.Sqrt(Math.Pow(p2.X - p1.X, 2) + Math.Pow(p2.Y - p1.Y, 2));
    }
    /// <summary>
    /// 计算两条线段是否相交
    /// </summary>
    /// <param name="p1"></param>
    /// <param name="p2"></param>
    /// <param name="p3"></param>
    /// <param name="p4"></param>
    /// <returns></returns>
    private bool IsIntersect(Point2D p1, Point2D p2, Point2D p3, Point2D p4)
    {
        double d1 = Direction(p3, p4, p1);
        double d2 = Direction(p3, p4, p2);
        double d3 = Direction(p1, p2, p3);
        double d4 = Direction(p1, p2, p4);

        if (d1 != d2 && d3 != d4)
            return true;

        if (d1 == 0 && OnSegment(p3, p4, p1)) return true;
        if (d2 == 0 && OnSegment(p3, p4, p2)) return true;
        if (d3 == 0 && OnSegment(p1, p2, p3)) return true;
        if (d4 == 0 && OnSegment(p1, p2, p4)) return true;

        return false;
    }

    // 计算方向
    private double Direction(Point2D p1, Point2D p2, Point2D p3)
    {
        return (p3.Y - p1.Y) * (p2.X - p1.X) - (p2.Y - p1.Y) * (p3.X - p1.X);
    }

    // 判断点是否在段上
    private bool OnSegment(Point2D p1, Point2D p2, Point2D p)
    {
        return p.X <= Math.Max(p1.X, p2.X) && p.X >= Math.Min(p1.X, p2.X) &&
               p.Y <= Math.Max(p1.Y, p2.Y) && p.Y >= Math.Min(p1.Y, p2.Y);
    }
}