namespace Hikari.Common.DateTimeExt;

/// <summary>
/// <see cref="DateTimeOffset"/>扩展类
/// </summary>
[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never), System.ComponentModel.Browsable(false)]
public static class DateTimeOffsetExtensions
{
    /// <summary>
    /// <see cref="DateTimeOffset"/>扩展类
    /// </summary>
    extension(in DateTimeOffset @this)
    {
        /// <summary>
        /// 获取此实例所表示的日期是该年中的第几周
        /// </summary>
        /// <returns>该年中的第几周</returns>
        public int WeekOfYear
        {
            get
            {
                ////一.找到第一周的最后一天（先获取1月1日是星期几，从而得知第一周周末是几）
                //int firstWeekend = 7 - Convert.ToInt32(DateTime.Parse(@this.Year + "-1-1").DayOfWeek);

                ////二.获取今天是一年当中的第几天
                //int currentDay = @this.DayOfYear;

                ////三.（今天 减去 第一周周末）/7 等于 距第一周有多少周 再加上第一周的1 就是今天是今年的第几周了
                ////    刚好考虑了惟一的特殊情况就是，今天刚好在第一周内，那么距第一周就是0 再加上第一周的1 最后还是1
                //return Convert.ToInt32(Math.Ceiling((currentDay - firstWeekend) / 7.0)) + 1;
                var gc = new System.Globalization.DateTimeFormatInfo().Calendar;
                return gc.GetWeekOfYear(@this.Date, System.Globalization.CalendarWeekRule.FirstDay, DayOfWeek.Sunday);
            }
            

        }

        /// <summary>
        /// 获取此实例所表示的日期是该月中的第几周
        /// </summary>
        /// <returns>该月中的第几周</returns>
        public int WeekOfMonth
        {
            get
            {
                var firstOfMonth = Convert.ToDateTime(@this.Date.Year + "-" + @this.Date.Month + "-" + 1);  // 该月第一天
                int i = (int)firstOfMonth.Date.DayOfWeek;
                if (i == 0)
                {
                    i = 7;
                }
                return (@this.Date.Day + i - 1) / 7 + 1;
            }

        }
        /// <summary>
        /// 获取此实例所表示的日期是该季度中的第几周
        /// </summary>
        /// <returns>该季度中的第几周</returns>
        public int WeekOfQuarter
        {
            get
            {
                // 该季度的第一天
                var firstDay = DateTimeOffsetHelper.GetDateQuarter(@this).Start;

                int week = @this.WeekOfYear;
                week = week - firstDay.WeekOfYear + 1;

                return week;
            }

        }
        /// <summary>
        /// 获取此实例所表示的日期是该年中的第几季度
        /// </summary>
        /// <returns>该年中的第几季</returns>
        public int QuarterOfYear => Math.Pow(2, @this.Month).ToInt32()?.Count() ?? 1;

        /// <summary>
        /// 获取此实例所表示的日期是该季度中的第几天
        /// </summary>
        /// <returns>该季中的第几天</returns>
        public int DayOfQuarter
        {
            get
            {
                var firstDay = DateTimeOffsetHelper.GetDateQuarter(@this).Start;  // 该季度的第一天
                return (@this.Date - firstDay).Days + 1;
            }

        }
        /// <summary>
        /// 获取此实例所表示的日期是该季度中的第几月
        /// </summary>
        /// <returns>该季度中的第几月</returns>
        public int MonthOfQuarter
        {
            get
            {
                var firstDay = DateTimeOffsetHelper.GetDateQuarter(@this).Start;  // 该季度的第一天
                return @this.Month - firstDay.Month + 1;
            }
        }
        /// <summary>
        /// 获取此实例所表示的日期所在季度的周数
        /// </summary>
        /// <returns>所在季度的周数</returns>
        public int WeeksOfQuarter
        {
            get
            {
                var lastDay = DateTimeOffsetHelper.GetDateQuarter(@this).End;  // 该季度的最后一天
                return lastDay.WeekOfQuarter;
            }
        }
        /// <summary>
        /// 获取此实例所表示的日期所在月的周数
        /// </summary>
        /// <returns>所在季度的周数</returns>
        public int WeeksOfMonth
        {
            get
            {
                var lastDay = DateTimeOffsetHelper.GetDateMonth(@this).End;
                return lastDay.WeekOfMonth;
            }
        }
        /// <summary>
        /// 获取此实例所表示的日期所在年的周数
        /// </summary>
        /// <returns>所在季度的周数</returns>
        public int WeeksOfYear
        {
            get
            {
                var end = new DateTime(@this.Year, 12, 31);
                var gc = new System.Globalization.DateTimeFormatInfo().Calendar;
                return gc.GetWeekOfYear(end, System.Globalization.CalendarWeekRule.FirstDay, DayOfWeek.Sunday);
            }
        }
        /// <summary>
        /// 返回本年有多少天
        /// </summary>
        /// <returns>本年的天数</returns>
        public int DaysOfYear => DateTime.IsLeapYear(@this.Year) ? 366 : 365;

        /// <summary>
        /// 本月有多少天
        /// </summary>
        /// <returns>天数</returns>
        public int DaysOfMonth
        {
            get
            {
                return @this.Month switch
                {
                    1 => 31,
                    2 => DateTime.IsLeapYear(@this.Year) ? 29 : 28,
                    3 => 31,
                    4 => 30,
                    5 => 31,
                    6 => 30,
                    7 => 31,
                    8 => 31,
                    9 => 30,
                    10 => 31,
                    11 => 30,
                    12 => 31,
                    _ => 0
                };
            }
        }

        /// <summary>
        /// 返回周中指定日期的区域性特定的全名
        /// </summary>
        /// <param name="cultureName">区域性名称, 如zh-CN</param>
        /// <returns>星期名称</returns>
        public string DayNameOfWeek(string cultureName = "")
        {
            if (string.IsNullOrWhiteSpace(cultureName))
            {
                cultureName = System.Globalization.CultureInfo.CurrentCulture.Name;
            }
            return System.Globalization.CultureInfo.GetCultureInfo(cultureName).DateTimeFormat.GetDayName(@this.DayOfWeek);
        }
        /// <summary>
        /// 判断时间是否在区间内
        /// </summary>
        /// <param name="start">开始</param>
        /// <param name="end">结束</param>
        /// <param name="mode">模式</param>
        /// <returns></returns>
        public bool In(DateTimeOffset? start, DateTimeOffset? end, RangeMode mode = RangeMode.Close)
        {
            start ??= DateTimeOffset.MinValue;
            end ??= DateTimeOffset.MaxValue;
            return mode switch
            {
                RangeMode.Open => start < @this && end > @this,
                RangeMode.Close => start <= @this && end >= @this,
                RangeMode.OpenClose => start < @this && end >= @this,
                RangeMode.CloseOpen => start <= @this && end > @this,
                _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null)
            };
        }
    }

}