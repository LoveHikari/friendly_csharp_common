namespace Hikari.Common.DateTimeExt;
/// <summary>
/// 时间段
/// </summary>
public class DateOnlyRange
{
    /// <summary>
    /// 起始日期
    /// </summary>
    public DateOnly Start { get; set; }

    /// <summary>
    /// 结束日期
    /// </summary>
    public DateOnly End { get; set; }
    /// <summary>
    /// 模式
    /// </summary>
    public RangeMode Mode { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public DateOnlyRange()
    {
        Start = DateOnly.MinValue;
        End = DateOnly.MaxValue;
        Mode = RangeMode.Close;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="start">起始日期</param>
    /// <param name="end">结束日期</param>
    /// <param name="mode">模式</param>
    /// <exception cref="Exception"></exception>
    public DateOnlyRange(DateOnly start, DateOnly end, RangeMode mode = RangeMode.Close)
    {
        if (start > end)
        {
            throw new Exception("开始日期不能大于结束日期");
        }

        Start = start;
        End = end;
        Mode = mode;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="range">[a,b]</param>
    public DateOnlyRange(string range)
    {
        // 取第一个字符
        var startMode = range[0];
        // 取最后一个字符
        var endMode = range[^1];
        // 取剩下的字符
        var timeRange = range.Substring(1, range.Length - 2);
        RangeMode mode = startMode switch
        {
            '(' when endMode == ')' => RangeMode.Open,
            '[' when endMode == ')' => RangeMode.CloseOpen,
            '(' when endMode == ']' => RangeMode.OpenClose,
            _ => RangeMode.Close
        };
        // 根据,分割时间
        var times = timeRange.Split(',');
        Start = DateOnly.Parse(times[0].Trim(' '));
        End = DateOnly.Parse(times[1].Trim(' '));
    }
    /// <summary>
    /// 是否相交
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <returns></returns>
    public bool HasIntersect(DateOnly start, DateOnly end)
    {
        return HasIntersect(new DateOnlyRange(start, end));
    }

    /// <summary>
    /// 是否相交
    /// </summary>
    /// <param name="range"></param>
    /// <returns></returns>
    public bool HasIntersect(DateOnlyRange range)
    {
        return Start.In(range.Start, range.End) || End.In(range.Start, range.End);
    }

    /// <summary>
    /// 相交时间段
    /// </summary>
    /// <param name="range"></param>
    /// <returns></returns>
    public DateOnlyRange? Intersect(DateOnlyRange range)
    {
        if (HasIntersect(range.Start, range.End))
        {
            var list = new List<DateOnly>() { Start, range.Start, End, range.End };
            list.Sort();
            return new DateOnlyRange(list[1], list[2]);
        }

        return null;
    }

    /// <summary>
    /// 相交时间段
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <returns></returns>
    public DateOnlyRange? Intersect(DateOnly start, DateOnly end)
    {
        return Intersect(new DateOnlyRange(start, end));
    }
    /// <summary>
    /// 是否包含时间段
    /// </summary>
    /// <param name="range"></param>
    /// <returns></returns>
    public bool Contains(DateOnlyRange range)
    {
        return range.Start.In(Start, End) && range.End.In(Start, End);
    }

    /// <summary>
    /// 是否包含时间段
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <returns></returns>
    public bool Contains(DateOnly start, DateOnly end)
    {
        return Contains(new DateOnlyRange(start, end));
    }

    /// <summary>
    /// 是否在时间段内
    /// </summary>
    /// <param name="range"></param>
    /// <returns></returns>
    public bool In(DateOnlyRange range)
    {
        return Start.In(range.Start, range.End) && End.In(range.Start, range.End);
    }

    /// <summary>
    /// 是否在时间段内
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <returns></returns>
    public bool In(DateOnly start, DateOnly end)
    {
        return In(new DateOnlyRange(start, end));
    }

    /// <summary>
    /// 合并时间段
    /// </summary>
    /// <param name="range"></param>
    /// <returns></returns>
    public DateOnlyRange Union(DateOnlyRange range)
    {
        if (HasIntersect(range))
        {
            int startMode; // 模式，0：开，1：闭
            int endMode;
            DateOnly start;
            DateOnly end;
            if (Start > range.Start)
            {
                start = range.Start;
                startMode = range.Mode switch
                {
                    RangeMode.Close or RangeMode.CloseOpen => 1,
                    RangeMode.Open or RangeMode.OpenClose => 0,
                    _ => 1
                };
            }
            else if (Start < range.Start)
            {
                start = Start;
                startMode = Mode switch
                {
                    RangeMode.Close or RangeMode.CloseOpen => 1,
                    RangeMode.Open or RangeMode.OpenClose => 0,
                    _ => 1
                };
            }
            else
            {
                start = Start;
                var startMode1 = Mode switch
                {
                    RangeMode.Close or RangeMode.CloseOpen => 1,
                    RangeMode.Open or RangeMode.OpenClose => 0,
                    _ => 1
                };
                var startMode2 = range.Mode switch
                {
                    RangeMode.Close or RangeMode.CloseOpen => 1,
                    RangeMode.Open or RangeMode.OpenClose => 0,
                    _ => 1
                };
                startMode = startMode1 >= startMode2 ? startMode1 : startMode2;
            }
            // 取结束时间
            if (End > range.End)
            {
                end = range.End;
                endMode = range.Mode switch
                {
                    RangeMode.Close or RangeMode.OpenClose => 1,
                    RangeMode.Open or RangeMode.CloseOpen => 0,
                    _ => 1
                };
            }
            else if (End < range.End)
            {
                end = End;
                endMode = Mode switch
                {
                    RangeMode.Close or RangeMode.OpenClose => 1,
                    RangeMode.Open or RangeMode.CloseOpen => 0,
                    _ => 1
                };
            }
            else
            {
                end = End;
                var endMode1 = Mode switch
                {
                    RangeMode.Close or RangeMode.OpenClose => 1,
                    RangeMode.Open or RangeMode.CloseOpen => 0,
                    _ => 1
                };
                var endMode2 = range.Mode switch
                {
                    RangeMode.Close or RangeMode.OpenClose => 1,
                    RangeMode.Open or RangeMode.CloseOpen => 0,
                    _ => 1
                };
                endMode = endMode1 >= endMode2 ? endMode1 : endMode2;
            }

            RangeMode mode = startMode switch
            {
                0 when endMode == 0 => RangeMode.Open,
                1 when endMode == 0 => RangeMode.CloseOpen,
                0 when endMode == 1 => RangeMode.OpenClose,
                _ => RangeMode.Close
            };
            return new DateOnlyRange(start, end, mode);
        }

        throw new Exception("不相交的时间段不能合并");
    }

    /// <summary>
    /// 合并时间段
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <returns></returns>
    public DateOnlyRange Union(DateOnly start, DateOnly end)
    {
        return Union(new DateOnlyRange(start, end));
    }

    /// <summary>返回一个表示当前对象的 string。</summary>
    /// <returns>表示当前对象的字符串。</returns>
    public override string ToString()
    {
        string str = Mode switch
        {
            RangeMode.Open => $"({Start:yyyy-MM-dd}, {End:yyyy-MM-dd})",
            RangeMode.CloseOpen => $"[{Start:yyyy-MM-dd}, {End:yyyy-MM-dd})",
            RangeMode.OpenClose => $"({Start:yyyy-MM-dd}, {End:yyyy-MM-dd}]",
            _ => $"[{Start:yyyy-MM-dd}, {End:yyyy-MM-dd}]"
        };
        return str;
    }

    public override bool Equals(object obj)
    {
        if (obj is DateOnlyRange range)
        {
            return Start == range.Start && End == range.End && Mode == range.Mode;
        }

        return false;
    }

}