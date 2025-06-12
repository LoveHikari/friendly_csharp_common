namespace Hikari.Common.EntityFrameworkCore.Domain
{
    /// <summary>
    /// Unit of Work：维护受业务事务影响的对象列表，并协调变化的写入和并发问题的解决
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        /// 开始一个事务
        /// </summary>
        /// <returns></returns>
        Task BeginTransactionAsync();

        /// <summary>
        /// 开始一个事务
        /// </summary>
        /// <returns></returns>
        void BeginTransaction();
        /// <summary>
        /// 执行非查询语句,并返回受影响的记录行数
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>受影响记录行数</returns>
        Task<int> ExecuteNonQueryAsync(string sql, params object[] parameters);
        /// <summary>
        /// 执行非查询语句,并返回受影响的记录行数
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>受影响记录行数</returns>
        int ExecuteNonQuery(string sql, params object[] parameters);
        /// <summary>
        /// 执行非查询语句,并返回首行首列的值
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>Object</returns>
        Task<object?> ExecuteScalarAsync(string sql, params object[] parameters);
        /// <summary>
        /// 执行非查询语句,并返回首行首列的值
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>Object</returns>
        object? ExecuteScalar(string sql, params object[] parameters);
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>是否成功</returns>
        Task<bool> AddAsync<TEntity>(TEntity entity) where TEntity : class;
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>是否成功</returns>
        bool Add<TEntity>(TEntity entity) where TEntity : class;
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entities">数据实体</param>
        /// <returns>是否成功</returns>
        Task<bool> AddListAsync<TEntity>(IEnumerable<TEntity> entities) where TEntity : class;

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entities">数据实体</param>
        /// <returns>是否成功</returns>
        bool AddList<TEntity>(IEnumerable<TEntity> entities) where TEntity : class;
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>是否成功</returns>
        Task<bool> UpdateAsync<TEntity>(TEntity entity) where TEntity : class;
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>是否成功</returns>
        bool Update<TEntity>(TEntity entity) where TEntity : class;
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entities">数据实体</param>
        /// <returns>是否成功</returns>
        Task<bool> UpdateListAsync<TEntity>(IEnumerable<TEntity> entities) where TEntity : class;
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entities">数据实体</param>
        /// <returns>是否成功</returns>
        bool UpdateList<TEntity>(IEnumerable<TEntity> entities) where TEntity : class;
        /// <summary>
        /// 清理
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<bool> CleanAsync<TEntity>(TEntity entity) where TEntity : class;
        /// <summary>
        /// 清理
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool Clean<TEntity>(TEntity entity) where TEntity : class;
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>是否成功</returns>
        Task<bool> DeleteAsync<TEntity>(TEntity entity) where TEntity : class;
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>是否成功</returns>
        bool Delete<TEntity>(TEntity entity) where TEntity : class;

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entities">数据实体</param>
        /// <returns>是否成功</returns>
        Task<bool> DeleteListAsync<TEntity>(IEnumerable<TEntity> entities) where TEntity : class;

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entities">数据实体</param>
        /// <returns>是否成功</returns>
        bool DeleteList<TEntity>(IEnumerable<TEntity> entities) where TEntity : class;
        /// <summary>
        /// 事务提交
        /// </summary>
        /// <returns></returns>
        Task<bool> CommitAsync();
        /// <summary>
        /// 事务提交
        /// </summary>
        /// <returns></returns>
        bool Commit();
        /// <summary>
        /// 事务回滚
        /// </summary>
        Task RollbackAsync();
        /// <summary>
        /// 事务回滚
        /// </summary>
        void Rollback();
    }
}