// Copyright (c) the Hikari. Foundation. All rights reserved.
// The Hikari. Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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

using System.Reflection;

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
        /// <param name="this"></param>
        /// <param name="s">指定为null时返回的值</param>
        /// <returns></returns>
        public static string ToNotNullString(this object? @this, string s = "")
        {
            if (@this is null || @this == DBNull.Value)
                return s;
            return @this.ToString()!;
        }
        /// <summary>
        /// 转换为等效的32位有符号整数，转换失败返回指定的数字，为null时返回0
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static int ToInt32(this object? @this)
        {
            return ToInt32(@this, 0);
        }
        /// <summary>
        /// 转换为等效的32位有符号整数，转换失败返回指定的数字，为null时返回0
        /// </summary>
        /// <param name="this"></param>
        /// <param name="i">指定转换失败时返回的值，默认为0</param>
        /// <returns></returns>
        public static int ToInt32(this object? @this, int i)
        {
            if (@this is null || string.IsNullOrWhiteSpace(@this.ToString()))
                return 0;
            if (@this is bool b)
            {
                return b ? 1 : 0;
            }
            return int.TryParse(@this.ToString(), out var result) ? result : i;
        }
        /// <summary>
        /// 转换为Long，为null时返回默认值，默认为0
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static long ToLong(this object? @this)
        {
            return ToLong(@this, 0L);
        }
        /// <summary>
        /// 转换为Long，为null时返回默认值，默认为0
        /// </summary>
        /// <param name="this"></param>
        /// <param name="i">指定转换失败时返回的值</param>
        /// <returns></returns>
        public static long ToLong(this object? @this, long i)
        {
            if (@this is null || string.IsNullOrWhiteSpace(@this.ToString()))
                return 0L;
            if (@this is bool b)
            {
                return b ? 1 : 0;
            }
            return long.TryParse(@this.ToString(), out var result) ? result : i;
        }
        /// <summary>
        /// 转换为Short，为null时返回默认值，默认为0
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static short ToShort(this object? @this)
        {
            return ToShort(@this, 0);
        }
        /// <summary>
        /// 转换为Short，为null时返回默认值
        /// </summary>
        /// <param name="this"></param>
        /// <param name="i">指定转换失败时返回的值，默认为0</param>
        /// <returns></returns>
        public static short ToShort(this object? @this, short i)
        {
            if (@this is null || string.IsNullOrWhiteSpace(@this.ToString()))
                return 0;
            if (@this is bool b)
            {
                return b ? (short)1 : (short)0;
            }
            return short.TryParse(@this.ToString(), out var result) ? result : i;
        }
        /// <summary>
        /// 转换为decimal类型，失败返回默认值,默认为0
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static decimal ToDecimal(this object? @this)
        {
            return ToDecimal(@this, 0M);
        }
        /// <summary>
        /// 转换为decimal类型，失败返回默认值
        /// </summary>
        /// <param name="this"></param>
        /// <param name="d">指定转换失败时返回的值</param>
        /// <returns></returns>
        public static decimal ToDecimal(this object? @this, decimal d)
        {
            if (@this is null || string.IsNullOrWhiteSpace(@this.ToString()))
                return 0;
            if (@this is bool b)
            {
                return b ? 1 : 0;
            }
            return decimal.TryParse(@this.ToString(), out var result) ? result : d;
        }
        /// <summary>
        /// 转换为double类型，失败返回默认值,默认为0
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static double ToDouble(this object? @this)
        {
            return ToDouble(@this, 0D);
        }
        /// <summary>
        /// 转换为double类型，失败返回默认值
        /// </summary>
        /// <param name="this"></param>
        /// <param name="d">指定转换失败时返回的值</param>
        /// <returns></returns>
        public static double ToDouble(this object? @this, double d)
        {
            if (@this is null || string.IsNullOrWhiteSpace(@this.ToString()))
                return 0;
            if (@this is bool b)
            {
                return b ? 1 : 0;
            }
            return double.TryParse(@this.ToString(), out var result) ? result : d;
        }
        /// <summary>
        /// 转换为float类型，失败返回默认值,默认为0
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static double ToFloat(this object? @this)
        {
            return ToFloat(@this, 0F);
        }
        /// <summary>
        /// 转换为float类型，失败返回默认值
        /// </summary>
        /// <param name="this"></param>
        /// <param name="d">指定转换失败时返回的值</param>
        /// <returns></returns>
        public static double ToFloat(this object? @this, float d)
        {
            if (@this is null || string.IsNullOrWhiteSpace(@this.ToString()))
                return 0;
            if (@this is bool b)
            {
                return b ? 1 : 0;
            }
            return double.TryParse(@this.ToString(), out var result) ? result : d;
        }
        /// <summary>
        /// 转换为Boolean类型，遵循非0即真原则
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static bool ToBoolean(this object? @this)
        {
            if (@this is not string source) return @this is not null;
            if (string.IsNullOrWhiteSpace(source)) return false;

            if (bool.TryParse(source, out var result)) return result;
            if (int.TryParse(source, out var i))  //如果是int
            {
                return i != 0;  //非0即真
            }

            return true;

        }
        /// <summary>
        /// 根据属性名获得属性值
        /// </summary>
        /// <param name="this"></param>
        /// <param name="propertyName">属性名</param>
        /// <returns>属性值</returns>
        public static object? GetValue(this object @this, string propertyName)
        {
            PropertyInfo[] properties = @this.GetType().GetProperties();
            var obj = properties.FirstOrDefault(p => p.Name == propertyName)?.GetValue(@this);
            return obj;
        }
    }
}
