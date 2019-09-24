using System;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FsLib.DDDBase.Domain;

namespace FsLib.DDDBase.Repository
{
    /// <summary>
    /// 仓储接口基类
    /// </summary>
    /// <typeparam name="TAggregateRoot">类型</typeparam>
    public interface IBaseRepository<TAggregateRoot> where TAggregateRoot : class, IAggregateRoot
    {
        /// <summary>
        /// 查询记录数
        /// </summary>
        /// <param name="predicate">条件表达式</param>
        /// <returns>记录数</returns>
        int Count(Expression<Func<TAggregateRoot, bool>> predicate);
        /// <summary>
        /// 查询记录数
        /// </summary>
        /// <param name="predicate">条件表达式</param>
        /// <returns>记录数</returns>
        Task<int> CountAsync(Expression<Func<TAggregateRoot, bool>> predicate);
        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="anyLambda">查询表达式</param>
        /// <returns>布尔值</returns>
        bool Exist(Expression<Func<TAggregateRoot, bool>> anyLambda);
        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="anyLambda">查询表达式</param>
        /// <returns>布尔值</returns>
        Task<bool> ExistAsync(Expression<Func<TAggregateRoot, bool>> anyLambda);
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="whereLambda">查询表达式</param>
        /// <returns>实体</returns>
        TAggregateRoot Find(Expression<Func<TAggregateRoot, bool>> whereLambda);
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="whereLambda">查询表达式</param>
        /// <returns>实体</returns>
        Task<TAggregateRoot> FindAsync(Expression<Func<TAggregateRoot, bool>> whereLambda);
        /// <summary>
        /// 查找数据列表
        /// </summary>
        /// <param name="whereLamdba">查询表达式</param>
        /// <param name="orderName">排序名称</param>
        /// <param name="isAsc">是否升序</param>
        /// <returns></returns>
        IQueryable<TAggregateRoot> FindList(Expression<Func<TAggregateRoot, bool>> whereLamdba = null, string orderName = "", bool isAsc = false);
        /// <summary>
        /// 查找分页数据列表
        /// </summary>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="whereLamdba">查询表达式</param>
        /// <param name="orderName">排序名称</param>
        /// <param name="isAsc">是否升序</param>
        /// <returns></returns>
        IQueryable<TAggregateRoot> FindPageList(int pageIndex, int pageSize, Expression<Func<TAggregateRoot, bool>> whereLamdba = null, string orderName = "", bool isAsc = false);
        /// <summary>
        /// 查找分页数据列表
        /// </summary>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="whereLamdba">查询表达式</param>
        /// <param name="orderName">排序名称</param>
        /// <param name="isAsc">是否升序</param>
        /// <returns>数据,数据总数,总页数,上一页,下一页</returns>
        (IQueryable<TAggregateRoot> list, int totalRecord, int pageCount, int prevPage, int nextPage) FindPageList2(int pageIndex, int pageSize,
            Expression<Func<TAggregateRoot, bool>> whereLamdba = null, string orderName = "", bool isAsc = false);

        /// <summary>
        /// 执行非查询语句,并返回受影响的记录行数
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>受影响记录行数</returns>
        int ExecuteNonQuery(string sql, params object[] parameters);
        /// <summary>
        /// 执行非查询语句,并返回受影响的记录行数
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>受影响记录行数</returns>
        Task<int> ExecuteNonQueryAsync(string sql, params object[] parameters);
        /// <summary>
        /// 执行查询，并以DataTable返回结果集
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>DataTable</returns>
        DataTable ExecuteDataTable(string sql, params object[] parameters);
        /// <summary>
        /// 执行查询，并以DataTable返回结果集
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>DataTable</returns>
        Task<DataTable> ExecuteDataTableAsync(string sql, params object[] parameters);
        /// <summary>
        /// 执行非查询语句,并返回首行首列的值
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>Object</returns>
        object ExecuteScalar(string sql, params object[] parameters);
        /// <summary>
        /// 执行非查询语句,并返回首行首列的值
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>Object</returns>
        Task<object> ExecuteScalarAsync(string sql, params object[] parameters);
    }
}