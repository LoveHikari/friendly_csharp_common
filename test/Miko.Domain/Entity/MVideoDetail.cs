using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FsLib.EfCore.Domain;

namespace Miko.Domain.Entity
{
    /// <summary>
    /// 视频内容表
    /// </summary>
    [Serializable, Table("m_video_detail")]
    public sealed partial class MVideoDetail : IAggregateRoot
    {
        [Key]
        [Column("id", TypeName = "int")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage = "必填", AllowEmptyStrings = true)]
        [Display(Name = "id")]
        public int Id { get; set; }
        /// <summary>
        /// 视频id
        /// </summary>
        [Column("video_id", TypeName = "int")]
        [Required(ErrorMessage = "必填", AllowEmptyStrings = true)]
        [ForeignKey(nameof(MVideo))]
        [Display(Name = "视频id")]
        public int VideoId { get; set; }
        /// <summary>
        /// 线路
        /// </summary>
        [Column("circuit", TypeName = "varchar(255)")]
        [StringLength(255, MinimumLength = 0, ErrorMessage = "{2}到{1}个字符")]
        [Required(ErrorMessage = "必填", AllowEmptyStrings = true)]
        [Display(Name = "线路")]
        public string Circuit { get; set; }
        /// <summary>
        /// 播放地址
        /// </summary>
        [Column("play_address", TypeName = "text")]
        [StringLength(255, MinimumLength = 0, ErrorMessage = "{2}到{1}个字符")]
        [Required(ErrorMessage = "必填", AllowEmptyStrings = true)]
        [Display(Name = "播放地址")]
        public string PlayAddress { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        [Column("add_time", TypeName = "bigint")]
        [Required(ErrorMessage = "必填", AllowEmptyStrings = true)]
        [Display(Name = "添加时间")]
        public long AddTime { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        [Column("update_time", TypeName = "bigint")]
        [Display(Name = "更新时间")]
        public long? UpdateTime { get; set; }
    }
}