/******************************************************************************************************************
 * 
 * 
 * 标  题： Object 帮助类(版本：Version1.0.0)
 * 作  者： YuXiaoWei
 * 日  期： 2016/11/23
 * 修  改：
 * 参  考： 
 * 说  明： 暂无...
 * 备  注： 暂无...
 * 调用示列：
 *
 * 
 * ***************************************************************************************************************/

using System;

namespace Hikari.Common
{
    /// <summary>
    /// Object 扩展类
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never), System.ComponentModel.Browsable(false)]
    public static class ObjectExtensions
    {
        /// <summary>
        /// 获取字符串 不返回null值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToString2(this object value)
        {
            if (value == null || value == System.DBNull.Value)
                return string.Empty;
            return value.ToString();
        }
        /// <summary>
        /// 获取字符串 不返回null值
        /// </summary>
        /// <param name="value"></param>
        /// <param name="s">指定为null时返回的值</param>
        /// <returns></returns>
        public static string ToString2(this object value, string s)
        {
            if (value == null || value == System.DBNull.Value)
                return s;
            return value.ToString();
        }
        /// <summary>
        /// 转换为等效的32位有符号整数，转换失败返回指定的数字，为null时返回0
        /// </summary>
        /// <param name="input"></param>
        /// <param name="i">指定转换失败时返回的值，默认为0</param>
        /// <returns></returns>
        public static int ToInt32(this object input, int i = 0)
        {
            int result;
            if (input == null || string.IsNullOrEmpty(input.ToString()))
                return 0;
            if (int.TryParse(input.ToString(), out result))
                return result;
            return i;
        }
        /// <summary>
        /// 转换为Long，为null时返回0
        /// </summary>
        /// <param name="input"></param>
        /// <param name="i">指定转换失败时返回的值，默认为0</param>
        /// <returns></returns>
        public static long ToLong(this object input, long i = 0L)
        {
            long result;
            if (input == null || string.IsNullOrEmpty(input.ToString()))
                return 0L;
            if (long.TryParse(input.ToString(), out result))
                return result;
            return i;
        }
        /// <summary>
        /// 转换为Short，为null时返回0
        /// </summary>
        /// <param name="source"></param>
        /// <param name="defaultValue">指定转换失败时返回的值，默认为0</param>
        /// <returns></returns>
        public static short ToShort(this object source, short defaultValue = 0)
        {
            if (source == null || string.IsNullOrEmpty(source.ToString()))
                return defaultValue;
            short result;
            if (!short.TryParse(source.ToString(), out result))
                result = defaultValue;
            return result;
        }
        /// <summary>
        /// 转换为decimal类型，失败返回0
        /// </summary>
        /// <param name="s"></param>
        /// <param name="d">指定转换失败时返回的值,默认为0</param>
        /// <returns></returns>
        public static decimal ToDecimal(this object s, decimal d = 0)
        {
            decimal result;
            if (s == null || string.IsNullOrEmpty(s.ToString()))
                return d;
            if (decimal.TryParse(s.ToString(), out result))
            {
                return result;
            }
            return d;
        }
        /// <summary>
        /// 转换为double类型，失败返回0
        /// </summary>
        /// <param name="s"></param>
        /// <param name="d">指定转换失败时返回的值,默认为0</param>
        /// <returns></returns>
        public static double ToDouble(this object s, double d = 0)
        {
            double result;
            if (s == null || string.IsNullOrEmpty(s.ToString()))
                return d;
            if (double.TryParse(s.ToString(), out result))
            {
                return result;
            }
            return d;
        }
        /// <summary>
        /// 转换为Boolean类型
        /// </summary>
        /// <param name="source"></param>
        /// <param name="defaultValue">指定转换失败时返回的值</param>
        /// <returns></returns>
        public static bool ToBoolean(this object source, bool defaultValue)
        {
            bool result;
            if (source == null || string.IsNullOrEmpty(source.ToString()))
                return defaultValue;
            if (!bool.TryParse(source.ToString(), out result))
                result = defaultValue;
            return result;
        }
        /// <summary>
        /// 转换为Boolean类型，遵循非0即真原则
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool ToBoolean(this object source)
        {
            bool result;
            int i;
            if (source == null || string.IsNullOrEmpty(source.ToString()))
                return false;
            if (!bool.TryParse(source.ToString(), out result))  //转化失败
            {
                if (int.TryParse(source.ToString(), out i))  //如果是int
                {
                    return i != 0;  //非0即真
                }
                result = true;
            }

            return result;
        }
        /// <summary>
        /// 转换成时间类型，失败则得到最小时间
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this object s)
        {
            return (DateTime)s.ToDateTime(DateTime.MaxValue);
        }
        /// <summary>
        /// 转换成时间类型，失败则得到最小时间
        /// </summary>
        /// <param name="s"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DateTime? ToDateTime(this object s, DateTime? value)
        {
            if (s == null || string.IsNullOrEmpty(s.ToString())) return value;
            DateTime result;
            if (DateTime.TryParse(s.ToString(), out result))
            {
                return result;
            }
            return value;
        }
    }
}
