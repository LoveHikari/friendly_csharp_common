using System.Data;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace FsLib.EfCore.Domain
{
    /// <summary>
    /// Unit of Work：维护受业务事务影响的对象列表，并协调变化的写入和并发问题的解决。工作单元记录在业务事务过程中对数据库有影响的所有变化，操作结束后，作为一种结果，工作单元了解所有需要对数据库做的改变，统一对数据库操作。
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDbContext _dbContext;
        private IDbContextTransaction _dbTransaction;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbContext">数据上下文</param>
        public UnitOfWork(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        /// <summary>
        /// 开始一个事务
        /// </summary>
        /// <returns></returns>
        public async Task BeginTransactionAsync()
        {
            _dbTransaction = await _dbContext.Database.BeginTransactionAsync();
        }
        /// <summary>
        /// 执行非查询语句,并返回受影响的记录行数
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>受影响记录行数</returns>
        public async Task<int> ExecuteNonQueryAsync(string sql, params object[] parameters)
        {
            return await _dbContext.Database.ExecuteSqlRawAsync(sql, parameters);
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
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>是否成功</returns>
        public async Task<bool> AddAsync<TEntity>(TEntity entity) where TEntity : class
        {
            _dbContext.Set<TEntity>().Add(entity);
            //_dbContext.Entry<TEntity>(entity).State = EntityState.Added;
            if (_dbTransaction != null)
                return await _dbContext.SaveChangesAsync() > 0;

            return true;
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>是否成功</returns>
        public async Task<bool> UpdateAsync<TEntity>(TEntity entity) where TEntity : class
        {
            _dbContext.Entry<TEntity>(entity).State = EntityState.Modified;
            if (_dbTransaction != null)
                return await _dbContext.SaveChangesAsync() > 0;
            return true;
        }
        /// <summary>
        /// 清理
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> CleanAsync<TEntity>(TEntity entity) where TEntity : class
        {
            _dbContext.Entry<TEntity>(entity).State = EntityState.Unchanged;
            if (_dbTransaction != null)
                return await _dbContext.SaveChangesAsync() > 0;
            return true;
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>是否成功</returns>
        public async Task<bool> DeleteAsync<TEntity>(TEntity entity) where TEntity : class
        {
            _dbContext.Set<TEntity>().Remove(entity);
            //_dbContext.Entry<TEntity>(entity).State = EntityState.Deleted;
            if (_dbTransaction != null)
                return await _dbContext.SaveChangesAsync() > 0;
            return true;
        }
        /// <summary>
        /// 事务提交
        /// </summary>
        /// <returns></returns>
        public async Task<bool> CommitAsync()
        {
            if (_dbTransaction == null)
                return await _dbContext.SaveChangesAsync() > 0;
            else
                _dbTransaction.Commit();
            return true;
        }
        /// <summary>
        /// 事务回滚
        /// </summary>
        public void Rollback()
        {
            _dbTransaction?.Rollback();
        }

        #region 私有方法

        /// <summary>
        /// 执行非查询语句,并返回首行首列的值
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="parameters">参数</param>
        /// <returns>Object</returns>
        private async Task<object> ExecuteScalarAsync(string sql, CommandType cmdType, params object[] parameters)
        {
            var conn = _dbContext.Database.GetDbConnection();
            try
            {
                conn.Open();
                using (var command = conn.CreateCommand())
                {
                    command.CommandType = cmdType;
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