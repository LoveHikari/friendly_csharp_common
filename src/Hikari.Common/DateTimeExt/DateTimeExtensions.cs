using System;

namespace Hikari.Common.DateTimeExt
{
    /// <summary>
    /// DateTime扩展类
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never), System.ComponentModel.Browsable(false)]
    [Obsolete("请使用DateTimeOffsetExtensions", true)]
    public static class DateTimeExtensions
    {
        /// <summary>
        /// 获取此实例所表示的日期是该年中的第几周
        /// </summary>
        /// <param name="this">给定的日期(年月日)</param>
        /// <returns>该年中的第几周</returns>
        public static int WeekOfYear(this in DateTime @this)
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

        /// <summary>
        /// 获取此实例所表示的日期是该月中的第几周
        /// </summary>
        /// <param name="this">给定的日期(年月日)</param>
        /// <returns>该月中的第几周</returns>
        public static int WeekOfMonth(this in DateTime @this)
        {
            var firstOfMonth = Convert.ToDateTime(@this.Date.Year + "-" + @this.Date.Month + "-" + 1);  // 该月第一天
            int i = (int)firstOfMonth.Date.DayOfWeek;
            if (i == 0)
            {
                i = 7;
            }
            return (@this.Date.Day + i - 1) / 7 + 1;
        }
        /// <summary>
        /// 获取此实例所表示的日期是该季度中的第几周
        /// </summary>
        /// <param name="this">给定的日期(年月日)</param>
        /// <returns>该季度中的第几周</returns>
        public static int WeekOfQuarter(this in DateTime @this)
        {
            // 该季度的第一天
            var firstDay = DateTimeHelper.GetDateQuarter(@this).Start;

            int week = @this.WeekOfYear();
            week = week - firstDay.WeekOfYear() + 1;

            return week;

        }
        /// <summary>
        /// 获取此实例所表示的日期是该年中的第几季度
        /// </summary>
        /// <param name="this">给定的日期(年月日)</param>
        /// <returns>该年中的第几季</returns>
        public static int QuarterOfYear(this in DateTime @this)
        {
            return Math.Pow(2, @this.Month).ToInt32()?.Count() ?? 1;
        }
        /// <summary>
        /// 获取此实例所表示的日期是该季度中的第几天
        /// </summary>
        /// <param name="this">给定的日期(年月日)</param>
        /// <returns>该季中的第几天</returns>
        public static int DayOfQuarter(this in DateTime @this)
        {
            var firstDay = DateTimeHelper.GetDateQuarter(@this).Start;  // 该季度的第一天
            return (@this.Date - firstDay).Days + 1;

        }
        /// <summary>
        /// 获取此实例所表示的日期是该季度中的第几月
        /// </summary>
        /// <param name="this">给定的日期(年月日)</param>
        /// <returns>该季度中的第几月</returns>
        public static int MonthOfQuarter(this in DateTime @this)
        {
            var firstDay = DateTimeHelper.GetDateQuarter(@this).Start;  // 该季度的第一天
            return @this.Month - firstDay.Month + 1;
        }
        /// <summary>
        /// 获取此实例所表示的日期所在季度的周数
        /// </summary>
        /// <param name="this">给定的日期(年月日)</param>
        /// <returns>所在季度的周数</returns>
        public static int WeeksOfQuarter(this in DateTime @this)
        {
            var lastDay = DateTimeHelper.GetDateQuarter(@this).End;  // 该季度的最后一天
            return lastDay.WeekOfQuarter();
        }
        /// <summary>
        /// 获取此实例所表示的日期所在月的周数
        /// </summary>
        /// <param name="this">给定的日期(年月日)</param>
        /// <returns>所在季度的周数</returns>
        public static int WeeksOfMonth(this in DateTime @this)
        {
            var lastDay = DateTimeHelper.GetDateMonth(@this).End;
            return lastDay.WeekOfMonth();
        }
        /// <summary>
        /// 获取此实例所表示的日期所在年的周数
        /// </summary>
        /// <param name="this">给定的日期(年月日)</param>
        /// <returns>所在季度的周数</returns>
        public static int WeeksOfYear(this in DateTime @this)
        {
            var end = new DateTime(@this.Year, 12, 31);
            var gc = new System.Globalization.DateTimeFormatInfo().Calendar;
            return gc.GetWeekOfYear(end, System.Globalization.CalendarWeekRule.FirstDay, DayOfWeek.Sunday);
        }
        /// <summary>
        /// 返回本年有多少天
        /// </summary>
        /// <param name="this">时间</param>
        /// <returns>本年的天数</returns>
        public static int DaysOfYear(this in DateTime @this)
        {
            return DateTime.IsLeapYear(@this.Year) ? 366 : 365;
        }

        /// <summary>
        /// 本月有多少天
        /// </summary>
        /// <param name="this">时间</param>
        /// <returns>天数</returns>
        public static int DaysOfMonth(this in DateTime @this)
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

        /// <summary>
        /// 返回周中指定日期的区域性特定的全名
        /// </summary>
        /// <param name="this">日期</param>
        /// <param name="cultureName">区域性名称, 如zh-CN</param>
        /// <returns>星期名称</returns>
        public static string DayNameOfWeek(this in DateTime @this, string cultureName = "")
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
        /// <param name="this"></param>
        /// <param name="start">开始</param>
        /// <param name="end">结束</param>
        /// <param name="mode">模式</param>
        /// <returns></returns>
        public static bool In(this in DateTime @this, DateTime? start, DateTime? end, RangeMode mode = RangeMode.Close)
        {
            start ??= DateTime.MinValue;
            end ??= DateTime.MaxValue;
            return mode switch
            {
                RangeMode.Open => start < @this && end > @this,
                RangeMode.Close => start <= @this && end >= @this,
                RangeMode.OpenClose => start < @this && end >= @this,
                RangeMode.CloseOpen => start <= @this && end > @this,
                _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null)
            };
        }




        private static readonly DateTime UnixEpochDateTimeUtc = new DateTime(621355968000000000L, DateTimeKind.Utc);  // Unix纪元开始的Utc时间
        private static readonly DateTime MinDateTimeUtc = new DateTime(1, 1, 1, 0, 0, 0, DateTimeKind.Utc);  // 最小utc时间
        /// <summary>
        /// Unix时间戳格式到秒
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static long ToUnixTimeSeconds(this in DateTime @this)
        {
            return (long)@this.ToDateTimeSinceUnixEpoch().TotalSeconds;
            // ToDateTimeSinceUnixEpoch(@this).Ticks / 10000000L;
        }
        /// <summary>
        /// Unix时间戳格式到毫秒
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static long ToUnixTimeMilliseconds(this in DateTime @this)
        {
            return (long)@this.ToDateTimeSinceUnixEpoch().TotalMilliseconds;
            // ToDateTimeSinceUnixEpoch(@this).Ticks / 10000000L;
        }
        /// <summary>
        /// Unix时间戳格式到微秒
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static long ToUnixTimeMicroseconds(this in DateTime @this)
        {
            return (long)@this.ToDateTimeSinceUnixEpoch().TotalMicroseconds;
        }
        /// <summary>
        /// Unix时间戳格式到纳秒
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static long ToUnixTimeNanoseconds(this in DateTime @this)
        {
            return (long)@this.ToDateTimeSinceUnixEpoch().TotalNanoseconds;
        }

        /// <summary>
        /// 自 Unix 纪元至今的时间
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        private static TimeSpan ToDateTimeSinceUnixEpoch(this DateTime dateTime)
        {
            DateTime dateTime1 = dateTime;
            if (dateTime.Kind != DateTimeKind.Utc)
                dateTime1 = dateTime.Kind != DateTimeKind.Unspecified || !(dateTime > DateTime.MinValue) || !(dateTime < DateTime.MaxValue) ? dateTime.ToStableUniversalTime() : DateTime.SpecifyKind(dateTime.Subtract(TimeZoneInfo.Local.GetUtcOffset(dateTime)), DateTimeKind.Utc);
            return dateTime1.Subtract(UnixEpochDateTimeUtc);
        }
        /// <summary>
        /// 转到世界标准时间
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime ToStableUniversalTime(this DateTime dateTime)
        {
            if (dateTime.Kind == DateTimeKind.Utc) return dateTime;

            if (dateTime == DateTime.MinValue)
            {
                return MinDateTimeUtc;
            }
            else
            {
                return TimeZoneInfo.ConvertTimeToUtc(dateTime, TimeZoneInfo.Local);
            }
        }
        /// <summary>
        /// 转到ISO8601标准时间
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static string ToISO8601DateTime(this in DateTime @this)
        {
            return @this.ToString("yyyy-MM-ddTHH:mm:ss.fffzzz");
        }
        
    }
}