﻿namespace System
{
    /// <summary>
    /// DateTime扩展类
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never), System.ComponentModel.Browsable(false)]
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
            var firstDay =  DateTimeHelper.GetDateQuarter(@this).firstDay;

            int week =  @this.WeekOfYear();
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
            return @this.Month / 4 + 1;
        }
        /// <summary>
        /// 获取此实例所表示的日期是该季度中的第几天
        /// </summary>
        /// <param name="this">给定的日期(年月日)</param>
        /// <returns>该季中的第几天</returns>
        public static int DayOfQuarter(this in DateTime @this)
        {
            var firstDay = DateTimeHelper.GetDateQuarter(@this).firstDay;  // 该季度的第一天
            return (@this.Date - firstDay).Days + 1;

        }
        /// <summary>
        /// 获取此实例所表示的日期是该季度中的第几月
        /// </summary>
        /// <param name="this">给定的日期(年月日)</param>
        /// <returns>该季度中的第几月</returns>
        public static int MonthOfQuarter(this in DateTime @this)
        {
            var firstDay = DateTimeHelper.GetDateQuarter(@this).firstDay;  // 该季度的第一天
            return @this.Month - firstDay.Month + 1;
        }
        /// <summary>
        /// 获取此实例所表示的日期所在季度的周数
        /// </summary>
        /// <param name="this">给定的日期(年月日)</param>
        /// <returns>所在季度的周数</returns>
        public static int WeeksOfQuarter(this in DateTime @this)
        {
            var lastDay = DateTimeHelper.GetDateQuarter(@this).lastDay;  // 该季度的最后一天
            return lastDay.WeekOfQuarter();
        }
        /// <summary>
        /// 获取此实例所表示的日期所在月的周数
        /// </summary>
        /// <param name="this">给定的日期(年月日)</param>
        /// <returns>所在季度的周数</returns>
        public static int WeeksOfMonth(this in DateTime @this)
        {
            var lastDay = DateTimeHelper.GetDateMonth(@this).lastDay;
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
            return @this.IsRuYear() ? 366 : 365;
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
                2 => (@this.IsRuYear() ? 29 : 28),
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
        /// 判断当前年份是否是闰年
        /// </summary>
        /// <param name="this">时间</param>
        /// <returns>是闰年：True ，不是闰年：False</returns>
        public static bool IsRuYear(this in DateTime @this)
        {
            //形式参数为年份
            //例如：2003
            var n = @this.Year;
            return n % 400 == 0 || n % 4 == 0 && n % 100 != 0;
        }

    }
}