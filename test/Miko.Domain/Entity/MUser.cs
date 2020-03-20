using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FsLib.EfCore.Domain;

namespace Miko.Domain.Entity
{
    /// <summary>
    /// 会员表
    /// </summary>
    [Serializable, Table("mac_user")]
    public class MUser : IAggregateRoot
    {
        #region Model
        /// <summary>
        /// id
        /// </summary>
        [Key]
        [Column("user_id", TypeName = "int")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage = "必填", AllowEmptyStrings = true)]
        [Display(Name = "id")]
        public int Id { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        [Column("user_name", TypeName = "varchar(30)")]
        [StringLength(30, MinimumLength = 0, ErrorMessage = "{2}到{1}个字符")]
        [Required(ErrorMessage = "必填", AllowEmptyStrings = true)]
        [Display(Name = "用户名")]
        public string UserName { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        [Column("user_nick_name", TypeName = "varchar(30)")]
        [StringLength(30, MinimumLength = 0, ErrorMessage = "{2}到{1}个字符")]
        [Required(ErrorMessage = "必填", AllowEmptyStrings = true)]
        [Display(Name = "昵称")]
        public string NickName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        [Column("user_pwd", TypeName = "varchar(32)")]
        [StringLength(32, MinimumLength = 0, ErrorMessage = "{2}到{1}个字符")]
        [Required(ErrorMessage = "必填", AllowEmptyStrings = true)]
        [Display(Name = "密码")]
        public string Password { get; set; }

        /// <summary>
        /// 注册时间
        /// </summary>
        [Column("user_reg_time", TypeName = "int")]
        [StringLength(10)]
        [Required(ErrorMessage = "必填", AllowEmptyStrings = true)]
        [Display(Name = "注册时间")]
        public long RegTime { get; set; }
        /// <summary>
        /// 状态,1：启用，0：禁用
        /// </summary>
        [Column("user_status", TypeName = "tinyint")]
        [StringLength(1)]
        [Required(ErrorMessage = "必填", AllowEmptyStrings = true)]
        [Display(Name = "状态")]
        public int Status { get; set; }

        #endregion Model
    }
}