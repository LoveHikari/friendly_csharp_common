using System;

namespace Hikari.Common.DateTimeExt
{
    /// <summary>
    /// DateOnly 帮助类
    /// </summary>
    public class DateOnlyHelper
    {
        /// <summary>
        /// 获取某日期所在的周的开始日期和结束日期
        /// </summary>
        /// <param name="dtNow">当前日期（年月日）</param>
        /// <returns>开始日期,结束日期</returns>
        public static DateOnlyRange GetDateWeek(DateOnly dtNow)
        {
            //今天是星期几
            int iNowOfWeek = (int)dtNow.DayOfWeek;
            DateOnly dtWeekSt = dtNow.AddDays(0 - iNowOfWeek);
            DateOnly dtWeekEd = dtNow.AddDays(6 - iNowOfWeek);
            return new DateOnlyRange(dtWeekSt, dtWeekEd);
        }
        /// <summary>
        /// 获取某年第某周的开始日期和结束日期
        /// </summary>
        /// <param name="year">当前日期（年月日）</param>
        /// <param name="numWeek">第几周</param>
        /// <returns>开始日期,结束日期</returns>
        public static DateOnlyRange GetDateWeek(int year, int numWeek)
        {
            var dt = new DateOnly(year, 1, 1);
            dt = dt.AddDays((numWeek - 1) * 7);
            return GetDateWeek(dt);
        }

        /// <summary>
        /// 获取某日期所在的月的月初日期和月末日期
        /// </summary>
        /// <param name="dtNow">当前日期</param>
        /// <returns>月初,月末</returns>
        public static DateOnlyRange GetDateMonth(DateOnly dtNow)
        {
            DateOnly dtFirstDay = dtNow.AddDays(1 - dtNow.Day);  //本月月初
            DateOnly dtLastDay = dtFirstDay.AddMonths(1).AddDays(-1);  //本月月末
            //dtMonthEd = dtMonthSt.AddDays((dtNow.AddMonths(1) - dtNow).Days - 1);  //本月月末
            return new DateOnlyRange(dtFirstDay, dtLastDay);
        }
        /// <summary>
        /// 获取某日期所在的季度的开始日期和结束日期
        /// </summary>
        /// <param name="dtNow">当前日期</param>
        /// <returns>本季度初,本季度末</returns>
        public static DateOnlyRange GetDateQuarter(DateOnly dtNow)
        {
            var dtQuarterSt = dtNow.AddMonths(0 - (dtNow.Month - 1) % 3).AddDays(1 - dtNow.Day);  //本季度初  
            var dtQuarterEd = dtQuarterSt.AddMonths(3).AddDays(-1);  //本季度末
            return new DateOnlyRange(dtQuarterSt, dtQuarterEd);
        }
        /// <summary>
        /// 获取某季度的开始日期和结束日期
        /// </summary>
        /// <param name="quarter">季度</param>
        /// <param name="year">年份，默认为当前年</param>
        /// <returns>本季度初,本季度末</returns>
        public static DateOnlyRange GetDateQuarter(int quarter, int? year = null)
        {
            var nowYear = year ?? DateTime.Now.Year;  // 当前年
            var startMonth = quarter * 3 - 2;
            var dtQuarterSt = DateOnly.FromDateTime(new DateTime(nowYear, startMonth, 1));  //本季度初
            var dtQuarterEd = dtQuarterSt.AddMonths(3).AddDays(-1);  //本季度末
            return new DateOnlyRange(dtQuarterSt, dtQuarterEd);
        }
        /// <summary>
        /// 获取某日期所在的年的开始日期和结束日期
        /// </summary>
        /// <param name="dtNow">当前日期</param>
        /// <returns>年初,年末</returns>
        public static DateOnlyRange GetDateYear(DateOnly dtNow)
        {
            var dtYearSt = new DateOnly(dtNow.Year, 1, 1);  //本年年初  
            var dtYearEd = new DateOnly(dtNow.Year, 12, 31);  //本年年末
            return new DateOnlyRange(dtYearSt, dtYearEd);
        }
        /// <summary>
        /// 获得一年有几周
        /// </summary>
        /// <param name="strYear"></param>
        /// <returns></returns>
        public static int GetYearWeekCount(int strYear)
        {
            DateOnly fDt = DateOnly.Parse(strYear.ToString() + "-01-01");
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
        /// 判断两个日期是否是在同一周
        /// </summary>
        /// <param name="date1"></param>
        /// <param name="date2"></param>
        /// <returns></returns>
        public static bool IsWeekSame(string date1, string date2)
        {

            DateOnly dt1 = DateOnly.Parse(date1);
            DateOnly dt2 = DateOnly.Parse(date2);
            DateOnly temp1 = dt1.AddDays(-(int)dt1.DayOfWeek);
            DateOnly temp2 = dt2.AddDays(-(int)dt2.DayOfWeek);
            bool result = temp1 == temp2;
            return result;
        }
        /// <summary>
        /// 判断两个日期是否是在同一月
        /// </summary>
        /// <param name="date1"></param>
        /// <param name="date2"></param>
        /// <returns></returns>
        public static bool IsMonthSame(string date1, string date2)
        {

            DateOnly dt1 = DateOnly.Parse(date1);
            DateOnly dt2 = DateOnly.Parse(date2);
            int temp1 = dt1.Month;
            int temp2 = dt2.Month;
            bool result = temp1 == temp2;
            return result;
        }
    }
}