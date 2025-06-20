using System;
using System.Numerics;
using System.Text;

namespace Hikari.Common;
/// <summary>
/// <see cref="int"/> 整型扩展类
/// </summary>
[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never), System.ComponentModel.Browsable(false)]
public static class IntegerExtensions
{
    /// <summary>
    /// 秒级时间戳转为本地时间
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
    /// 秒级时间戳转为UTC时间
    /// </summary>
    /// <param name="this"></param>
    /// <returns></returns>
    public static DateTime FromUnixTimeSecondsToUtc(this in long @this)
    {
        DateTime unixEpochDateTimeUtc = new DateTime(621355968000000000L, DateTimeKind.Utc);
        return unixEpochDateTimeUtc + TimeSpan.FromSeconds(@this);
    }
    /// <summary>
    /// 毫秒级时间戳转为本地时间
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
    /// 毫秒级时间戳转为UTC时间
    /// </summary>
    /// <param name="this"></param>
    /// <returns></returns>
    public static DateTime FromUnixTimeMillisecondsToUtc(this in long @this)
    {
        DateTime unixEpochDateTimeUtc = new DateTime(621355968000000000L, DateTimeKind.Utc);
        return unixEpochDateTimeUtc + TimeSpan.FromMilliseconds(@this);
    }

    /// <summary>
    /// 数字转ip地址
    /// </summary>
    /// <param name="ipInt">ip数字</param>
    /// <returns>ip地址</returns>
    public static string ToIPv4(this in long ipInt)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(ipInt >> 0x18 & 0xff).Append(".");
        sb.Append(ipInt >> 0x10 & 0xff).Append(".");
        sb.Append(ipInt >> 0x8 & 0xff).Append(".");
        sb.Append(ipInt & 0xff);
        return sb.ToString();
    }

    #region 十进制转任意进制

    /// <summary>
    /// 十进制转任意进制
    /// </summary>
    /// <param name="value"></param>
    /// <param name="targetBase">进制</param>
    /// <returns></returns>
    public static string ToBase(this int value, byte targetBase)
    {
        return new BigInteger(value).ToBase(targetBase);
    }
    /// <summary>
    /// 十进制转任意进制
    /// </summary>
    /// <param name="value"></param>
    /// <param name="targetBase">进制</param>
    /// <returns></returns>
    public static string ToBase(this long value, byte targetBase)
    {
        return new BigInteger(value).ToBase(targetBase);
    }


    /// <summary>
    /// 十进制转任意进制
    /// </summary>
    /// <param name="value"></param>
    /// <param name="targetBase">进制</param>
    /// <returns></returns>
    public static string ToBase(this BigInteger value, byte targetBase)
    {
        // 将 BigInteger 转换为字节数组
        byte[] byteArray = value.ToByteArray();

        // 计算数组长度
        int arrayLength = (int)Math.Ceiling(byteArray.Length * 8.0 / Math.Log(targetBase, 2));

        // 创建结果数组
        int[] resultArray = new int[arrayLength];

        // 进行进制转换
        int currentIndex = 0;
        foreach (byte b in byteArray)
        {
            int currentByte = b;
            for (int j = 0; j < 8; j += Math.Max(1, (int)Math.Log(targetBase, 2)))
            {
                int remainder = currentByte % targetBase;
                currentByte = currentByte / targetBase;
                resultArray[currentIndex++] = remainder;
            }
        }

        // 反转数组
        Array.Reverse(resultArray);
        return string.Join("", resultArray.Select(i => Convert.ToString(i, targetBase)));

    }

    #endregion

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
    /// <param name="this"></param>
    /// <returns>true为质数</returns>
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
    /// <summary>
    /// 获得当前整数的长度
    /// </summary>
    /// <param name="this"></param>
    /// <returns></returns>
    public static int Count(this in int @this)
    {
        return @this.ToString().Length;
    }
}