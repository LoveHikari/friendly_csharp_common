using System;
using System.Collections;
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

        /// <summary>
        /// 转化成树结构
        /// </summary>
        /// <param name="menuList">菜单的平级list</param>
        /// <param name="idField">指定id字段，默认为id</param>
        /// <param name="parentIdField">指定父节点字段，默认为ParentId</param>
        /// <param name="childrenField">指定子项列表字段，默认为Children</param>
        /// <returns>树结构</returns>
        public static IEnumerable<IDictionary<string, object?>> ToTreeList(this IEnumerable<IDictionary<string, object?>> menuList, string idField = "Id", string parentIdField = "ParentId", string childrenField = "Children")
        {
            var dic = new Dictionary<object, IDictionary<string, object?>>();
            foreach (var chapter in menuList)
            {
                if (chapter[idField] != null)
                {
                    dic.Add(chapter[idField], chapter);
                }
               
            }
            var ids = new List<string>();
            foreach (var chapter in dic.Values)
            {
                var parentId = chapter[parentIdField];

                if (parentId != null && dic.ContainsKey(parentId))
                {
                    if (!dic[parentId].ContainsKey(childrenField))
                    {
                        dic[parentId].Add(childrenField, new List<IDictionary<string, object?>>());
                    }

                    dic[parentId][childrenField] ??= new List<IDictionary<string, object?>>();

                    ((List<IDictionary<string, object?>>)dic[parentId][childrenField]).Add(chapter);
                    ids.Add(chapter[idField].ToString());
                }
            }

            var dicList = dic.Values.Where(t => !ids.Contains(t[idField].ToString()));

            return dicList;
        }
        /// <summary>
        /// 平铺树结构到list
        /// </summary>
        /// <param name="treeList">树形列表</param>
        /// <param name="childrenField">指定子项列表字段，默认为Children</param>
        /// <returns>平级list</returns>
        public static IEnumerable<IDictionary<string, object>> TileTreeList(this IEnumerable<IDictionary<string, object?>> treeList, string childrenField = "Children")
        {
            List<IDictionary<string, object?>> list = new();

            Action<IEnumerable<IDictionary<string, object?>>>? func = null;
            func = list1 =>
            {
                foreach (var item in list1)
                {
                    list.Add(item);
                    if (item.ContainsKey(childrenField))
                    {
                        var children = (List<IDictionary<string, object?>>?)item[childrenField];
                        if (children != null)
                        {
                            func?.Invoke(children);
                        }
                    }

                }
            };
            func(treeList);

            return list;

        }

        /// <summary>
        /// <see cref="IDictionary"/> 转换为实体类
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="nvc"><see cref="IDictionary"/> 对象</param>
        /// <returns>实体类</returns>
        public static T? ToEntity<T>(this IDictionary<string, object?> nvc) where T : class, new()
        {
            T t = new T();
            // 获得此模型的公共属性 
            PropertyInfo[] propertys = t.GetType().GetProperties();

            //遍历该对象的所有属性 
            foreach (PropertyInfo pi in propertys)
            {
                string tempName = pi.Name; //将属性名称赋值给临时变量
                //检查DataTable是否包含此列
                if (nvc.ContainsKey(tempName))
                {
                    //取值
                    object? value = nvc[tempName];
                    //如果非空，则赋给对象的属性
                    pi.SetValue(t, ConvertHelper.ChangeType(value, pi.PropertyType), null);


                }

            }

            return t;
        }
    }
    



}