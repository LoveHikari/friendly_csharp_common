using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Hikari.Common.EntityFrameworkCore.Domain;

/// <summary>
/// 数据库上下文接口
/// 定义了数据库操作的基本契约
/// </summary>
public interface IDbContext
{
    /// <summary>
    /// 数据库访问门面，提供数据库级别的操作
    /// </summary>
    DatabaseFacade Database { get; }

    /// <summary>
    /// 变更跟踪器，用于跟踪实体状态变化
    /// </summary>
    ChangeTracker ChangeTracker { get; }

    /// <summary>
    /// 获取指定类型的实体集合
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <returns>实体集合</returns>
    DbSet<TEntity> Set<TEntity>() where TEntity : class;

    /// <summary>
    /// 获取指定实体的入口，用于访问实体状态和跟踪信息
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="entity">实体实例</param>
    /// <returns>实体入口</returns>
    EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

    /// <summary>
    /// 异步保存所有更改到数据库
    /// </summary>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>受影响的行数</returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// 同步保存所有更改到数据库
    /// </summary>
    /// <returns>受影响的行数</returns>
    int SaveChanges();
}

