using FsLib.EfCore.Domain;

namespace Miko.Domain.Entity
{
    /// <summary>
    /// 任务表
    /// </summary>
    public class MWork : IAggregateRoot
    {
        public int Id { get; set; }
    }
}