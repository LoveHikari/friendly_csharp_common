using Hikari.Common.EfCore.Domain;

namespace Hikari.Common.EfCore.Application
{
    /// <summary>
    /// 业务接口基类
    /// </summary>
    /// <typeparam name="TAggregateRoot">类型</typeparam>
    public interface IBaseService<TAggregateRoot> where TAggregateRoot : class, IAggregateRoot
    {
    }
}