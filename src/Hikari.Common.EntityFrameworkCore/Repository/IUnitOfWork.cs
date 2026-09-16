namespace Hikari.Common.EntityFrameworkCore.Repository;
/// <summary>
/// Unit of Work：维护受业务事务影响的对象列表，并协调变化的写入和并发问题的解决
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    /// 开始事务作用域（推荐）。已有事务则复用；用 <c>await using</c>，成功时 <see cref="IUnitOfWorkTransactionScope.CompleteAsync"/>。
    /// </summary>
    Task<IUnitOfWorkTransactionScope> BeginTransactionScopeAsync(CancellationToken ct = default);

    /// <summary>
    /// 开始事务作用域；<paramref name="deferSave"/> 为 true 时同时开启延迟保存（与 <see cref="BeginDeferredSave"/> 等价叠加）。
    /// Complete 时若本层释放后已无 defer，会先 <see cref="SaveChangesAsync(System.Threading.CancellationToken)"/> 再 Commit。
    /// </summary>
    Task<IUnitOfWorkTransactionScope> BeginTransactionScopeAsync(bool deferSave, CancellationToken ct = default);

    /// <summary>
    /// 同步版 <see cref="BeginTransactionScopeAsync(System.Threading.CancellationToken)"/>。
    /// </summary>
    IUnitOfWorkTransactionScope BeginTransactionScope(bool deferSave = false);

    /// <summary>
    /// 延迟保存作用域：期间普通 <see cref="SaveChangesAsync(System.Threading.CancellationToken)"/> 为空操作，退出后由调用方再 Save，或使用 force。
    /// </summary>
    IDisposable BeginDeferredSave();

    /// <summary>
    /// 开始一个事务（已有事务则深度 +1 复用，不重复 Begin）。
    /// 更推荐 <see cref="BeginTransactionScopeAsync"/>，避免手写 try/finally。
    /// </summary>
    Task BeginTransactionAsync();

    /// <summary>
    /// 开始一个事务（已有事务则深度 +1 复用，不重复 Begin）。
    /// </summary>
    void BeginTransaction();

    /// <summary>
    /// 提交一层事务深度；仅 outermost 真正 Commit。任一内层曾 Rollback 则改为回滚。
    /// </summary>
    Task CommitAsync(CancellationToken ct = default);

    /// <summary>
    /// 同步版 <see cref="CommitAsync"/>。
    /// </summary>
    void Commit();

    /// <summary>
    /// 注册「确认落库后」才执行的副作用，典型用途是通知外部系统（ERP / 物流）。
    /// <para>
    /// 事务中时在最外层提交成功后触发；无事务时在下一次真实保存成功后触发；回滚则丢弃。
    /// 直接在改状态处调用外部接口会出现「本地没落库、对方已收到通知」的劈叉。
    /// </para>
    /// </summary>
    void RegisterPostCommitAction(Action action);

    /// <summary>
    /// 回滚：标记整笔事务作废并减少一层深度；仅 outermost 真正 Rollback。
    /// </summary>
    Task RollbackAsync(CancellationToken ct = default);

    /// <summary>
    /// 同步版 <see cref="RollbackAsync"/>。
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
    /// <param name="entity">数据实体</param>
    /// <param name="ct"></param>
    /// <returns>是否成功</returns>
    Task UpdateAsync<TEntity>(TEntity entity, CancellationToken ct = default) where TEntity : class;
    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="entities">数据实体</param>
    /// <returns>是否成功</returns>
    void UpdateRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class;

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="entities">数据实体</param>
    /// <param name="ct"></param>
    /// <returns>是否成功</returns>
    Task UpdateRangeAsync<TEntity>(IEnumerable<TEntity> entities, CancellationToken ct = default) where TEntity : class;
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
    /// 执行变更并返回受影响行数。处于 <see cref="BeginDeferredSave"/> 时为空操作（除非使用 force 重载）。
    /// 保存失败不清空 ChangeTracker，以免同一请求里其它已跟踪实体丢失。回滚时才清空。
    /// </summary>
    Task<int> SaveChangesAsync(CancellationToken ct = default);

    /// <summary>
    /// 执行变更；<paramref name="force"/> 为 true 时忽略延迟保存抑制（用于中途取库生成值，或 Scope Complete 刷库）。
    /// </summary>
    Task<int> SaveChangesAsync(bool force, CancellationToken ct = default);

    /// <summary>
    /// 同步版 <see cref="SaveChangesAsync(System.Threading.CancellationToken)"/>。
    /// </summary>
    int SaveChanges();

    /// <summary>
    /// 同步版 <see cref="SaveChangesAsync(bool, System.Threading.CancellationToken)"/>。
    /// </summary>
    int SaveChanges(bool force);

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
    /// 执行查询并返回首行首列。使用当前连接与事务；参数写法与 <see cref="ExecuteNonQueryAsync"/> 相同（可用 {0} 占位）。
    /// </summary>
    Task<object?> ExecuteScalarAsync(string sql, params object[] parameters);

    /// <summary>
    /// 同步版 <see cref="ExecuteScalarAsync"/>。
    /// </summary>
    object? ExecuteScalar(string sql, params object[] parameters);
}
