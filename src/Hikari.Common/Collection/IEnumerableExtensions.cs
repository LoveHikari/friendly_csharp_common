﻿using System;
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
        public static string Join(this IEnumerable<string> strs, string separate = ", ", bool removeEmptyEntry = false)
        {
            return string.Join(separate, removeEmptyEntry ? strs.Where(s => !string.IsNullOrEmpty(s)) : strs);
        }

        public static string Join<T>(this IEnumerable<T> strs, string separate = ", ", bool removeEmptyEntry = false)
        {
            return string.Join(separate, removeEmptyEntry ? strs.Where(t => t != null) : strs);
        }


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
        public static List<T>? ToList<T>(this IEnumerable<dynamic> source) where T : new()
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
                        object? value = (object?)type.GetProperty(pro.Name)?.GetValue(o, null);

                        pro.SetValue(t, value.ChangeTypeTo(pro.PropertyType), null);


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
        /// 合并多个 Dictionary
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dictionaries"></param>
        /// <returns></returns>
        public static Dictionary<TKey, TValue> Merge<TKey, TValue>(IEnumerable<Dictionary<TKey, TValue>> dictionaries)
        {
            //var result = new Dictionary<TKey, TValue>();
            //foreach (var dict in dictionaries)
            //    foreach (var x in dict)
            //        result[x.Key] = x.Value;
            //return result;
            var result = dictionaries.SelectMany(dict => dict)
                         .ToLookup(pair => pair.Key, pair => pair.Value)
                         .ToDictionary(group => group.Key, group => group.First());
            return result;
        }
        /// <summary>
        /// List转化成树结构
        /// </summary>
        /// <typeparam name="T1">List的对象类型</typeparam>
        /// <typeparam name="T2">返回的树的对象类型</typeparam>
        /// <param name="menuList">菜单的平级list</param>
        /// <param name="idField">指定id字段，默认为id</param>
        /// <param name="parentIdField">指定父节点字段，默认为ParentId</param>
        /// <param name="childrenField">指定子项列表字段，默认为Children</param>
        /// <returns>树结构</returns>
        public static List<T2> ToTreeList<T1, T2>(this List<T1> menuList, string idField = "Id", string parentIdField = "ParentId", string childrenField = "Children") where T1 : class, new() where T2 : class, new()
        {
            var dic = new Dictionary<object, T2>(menuList.Count);
            foreach (var chapter in menuList)
            {

                dic.Add(chapter.GetValue(idField), chapter.ChangeTypeTo<T2>());
            }
            var ids = new List<string>();
            foreach (var chapter in dic.Values)
            {
                var parentId = chapter.GetValue(parentIdField);

                if (parentId != null && dic.ContainsKey(parentId))
                {
                    if (dic[parentId].GetValue(childrenField) == null)
                    {
                        dic[parentId].GetType().GetProperty(childrenField).SetValue(dic[parentId], new List<T2>());
                    }

                    ((List<T2>)dic[parentId].GetValue(childrenField))?.Add(chapter);
                    ids.Add(chapter.GetValue(idField).ToString());
                }
            }
            return dic.Values.Where(t => !ids.Contains(t.GetValue(idField).ToString())).ToList();
        }

        /// <summary>
        /// 平铺树结构到list
        /// </summary>
        /// <typeparam name="T">List的对象类型</typeparam>
        /// <param name="treeList"></param>
        /// <param name="childrenField">指定子项列表字段，默认为Children</param>
        /// <returns></returns>
        public static List<T> TileTreeList<T>(this List<T> treeList, string childrenField = "Children")
        {
            List<T> list = new();

            Action<List<T>>? func = null;
            func = list1 =>
            {
                foreach (var item in list1)
                {
                    if (item != null)
                    {
                        list.Add(item);
                        var children = (List<T>?)item.GetValue(childrenField);
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
        /// 获取该树形结构的最后一级的所有节点
        /// </summary>
        /// <typeparam name="T">List的对象类型</typeparam>
        /// <param name="tree"></param>
        /// <param name="childrenField">指定子项列表字段，默认为Children</param>
        /// <returns></returns>
        public static List<T> GetAllNodesAtLastLevel<T>(this List<T> tree, string childrenField = "Children") where T : class, new()
        {
            var nodes = new List<T>();
            void Traverse(T node, int level)
            {
                var children = node.GetValue(childrenField) != null ? (List<T>?)node.GetValue(childrenField) : null;
                if (children == null || !children.Any())
                {
                    nodes.Add(node);
                    return;
                }
                foreach (var child in children)
                {
                    Traverse(child, level + 1);
                }
            }
            var temp = new Dictionary<string, object>()
            {
                {childrenField, tree}
            };

            Traverse(temp.ToEntity<T>(), 0);

            return nodes;
        }
        /// <summary>
        /// 获取某一层的所有数据
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="tree">树形列表</param>
        /// <param name="level">某一层，从0开始</param>
        /// <param name="childrenField">指定子项列表字段，默认为Children</param>
        /// <returns></returns>
        public static List<T> GetNodesAtLevel<T>(this List<T> tree, int level, string childrenField = "Children") where T : class, new()
        {
            level++;
            var nodes = new List<T>();

            void Traverse(T node, int currentLevel)
            {
                if (currentLevel == level)
                {
                    nodes.Add(node);
                }
                else
                {
                    var childrenProperty = node.GetType().GetProperty(childrenField);
                    if (childrenProperty != null && childrenProperty.GetValue(node) is IEnumerable<T> children)
                    {
                        foreach (var child in children)
                        {
                            Traverse(child, currentLevel + 1);
                        }
                    }

                }
            }

            var temp = new Dictionary<string, object>()
            {
                { childrenField, tree }
            };
            Traverse(temp.ToEntity<T>(), 0);
            return nodes;
        }
        /// <summary>
        /// 向上筛选树结构, 返回包含的树
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="tree">树形列表</param>
        /// <param name="func"></param>
        /// <param name="childrenField">指定子项列表字段，默认为Children</param>
        /// <returns></returns>
        public static List<T> TreeFilter<T>(this List<T> tree, Func<T, bool> func, string childrenField = "Children")
        {
            T CopyNode(T node)
            {
                // Assuming T is a class type
                var copy = Activator.CreateInstance<T>();
                var properties = typeof(T).GetProperties();
                foreach (var property in properties)
                {
                    var value = property.GetValue(node);
                    property.SetValue(copy, value);
                }
                return copy;
            }

            return tree
                .Select(CopyNode)
                .Where(node =>
                {
                    var childrenProperty = node.GetType().GetProperty(childrenField);
                    List<T>? children = childrenProperty.GetValue(node) as List<T>;

                    children = children != null ? TreeFilter(children, func) : null;
                    return func(node) || children is { Count: > 0 };
                })
                .ToList();
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