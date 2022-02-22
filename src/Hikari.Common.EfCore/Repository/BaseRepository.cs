using FsLib.EfCore.Domain;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq.Expressions;

namespace FsLib.EfCore.Repository
{
    /// <summary>
    /// 仓储基类
    /// </summary>
    /// <typeparam name="TAggregateRoot">类型</typeparam>
    public class BaseRepository<TAggregateRoot> : IBaseRepository<TAggregateRoot> where TAggregateRoot : class, IAggregateRoot
    {
        protected IDbContext _nContext;  // 连接上下文
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbContext"></param>
        public BaseRepository(IDbContext dbContext)
        {
            _nContext = dbContext;
        }

        #region 方法
        /// <summary>
        /// 查询记录数
        /// </summary>
        /// <param name="predicate">条件表达式</param>
        /// <returns>记录数</returns>
        public int Count(Expression<Func<TAggregateRoot, bool>> predicate)
        {
            return _nContext.Set<TAggregateRoot>().Count(predicate);
        }
        /// <summary>
        /// 查询记录数
        /// </summary>
        /// <param name="predicate">条件表达式</param>
        /// <returns>记录数</returns>
        public async Task<int> CountAsync(Expression<Func<TAggregateRoot, bool>> predicate)
        {
            return await _nContext.Set<TAggregateRoot>().CountAsync(predicate);
        }

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="predicate">查询表达式</param>
        /// <returns>布尔值</returns>
        public bool Any(Expression<Func<TAggregateRoot, bool>> predicate)
        {
            return _nContext.Set<TAggregateRoot>().Any(predicate);
        }
        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="predicate">查询表达式</param>
        /// <returns>布尔值</returns>
        public async Task<bool> AnyAsync(Expression<Func<TAggregateRoot, bool>> predicate)
        {
            return await _nContext.Set<TAggregateRoot>().AnyAsync(predicate);
        }
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="predicate">查询表达式</param>
        /// <returns>实体</returns>
        public TAggregateRoot? Find(Expression<Func<TAggregateRoot, bool>> predicate)
        {
            TAggregateRoot? entity = _nContext.Set<TAggregateRoot>().FirstOrDefault(predicate);
            return entity;
        }
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="predicate">查询表达式</param>
        /// <returns>实体</returns>
        public async Task<TAggregateRoot?> FindAsync(Expression<Func<TAggregateRoot, bool>> predicate)
        {
            TAggregateRoot? entity = await _nContext.Set<TAggregateRoot>().FirstOrDefaultAsync(predicate);
            return entity;
        }

        /// <summary>
        /// 查找数据列表
        /// </summary>
        /// <param name="predicate">查询表达式</param>
        /// <param name="orderName">排序名称</param>
        /// <param name="isAsc">是否升序</param>
        /// <returns></returns>
        public IQueryable<TAggregateRoot> FindList(Expression<Func<TAggregateRoot, bool>>? predicate = null, string orderName = "", bool isAsc = false)
        {
            IQueryable<TAggregateRoot> list = _nContext.Set<TAggregateRoot>();
            if (predicate != null)
            {
                list = list.Where(predicate);
            }

            if (!string.IsNullOrWhiteSpace(orderName))
            {
                list = OrderBy(list, orderName, isAsc);
            }

            return list;
        }
        /// <summary>
        /// 查找分页数据列表
        /// </summary>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="predicate">查询表达式</param>
        /// <param name="orderName">排序名称</param>
        /// <param name="isAsc">是否升序</param>
        /// <returns></returns>
        public IQueryable<TAggregateRoot> FindPageList(int pageIndex, int pageSize, Expression<Func<TAggregateRoot, bool>>? predicate = null, string orderName = "", bool isAsc = false)
        {
            var list = FindPageList(pageIndex, pageSize, out _, predicate, orderName, isAsc);
            return list;
        }
        /// <summary>
        /// 查找分页数据列表
        /// </summary>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="predicate">查询表达式</param>
        /// <param name="orderName">排序名称</param>
        /// <param name="isAsc">是否升序</param>
        /// <returns>数据,数据总数,总页数,上一页,下一页</returns>
        public (IQueryable<TAggregateRoot> list, int totalRecord, int pageCount, int prevPage, int nextPage) FindPageList2(int pageIndex, int pageSize,
            Expression<Func<TAggregateRoot, bool>>? predicate = null, string orderName = "", bool isAsc = false)
        {
            var list = FindPageList(pageIndex, pageSize, out var totalRecord, predicate, orderName, isAsc);
            int pageCount = Convert.ToInt32(Math.Ceiling(totalRecord / Convert.ToDouble(pageSize)));
            int prevPage = pageIndex > 0 ? pageIndex - 1 : 0;
            int nextPage = pageIndex < pageCount ? pageIndex + 1 : 0;
            return (list, totalRecord, pageCount, prevPage, nextPage);
        }

        #endregion

        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="source">原IQueryable</param>
        /// <param name="propertyName">排序属性名</param>
        /// <param name="isAsc">是否正序</param>
        /// <returns>排序后的IQueryable</returns>
        protected IQueryable<TAggregateRoot> OrderBy(IQueryable<TAggregateRoot> source, string propertyName, bool isAsc)
        {
            if (string.IsNullOrWhiteSpace(propertyName)) return source;
            var parameter = Expression.Parameter(source.ElementType);
            var property = Expression.Property(parameter, propertyName);
            var lambda = Expression.Lambda(property, parameter);
            var methodName = isAsc ? "OrderBy" : "OrderByDescending";
            var resultExpression = Expression.Call(typeof(Queryable), methodName, new [] { source.ElementType, property.Type }, source.Expression, Expression.Quote(lambda));
            return source.Provider.CreateQuery<TAggregateRoot>(resultExpression);
        }

        /// <summary>
        /// 查找分页数据列表
        /// </summary>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="totalRecord">总记录数</param>
        /// <param name="predicate">查询表达式</param>
        /// <param name="orderName">排序名称</param>
        /// <param name="isAsc">是否升序</param>
        /// <returns></returns>
        private IQueryable<TAggregateRoot> FindPageList(int pageIndex, int pageSize, out int totalRecord, Expression<Func<TAggregateRoot, bool>>? predicate, string orderName = "", bool isAsc = false)
        {
            IQueryable<TAggregateRoot> list = _nContext.Set<TAggregateRoot>();
            if (predicate != null)
            {
                list = list.Where(predicate);
            }
            totalRecord = list.Count();
            if (!string.IsNullOrWhiteSpace(orderName))
            {
                list = OrderBy(list, orderName, isAsc);
            }
            list = list.Skip((pageIndex - 1) * pageSize).Take(pageSize);

            return list;
        }
        /// <summary>
        /// 执行查询，并以DataTable返回结果集
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>DataTable</returns>
        public DataTable ExecuteDataTable(string sql, params object[] parameters)
        {
            DataTable dt = SqlQueryAsync(sql, CommandType.Text, parameters).GetAwaiter().GetResult();
            return dt;
        }
        /// <summary>
        /// 执行查询，并以DataTable返回结果集
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>DataTable</returns>
        public async Task<DataTable> ExecuteDataTableAsync(string sql, params object[] parameters)
        {
            DataTable dt = await SqlQueryAsync(sql, CommandType.Text, parameters);
            return dt;
        }

        #region 私有方法

        //public IList<T> SqlQuery<T>(string sql, params object[] parameters) where T : new()
        //{
        //    //注意：不要对GetDbConnection获取到的conn进行using或者调用Dispose，否则DbContext后续不能再进行使用了，会抛异常
        //    var conn = NContext.Database.GetDbConnection();
        //    try
        //    {
        //        conn.Open();
        //        using (var command = conn.CreateCommand())
        //        {
        //            command.CommandText = sql;
        //            command.Parameters.AddRange(parameters);
        //            var propts = typeof(T).GetProperties();
        //            var rtnList = new List<T>();
        //            T model;
        //            object val;
        //            using (var reader = command.ExecuteReader())
        //            {
        //                while (reader.Read())
        //                {
        //                    model = new T();
        //                    foreach (var l in propts)
        //                    {
        //                        val = reader[l.Name];
        //                        if (val == DBNull.Value)
        //                        {
        //                            l.SetValue(model, null);
        //                        }
        //                        else
        //                        {
        //                            l.SetValue(model, val);
        //                        }
        //                    }
        //                    rtnList.Add(model);
        //                }
        //            }
        //            return rtnList;
        //        }
        //    }
        //    finally
        //    {
        //        conn.Close();
        //    }
        //}
        /// <summary>
        /// sql查询
        /// </summary>
        /// <param name="sql">sql</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        private async Task<DataTable> SqlQueryAsync(string sql, CommandType cmdType, params object[] parameters)
        {
            //注意：不要对GetDbConnection获取到的conn进行using或者调用Dispose，否则DbContext后续不能再进行使用了，会抛异常
            await using var conn = _nContext.Database.GetDbConnection();
            await using var command = conn.CreateCommand();
            command.CommandType = cmdType;
            command.CommandText = sql;
            command.Parameters.AddRange(parameters);

            DataTable objDataTable = new DataTable("Table");

            await using var reader = await command.ExecuteReaderAsync();
            int intFieldCount = reader.FieldCount;
            for (int intCounter = 0; intCounter < intFieldCount; ++intCounter)
            {
                objDataTable.Columns.Add(reader.GetName(intCounter), reader.GetFieldType(intCounter));
            }

            objDataTable.BeginLoadData();
            object[] objValues = new object[intFieldCount];
            while (await reader.ReadAsync())
            {
                reader.GetValues(objValues);
                objDataTable.LoadDataRow(objValues, true);
            }
            objDataTable.EndLoadData();
            return objDataTable;
        }
        #endregion

    }
}