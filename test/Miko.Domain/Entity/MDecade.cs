using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FsLib.EfCore.Domain;

namespace Miko.Domain.Entity
{
    /// <summary>
    /// 年代表
    /// </summary>
    [Serializable, Table("m_decade")]
    public sealed partial class MDecade : IAggregateRoot
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        [Column("name")]
        public string Name { get; set; }
        /// <summary>
        /// 关键字
        /// </summary>
        [Column("keyword")]
        public string Keyword { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        [Column("status")]
        public int Status { get; set; }
        /// <summary>
        /// 权重
        /// </summary>
        [Column("weight")]
        public int Weight { get; set; }
    }
}