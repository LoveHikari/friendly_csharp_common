namespace Hikari.Common.DateTimeExt;
/// <summary>
/// 时间段
/// </summary>
public class DateTimeOffsetRange
{
    /// <summary>
    /// 起始时间
    /// </summary>
    public DateTimeOffset Start { get; set; }

    /// <summary>
    /// 结束时间
    /// </summary>
    public DateTimeOffset End { get; set; }
    /// <summary>
    /// 模式
    /// </summary>
    public RangeMode Mode { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public DateTimeOffsetRange()
    {
        Start = DateTimeOffset.MinValue;
        End = DateTimeOffset.MaxValue;
        Mode = RangeMode.Close;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="start">起始时间</param>
    /// <param name="end">结束时间</param>
    /// <param name="mode">模式</param>
    /// <exception cref="Exception"></exception>
    public DateTimeOffsetRange(DateTimeOffset start, DateTimeOffset end, RangeMode mode = RangeMode.Close)
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
    /// 支持字符串解析："[2025-01-01 00:00:00+08:00, 2025-01-02 00:00:00+08:00)"
    /// </summary>
    /// <param name="range">[a,b]</param>
    public DateTimeOffsetRange(string range)
    {
        if (string.IsNullOrWhiteSpace(range) || range.Length < 5)
            throw new FormatException("时间范围格式不正确");
        // 取第一个字符
        var startMode = range[0];
        // 取最后一个字符
        var endMode = range[^1];
        // 取剩下的字符
        var timeRange = range.Substring(1, range.Length - 2);
        Mode = (startMode, endMode) switch
        {
            ('(', ')') => RangeMode.Open,
            ('[', ')') => RangeMode.CloseOpen,
            ('(', ']') => RangeMode.OpenClose,
            ('[', ']') => RangeMode.Close,
            _ => throw new FormatException("不支持的时间范围格式")
        };
        // 根据,分割时间
        var parts = timeRange.Split(',', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length != 2)
            throw new FormatException("时间范围必须包含两个时间点");
        Start = DateTimeOffset.Parse(parts[0].Trim(' ').Trim('\'').Trim('\"'));
        End = DateTimeOffset.Parse(parts[1].Trim(' ').Trim('\'').Trim('\"'));
    }
    /// <summary>
    /// 是否相交
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <returns></returns>
    public bool HasIntersect(DateTimeOffset start, DateTimeOffset end)
    {
        return HasIntersect(new DateTimeOffsetRange(start, end));
    }

    /// <summary>
    /// 是否相交
    /// </summary>
    /// <param name="range"></param>
    /// <returns></returns>
    public bool HasIntersect(DateTimeOffsetRange range)
    {
        return Start.In(range.Start, range.End) || End.In(range.Start, range.End);
    }

    /// <summary>
    /// 相交时间段
    /// </summary>
    /// <param name="range"></param>
    /// <returns></returns>
    public DateTimeOffsetRange? Intersect(DateTimeOffsetRange range)
    {
        if (HasIntersect(range.Start, range.End))
        {
            var list = new List<DateTimeOffset>() { Start, range.Start, End, range.End };
            list.Sort();
            return new DateTimeOffsetRange(list[1], list[2]);
        }

        return null;
    }

    /// <summary>
    /// 相交时间段
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <returns></returns>
    public DateTimeOffsetRange? Intersect(DateTimeOffset start, DateTimeOffset end)
    {
        return Intersect(new DateTimeOffsetRange(start, end));
    }
    /// <summary>
    /// 是否包含时间段
    /// </summary>
    /// <param name="range"></param>
    /// <returns></returns>
    public bool Contains(DateTimeOffsetRange range)
    {
        return range.Start.In(Start, End) && range.End.In(Start, End);
    }

    /// <summary>
    /// 是否包含时间段
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <returns></returns>
    public bool Contains(DateTimeOffset start, DateTimeOffset end)
    {
        return Contains(new DateTimeOffsetRange(start, end));
    }

    /// <summary>
    /// 是否在时间段内
    /// </summary>
    /// <param name="range"></param>
    /// <returns></returns>
    public bool In(DateTimeOffsetRange range)
    {
        return Start.In(range.Start, range.End) && End.In(range.Start, range.End);
    }

    /// <summary>
    /// 是否在时间段内
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <returns></returns>
    public bool In(DateTimeOffset start, DateTimeOffset end)
    {
        return In(new DateTimeOffsetRange(start, end));
    }

    /// <summary>
    /// 合并时间段
    /// </summary>
    /// <param name="range"></param>
    /// <returns></returns>
    public DateTimeOffsetRange Union(DateTimeOffsetRange range)
    {
        if (HasIntersect(range))
        {
            int startMode; // 模式，0：开，1：闭
            int endMode;
            DateTimeOffset start;
            DateTimeOffset end;
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
            return new DateTimeOffsetRange(start, end, mode);
        }

        throw new Exception("不相交的时间段不能合并");
    }

    /// <summary>
    /// 合并时间段
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <returns></returns>
    public DateTimeOffsetRange Union(DateTimeOffset start, DateTimeOffset end)
    {
        return Union(new DateTimeOffsetRange(start, end));
    }

    /// <summary>返回一个表示当前对象的 string。</summary>
    /// <returns>表示当前对象的字符串。</returns>
    public override string ToString()
    {
        var startBracket = Mode == RangeMode.Open || Mode == RangeMode.OpenClose ? "(" : "[";
        var endBracket = Mode == RangeMode.Open || Mode == RangeMode.CloseOpen ? ")" : "]";

        return $"{startBracket}{Start:yyyy-MM-dd HH:mm:ss.fffffffzzz}, {End:yyyy-MM-dd HH:mm:ss.fffffffzzz}{endBracket}";
    }
    /// <summary>
    /// 判断是否相等
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals(object? obj)
    {
        if (obj is DateTimeOffsetRange range)
        {
            return Start == range.Start && End == range.End && Mode == range.Mode;
        }

        return false;
    }
    /// <summary>
    /// 获取哈希码
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}