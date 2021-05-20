using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Hikari.Common.Collection
{
    /// <summary>
    /// <see cref="IEnumerable{T}"/> 扩展类
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never), System.ComponentModel.Browsable(false)]
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// 确认序列中是否包含指定元素
        /// </summary>
        /// <param name="source">需要确认的序列</param>
        /// <param name="value">要寻找的值</param>
        /// <param name="sc">是否忽略大小写</param>
        /// <returns></returns>
        public static bool Contains(this IEnumerable<string> source, string value, StringComparison sc)
        {
            if (sc == StringComparison.CurrentCultureIgnoreCase || sc == StringComparison.InvariantCultureIgnoreCase || sc == StringComparison.OrdinalIgnoreCase)
            {
                CompareOnly c = new CompareOnly();
                return source.Contains(value, c);
            }
            else
            {
                return source.Contains(value);
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static List<T> ToList<T>(this IEnumerable<dynamic> source) where T : new()
        {
            var list = source.ToList();
            if (!list.Any())
            {
                return null;
            }
            else
            {
                List<T> ls = new List<T>();

                foreach (var o in list)
                {
                    Type type = o.GetType();

                    T t = new T();
                    Type ty = t.GetType();
                    var pros = ty.GetProperties();
                    foreach (PropertyInfo pro in pros)
                    {
                        object value = (object)type.GetProperty(pro.Name).GetValue(o, null);

                        pro.SetValue(t, ConvertHelper.ChangeType(value, pro.PropertyType), null);


                    }

                    ls.Add(t);
                }
                return ls;
            }
        }
        /// <summary>
        /// 转换成为Dynamic对象
        /// </summary>
        /// <param name="dict"></param>
        /// <returns></returns>
        public static dynamic ToDynamic(this IEnumerable<KeyValuePair<string, object>> dict)
        {
            dynamic result = new System.Dynamic.ExpandoObject();

            foreach (var entry in dict)
            {
                (result as ICollection<KeyValuePair<string, object>>).Add(new KeyValuePair<string, object>(entry.Key, entry.Value));
            }

            return result;
        }


        /// <summary>
        /// 比较器
        /// </summary>
        class CompareOnly : IEqualityComparer<string>
        {
            public bool Equals(string x, string y)
            {
                return x.ToUpper() == y.ToUpper();
            }

            public int GetHashCode(string obj)
            {
                return obj.GetHashCode();
            }
        }
    }
}