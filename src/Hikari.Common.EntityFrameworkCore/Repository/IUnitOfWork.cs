namespace Hikari.Common.EntityFrameworkCore.Repository;
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
    /// 事务提交
    /// </summary>
    /// <returns></returns>
    Task CommitAsync(CancellationToken ct = default);
    /// <summary>
    /// 事务提交
    /// </summary>
    /// <returns></returns>
    void Commit();
    /// <summary>
    /// 事务回滚
    /// </summary>
    Task RollbackAsync(CancellationToken ct = default);
    /// <summary>
    /// 事务回滚
    /// </summary>
    void Rollback();

    /// <summary>
    /// 添加
    /// </summary>
    /// <param name="entity">数据实体</param>
    /// <param name="ct"></param>
    /// <returns>是否成功</returns>
    Task AddAsync<TEntity>(TEntity entity, CancellationToken ct = default) where TEntity : class;
    /// <summary>
    /// 添加
    /// </summary>
    /// <param name="entity">数据实体</param>
    /// <returns>是否成功</returns>
    void Add<TEntity>(TEntity entity) where TEntity : class;

    /// <summary>
    /// 添加
    /// </summary>
    /// <param name="entities">数据实体</param>
    /// <param name="ct"></param>
    /// <returns>是否成功</returns>
    Task AddRangeAsync<TEntity>(IEnumerable<TEntity> entities, CancellationToken ct = default) where TEntity : class;

    /// <summary>
    /// 添加
    /// </summary>
    /// <param name="entities">数据实体</param>
    /// <returns>是否成功</returns>
    void AddRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class;

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="entity">数据实体</param>
    /// <returns>是否成功</returns>
    void Update<TEntity>(TEntity entity) where TEntity : class;

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="entities">数据实体</param>
    /// <returns>是否成功</returns>
    void UpdateRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class;

    /// <summary>
    /// 清理
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="entity"></param>
    /// <returns></returns>
    void Clean<TEntity>(TEntity entity) where TEntity : class;

    /// <summary>
    /// 停止跟踪实体
    /// </summary>
    void Detach<TEntity>(TEntity entity) where TEntity : class;
    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="entity">数据实体</param>
    /// <returns>是否成功</returns>
    void Delete<TEntity>(TEntity entity) where TEntity : class;

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="entities">数据实体</param>
    /// <returns>是否成功</returns>
    void DeleteRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class;

    /// <summary>
    /// 执行变更并返回受影响行数（即使在事务中也可多次调用，用于获取数据库生成的值）
    /// </summary>
    /// <returns>受影响行数</returns>
    Task<int> SaveChangesAsync(CancellationToken ct = default);
    /// <summary>
    /// 执行变更并返回受影响行数（即使在事务中也可多次调用，用于获取数据库生成的值）
    /// </summary>
    /// <returns>受影响行数</returns>
    int SaveChanges();

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
}
