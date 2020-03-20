using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FsLib.EfCore.Domain;

namespace Miko.Domain.Entity
{
    /// <summary>
    /// 视频表
    /// </summary>
    [Serializable, Table("mac_vod")]
    public sealed partial class MVideo : IAggregateRoot
    {
        [Key]
        [Column("vod_id", TypeName = "int")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [StringLength(10)]
        [Required(ErrorMessage = "必填", AllowEmptyStrings = true)]
        [Display(Name = "id")]
        public int Id { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        [Column("vod_name", TypeName = "varchar(255)")]
        [StringLength(255)]
        [Required(ErrorMessage = "必填", AllowEmptyStrings = true)]
        [Display(Name = "标题")]
        public string Name { get; set; }
        /// <summary>
        /// 图片
        /// </summary>
        [Column("vod_pic", TypeName = "varchar(255)")]
        [StringLength(255)]
        [Required(ErrorMessage = "必填", AllowEmptyStrings = true)]
        [Display(Name = "图片")]
        public string Pic { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        [Column("type_id", TypeName = "smallint")]
        [StringLength(6)]
        [Required(ErrorMessage = "必填", AllowEmptyStrings = true)]
        [Display(Name = "类型")]
        // [ForeignKey(MSort)]
        public short TypeId { get; set; }
        /// <summary>
        /// 大类型
        /// </summary>
        [Column("type_id_1", TypeName = "smallint")]
        [StringLength(6)]
        [Required(ErrorMessage = "必填", AllowEmptyStrings = true)]
        [Display(Name = "大类型")]
        public short TypeId1 { get; set; }
        /// <summary>
        /// 导演
        /// </summary>
        [Column("vod_director", TypeName = "varchar(255)")]
        [StringLength(255)]
        [Required(ErrorMessage = "必填", AllowEmptyStrings = true)]
        [Display(Name = "导演")]
        public string Director { get; set; }
        /// <summary>
        /// 主演
        /// </summary>
        [Column("vod_actor", TypeName = "varchar(255)")]
        [StringLength(255)]
        [Required(ErrorMessage = "必填", AllowEmptyStrings = true)]
        [Display(Name = "主演")]
        public string Actor { get; set; }
        /// <summary>
        /// 地区
        /// </summary>
        [Column("vod_area", TypeName = "varchar(20)")]
        [StringLength(20)]
        [Required(ErrorMessage = "必填", AllowEmptyStrings = true)]
        [Display(Name = "地区")]
        public string Area { get; set; }
        /// <summary>
        /// 语言
        /// </summary>
        [Column("vod_lang", TypeName = "varchar(10)")]
        [StringLength(10)]
        [Required(ErrorMessage = "必填", AllowEmptyStrings = true)]
        [Display(Name = "语言")]
        public string Lang { get; set; }
        /// <summary>
        /// 年份
        /// </summary>
        [Column("vod_year", TypeName = "varchar(10)")]
        [StringLength(10)]
        [Required(ErrorMessage = "必填", AllowEmptyStrings = true)]
        [Display(Name = "年份")]
        public string Year { get; set; }
        /// <summary>
        /// 评分
        /// </summary>
        [Column("vod_score", TypeName = "decimal(3,1)")]
        [StringLength(3)]
        [Required(ErrorMessage = "必填", AllowEmptyStrings = true)]
        [Display(Name = "评分")]
        public decimal Score { get; set; }
        /// <summary>
        /// 所有评分
        /// </summary>
        [Column("vod_score_all", TypeName = "mediumint")]
        [StringLength(8)]
        [Required(ErrorMessage = "必填", AllowEmptyStrings = true)]
        [Display(Name = "所有评分")]
        public int ScoreAll { get; set; }
        ///// <summary>
        ///// 剧情介绍
        ///// </summary>
        //[Column("synopsis", TypeName = "varchar(255)")]
        //[StringLength(255, MinimumLength = 0, ErrorMessage = "{2}到{1}个字符")]
        //[Required(ErrorMessage = "必填", AllowEmptyStrings = true)]
        //[Display(Name = "剧情介绍")]
        //public string Synopsis { get; set; }
        /// <summary>
        /// 人气
        /// </summary>
        [Column("vod_hits", TypeName = "mediumint")]
        [StringLength(8)]
        [Required(ErrorMessage = "必填", AllowEmptyStrings = true)]
        [Display(Name = "人气")]
        public int Hits { get; set; }
        /// <summary>
        /// 日人气
        /// </summary>
        [Column("vod_hits_day", TypeName = "mediumint")]
        [StringLength(8)]
        [Required(ErrorMessage = "必填", AllowEmptyStrings = true)]
        [Display(Name = "日人气")]
        public int HitsDay { get; set; }
        /// <summary>
        /// 周人气
        /// </summary>
        [Column("vod_hits_week", TypeName = "mediumint")]
        [StringLength(8)]
        [Required(ErrorMessage = "必填", AllowEmptyStrings = true)]
        [Display(Name = "周人气")]
        public int HitsWeek { get; set; }
        /// <summary>
        /// 月人气
        /// </summary>
        [Column("vod_hits_month", TypeName = "mediumint")]
        [StringLength(8)]
        [Required(ErrorMessage = "必填", AllowEmptyStrings = true)]
        [Display(Name = "月人气")]
        public int HitsMonth { get; set; }
        /// <summary>
        /// 最新一集备注
        /// </summary>
        [Column("vod_remarks", TypeName = "varchar(255)")]
        [StringLength(100)]
        [Required(ErrorMessage = "必填", AllowEmptyStrings = true)]
        [Display(Name = "最新一集备注")]
        public string Remarks { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        [Column("vod_time_add", TypeName = "int")]
        [StringLength(10)]
        [Required(ErrorMessage = "必填", AllowEmptyStrings = true)]
        [Display(Name = "添加时间")]
        public int AddTime { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        [Column("vod_time", TypeName = "int")]
        [StringLength(10)]
        [Required(ErrorMessage = "必填", AllowEmptyStrings = true)]
        [Display(Name = "更新时间")]
        public int UpdateTime { get; set; }
        /// <summary>
        /// 推荐等级,1~9，为9时为轮播图
        /// </summary>
        [Column("vod_level", TypeName = "tinyint")]
        [StringLength(1)]
        [Required(ErrorMessage = "必填", AllowEmptyStrings = true)]
        [Display(Name = "推荐")]
        public int Level { get; set; }
        /// <summary>
        /// 扩展分类
        /// </summary>
        [Column("vod_class", TypeName = "varchar(255)")]
        [StringLength(255)]
        [Required(ErrorMessage = "必填", AllowEmptyStrings = true)]
        [Display(Name = "扩展分类")]
        public string Class { get; set; }
        /// <summary>
        /// 首字母
        /// </summary>
        [Column("vod_letter")]
        [StringLength(1)]
        [Required(ErrorMessage = "必填", AllowEmptyStrings = true)]
        [Display(Name = "首字母")]
        public string Letter { get; set; }
    }
}