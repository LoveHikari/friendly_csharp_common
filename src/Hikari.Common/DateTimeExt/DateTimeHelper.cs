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

using System;

namespace Hikari.Common.DateTimeExt
{
    /// <summary>
    /// DateTime 帮助类
    /// </summary>
    public class DateTimeHelper
    {
        /// <summary>
        /// 获取某日期所在的周的开始日期和结束日期
        /// </summary>
        /// <param name="dtNow">当前日期（年月日）</param>
        /// <returns>开始日期,结束日期</returns>
        public static DateTimeRange GetDateWeek(DateTime dtNow)
        {
            //今天是星期几
            int iNowOfWeek = (int)dtNow.DayOfWeek;
            DateTime dtWeekSt = dtNow.AddDays(0 - iNowOfWeek).Date;
            DateTime dtWeekEd = dtNow.AddDays(6 - iNowOfWeek).Date;
            return new DateTimeRange(dtWeekSt, dtWeekEd);
        }

        /// <summary>
        /// 获取某年第某周的开始日期和结束日期
        /// </summary>
        /// <param name="year">当前日期（年月日）</param>
        /// <param name="numWeek">第几周</param>
        /// <returns>开始日期,结束日期</returns>
        public static DateTimeRange GetDateWeek(int year, int numWeek)
        {
            var dt = new DateTime(year, 1, 1);
            dt += new TimeSpan((numWeek - 1) * 7, 0, 0, 0);
            return GetDateWeek(dt);
        }

        /// <summary>
        /// 获取某日期所在的月的月初日期和月末日期
        /// </summary>
        /// <param name="dtNow">当前日期</param>
        /// <returns>月初,月末</returns>
        public static DateTimeRange GetDateMonth(DateTime dtNow)
        {
            DateTime dtFirstDay = dtNow.AddDays(1 - dtNow.Day).Date;  //本月月初
            DateTime dtLastDay = dtFirstDay.AddMonths(1).AddDays(-1).Date;  //本月月末
            //dtMonthEd = dtMonthSt.AddDays((dtNow.AddMonths(1) - dtNow).Days - 1);  //本月月末
            return new DateTimeRange(dtFirstDay, dtLastDay);
        }
        /// <summary>
        /// 获取某日期所在的季度的开始日期和结束日期
        /// </summary>
        /// <param name="dtNow">当前日期</param>
        /// <returns>本季度初,本季度末</returns>
        public static DateTimeRange GetDateQuarter(DateTime dtNow)
        {
            var dtQuarterSt = dtNow.AddMonths(0 - (dtNow.Month - 1) % 3).AddDays(1 - dtNow.Day).Date;  //本季度初  
            var dtQuarterEd = dtQuarterSt.AddMonths(3).AddDays(-1).Date;  //本季度末
            return new DateTimeRange(dtQuarterSt, dtQuarterEd);
        }
        /// <summary>
        /// 获取某季度的开始日期和结束日期
        /// </summary>
        /// <param name="quarter">季度</param>
        /// <param name="year">年份，默认为当前年</param>
        /// <returns>本季度初,本季度末</returns>
        public static DateTimeRange GetDateQuarter(int quarter, int? year = null)
        {
            var nowYear = year ?? DateTime.Now.Year;  // 当前年
            var startMonth = quarter * 3 - 2;
            var dtQuarterSt = new DateTime(nowYear, startMonth, 1);  //本季度初  
            var dtQuarterEd = dtQuarterSt.AddMonths(3).AddDays(-1).Date;  //本季度末
            return new DateTimeRange(dtQuarterSt, dtQuarterEd);
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
        public static DateTimeRange GetDateYear(DateTime dtNow)
        {
            var dtYearSt = new DateTime(dtNow.Year, 1, 1);  //本年年初  
            var dtYearEd = new DateTime(dtNow.Year, 12, 31);  //本年年末
            return new DateTimeRange(dtYearSt, dtYearEd);
        }
        /// <summary>
        /// 获得一年有几周
        /// </summary>
        /// <param name="strYear"></param>
        /// <returns></returns>
        public static int GetYearWeekCount(int strYear)
        {
            System.DateTime fDt = DateTime.Parse(strYear.ToString() + "-01-01");
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
        public static bool IsMonthSame(string date1, string date2)
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
