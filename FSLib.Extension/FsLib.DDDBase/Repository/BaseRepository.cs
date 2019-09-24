using System;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FsLib.DDDBase.Domain;
using Microsoft.EntityFrameworkCore;

namespace FsLib.DDDBase.Repository
{
    /// <summary>
    /// 仓储基类
    /// </summary>
    /// <typeparam name="TAggregateRoot">类型</typeparam>
    public class BaseRepository<TAggregateRoot> : IBaseRepository<TAggregateRoot> where TAggregateRoot : class, IAggregateRoot
    {
        protected IDbContext NContext;

        public BaseRepository(IDbContext dbContext)
        {
            NContext = dbContext;
        }

        #region 方法
        /// <summary>
        /// 查询记录数
        /// </summary>
        /// <param name="predicate">条件表达式</param>
        /// <returns>记录数</returns>
        public int Count(Expression<Func<TAggregateRoot, bool>> predicate)
        {
            return NContext.Set<TAggregateRoot>().Count(predicate);
        }
        /// <summary>
        /// 查询记录数
        /// </summary>
        /// <param name="predicate">条件表达式</param>
        /// <returns>记录数</returns>
        public async Task<int> CountAsync(Expression<Func<TAggregateRoot, bool>> predicate)
        {
            return await NContext.Set<TAggregateRoot>().CountAsync(predicate);
        }

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="anyLambda">查询表达式</param>
        /// <returns>布尔值</returns>
        public bool Exist(Expression<Func<TAggregateRoot, bool>> anyLambda)
        {
            return NContext.Set<TAggregateRoot>().Any(anyLambda);
        }
        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="anyLambda">查询表达式</param>
        /// <returns>布尔值</returns>
        public async Task<bool> ExistAsync(Expression<Func<TAggregateRoot, bool>> anyLambda)
        {
            return await NContext.Set<TAggregateRoot>().AnyAsync(anyLambda);
        }
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="whereLambda">查询表达式</param>
        /// <returns>实体</returns>
        public TAggregateRoot Find(Expression<Func<TAggregateRoot, bool>> whereLambda)
        {
            TAggregateRoot entity = NContext.Set<TAggregateRoot>().FirstOrDefault<TAggregateRoot>(whereLambda);
            return entity;
        }
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="whereLambda">查询表达式</param>
        /// <returns>实体</returns>
        public async Task<TAggregateRoot> FindAsync(Expression<Func<TAggregateRoot, bool>> whereLambda)
        {
            TAggregateRoot entity = await NContext.Set<TAggregateRoot>().FirstOrDefaultAsync<TAggregateRoot>(whereLambda);
            return entity;
        }

        /// <summary>
        /// 查找数据列表
        /// </summary>
        /// <param name="whereLamdba">查询表达式</param>
        /// <param name="orderName">排序名称</param>
        /// <param name="isAsc">是否升序</param>
        /// <returns></returns>
        public IQueryable<TAggregateRoot> FindList(Expression<Func<TAggregateRoot, bool>> whereLamdba = null, string orderName = "", bool isAsc = false)
        {
            IQueryable<TAggregateRoot> list = NContext.Set<TAggregateRoot>();
            if (whereLamdba != null)
            {
                list = list.Where(whereLamdba);
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
        /// <param name="whereLamdba">查询表达式</param>
        /// <param name="orderName">排序名称</param>
        /// <param name="isAsc">是否升序</param>
        /// <returns></returns>
        public IQueryable<TAggregateRoot> FindPageList(int pageIndex, int pageSize, Expression<Func<TAggregateRoot, bool>> whereLamdba = null, string orderName = "", bool isAsc = false)
        {
            var list = FindPageList(pageIndex, pageSize, out _, whereLamdba, orderName, isAsc);
            return list;
        }
        /// <summary>
        /// 查找分页数据列表
        /// </summary>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="whereLamdba">查询表达式</param>
        /// <param name="orderName">排序名称</param>
        /// <param name="isAsc">是否升序</param>
        /// <returns>数据,数据总数,总页数,上一页,下一页</returns>
        public (IQueryable<TAggregateRoot> list, int totalRecord, int pageCount, int prevPage, int nextPage) FindPageList2(int pageIndex, int pageSize,
            Expression<Func<TAggregateRoot, bool>> whereLamdba = null, string orderName = "", bool isAsc = false)
        {
            int totalRecord;
            var list = FindPageList(pageIndex, pageSize, out totalRecord, whereLamdba, orderName, isAsc);
            int pageCount = Convert.ToInt32(Math.Ceiling(totalRecord / Convert.ToDouble(pageSize)));
            int prevPage = pageIndex > 0 ? pageIndex - 1 : 0;
            int nextPage = pageIndex < pageCount ? pageIndex + 1 : 0;
            return (list, totalRecord, pageCount, prevPage, nextPage);
        }

        #endregion

        /// <summary>
        /// 排序
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="source">原IQueryable</param>
        /// <param name="propertyName">排序属性名</param>
        /// <param name="isAsc">是否正序</param>
        /// <returns>排序后的IQueryable<T></returns>
        protected IQueryable<TAggregateRoot> OrderBy(IQueryable<TAggregateRoot> source, string propertyName, bool isAsc)
        {
            if (source == null) throw new ArgumentNullException("source", "不能为空");
            if (string.IsNullOrEmpty(propertyName)) return source;
            var parameter = Expression.Parameter(source.ElementType);
            var property = Expression.Property(parameter, propertyName);
            if (property == null) throw new ArgumentNullException("propertyName", "属性不存在");
            var lambda = Expression.Lambda(property, parameter);
            var methodName = isAsc ? "OrderBy" : "OrderByDescending";
            var resultExpression = Expression.Call(typeof(Queryable), methodName, new Type[] { source.ElementType, property.Type }, source.Expression, Expression.Quote(lambda));
            return source.Provider.CreateQuery<TAggregateRoot>(resultExpression);
        }

        /// <summary>
        /// 查找分页数据列表
        /// </summary>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="totalRecord">总记录数</param>
        /// <param name="whereLamdba">查询表达式</param>
        /// <param name="orderName">排序名称</param>
        /// <param name="isAsc">是否升序</param>
        /// <returns></returns>
        private IQueryable<TAggregateRoot> FindPageList(int pageIndex, int pageSize, out int totalRecord, Expression<Func<TAggregateRoot, bool>> whereLamdba, string orderName = "", bool isAsc = false)
        {
            IQueryable<TAggregateRoot> list = NContext.Set<TAggregateRoot>();
            if (whereLamdba != null)
            {
                list = list.Where<TAggregateRoot>(whereLamdba);
            }
            totalRecord = list.Count();
            if (!string.IsNullOrWhiteSpace(orderName))
            {
                list = OrderBy(list, orderName, isAsc);
            }
            list = list.Skip<TAggregateRoot>((pageIndex - 1) * pageSize).Take<TAggregateRoot>(pageSize);

            return list;
        }
        /// <summary>
        /// 执行非查询语句,并返回受影响的记录行数
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>受影响记录行数</returns>
        public int ExecuteNonQuery(string sql, params object[] parameters)
        {
            return NContext.Database.ExecuteSqlRaw(sql, parameters);
        }
        /// <summary>
        /// 执行非查询语句,并返回受影响的记录行数
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>受影响记录行数</returns>
        public async Task<int> ExecuteNonQueryAsync(string sql, params object[] parameters)
        {
            return await NContext.Database.ExecuteSqlRawAsync(sql, parameters);
        }
        /// <summary>
        /// 执行查询，并以DataTable返回结果集
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>DataTable</returns>
        public DataTable ExecuteDataTable(string sql, params object[] parameters)
        {
            DataTable dt = SqlQuery(sql, CommandType.Text, parameters);
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
        /// <summary>
        /// 执行非查询语句,并返回首行首列的值
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>Object</returns>
        public object ExecuteScalar(string sql, params object[] parameters)
        {
            return ExecuteScalar(sql, CommandType.Text, parameters);
        }
        /// <summary>
        /// 执行非查询语句,并返回首行首列的值
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>Object</returns>
        public async Task<object> ExecuteScalarAsync(string sql, params object[] parameters)
        {
            return await ExecuteScalarAsync(sql, CommandType.Text, parameters);
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

        private DataTable SqlQuery(string sql, CommandType cmdtype, params object[] parameters)
        {
            //注意：不要对GetDbConnection获取到的conn进行using或者调用Dispose，否则DbContext后续不能再进行使用了，会抛异常
            var conn = NContext.Database.GetDbConnection();
            try
            {
                conn.Open();
                using (var command = conn.CreateCommand())
                {
                    command.CommandType = cmdtype;
                    command.CommandText = sql;
                    command.Parameters.AddRange(parameters);

                    DataTable objDataTable = new DataTable("Table");

                    using (var reader = command.ExecuteReader())
                    {
                        int intFieldCount = reader.FieldCount;
                        for (int intCounter = 0; intCounter < intFieldCount; ++intCounter)
                        {
                            objDataTable.Columns.Add(reader.GetName(intCounter), reader.GetFieldType(intCounter));
                        }

                        objDataTable.BeginLoadData();
                        object[] objValues = new object[intFieldCount];
                        while (reader.Read())
                        {
                            reader.GetValues(objValues);
                            objDataTable.LoadDataRow(objValues, true);
                        }
                        reader.Close();
                        objDataTable.EndLoadData();


                    }
                    return objDataTable;
                }
            }
            finally
            {
                conn.Close();
            }
        }
        private async Task<DataTable> SqlQueryAsync(string sql, CommandType cmdtype, params object[] parameters)
        {
            //注意：不要对GetDbConnection获取到的conn进行using或者调用Dispose，否则DbContext后续不能再进行使用了，会抛异常
            var conn = NContext.Database.GetDbConnection();
            try
            {
                conn.Open();
                using (var command = conn.CreateCommand())
                {
                    command.CommandType = cmdtype;
                    command.CommandText = sql;
                    command.Parameters.AddRange(parameters);

                    DataTable objDataTable = new DataTable("Table");

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        int intFieldCount = reader.FieldCount;
                        for (int intCounter = 0; intCounter < intFieldCount; ++intCounter)
                        {
                            objDataTable.Columns.Add(reader.GetName(intCounter), reader.GetFieldType(intCounter));
                        }

                        objDataTable.BeginLoadData();
                        object[] objValues = new object[intFieldCount];
                        while (reader.Read())
                        {
                            reader.GetValues(objValues);
                            objDataTable.LoadDataRow(objValues, true);
                        }
                        reader.Close();
                        objDataTable.EndLoadData();


                    }
                    return objDataTable;
                }
            }
            finally
            {
                conn.Close();
            }
        }
        /// <summary>
        /// 执行非查询语句,并返回首行首列的值
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="cmdtype">命令类型</param>
        /// <param name="parameters">参数</param>
        /// <returns>Object</returns>
        private object ExecuteScalar(string sql, CommandType cmdtype, params object[] parameters)
        {
            var conn = NContext.Database.GetDbConnection();
            try
            {
                conn.Open();
                using (var command = conn.CreateCommand())
                {
                    command.CommandType = cmdtype;
                    command.CommandText = sql;
                    command.Parameters.AddRange(parameters);

                    return command.ExecuteScalar();


                }
            }
            finally
            {
                conn.Close();
            }
        }
        /// <summary>
        /// 执行非查询语句,并返回首行首列的值
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="cmdtype">命令类型</param>
        /// <param name="parameters">参数</param>
        /// <returns>Object</returns>
        private async Task<object> ExecuteScalarAsync(string sql, CommandType cmdtype, params object[] parameters)
        {
            var conn = NContext.Database.GetDbConnection();
            try
            {
                conn.Open();
                using (var command = conn.CreateCommand())
                {
                    command.CommandType = cmdtype;
                    command.CommandText = sql;
                    command.Parameters.AddRange(parameters);

                    return await command.ExecuteScalarAsync();


                }
            }
            finally
            {
                conn.Close();
            }
        }
        #endregion

    }
}