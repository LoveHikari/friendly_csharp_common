using System.ComponentModel;

namespace Hikari.Common;

/// <summary>
/// <see cref="Enum"/> 扩展类
/// </summary>
[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never), System.ComponentModel.Browsable(false)]
public static class EnumExtensions
{
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

}