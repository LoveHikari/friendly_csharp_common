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
        /// 时间戳转为本地时间
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static DateTime FromUnixTimeSeconds(this in long @this)
        {
            // DateTime dtStart = TimeZoneInfo.ConvertTime(new System.DateTime(1970, 1, 1), TimeZoneInfo.Local);
            System.DateTime dtStart = System.TimeZoneInfo.ConvertTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0), TimeZoneInfo.Utc, TimeZoneInfo.Local);
            return dtStart + TimeSpan.FromSeconds(@this);
        }
        /// <summary>
        /// 时间戳转为UTC时间
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static DateTime FromUnixTimeSecondsToUtc(this in long @this)
        {
            DateTime unixEpochDateTimeUtc = new DateTime(621355968000000000L, DateTimeKind.Utc);
            return unixEpochDateTimeUtc + TimeSpan.FromSeconds(@this);
        }
        /// <summary>
        /// 时间戳转为本地时间
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static DateTime FromUnixTimeMilliseconds(this in long @this)
        {
            // DateTime dtStart = TimeZoneInfo.ConvertTime(new System.DateTime(1970, 1, 1), TimeZoneInfo.Local);
            System.DateTime dtStart = System.TimeZoneInfo.ConvertTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0), TimeZoneInfo.Utc, TimeZoneInfo.Local);
            return dtStart + TimeSpan.FromMilliseconds(@this);
        }
        /// <summary>
        /// 时间戳转为UTC时间
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static DateTime FromUnixTimeMillisecondsToUtc(this in long @this)
        {
            DateTime unixEpochDateTimeUtc = new DateTime(621355968000000000L, DateTimeKind.Utc);
            return unixEpochDateTimeUtc + TimeSpan.FromMilliseconds(@this);
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