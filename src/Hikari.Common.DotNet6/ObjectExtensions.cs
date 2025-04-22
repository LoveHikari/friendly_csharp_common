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

namespace Hikari.Common;
/// <summary>
/// Object 扩展类
/// </summary>
[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never), System.ComponentModel.Browsable(false)]
public static class ObjectExtensions
{
    ///// <summary>
    ///// 获取字符串 不返回null值
    ///// </summary>
    ///// <param name="this"></param>
    ///// <param name="s">指定为null时返回的值</param>
    ///// <returns></returns>
    //public static string ToNotNullString(this object? @this, string s = "")
    //{
    //    if (@this is null || @this == DBNull.Value)
    //        return s;
    //    return @this.ToString()!;
    //}
    /// <summary>
    /// 转换为等效的32位有符号整数
    /// </summary>
    /// <param name="this"></param>
    /// <returns></returns>
    public static int? ToInt32(this object? @this)
    {
        if (@this is null || string.IsNullOrWhiteSpace(@this.ToString()))
            return null;
        if (@this is bool b)
        {
            return b ? 1 : 0;
        }
        if (double.TryParse(@this.ToString(), out var doubleResult))
        {
            return (int)doubleResult; // 转换为int
        }
        return int.TryParse(@this.ToString(), out var result) ? result : null;
    }
    /// <summary>
    /// 转换为等效的32位有符号整数，转换失败返回指定的数字
    /// </summary>
    /// <param name="this"></param>
    /// <param name="i">指定转换失败时返回的值</param>
    /// <returns></returns>
    public static int ToInt32(this object? @this, int i)
    {
        if (@this is null || string.IsNullOrWhiteSpace(@this.ToString()))
            return 0;
        if (@this is bool b)
        {
            return b ? 1 : 0;
        }
        if (double.TryParse(@this.ToString(), out var doubleResult))
        {
            return (int)doubleResult; // 转换为int
        }
        return int.TryParse(@this.ToString(), out var result) ? result : i;
    }
    /// <summary>
    /// 转换为Long
    /// </summary>
    /// <param name="this"></param>
    /// <returns></returns>
    public static long? ToLong(this object? @this)
    {
        if (@this is null || string.IsNullOrWhiteSpace(@this.ToString()))
            return null;
        if (@this is bool b)
        {
            return b ? 1 : 0;
        }
        if (double.TryParse(@this.ToString(), out var doubleResult))
        {
            return (long)doubleResult; // 转换为long
        }
        return long.TryParse(@this.ToString(), out var result) ? result : null;
    }
    /// <summary>
    /// 转换为Long
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
        if (double.TryParse(@this.ToString(), out var doubleResult))
        {
            return (long)doubleResult; // 转换为long
        }
        return long.TryParse(@this.ToString(), out var result) ? result : i;
    }
    /// <summary>
    /// 转换为Short
    /// </summary>
    /// <param name="this"></param>
    /// <returns></returns>
    public static short? ToShort(this object? @this)
    {
        if (@this is null || string.IsNullOrWhiteSpace(@this.ToString()))
            return null;
        if (@this is bool b)
        {
            return b ? (short)1 : (short)0;
        }
        if (double.TryParse(@this.ToString(), out var doubleResult))
        {
            return (short)doubleResult; // 转换为long
        }
        return short.TryParse(@this.ToString(), out var result) ? result : null;
    }
    /// <summary>
    /// 转换为Short
    /// </summary>
    /// <param name="this"></param>
    /// <param name="i">指定转换失败时返回的值</param>
    /// <returns></returns>
    public static short ToShort(this object? @this, short i)
    {
        if (@this is null || string.IsNullOrWhiteSpace(@this.ToString()))
            return 0;
        if (@this is bool b)
        {
            return b ? (short)1 : (short)0;
        }
        if (double.TryParse(@this.ToString(), out var doubleResult))
        {
            return (short)doubleResult; // 转换为long
        }
        return short.TryParse(@this.ToString(), out var result) ? result : i;
    }
    /// <summary>
    /// 转换为decimal类型
    /// </summary>
    /// <param name="this"></param>
    /// <returns></returns>
    public static decimal? ToDecimal(this object? @this)
    {
        if (@this is null || string.IsNullOrWhiteSpace(@this.ToString()))
            return null;
        if (@this is bool b)
        {
            return b ? 1 : 0;
        }
        return decimal.TryParse(@this.ToString(), out var result) ? result : null;
    }
    /// <summary>
    /// 转换为decimal类型
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
    /// 转换为double类型
    /// </summary>
    /// <param name="this"></param>
    /// <returns></returns>
    public static double? ToDouble(this object? @this)
    {
        if (@this is null || string.IsNullOrWhiteSpace(@this.ToString()))
            return null;
        if (@this is bool b)
        {
            return b ? 1 : 0;
        }
        return double.TryParse(@this.ToString(), out var result) ? result : null;
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
    /// 转换为float类型
    /// </summary>
    /// <param name="this"></param>
    /// <returns></returns>
    public static float? ToFloat(this object? @this)
    {
        if (@this is null || string.IsNullOrWhiteSpace(@this.ToString()))
            return null;
        if (@this is bool b)
        {
            return b ? 1 : 0;
        }
        return float.TryParse(@this.ToString(), out var result) ? result : null;
    }
    /// <summary>
    /// 转换为float类型，失败返回默认值
    /// </summary>
    /// <param name="this"></param>
    /// <param name="d">指定转换失败时返回的值</param>
    /// <returns></returns>
    public static float ToFloat(this object? @this, float d)
    {
        if (@this is null || string.IsNullOrWhiteSpace(@this.ToString()))
            return 0;
        if (@this is bool b)
        {
            return b ? 1 : 0;
        }
        return float.TryParse(@this.ToString(), out var result) ? result : d;
    }
    /// <summary>
    /// 转换为Boolean类型，遵循非0即真原则
    /// </summary>
    /// <param name="this"></param>
    /// <returns></returns>
    public static bool ToBoolean(this object? @this)
    {
        if (@this == null) return false;
        // 检查对象类型并根据不同情况转换为bool
        return @this switch
        {
            bool boolValue => boolValue,
            int intValue => intValue != 0,
            double doubleValue => Math.Abs(doubleValue) > double.Epsilon,
            float floatValue => Math.Abs(floatValue) > float.Epsilon,
            string strValue => bool.TryParse(strValue, out var boolResult) ? boolResult :
                double.TryParse(strValue, out var num) ? num != 0.0 :
                !string.IsNullOrEmpty(strValue),
            IEnumerable<object> enumerableValue => enumerableValue.GetEnumerator().MoveNext(), // 检查是否为空集合
            _ => true
        };

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
    /// <summary>
    /// 范围判断函数，检查给定的值是否在指定的最小值和最大值之间。
    /// 例如，可以用来判断当前日期是否在开始日期和结束日期之间。
    /// 该方法适用于任何实现了 IComparable 接口的类型，比如 int、double、DateTime 等等。
    /// </summary>
    /// <typeparam name="T">实现了 IComparable 接口的泛型类型参数</typeparam>
    /// <param name="value">要比较的值</param>
    /// <param name="min">范围的最小值</param>
    /// <param name="max">范围的最大值</param>
    /// <returns>如果 value 在 min 和 max 之间，则返回 true；否则返回 false</returns>
    public static bool Between<T>(this T value, T min, T max) where T : IComparable<T>
    {
        // 使用 CompareTo 方法比较 value、min 和 max 的大小关系
        // value 必须大于或等于 min，并且小于或等于 max
        // 这里可以根据实际业务场景需求调整
        return value.CompareTo(min) >= 0 && value.CompareTo(max) <= 0;
    }
    /// <summary>
    /// 深拷贝
    /// </summary>
    /// <typeparam name="T">泛型类型参数</typeparam>
    /// <param name="info"></param>
    /// <returns></returns>
    public static T? DeepCopy<T>(this T? info) where T : class
    {
        return info == null ? null : System.Text.Json.JsonSerializer.Deserialize<T>(System.Text.Json.JsonSerializer.Serialize(info));
    }
}

