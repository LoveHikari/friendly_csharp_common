using FsLib.EfCore.Domain;
using Hikari.Common;

namespace FsLib.EfCore.Application
{
    /// <summary>
    /// 业务接口基类
    /// </summary>
    /// <typeparam name="TAggregateRoot">类型</typeparam>
    public interface IBaseService<TAggregateRoot> where TAggregateRoot : class, IAggregateRoot
    {
        /// <summary>
        /// 生成分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页数据数</param>
        /// <param name="v">需要分页的数据</param>
        /// <returns></returns>
        Task<Pager<T>> GeneratePageAsync<T>(int pageIndex, int pageSize, IQueryable<T> v);

        /// <summary>
        /// 生成分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页数据数</param>
        /// <param name="v">需要分页的数据</param>
        /// <returns></returns>
        Task<Pager<T>> GeneratePageAsync<T>(int pageIndex, int pageSize, IAsyncQueryable<T> v);
    }
}