﻿namespace FsLib.EfCore.Domain
{
    /// <summary>
    /// 聚合根接口，所有的领域模型都应该实现这个接口
    /// </summary>
    public interface IAggregateRoot
    {
        int Id { get; }
    }
}