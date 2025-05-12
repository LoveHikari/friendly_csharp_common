using System.ComponentModel;
using System.Reflection;

namespace Hikari.Common;

/// <summary>
/// <see cref="Enum"/> 扩展类
/// </summary>
[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never), System.ComponentModel.Browsable(false)]
public static class EnumExtensions
{
    /// <summary>
    /// 获取枚举对象Key与显示名称的字典
    /// </summary>
    /// <param name="enumType"></param>
    /// <returns></returns>
    public static Dictionary<int, string> ToDictionary(this Type enumType)
    {
        if (!enumType.IsEnum)
        {
            throw new Exception("给定的类型不是枚举类型");
        }

        var enumItems = enumType.GetFields(BindingFlags.Public | BindingFlags.Static);
        var names = new Dictionary<int, string>(enumItems.Length);
        foreach (var enumItem in enumItems)
        {
            names[(int)enumItem.GetValue(enumType)!] = enumItem.Name;
        }

        return names;
    }
    /// <summary>
    /// 获得描述
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static string GetDescription(this Enum obj)
    {
        object[]? array = obj.GetType().GetField(obj.ToString())?.GetCustomAttributes(typeof(DescriptionAttribute), inherit: true);
        if (array is not null)
        {
            var attr = array.FirstOrDefault(x => x is DescriptionAttribute);
            if (attr is not null)
            {
                return ((DescriptionAttribute)attr).Description;
            }

        }

        return string.Empty;
    }
#if NET10_0
    extension(Enum obj)
    {
        /// <summary>
        /// 获得描述
        /// </summary>
        /// <returns></returns>
        public string Description {
            get
            {
                object[]? array = obj.GetType().GetField(obj.ToString())?.GetCustomAttributes(typeof(DescriptionAttribute), inherit: true);
                if (array is not null)
                {
                    var attr = array.FirstOrDefault(x => x is DescriptionAttribute);
                    if (attr is not null)
                    {
                        return ((DescriptionAttribute)attr).Description;
                    }

                }

                return string.Empty;
            } }
    }
#endif
}