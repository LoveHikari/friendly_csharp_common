using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace FsLib.DDDBase.Domain
{
    /// <summary>
    /// Unit of Work：维护受业务事务影响的对象列表，并协调变化的写入和并发问题的解决。工作单元记录在业务事务过程中对数据库有影响的所有变化，操作结束后，作为一种结果，工作单元了解所有需要对数据库做的改变，统一对数据库操作。
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private IDbContext _dbContext;
        private IDbContextTransaction _dbTransaction;

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
        public async Task<int> ExecuteSqlCommandAsync(string sql, params object[] parameters)
        {
            return await _dbContext.Database.ExecuteSqlRawAsync(sql, parameters);
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
            if (_dbTransaction != null)
                _dbTransaction.Rollback();
        }
    }
}