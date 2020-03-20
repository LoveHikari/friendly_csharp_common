using FsLib.EfCore.Domain;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Miko.Domain.Entity
{
    /// <summary>
    /// 分类表
    /// </summary>
    [Serializable, Table("mac_type")]
    public class MType : IAggregateRoot
    {
        [Key]
        [Column("type_id", TypeName = "int")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [StringLength(10)]
        [Required(ErrorMessage = "必填", AllowEmptyStrings = true)]
        [Display(Name = "id")]
        public int Id { get; set; }
        /// <summary>
        /// 分类名称
        /// </summary>
        [Column("type_name")]
        public string TypeName { get; set; }
        /// <summary>
        /// 分类英文名称
        /// </summary>
        [Column("type_en")]
        public string TypeEn { get; set; }
        /// <summary>
        /// 父id
        /// </summary>
        [Column("type_pid")]
        public int ParentId { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        [Column("type_sort")]
        public int Sort { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        [Column("type_status")]
        public int Status { get; set; }
    }
}