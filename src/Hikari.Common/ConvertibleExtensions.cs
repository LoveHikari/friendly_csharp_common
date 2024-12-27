using System.Reflection;

namespace Hikari.Common;
/// <summary>
/// <see cref="object"/> 扩展类
/// </summary>
[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never), System.ComponentModel.Browsable(false)]
public static class ConvertibleExtensions
{
    /// <summary>
    /// 匿名类型转强类型
    /// </summary>
    /// <typeparam name="T">类型</typeparam>
    /// <param name="obj">匿名对象</param>
    /// <para>Cast(new { Name = "Tom", Age = 25 });</para>
    /// <returns></returns>
    public static T? CastTo<T>(this object obj) where T : class
    {
        var t = typeof(T);
        var o = System.Activator.CreateInstance(t);
        if (o is null)
        {
            return null;
        }
        var pros = t.GetProperties();
        var t2 = obj.GetType();
        if (t2 == typeof(System.Dynamic.ExpandoObject))
        {
            var dic = (IDictionary<string, object>)obj;
            foreach (var pro in pros)
            {
                pro.SetValue(o, dic[pro.Name]);
            }
        }
        else
        {

            foreach (var pro in pros)
            {
                pro.SetValue(o, t2.GetProperty(pro.Name)?.GetValue(obj, null));
            }


        }
        return (T)o;
    }

    /// <summary>
    /// 返回一个指定类型的对象，该对象的值等效于指定的对象。
    /// </summary>
    /// <param name="value">需要转化的对象</param>
    /// <param name="conversionType">转化后的类型</param>
    /// <returns>转化后的对象</returns>
    public static object? ChangeTypeTo(this object? value, Type conversionType)
    {
        if (value == null)
            return null;
        if (conversionType.IsGenericType && conversionType.GetGenericTypeDefinition() == typeof(Nullable<>))
        {
            System.ComponentModel.NullableConverter nullableConverter = new System.ComponentModel.NullableConverter(conversionType);
            conversionType = nullableConverter.UnderlyingType;
        }

        try
        {
            return Convert.ChangeType(value, conversionType);
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// 以最大的可能性返回一个指定类型的对象，该对象的值等效于指定的对象。
    /// </summary>
    /// <typeparam name="T">转化后的类型</typeparam>
    /// <param name="value">需要转化的对象</param>
    /// <returns>转化后的对象</returns>
    public static T ChangeTypeTo<T>(this object value)
    {
        Type conversionType = typeof(T);
        object obj = Activator.CreateInstance(conversionType)!;

        Type oldType = value.GetType();
        PropertyInfo[] propertyInfos = conversionType.GetProperties();
        foreach (PropertyInfo propertyInfo in propertyInfos)
        {
            var v = oldType.GetProperty(propertyInfo.Name)?.GetValue(value);

            propertyInfo.SetValue(obj, ChangeTypeTo(v, propertyInfo.PropertyType), null);
        }

        return (T)obj;
    }
    /// <summary>
    /// 以最大的可能性返回一个指定类型的对象列表，该对象的值等效于指定的对象列表。
    /// </summary>
    /// <typeparam name="T">转化后的类型</typeparam>
    /// <param name="value">需要转化的对象</param>
    /// <returns>转化后的对象</returns>
    public static List<T> ChangeTypeTo<T>(this IEnumerable<object> value)
    {
        return value.Select(ChangeTypeTo<T>).ToList();
    }
}