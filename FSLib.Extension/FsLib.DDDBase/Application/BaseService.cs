using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FsLib.DDDBase.Domain;
using Microsoft.EntityFrameworkCore;

namespace FsLib.DDDBase.Application
{
    /// <summary>
    /// 业务基类
    /// </summary>
    /// <typeparam name="TAggregateRoot"></typeparam>
    public class BaseService<TAggregateRoot> : IBaseService<TAggregateRoot> where TAggregateRoot : class, IAggregateRoot
    {
        /// <summary>
        /// 生成分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页数据数</param>
        /// <param name="v">需要分页的数据</param>
        /// <returns></returns>
        public async Task<Page<T>> GeneratePage<T>(int pageIndex, int pageSize, IQueryable<T> v)
        {
            int totalRecord = v.ToList().Count;
            var list = await v.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            int pageCount = Convert.ToInt32(Math.Ceiling(totalRecord / Convert.ToDouble(pageSize)));
            int prevPage = pageIndex > 0 ? pageIndex - 1 : 0;
            int nextPage = pageIndex < pageCount ? pageIndex + 1 : 0;
            var pages = new Page<T>()
            {
                PageSize = pageSize,
                PageIndex = pageIndex,
                NextPage = nextPage,
                TotalRecord = totalRecord,
                PrevPage = prevPage,
                PageCount = pageCount,
                Content = list
            };
            return pages;
        }
    }
}