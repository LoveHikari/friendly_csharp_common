/******************************************************************************************************************
 * 
 * 
 * 标  题： DateTime 帮助类(版本：Version1.0.0)
 * 作  者： YuXiaoWei
 * 日  期： 2016/11/30
 * 修  改：
 * 参  考： http://www.cnblogs.com/codemo/archive/2012/05/18/2507251.html
 * 说  明： 暂无...
 * 备  注： 暂无...
 * 调用示列：
 *
 * 
 * ***************************************************************************************************************/

namespace System
{
    /// <summary>
    /// DateTime 帮助类
    /// </summary>
    public class DateTimeHelper
    {
        /// <summary>
        /// 获取当日所在的周的开始日期和结束日期
        /// </summary>
        /// <param name="dtNow">当前日期（年月日）</param>
        /// <returns>开始日期,结束日期</returns>
        public static (DateTime weekSt, DateTime weekEd) GetDateWeek(DateTime dtNow)
        {
            //今天是星期几
            int iNowOfWeek = (int)dtNow.DayOfWeek;
            DateTime dtWeekSt = dtNow.AddDays(0 - iNowOfWeek);
            DateTime dtWeekEd = dtNow.AddDays(6 - iNowOfWeek);
            return (dtWeekSt, dtWeekEd);
        }

        /// <summary>
        /// 返回指定日期所在月的第一天和最后一天
        /// </summary>
        /// <param name="dtNow">当前日期</param>
        /// <returns>第一天,最后一天</returns>
        public static (DateTime firstDay, DateTime lastDay) GetDateMonth(DateTime dtNow)
        {
            DateTime lastDay;
            int year = dtNow.Year;
            int month = dtNow.Month;

            DateTime firstDay = new DateTime(year, month, 1);
            var mold = month % 2;  //为0为偶数
            if (month == 2)  // 如果为2月
            {

                if (DateTime.IsLeapYear(year))  // 是闰年
                    lastDay = new DateTime(year, month, 29);
                else
                    lastDay = new DateTime(year, month, 28);
            }
            else if ((month <= 7 && mold == 0) || (month > 7 && mold != 0))
            {
                lastDay = new DateTime(year, month, 30);
            }
            else
            {
                lastDay = new DateTime(year, month, 31);
            }

            return (firstDay, lastDay);
        }

        /// <summary>
        /// 取指定日期是一年中的第几周
        /// </summary>
        /// <param name="dtNow">给定的日期(年月日)</param>
        /// <returns>一年中的第几周</returns>
        public static int GetWeekOfYear(DateTime dtNow)
        {
            //一.找到第一周的最后一天（先获取1月1日是星期几，从而得知第一周周末是几）
            int firstWeekend = 7 - Convert.ToInt32(DateTime.Parse(dtNow.Year + "-1-1").DayOfWeek);

            //二.获取今天是一年当中的第几天
            int currentDay = dtNow.DayOfYear;

            //三.（今天 减去 第一周周末）/7 等于 距第一周有多少周 再加上第一周的1 就是今天是今年的第几周了
            //    刚好考虑了惟一的特殊情况就是，今天刚好在第一周内，那么距第一周就是0 再加上第一周的1 最后还是1
            return Convert.ToInt32(Math.Ceiling((currentDay - firstWeekend) / 7.0)) + 1;
        }

        /// <summary>
        /// 取指定日期是一年中的第几周,并返回该周第一天和最后一天
        /// </summary>
        /// <param name="dtNow">给定的日期(年月日)</param>
        /// <returns>GetWeekDay[0]=周次;GetWeekDay[1]=该周第一天;GetWeekDay[2]=该周最后一天</returns>
        public static string[] GetWeekDay(DateTime dtNow)
        {
            string[] inti = new string[3];
            DateTime day = DateTime.Parse(dtNow.Year + "-1-1");  //一年的第一天
            int weekday = (int)day.DayOfWeek; //一年的第一天是星期几
            int DayCount = dtNow.DayOfYear;  //今天是一年中的第几天
            int i = Convert.ToInt32(Math.Ceiling((DayCount - 7 + weekday) / 7.0)) + 1;  //第几周
            inti[0] = i.ToString();

            //今天是星期几
            int iNowOfWeek = (int)dtNow.DayOfWeek;
            inti[1] = dtNow.AddDays(0 - iNowOfWeek).ToString();
            inti[2] = dtNow.AddDays(6 - iNowOfWeek).ToString();
            return inti;
        }

        /// <summary>
        /// 某日期是当月的第几周
        /// </summary>
        /// <param name="day">给定的日期(年月日)</param>
        /// <returns>第几周</returns>
        public static int GetWeekOfMonth(DateTime day)
        {
            DateTime FirstofMonth;
            FirstofMonth = Convert.ToDateTime(day.Date.Year + "-" + day.Date.Month + "-" + 1);
            int i = (int)FirstofMonth.Date.DayOfWeek;
            if (i == 0)
            {
                i = 7;
            }
            return (day.Date.Day + i - 1) / 7 + 1;
        }

        /// <summary>
        /// 某日期是当月的第几周,并返回该周的第一天和最后一天
        /// </summary>
        /// <param name="day">给定日期（年月日）</param>
        /// <returns>周次,该周第一天,该周最后一天</returns>
        public static (int week, DateTime weekSt, DateTime weekEd) GetWeekOfDay(DateTime day)
        {
            int week = GetWeekOfMonth(day);
            var dt = GetDateWeek(day);
            
            return (week, dt.weekSt, dt.weekEd);

        }

        /// <summary>
        /// 获取当日所在的月的月初日期和月末日期
        /// </summary>
        /// <param name="dtNow">当前日期</param>
        /// <returns>月初,月末</returns>
        public static ( DateTime monthSt,  DateTime monthEd) GetDatetMonth(DateTime dtNow)
        {
            DateTime dtMonthSt = dtNow.AddDays(1 - dtNow.Day);  //本月月初
            DateTime dtMonthEd = dtMonthSt.AddMonths(1).AddDays(-1);  //本月月末
            //dtMonthEd = dtMonthSt.AddDays((dtNow.AddMonths(1) - dtNow).Days - 1);  //本月月末
            return (dtMonthSt, dtMonthEd);
        }
        /// <summary>
        /// 获取当日所在的季度的开始日期和结束日期
        /// </summary>
        /// <param name="dtNow">当前日期</param>
        /// <returns>本季度初,本季度末</returns>
        public static (DateTime quarterSt, DateTime quarterEd) GetDatetQuarter(DateTime dtNow)
        {
            var dtQuarterSt = dtNow.AddMonths(0 - (dtNow.Month - 1) % 3).AddDays(1 - dtNow.Day);  //本季度初  
            var dtQuarterEd = dtQuarterSt.AddMonths(3).AddDays(-1);  //本季度末
            return (dtQuarterSt, dtQuarterEd);
        }
        ///// <summary>
        ///// 获取当日所在的年的开始日期和结束日期
        ///// </summary>
        ///// <param name="dtNow">当前日期</param>
        ///// <param name="dtYearSt">年初</param>
        ///// <param name="dtYearEd">年末</param>
        //public static void ReturnDatetYear(DateTime dtNow, out DateTime dtYearSt, out DateTime dtYearEd)
        //{
        //    dtYearSt = new DateTime(dtNow.Year, 1, 1);  //本年年初  
        //    dtYearEd = new DateTime(dtNow.Year, 12, 31);  //本年年末
        //}

        #region 英文转化为中文的星期几
        /// <summary>
        /// 英文转化为中文的星期几
        /// </summary>
        /// <param name="week">星期</param>
        /// <param name="cultureName">地区限制匹配规范</param>
        /// <returns></returns>
        public static string WeekToCulture(DayOfWeek week, string cultureName = "zh-CN")
        {
            return System.Globalization.CultureInfo.GetCultureInfo(cultureName).DateTimeFormat.GetDayName(week);
        }
        #endregion

        /// <summary>
        /// 时间戳转为C#格式时间
        /// </summary>
        /// <param name="timeStamp">Unix时间戳</param>
        /// <param name="unit">时间精度，秒(s) 毫秒(ms)</param>
        /// <returns>时间</returns>
        public static DateTime ConvertDateTime(string timeStamp, string unit = "s")
        {
            // DateTime dtStart = TimeZoneInfo.ConvertTime(new System.DateTime(1970, 1, 1), TimeZoneInfo.Local);
            System.DateTime dtStart = System.TimeZoneInfo.ConvertTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0), TimeZoneInfo.Utc, TimeZoneInfo.Local);
            long lTime;
            if (unit == "s")
            {
                lTime = long.Parse(timeStamp + "0000000");
            }
            else
            {
                lTime = long.Parse(timeStamp + "0000");
            }
            TimeSpan toNow = new TimeSpan(lTime); return dtStart.Add(toNow);
        }

        /// <summary>
        /// DateTime时间格式转换为Unix时间戳格式
        /// </summary>
        /// <param name="time">时间</param>
        /// <param name="unit">时间精度，秒(s) 毫秒(ms)</param>
        /// <returns>Unix时间戳</returns>
        public static long ConvertDateTimeInt(System.DateTime time, string unit = "s")
        {
            //System.DateTime startTime = TimeZoneInfo.ConvertTime(new System.DateTime(1970, 1, 1), TimeZoneInfo.Local);
            System.DateTime startTime = System.TimeZoneInfo.ConvertTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0), TimeZoneInfo.Utc, TimeZoneInfo.Local);
            if (unit.ToLower() == "s")
            {
                return (long)(time - startTime).TotalSeconds;
            }
            else
            {
                return (long)(time - startTime).TotalMilliseconds;
            }

        }
        /// <summary>
        /// 判断两个时间是否是在同一周
        /// </summary>
        /// <param name="date1"></param>
        /// <param name="date2"></param>
        /// <returns></returns>
        public static bool IsWeekSame(String date1, String date2)
        {

            DateTime dt1 = DateTime.Parse(date1);
            DateTime dt2 = DateTime.Parse(date2);
            DateTime temp1 = dt1.AddDays(-(int)dt1.DayOfWeek).Date;
            DateTime temp2 = dt2.AddDays(-(int)dt2.DayOfWeek).Date;
            bool result = temp1 == temp2;
            return result;
        }
        /// <summary>
        /// 判断两个时间是否是在同一月
        /// </summary>
        /// <param name="date1"></param>
        /// <param name="date2"></param>
        /// <returns></returns>
        public static bool IsMonthSame(String date1, String date2)
        {

            DateTime dt1 = DateTime.Parse(date1);
            DateTime dt2 = DateTime.Parse(date2);
            int temp1 = dt1.Month;
            int temp2 = dt2.Month;
            bool result = temp1 == temp2;
            return result;
        }
        /// <summary>
        /// 计算时间距离当前时间的时间差
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string GetTimeInterval(DateTime dateTime)
        {
            DateTime nowTime = DateTime.Now;
            var timeSpan = nowTime - dateTime;
            if (timeSpan.TotalDays > 30)
            {
                return dateTime.ToString("yyyy-MM-dd HH:mm:ss");
            }
            if (timeSpan.TotalHours > 24)
            {
                return timeSpan.Days + "天前";
            }
            if (timeSpan.TotalMinutes > 60)
            {
                return timeSpan.Hours + "小时前";
            }
            if (timeSpan.TotalSeconds > 60)
            {
                return timeSpan.Minutes + "分钟前";
            }
            else
            {
                return timeSpan.Seconds + "秒前";
            }

        }
    }
}
