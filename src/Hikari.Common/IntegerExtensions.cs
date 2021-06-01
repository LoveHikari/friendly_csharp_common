using System;

namespace Hikari.Common
{
    /// <summary>
    /// <see cref="int"/> 整型扩展类
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never), System.ComponentModel.Browsable(false)]
    public static class IntegerExtensions
    {
        /// <summary>
        /// 时间戳转为C#格式时间
        /// </summary>
        /// <param name="this">Unix时间戳</param>
        /// <param name="unit">时间精度，秒(s) 毫秒(ms)</param>
        /// <returns>时间</returns>
        public static DateTime ToDateTime(this in long @this, string unit)
        {
            // DateTime dtStart = TimeZoneInfo.ConvertTime(new System.DateTime(1970, 1, 1), TimeZoneInfo.Local);
            System.DateTime dtStart = System.TimeZoneInfo.ConvertTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0), TimeZoneInfo.Utc, TimeZoneInfo.Local);
            var lTime = unit == "s" ? long.Parse(@this + "0000000") : long.Parse(@this + "0000");
            TimeSpan toNow = new TimeSpan(lTime); 
            return dtStart.Add(toNow);
        }
        /// <summary>
        /// 时间戳转为C#格式时间，默认精度到秒
        /// </summary>
        /// <param name="this">Unix时间戳</param>
        /// <returns>时间</returns>
        public static DateTime ToDateTime(this in long @this)
        {
            return @this.ToDateTime("s");
        }

        /// <summary>
        /// 是否是奇数
        /// </summary>
        /// <param name="this"></param>
        /// <returns>true：是奇数</returns>
        public static bool IsOdd(this int @this)
        {
            return @this % 2 == 1;
        }
        /// <summary>
        /// 判断给定的数字是否为素数(质数)
        /// </summary>
        /// <param name="this">true为质数</param>
        /// <returns></returns>
        public static bool IsPrime(this in int @this)
        {
            if (@this < 2)
            {
                return false;
            }

            for (int i = 2; i * i <= @this; i++)
            {
                if (@this % i == 0)
                {
                    return false;
                }
            }

            return true;
        }
    }
}