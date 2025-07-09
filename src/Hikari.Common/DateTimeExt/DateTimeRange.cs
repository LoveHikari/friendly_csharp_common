namespace Hikari.Common.DateTimeExt;
/// <summary>
/// 时间段
/// </summary>
public class DateTimeRange
{
    /// <summary>
    /// 起始时间
    /// </summary>
    public DateTime Start { get; set; }

    /// <summary>
    /// 结束时间
    /// </summary>
    public DateTime End { get; set; }
    /// <summary>
    /// 模式
    /// </summary>
    public RangeMode Mode { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public DateTimeRange()
    {
        Start = DateTime.MinValue;
        End = DateTime.MaxValue;
        Mode = RangeMode.Close;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="start">起始时间</param>
    /// <param name="end">结束时间</param>
    /// <param name="mode">模式</param>
    /// <exception cref="Exception"></exception>
    public DateTimeRange(DateTime start, DateTime end, RangeMode mode = RangeMode.Close)
    {
        if (start > end)
        {
            throw new Exception("开始时间不能大于结束时间");
        }

        Start = start;
        End = end;
        Mode = mode;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="range">[a,b]</param>
    public DateTimeRange(string range)
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
        Start = DateTime.Parse(times[0].Trim(' ').Trim('\'').Trim('\"'));
        End = DateTime.Parse(times[1].Trim(' ').Trim('\'').Trim('\"'));
    }
    /// <summary>
    /// 是否相交
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <returns></returns>
    public bool HasIntersect(DateTime start, DateTime end)
    {
        return HasIntersect(new DateTimeRange(start, end));
    }

    /// <summary>
    /// 是否相交
    /// </summary>
    /// <param name="range"></param>
    /// <returns></returns>
    public bool HasIntersect(DateTimeRange range)
    {
        return Start.In(range.Start, range.End) || End.In(range.Start, range.End);
    }

    /// <summary>
    /// 相交时间段
    /// </summary>
    /// <param name="range"></param>
    /// <returns></returns>
    public DateTimeRange? Intersect(DateTimeRange range)
    {
        if (HasIntersect(range.Start, range.End))
        {
            var list = new List<DateTime>() { Start, range.Start, End, range.End };
            list.Sort();
            return new DateTimeRange(list[1], list[2]);
        }

        return null;
    }

    /// <summary>
    /// 相交时间段
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <returns></returns>
    public DateTimeRange? Intersect(DateTime start, DateTime end)
    {
        return Intersect(new DateTimeRange(start, end));
    }
    /// <summary>
    /// 是否包含时间段
    /// </summary>
    /// <param name="range"></param>
    /// <returns></returns>
    public bool Contains(DateTimeRange range)
    {
        return range.Start.In(Start, End) && range.End.In(Start, End);
    }

    /// <summary>
    /// 是否包含时间段
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <returns></returns>
    public bool Contains(DateTime start, DateTime end)
    {
        return Contains(new DateTimeRange(start, end));
    }

    /// <summary>
    /// 是否在时间段内
    /// </summary>
    /// <param name="range"></param>
    /// <returns></returns>
    public bool In(DateTimeRange range)
    {
        return Start.In(range.Start, range.End) && End.In(range.Start, range.End);
    }

    /// <summary>
    /// 是否在时间段内
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <returns></returns>
    public bool In(DateTime start, DateTime end)
    {
        return In(new DateTimeRange(start, end));
    }

    /// <summary>
    /// 合并时间段
    /// </summary>
    /// <param name="range"></param>
    /// <returns></returns>
    public DateTimeRange Union(DateTimeRange range)
    {
        if (HasIntersect(range))
        {
            int startMode; // 模式，0：开，1：闭
            int endMode;
            DateTime start;
            DateTime end;
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
            return new DateTimeRange(start, end, mode);
        }

        throw new Exception("不相交的时间段不能合并");
    }

    /// <summary>
    /// 合并时间段
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <returns></returns>
    public DateTimeRange Union(DateTime start, DateTime end)
    {
        return Union(new DateTimeRange(start, end));
    }

    /// <summary>返回一个表示当前对象的 string。</summary>
    /// <returns>表示当前对象的字符串。</returns>
    public override string ToString()
    {
        string str = Mode switch
        {
            RangeMode.Open => $"({Start:yyyy-MM-dd HH:mm:ss}, {End:yyyy-MM-dd HH:mm:ss})",
            RangeMode.CloseOpen => $"[{Start:yyyy-MM-dd HH:mm:ss}, {End:yyyy-MM-dd HH:mm:ss})",
            RangeMode.OpenClose => $"({Start:yyyy-MM-dd HH:mm:ss}, {End:yyyy-MM-dd HH:mm:ss}]",
            _ => $"[{Start:yyyy-MM-dd HH:mm:ss}, {End:yyyy-MM-dd HH:mm:ss}]"
        };
        return str;
    }

    public override bool Equals(object obj)
    {
        if (obj is DateTimeRange range)
        {
            return Start == range.Start && End == range.End && Mode == range.Mode;
        }

        return false;
    }
}