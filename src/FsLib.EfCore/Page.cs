using System.Collections.Generic;

namespace FsLib.EfCore
{
    /// <summary>
    /// 分页
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Page<T>
    {
        /// <summary>
        /// 当前页
        /// </summary>
        public int PageIndex { get; set; }
        /// <summary>
        /// 每页数量
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// 数据总数
        /// </summary>
        public int TotalRecord { get; set; }
        /// <summary>
        /// 总页数
        /// </summary>
        public int PageCount { get; set; }
        /// <summary>
        /// 上一页
        /// </summary>
        public int PrevPage { get; set; }
        /// <summary>
        /// 下一页
        /// </summary>
        public int NextPage { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public List<T> Content { get; set; }
    }
}