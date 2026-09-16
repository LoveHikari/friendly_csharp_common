using System.Data;
using System.Data.Common;
using Hikari.Common.EntityFrameworkCore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace Hikari.Common.EntityFrameworkCore.Repository;

/// <summary>
/// Unit of Work：维护受业务事务影响的对象列表，并协调变化的写入和并发问题的解决。工作单元记录在业务事务过程中对数据库有影响的所有变化，操作结束后，作为一种结果，工作单元了解所有需要对数据库做的改变，统一对数据库操作。
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly IDbContext _dbContext;
    private IDbContextTransaction? _dbTransaction;
    /// <summary>事务嵌套深度；&gt;0 表示正处于事务中。</summary>
    private int _transactionDepth;
    /// <summary>任一内层回滚后置位，outermost 提交时改为回滚。</summary>
    private bool _rollbackRequested;
    /// <summary>是否由本 UoW 创建事务（复用外部 CurrentTransaction 时为 false）。</summary>
    private bool _ownsTransaction;
    /// <summary>延迟保存嵌套深度；&gt;0 时普通 SaveChanges 为空操作。</summary>
    private int _deferSaveDepth;
    /// <summary>待「落库确认后」执行的副作用（外部系统通知），回滚时丢弃。</summary>
    private readonly List<Action> _postCommitActions = [];

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="dbContext">数据上下文</param>
    public UnitOfWork(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    #region 事务控制

    /// <inheritdoc />
    public Task<IUnitOfWorkTransactionScope> BeginTransactionScopeAsync(CancellationToken ct = default) =>
        BeginTransactionScopeAsync(deferSave: false, ct);

    /// <inheritdoc />
    public async Task<IUnitOfWorkTransactionScope> BeginTransactionScopeAsync(bool deferSave, CancellationToken ct = default)
    {
        IDisposable? deferred = deferSave ? BeginDeferredSave() : null;
        try
        {
            await BeginTransactionAsync();
            return new UnitOfWorkTransactionScope(this, deferred);
        }
        catch
        {
            deferred?.Dispose();
            throw;
        }
    }

    /// <inheritdoc />
    public IUnitOfWorkTransactionScope BeginTransactionScope(bool deferSave = false)
    {
        IDisposable? deferred = deferSave ? BeginDeferredSave() : null;
        try
        {
            BeginTransaction();
            return new UnitOfWorkTransactionScope(this, deferred);
        }
        catch
        {
            deferred?.Dispose();
            throw;
        }
    }

    /// <inheritdoc />
    public IDisposable BeginDeferredSave()
    {
        _deferSaveDepth++;
        return new DeferredSaveScope(this);
    }

    private void EndDeferredSave()
    {
        if (_deferSaveDepth > 0)
        {
            _deferSaveDepth--;
        }
    }

    private bool IsDeferredSaveActive => _deferSaveDepth > 0;

    /// <inheritdoc />
    public void RegisterPostCommitAction(Action action)
    {
        if (action != null)
        {
            _postCommitActions.Add(action);
        }
    }

    /// <summary>落库已确认，依次执行并清空排队的外部副作用。</summary>
    private void RunPostCommitActions()
    {
        if (_postCommitActions.Count == 0)
        {
            return;
        }

        var actions = _postCommitActions.ToArray();
        _postCommitActions.Clear();
        foreach (var action in actions)
        {
            action();
        }
    }

    /// <inheritdoc />
    public async Task BeginTransactionAsync()
    {
        if (_transactionDepth == 0)
        {
            var existing = _dbContext.Database.CurrentTransaction;
            if (existing != null)
            {
                _dbTransaction = existing;
                _ownsTransaction = false;
            }
            else
            {
                _dbTransaction = await _dbContext.Database.BeginTransactionAsync();
                _ownsTransaction = true;
            }

            _rollbackRequested = false;
        }

        _transactionDepth++;
    }

    /// <inheritdoc />
    public void BeginTransaction()
    {
        if (_transactionDepth == 0)
        {
            var existing = _dbContext.Database.CurrentTransaction;
            if (existing != null)
            {
                _dbTransaction = existing;
                _ownsTransaction = false;
            }
            else
            {
                _dbTransaction = _dbContext.Database.BeginTransaction();
                _ownsTransaction = true;
            }

            _rollbackRequested = false;
        }

        _transactionDepth++;
    }

    /// <inheritdoc />
    public async Task CommitAsync(CancellationToken ct = default)
    {
        if (_transactionDepth == 0)
        {
            return;
        }

        _transactionDepth--;
        if (_transactionDepth > 0)
        {
            return;
        }

        await FinishOutermostAsync(commit: !_rollbackRequested, ct);
    }

    /// <inheritdoc />
    public void Commit()
    {
        if (_transactionDepth == 0)
        {
            return;
        }

        _transactionDepth--;
        if (_transactionDepth > 0)
        {
            return;
        }

        FinishOutermost(commit: !_rollbackRequested);
    }

    /// <inheritdoc />
    public async Task RollbackAsync(CancellationToken ct = default)
    {
        if (_transactionDepth == 0)
        {
            return;
        }

        _rollbackRequested = true;
        _transactionDepth--;
        if (_transactionDepth > 0)
        {
            return;
        }

        await FinishOutermostAsync(commit: false, ct);
    }

    /// <inheritdoc />
    public void Rollback()
    {
        if (_transactionDepth == 0)
        {
            return;
        }

        _rollbackRequested = true;
        _transactionDepth--;
        if (_transactionDepth > 0)
        {
            return;
        }

        FinishOutermost(commit: false);
    }

    /// <summary>outermost 真正提交或回滚，并清理本地事务状态。</summary>
    private async Task FinishOutermostAsync(bool commit, CancellationToken ct)
    {
        var tx = _dbTransaction;
        var owns = _ownsTransaction;
        ClearTransactionState();

        if (!commit)
        {
            _postCommitActions.Clear();
        }

        if (tx == null)
        {
            if (commit)
            {
                RunPostCommitActions();
            }

            return;
        }

        try
        {
            if (!owns)
            {
                // 复用外部事务：不 Commit/Dispose，仅在需要回滚时 Rollback 整笔
                if (!commit)
                {
                    await tx.RollbackAsync(ct);
                    _dbContext.ChangeTracker.Clear();
                }

                return;
            }

            if (commit)
            {
                await tx.CommitAsync(ct);
            }
            else
            {
                await tx.RollbackAsync(ct);
                _dbContext.ChangeTracker.Clear();
            }
        }
        finally
        {
            if (owns)
            {
                await tx.DisposeAsync();
            }
        }

        if (commit)
        {
            RunPostCommitActions();
        }
    }

    /// <summary>同步版 <see cref="FinishOutermostAsync"/>。</summary>
    private void FinishOutermost(bool commit)
    {
        var tx = _dbTransaction;
        var owns = _ownsTransaction;
        ClearTransactionState();

        if (!commit)
        {
            _postCommitActions.Clear();
        }

        if (tx == null)
        {
            if (commit)
            {
                RunPostCommitActions();
            }

            return;
        }

        try
        {
            if (!owns)
            {
                if (!commit)
                {
                    tx.Rollback();
                    _dbContext.ChangeTracker.Clear();
                }

                return;
            }

            if (commit)
            {
                tx.Commit();
            }
            else
            {
                tx.Rollback();
                _dbContext.ChangeTracker.Clear();
            }
        }
        finally
        {
            if (owns)
            {
                tx.Dispose();
            }
        }

        if (commit)
        {
            RunPostCommitActions();
        }
    }

    private void ClearTransactionState()
    {
        _dbTransaction = null;
        _ownsTransaction = false;
        _rollbackRequested = false;
        _transactionDepth = 0;
    }

    /// <summary>延迟保存层：Dispose 时减少一层 defer 深度。</summary>
    private sealed class DeferredSaveScope : IDisposable
    {
        private UnitOfWork? _uow;

        public DeferredSaveScope(UnitOfWork uow) => _uow = uow;

        public void Dispose()
        {
            var uow = Interlocked.Exchange(ref _uow, null);
            uow?.EndDeferredSave();
        }
    }

    /// <summary>事务作用域：Complete 提交本层；未 Complete 则 Dispose 时回滚本层。</summary>
    private sealed class UnitOfWorkTransactionScope : IUnitOfWorkTransactionScope
    {
        private readonly UnitOfWork _uow;
        private IDisposable? _deferredSave;
        private bool _completed;
        private bool _disposed;

        public UnitOfWorkTransactionScope(UnitOfWork uow, IDisposable? deferredSave)
        {
            _uow = uow;
            _deferredSave = deferredSave;
        }

        public async Task CompleteAsync(CancellationToken ct = default)
        {
            ObjectDisposedException.ThrowIf(_disposed, this);
            if (_completed)
            {
                return;
            }

            // 须在 Flush/Commit 都成功后再标记；否则失败时 Dispose 不会回滚。
            await FlushOwnedDeferredSaveAsync(ct);
            await _uow.CommitAsync(ct);
            _completed = true;
        }

        public void Complete()
        {
            ObjectDisposedException.ThrowIf(_disposed, this);
            if (_completed)
            {
                return;
            }

            FlushOwnedDeferredSave();
            _uow.Commit();
            _completed = true;
        }

        public async ValueTask DisposeAsync()
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;
            if (!_completed)
            {
                ReleaseDeferredSave();
                await _uow.RollbackAsync();
            }
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;
            if (!_completed)
            {
                ReleaseDeferredSave();
                _uow.Rollback();
            }
        }

        /// <summary>释放本 Scope 拥有的 defer；若已无外层 defer 则刷库一次。</summary>
        private async Task FlushOwnedDeferredSaveAsync(CancellationToken ct)
        {
            var owned = _deferredSave != null;
            ReleaseDeferredSave();
            if (owned && !_uow.IsDeferredSaveActive)
            {
                await _uow.SaveChangesAsync(force: true, ct);
            }
        }

        private void FlushOwnedDeferredSave()
        {
            var owned = _deferredSave != null;
            ReleaseDeferredSave();
            if (owned && !_uow.IsDeferredSaveActive)
            {
                _uow.SaveChanges(force: true);
            }
        }

        private void ReleaseDeferredSave()
        {
            _deferredSave?.Dispose();
            _deferredSave = null;
        }
    }

    #endregion

    #region 变更跟踪（仅跟踪，不立即提交）
    /// <summary>
    /// 添加
    /// </summary>
    /// <param name="entity">数据实体</param>
    /// <param name="ct"></param>
    public async Task AddAsync<TEntity>(TEntity entity, CancellationToken ct = default) where TEntity : class
    {
        await _dbContext.Set<TEntity>().AddAsync(entity, ct);
        //_dbContext.Entry<TEntity>(entity).State = EntityState.Added;
    }
    /// <summary>
    /// 添加
    /// </summary>
    /// <param name="entity">数据实体</param>
    public void Add<TEntity>(TEntity entity) where TEntity : class
    {
        _dbContext.Set<TEntity>().Add(entity);
        //_dbContext.Entry<TEntity>(entity).State = EntityState.Added;
    }

    /// <summary>
    /// 添加
    /// </summary>
    /// <param name="entities">数据实体</param>
    /// <param name="ct"></param>
    public async Task AddRangeAsync<TEntity>(IEnumerable<TEntity> entities, CancellationToken ct = default) where TEntity : class
    {
        await _dbContext.Set<TEntity>().AddRangeAsync(entities, ct);
        //_dbContext.Entry<TEntity>(entity).State = EntityState.Added;

    }
    /// <summary>
    /// 添加
    /// </summary>
    /// <param name="entities">数据实体</param>
    public void AddRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
    {
        _dbContext.Set<TEntity>().AddRange(entities);
        //_dbContext.Entry<TEntity>(entity).State = EntityState.Added;

    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="entity">数据实体</param>
    public void Update<TEntity>(TEntity entity) where TEntity : class
    {
        _dbContext.Set<TEntity>().Update(entity);
        // _dbContext.Entry(entity).State = EntityState.Modified;
    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="entity">数据实体</param>
    /// <param name="ct"></param>
    public async Task UpdateAsync<TEntity>(TEntity entity, CancellationToken ct = default) where TEntity : class
    {
        await Task.Run(() =>
        {
            _dbContext.Set<TEntity>().Update(entity);
            // _dbContext.Entry(entity).State = EntityState.Modified;
        }, ct);

    }
    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="entities">数据实体</param>
    public void UpdateRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
    {
        _dbContext.Set<TEntity>().UpdateRange(entities);
    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="entities">数据实体</param>
    /// <param name="ct"></param>
    public async Task UpdateRangeAsync<TEntity>(IEnumerable<TEntity> entities, CancellationToken ct = default) where TEntity : class
    {
        await Task.Run(() =>
        {
            _dbContext.Set<TEntity>().UpdateRange(entities);
        }, ct);

    }
    /// <summary>
    /// 清理
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="entity"></param>
    public void Clean<TEntity>(TEntity entity) where TEntity : class
    {
        _dbContext.Entry(entity).State = EntityState.Unchanged;
    }
    /// <summary>
    /// 停止跟踪实体
    /// </summary>
    public void Detach<TEntity>(TEntity entity) where TEntity : class
    {
        _dbContext.Entry(entity).State = EntityState.Detached;
    }
    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="entity">数据实体</param>
    public void Delete<TEntity>(TEntity entity) where TEntity : class
    {
        _dbContext.Set<TEntity>().Remove(entity);
        //_dbContext.Entry<TEntity>(entity).State = EntityState.Deleted;
    }
    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="entities">数据实体</param>
    public void DeleteRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
    {
        _dbContext.Set<TEntity>().RemoveRange(entities);
        //_dbContext.Entry<TEntity>(entity).State = EntityState.Deleted;
    }

    /// <inheritdoc />
    public Task<int> SaveChangesAsync(CancellationToken ct = default) =>
        SaveChangesAsync(force: false, ct);

    /// <inheritdoc />
    public async Task<int> SaveChangesAsync(bool force, CancellationToken ct = default)
    {
        if (!force && _deferSaveDepth > 0)
        {
            return 0;
        }

        var affected = await _dbContext.SaveChangesAsync(ct);

        // 无事务时本次保存即最终落库；事务中则等最外层提交成功再冲刷
        if (_transactionDepth == 0)
        {
            RunPostCommitActions();
        }

        return affected;
    }

    /// <inheritdoc />
    public int SaveChanges() => SaveChanges(force: false);

    /// <inheritdoc />
    public int SaveChanges(bool force)
    {
        if (!force && _deferSaveDepth > 0)
        {
            return 0;
        }

        var affected = _dbContext.SaveChanges();

        if (_transactionDepth == 0)
        {
            RunPostCommitActions();
        }

        return affected;
    }
    #endregion
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
    /// 执行非查询语句,并返回受影响的记录行数
    /// </summary>
    /// <param name="sql">SQL语句</param>
    /// <param name="parameters">参数</param>
    /// <returns>受影响记录行数</returns>
    public int ExecuteNonQuery(string sql, params object[] parameters)
    {
        return _dbContext.Database.ExecuteSqlRaw(sql, parameters);
    }

    /// <inheritdoc />
    public async Task<object?> ExecuteScalarAsync(string sql, params object[] parameters)
    {
        var database = _dbContext.Database;
        await database.OpenConnectionAsync();
        try
        {
            await using var command = CreateScalarCommand(sql, parameters);
            return await command.ExecuteScalarAsync();
        }
        finally
        {
            await database.CloseConnectionAsync();
        }
    }

    /// <inheritdoc />
    public object? ExecuteScalar(string sql, params object[] parameters)
    {
        var database = _dbContext.Database;
        database.OpenConnection();
        try
        {
            using var command = CreateScalarCommand(sql, parameters);
            return command.ExecuteScalar();
        }
        finally
        {
            database.CloseConnection();
        }
    }

    /// <summary>
    /// 在当前连接上建命令，并挂上当前事务与参数。调用方负责 Dispose 命令。
    /// </summary>
    private DbCommand CreateScalarCommand(string sql, object[]? parameters)
    {
        var database = _dbContext.Database;
        var command = database.GetDbConnection().CreateCommand();
        command.CommandType = CommandType.Text;
        var currentTransaction = database.CurrentTransaction;
        if (currentTransaction != null)
        {
            command.Transaction = currentTransaction.GetDbTransaction();
        }

        command.CommandText = BindScalarParameters(command, sql, parameters ?? []);
        return command;
    }

    /// <summary>
    /// 绑定参数：<see cref="DbParameter"/> 原样加入；其余按 ExecuteSqlRaw 把 {i} 换成提供方参数名。
    /// </summary>
    private string BindScalarParameters(DbCommand command, string sql, object[] parameters)
    {
        if (parameters.Length == 0)
        {
            return sql;
        }

        var helper = _dbContext.Database.GetService<ISqlGenerationHelper>();
        for (var i = parameters.Length - 1; i >= 0; i--)
        {
            var value = parameters[i];
            if (value is DbParameter dbParameter)
            {
                command.Parameters.Add(dbParameter);
                continue;
            }

            var parameterName = "p" + i;
            var parameter = command.CreateParameter();
            parameter.ParameterName = parameterName;
            parameter.Value = value ?? DBNull.Value;
            command.Parameters.Add(parameter);
            sql = sql.Replace("{" + i + "}", helper.GenerateParameterName(parameterName), StringComparison.Ordinal);
        }

        return sql;
    }
}
