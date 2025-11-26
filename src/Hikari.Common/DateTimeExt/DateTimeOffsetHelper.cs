/******************************************************************************************************************
 * 
 * 
 * 标  题： DateTimeOffset 帮助类(版本：Version1.0.0)
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

namespace Hikari.Common.DateTimeExt
{
    /// <summary>
    /// DateTimeOffset 帮助类
    /// </summary>
    public class DateTimeOffsetHelper
    {
        /// <summary>
        /// 获取某日期所在的周的开始日期和结束日期
        /// </summary>
        /// <param name="dtNow">当前日期（年月日）</param>
        /// <returns>开始日期,结束日期</returns>
        public static DateTimeOffsetRange GetDateWeek(DateTimeOffset dtNow)
        {
            //今天是星期几
            int iNowOfWeek = (int)dtNow.DayOfWeek;
            DateTimeOffset dtWeekSt = dtNow.AddDays(0 - iNowOfWeek).Date;
            DateTimeOffset dtWeekEd = dtNow.AddDays(6 - iNowOfWeek).Date;
            return new DateTimeOffsetRange(dtWeekSt, dtWeekEd);
        }

        /// <summary>
        /// 获取某年第某周的开始日期和结束日期
        /// </summary>
        /// <param name="year">当前年</param>
        /// <param name="numWeek">第几周</param>
        /// <returns>开始日期,结束日期</returns>
        public static DateTimeOffsetRange GetDateWeek(int year, int numWeek)
        {
            var dt = new DateTimeOffset(year, 1, 1, 0, 0, 0, TimeSpan.Zero);
            dt += new TimeSpan((numWeek - 1) * 7, 0, 0, 0);
            return GetDateWeek(dt);
        }

        /// <summary>
        /// 获取某日期所在的月的月初日期和月末日期
        /// </summary>
        /// <param name="dtNow">当前日期</param>
        /// <returns>月初,月末</returns>
        public static DateTimeOffsetRange GetDateMonth(DateTimeOffset dtNow)
        {
            DateTimeOffset dtFirstDay = dtNow.AddDays(1 - dtNow.Day).Date;  //本月月初
            DateTimeOffset dtLastDay = dtFirstDay.AddMonths(1).AddDays(-1).Date;  //本月月末
            //dtMonthEd = dtMonthSt.AddDays((dtNow.AddMonths(1) - dtNow).Days - 1);  //本月月末
            return new DateTimeOffsetRange(dtFirstDay, dtLastDay);
        }
        /// <summary>
        /// 获取某日期所在的季度的开始日期和结束日期
        /// </summary>
        /// <param name="dtNow">当前日期</param>
        /// <returns>本季度初,本季度末</returns>
        public static DateTimeOffsetRange GetDateQuarter(DateTimeOffset dtNow)
        {
            var dtQuarterSt = dtNow.AddMonths(0 - (dtNow.Month - 1) % 3).AddDays(1 - dtNow.Day).Date;  //本季度初  
            var dtQuarterEd = dtQuarterSt.AddMonths(3).AddDays(-1).Date;  //本季度末
            return new DateTimeOffsetRange(dtQuarterSt, dtQuarterEd);
        }
        /// <summary>
        /// 获取某季度的开始日期和结束日期
        /// </summary>
        /// <param name="quarter">季度</param>
        /// <param name="year">年份，默认为当前年</param>
        /// <returns>本季度初,本季度末</returns>
        public static DateTimeOffsetRange GetDateQuarter(int quarter, int? year = null)
        {
            var nowYear = year ?? DateTimeOffset.Now.Year;  // 当前年
            var startMonth = quarter * 3 - 2;
            var dtQuarterSt = new DateTimeOffset(nowYear, startMonth, 1, 0, 0, 0, TimeSpan.Zero);  //本季度初  
            var dtQuarterEd = dtQuarterSt.AddMonths(3).AddDays(-1).Date;  //本季度末
            return new DateTimeOffsetRange(dtQuarterSt, dtQuarterEd);
        }
        /// <summary>
        /// 获得月份是第几季度
        /// </summary>
        /// <param name="month">月份</param>
        /// <returns>第几季度</returns>
        public static int GetQuarter(int month)
        {
            return Math.Pow(2, month).ToInt32()?.Count() ?? 1;
        }
        /// <summary>
        /// 获取某日期所在的年的开始日期和结束日期
        /// </summary>
        /// <param name="dtNow">当前日期</param>
        /// <returns>年初,年末</returns>
        public static DateTimeOffsetRange GetDateYear(DateTimeOffset dtNow)
        {
            var dtYearSt = new DateTimeOffset(dtNow.Year, 1, 1, 0, 0, 0, TimeSpan.Zero);  //本年年初
            var dtYearEd = new DateTimeOffset(dtNow.Year, 12, 31, 23, 59, 59, TimeSpan.MaxValue);  //本年年末
            return new DateTimeOffsetRange(dtYearSt, dtYearEd);
        }
        /// <summary>
        /// 获得一年有几周
        /// </summary>
        /// <param name="strYear"></param>
        /// <returns></returns>
        public static int GetYearWeekCount(int strYear)
        {
            System.DateTimeOffset fDt = DateTimeOffset.Parse(strYear.ToString() + "-01-01");
            int k = Convert.ToInt32(fDt.DayOfWeek);//得到该年的第一天是周几 
            if (k == 0)
            {
                int countDay = fDt.AddYears(1).AddDays(-1).DayOfYear;
                int countWeek = countDay / 7;
                return countWeek;

            }
            else
            {
                int countDay = fDt.AddYears(1).AddDays(-1).DayOfYear;
                int countWeek = countDay / 7 + 1;
                return countWeek;
            }

        }

        /// <summary>
        /// 判断两个时间是否是在同一周
        /// </summary>
        /// <param name="date1"></param>
        /// <param name="date2"></param>
        /// <returns></returns>
        public static bool IsWeekSame(string date1, string date2)
        {

            DateTimeOffset dt1 = DateTimeOffset.Parse(date1);
            DateTimeOffset dt2 = DateTimeOffset.Parse(date2);
            DateTimeOffset temp1 = dt1.AddDays(-(int)dt1.DayOfWeek).Date;
            DateTimeOffset temp2 = dt2.AddDays(-(int)dt2.DayOfWeek).Date;
            bool result = temp1 == temp2;
            return result;
        }
        /// <summary>
        /// 判断两个时间是否是在同一月
        /// </summary>
        /// <param name="date1"></param>
        /// <param name="date2"></param>
        /// <returns></returns>
        public static bool IsMonthSame(string date1, string date2)
        {

            DateTimeOffset dt1 = DateTimeOffset.Parse(date1);
            DateTimeOffset dt2 = DateTimeOffset.Parse(date2);
            int temp1 = dt1.Month;
            int temp2 = dt2.Month;
            bool result = temp1 == temp2;
            return result;
        }
        /// <summary>
        /// 计算时间距离当前时间的时间差
        /// </summary>
        /// <param name="DateTimeOffset"></param>
        /// <returns></returns>
        public static string GetTimeInterval(DateTimeOffset DateTimeOffset)
        {
            DateTimeOffset nowTime = DateTimeOffset.Now;
            var timeSpan = nowTime - DateTimeOffset;
            if (timeSpan.TotalDays > 30)
            {
                return DateTimeOffset.ToString("yyyy-MM-dd HH:mm:ss");
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
