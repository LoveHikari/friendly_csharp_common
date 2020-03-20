using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FsLib.EfCore.Domain;

namespace Miko.Domain.Entity
{
    /// <summary>
    /// 番剧更新表
    /// </summary>
    [Serializable, Table("mac_bgm")]
    public sealed partial class MBgm : IAggregateRoot
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        [Column("bgm_id", TypeName = "int")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage = "必填", AllowEmptyStrings = true)]
        [Display(Name = "id")]
        public int Id { get; set; }
        /// <summary>
        /// 视频id
        /// </summary>
        [Column("vod_id", TypeName = "int")]
        [Required(ErrorMessage = "必填", AllowEmptyStrings = true)]
        [Display(Name = "视频id")]
        [ForeignKey(nameof(MVideo))]
        public int VodId { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        [Column("bgm_name", TypeName = "varchar(255)")]
        [StringLength(255)]
        [Required(ErrorMessage = "必填", AllowEmptyStrings = true)]
        [Display(Name = "名称")]
        public string Name { get; set; }
        /// <summary>
        /// 最新一集备注
        /// </summary>
        [Column("bgm_remarks", TypeName = "varchar(100)")]
        [StringLength(100)]
        [Required(ErrorMessage = "必填", AllowEmptyStrings = true)]
        [Display(Name = "最新一集备注")]
        public string Remarks { get; set; }
        /// <summary>
        /// 图片
        /// </summary>
        [Column("bgm_pic", TypeName = "varchar(255)")]
        [StringLength(255)]
        [Required(ErrorMessage = "必填", AllowEmptyStrings = true)]
        [Display(Name = "图片")]
        public string Pic { get; set; }
        /// <summary>
        /// 更新周天 0~6
        /// </summary>
        [Column("bgm_week_day", TypeName = "int")]
        [Range(0, 6)]
        [Required(ErrorMessage = "必填", AllowEmptyStrings = true)]
        [Display(Name = "更新周天")]
        public int WeekDay { get; set; }
        /// <summary>
        /// 视频类型id
        /// </summary>
        [Column("type_id", TypeName = "int")]
        [Required(ErrorMessage = "必填", AllowEmptyStrings = true)]
        [Display(Name = "视频类型id")]
        public int TypeId { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        [Column("bgm_time_add", TypeName = "int")]
        [StringLength(10)]
        [Required(ErrorMessage = "必填", AllowEmptyStrings = true)]
        [Display(Name = "添加时间")]
        public int AddTime { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        [Column("bgm_time", TypeName = "int")]
        [StringLength(10)]
        [Required(ErrorMessage = "必填", AllowEmptyStrings = true)]
        [Display(Name = "更新时间")]
        public int UpdateTime { get; set; }
    }
}