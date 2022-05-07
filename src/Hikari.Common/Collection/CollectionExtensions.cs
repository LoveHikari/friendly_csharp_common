using System;
using System.Collections.Specialized;
using System.Reflection;

namespace Hikari.Common.Collection
{
    /// <summary>
    /// <see cref="NameValueCollection"/> 扩展类
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never), System.ComponentModel.Browsable(false)]
    public static class CollectionExtensions
    {
        /// <summary>
        /// <see cref="NameValueCollection"/> 转换为实体类
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="nvc"><see cref="NameValueCollection"/> 对象</param>
        /// <param name="sc">默认忽略大小写</param>
        /// <returns>实体类</returns>
        public static T? ToEntity<T>(this NameValueCollection nvc, StringComparison sc = StringComparison.OrdinalIgnoreCase) where T : class, new()
        {
            T t = new T();
            // 获得此模型的公共属性 
            PropertyInfo[] propertys = t.GetType().GetProperties();

            //遍历该对象的所有属性 
            foreach (PropertyInfo pi in propertys)
            {
                string tempName = pi.Name; //将属性名称赋值给临时变量
                //检查DataTable是否包含此列（列名==对象的属性名）
                if (nvc.AllKeys!.Contains(tempName, sc))
                {
                    //取值
                    object? value = nvc[tempName];
                    //如果非空，则赋给对象的属性
                    pi.SetValue(t, ConvertHelper.ChangeType(value, pi.PropertyType), null);


                }

            }

            return t;
        }
        /// <summary>
        /// 实体类转Dictionary
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static IDictionary<string, object?> ToDictionary(this object obj)
        {
            IDictionary<string, object?> dic = new Dictionary<string, object?>();
            PropertyInfo[] propertys = obj.GetType().GetProperties();
            foreach (var pi in propertys)
            {
                string name = pi.Name;
                object? value = pi.GetValue(obj, null);
                dic[name] = value;
            }
            return dic;
        }
    }
    



}