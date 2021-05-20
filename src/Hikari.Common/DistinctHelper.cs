using System;
using System.Collections.Generic;
using System.Linq;

/******************************************************************************************************************
 * 
 * 
 * 标  题： Distinct 扩展类(版本：Version1.0.0)
 * 作  者： YuXiaoWei
 * 日  期： 2016/11/18
 * 修  改：
 * 参  考： http://www.cnblogs.com/ldp615/archive/2011/08/01/distinct-entension.html#improve
 * 说  明： 暂无...
 * 备  注： 暂无...
 * 调用示列：
 *
 * 
 * ***************************************************************************************************************/
namespace Hikari.Common
{
    /// <summary>
    /// Distinct 扩展类
    /// </summary>
    public static class DistinctExtensions
    {
        /// <summary>
        /// Distinct lambda 扩展，可以使用一个简单的 lambda 作为参数
        /// 调用示例：var p1 = products.Distinct(p => p.ID);
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TV"></typeparam>
        /// <param name="source"></param>
        /// <param name="keySelector">lambda表达式</param>
        /// <returns></returns>
        public static IEnumerable<T> Distinct<T, TV>(this IEnumerable<T> source, Func<T, TV> keySelector)
        {
            return source.Distinct(new CommonEqualityComparer<T, TV>(keySelector));
        }
        /// <summary>
        /// Distinct lambda 扩展，可以使用一个简单的 lambda 作为参数
        /// 调用示例：var p1 = products.Distinct(p => p.ID, StringComparer.CurrentCultureIgnoreCase);
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TV"></typeparam>
        /// <param name="source"></param>
        /// <param name="keySelector">lambda表达式</param>
        /// <param name="comparer"></param>
        /// <returns></returns>
        public static IEnumerable<T> Distinct<T, TV>(this IEnumerable<T> source, Func<T, TV> keySelector, IEqualityComparer<TV> comparer)
        {
            return source.Distinct(new CommonEqualityComparer<T, TV>(keySelector, comparer));
        }
    }

    public class CommonEqualityComparer<T, TV> : IEqualityComparer<T>
    {
        private readonly Func<T, TV> _keySelector;
        private readonly IEqualityComparer<TV> _comparer;

        public CommonEqualityComparer(Func<T, TV> keySelector, IEqualityComparer<TV> comparer)
        {
            this._keySelector = keySelector;
            this._comparer = comparer;
        }

        public CommonEqualityComparer(Func<T, TV> keySelector)
            : this(keySelector, EqualityComparer<TV>.Default)
        { }

        public bool Equals(T x, T y)
        {
            return _comparer.Equals(_keySelector(x), _keySelector(y));
        }

        public int GetHashCode(T obj)
        {
            return _comparer.GetHashCode(_keySelector(obj));
        }
    }
}