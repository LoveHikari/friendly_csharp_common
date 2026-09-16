namespace Hikari.Common.EntityFrameworkCore.Repository;

/// <summary>
/// 工作单元事务作用域：推荐用 <c>await using</c> 包裹，
/// 调用 <see cref="CompleteAsync"/> 表示成功；未 Complete 或异常退出时在 Dispose 中回滚。
/// </summary>
public interface IUnitOfWorkTransactionScope : IAsyncDisposable, IDisposable
{
    /// <summary>
    /// 标记本层成功并提交一层事务深度（仅 outermost 真正 Commit）。
    /// </summary>
    Task CompleteAsync(CancellationToken ct = default);

    /// <summary>
    /// 同步版 <see cref="CompleteAsync"/>。
    /// </summary>
    void Complete();
}
