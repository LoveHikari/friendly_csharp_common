using System.Data;
using Hikari.Common.EntityFrameworkCore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Hikari.Common.EntityFrameworkCore.Repository;

/// <summary>
/// Unit of Work：维护受业务事务影响的对象列表，并协调变化的写入和并发问题的解决。工作单元记录在业务事务过程中对数据库有影响的所有变化，操作结束后，作为一种结果，工作单元了解所有需要对数据库做的改变，统一对数据库操作。
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly IDbContext _dbContext;
    private IDbContextTransaction? _dbTransaction;
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="dbContext">数据上下文</param>
    public UnitOfWork(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    #region 事务控制
    /// <summary>
    /// 开始一个事务
    /// </summary>
    /// <returns></returns>
    public async Task BeginTransactionAsync()
    {
        _dbTransaction = await _dbContext.Database.BeginTransactionAsync();
    }
    /// <summary>
    /// 开始一个事务
    /// </summary>
    /// <returns></returns>
    public void BeginTransaction()
    {
        _dbTransaction = _dbContext.Database.BeginTransaction();
    }
    /// <summary>
    /// 事务提交
    /// </summary>
    /// <returns></returns>
    public async Task CommitAsync(CancellationToken ct = default)
    {
        if (_dbTransaction != null)
        {
            await _dbTransaction.CommitAsync(ct);
            await DisposeAsync();
        }
    }
    /// <summary>
    /// 事务提交
    /// </summary>
    /// <returns></returns>
    public void Commit()
    {
        if (_dbTransaction != null)
        {
            _dbTransaction.Commit();
            Dispose();
        }
    }
    /// <summary>
    /// 事务回滚
    /// </summary>
    public async Task RollbackAsync(CancellationToken ct = default)
    {
        if (_dbTransaction != null)
        {
            await _dbTransaction.RollbackAsync(ct);
            await DisposeAsync();
        }
    }
    /// <summary>
    /// 事务回滚
    /// </summary>
    public void Rollback()
    {
        _dbTransaction?.Rollback();
        Dispose();
    }
    private async ValueTask DisposeAsync()
    {
        if (_dbTransaction != null)
        {
            await _dbTransaction.DisposeAsync();
            _dbTransaction = null;
        }
        if (_dbContext is IAsyncDisposable asyncDisposable)
            await asyncDisposable.DisposeAsync();
        else if (_dbContext is IDisposable disposable)
            disposable.Dispose();
    }

    private void Dispose()
    {
        _dbTransaction?.Dispose();
        _dbTransaction = null;
        if (_dbContext is IDisposable disposable)
            disposable.Dispose();
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
    /// <param name="entities">数据实体</param>
    public void UpdateRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
    {
        _dbContext.Set<TEntity>().UpdateRange(entities);
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

    /// <summary>
    /// 执行变更并返回受影响行数（即使在事务中也可多次调用，用于获取数据库生成的值）
    /// </summary>
    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        return await _dbContext.SaveChangesAsync(ct);
    }
    public int SaveChanges()
    {
        return _dbContext.SaveChanges();
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
    /// <summary>
    /// 执行非查询语句,并返回首行首列的值
    /// </summary>
    /// <param name="sql">SQL语句</param>
    /// <param name="parameters">参数</param>
    /// <returns>Object</returns>
    public async Task<object?> ExecuteScalarAsync(string sql, params object[] parameters)
    {
        return await ExecuteScalarAsync(sql, CommandType.Text, parameters);
    }
    /// <summary>
    /// 执行非查询语句,并返回首行首列的值
    /// </summary>
    /// <param name="sql">SQL语句</param>
    /// <param name="parameters">参数</param>
    /// <returns>Object</returns>
    public object? ExecuteScalar(string sql, params object[] parameters)
    {
        return ExecuteScalar(sql, CommandType.Text, parameters);
    }



    #region 私有方法

    /// <summary>
    /// 执行非查询语句,并返回首行首列的值
    /// </summary>
    /// <param name="sql">SQL语句</param>
    /// <param name="cmdType">命令类型</param>
    /// <param name="parameters">参数</param>
    /// <returns>Object</returns>
    private async Task<object?> ExecuteScalarAsync(string sql, CommandType cmdType, params object[] parameters)
    {
        await using var command = _dbContext.Database.GetDbConnection().CreateCommand();
        command.CommandType = cmdType;
        command.CommandText = sql;
        command.Parameters.AddRange(parameters);

        return await command.ExecuteScalarAsync();
    }
    /// <summary>
    /// 执行非查询语句,并返回首行首列的值
    /// </summary>
    /// <param name="sql">SQL语句</param>
    /// <param name="cmdType">命令类型</param>
    /// <param name="parameters">参数</param>
    /// <returns>Object</returns>
    private object? ExecuteScalar(string sql, CommandType cmdType, params object[] parameters)
    {
        using var command = _dbContext.Database.GetDbConnection().CreateCommand();
        command.CommandType = cmdType;
        command.CommandText = sql;
        command.Parameters.AddRange(parameters);

        return command.ExecuteScalar();
    }
    #endregion
}
