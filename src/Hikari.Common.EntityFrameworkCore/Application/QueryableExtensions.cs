using Microsoft.EntityFrameworkCore;

namespace Hikari.Common.EntityFrameworkCore.Application;
/// <summary>
/// EF Core 查询分页扩展
/// </summary>
[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never), System.ComponentModel.Browsable(false)]
public static class QueryableExtensions
{
    /// <summary>
    /// 生成分页
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="pageIndex">当前页码</param>
    /// <param name="pageSize">每页数据数</param>
    /// <param name="v">需要分页的数据</param>
    /// <returns></returns>
    public static async Task<Paginated<T>> GeneratePageAsync<T>(this IQueryable<T> v, int pageIndex, int pageSize)
    {
        int totalRecord = await v.CountAsync();
        var list = await v.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
        int pageCount = Convert.ToInt32(Math.Ceiling(totalRecord / Convert.ToDouble(pageSize)));
        int prevPage = pageIndex > 0 ? pageIndex - 1 : 0;
        int nextPage = pageIndex < pageCount ? pageIndex + 1 : 0;
        var pages = new Paginated<T>()
        {
            PageSize = pageSize,
            PageIndex = pageIndex,
            NextPage = nextPage,
            TotalRecord = totalRecord,
            PrevPage = prevPage,
            PageCount = pageCount,
            Data = list
        };
        return pages;
    }
    /// <summary>
    /// 生成分页
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="pageIndex">当前页码</param>
    /// <param name="pageSize">每页数据数</param>
    /// <param name="v">需要分页的数据</param>
    /// <returns></returns>
    public static Paginated<T> GeneratePage<T>(this IQueryable<T> v, int pageIndex, int pageSize)
    {
        int totalRecord = v.Count();
        var list = v.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        int pageCount = Convert.ToInt32(Math.Ceiling(totalRecord / Convert.ToDouble(pageSize)));
        int prevPage = pageIndex > 0 ? pageIndex - 1 : 0;
        int nextPage = pageIndex < pageCount ? pageIndex + 1 : 0;
        var pages = new Paginated<T>()
        {
            PageSize = pageSize,
            PageIndex = pageIndex,
            NextPage = nextPage,
            TotalRecord = totalRecord,
            PrevPage = prevPage,
            PageCount = pageCount,
            Data = list
        };
        return pages;
    }
    /// <summary>
    /// 生成分页
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="pageIndex">当前页码</param>
    /// <param name="pageSize">每页数据数</param>
    /// <param name="v">需要分页的数据</param>
    /// <returns></returns>
    public static async Task<Paginated<T>> GeneratePageAsync<T>(this IAsyncQueryable<T> v, int pageIndex, int pageSize)
    {
        int totalRecord = await v.CountAsync();
        var list = await v.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
        int pageCount = Convert.ToInt32(Math.Ceiling(totalRecord / Convert.ToDouble(pageSize)));
        int prevPage = pageIndex > 0 ? pageIndex - 1 : 0;
        int nextPage = pageIndex < pageCount ? pageIndex + 1 : 0;
        var pages = new Paginated<T>()
        {
            PageSize = pageSize,
            PageIndex = pageIndex,
            NextPage = nextPage,
            TotalRecord = totalRecord,
            PrevPage = prevPage,
            PageCount = pageCount,
            Data = list
        };
        return pages;
    }
}