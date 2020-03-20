using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FsLib.EfCore.Domain;

namespace Miko.Domain.Entity
{
    /// <summary>
    /// 账号表
    /// </summary>
    [Serializable, Table("m_account")]
    public sealed partial class MAccount : IAggregateRoot
    {
        #region Model
        /// <summary>
        /// 
        /// </summary>
        [Key]
        [Column("id", TypeName = "int")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage = "必填", AllowEmptyStrings = true)]
        [Display(Name = "id")]
        public int Id { get; set; }
        /// <summary>
        /// 账号
        /// </summary>
        [Column("account_no", TypeName = "int")]
        [Required(ErrorMessage = "必填", AllowEmptyStrings = true)]
        [Display(Name = "账号")]
        public int AccountNo { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        [Column("nickname", TypeName = "varchar(50)")]
        [StringLength(50, MinimumLength = 0, ErrorMessage = "{2}到{1}个字符")]
        [Display(Name = "昵称")]
        public string Nickname { get; set; }

        /// <summary>
        /// 真实姓名
        /// </summary>
        [Column("real_name", TypeName = "varchar(50)")]
        [StringLength(50, MinimumLength = 0, ErrorMessage = "{2}到{1}个字符")]
        [Display(Name = "真实姓名")]
        public string RealName { get; set; }
        /// <summary>
        /// 证件号码
        /// </summary>
        [Column("certificate_no", TypeName = "varchar(50)")]
        [StringLength(50, MinimumLength = 0, ErrorMessage = "{2}到{1}个字符")]
        [Display(Name = "证件号码")]
        public string CertificateNo { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        [Column("phone_mob", TypeName = "varchar(50)")]
        [StringLength(50, MinimumLength = 0, ErrorMessage = "{2}到{1}个字符")]
        [Display(Name = "手机号")]
        public string PhoneMob { get; set; }
        /// <summary>
        /// 邀请码
        /// </summary>
        [Column("referee_code", TypeName = "varchar(50)")]
        [StringLength(50, MinimumLength = 0, ErrorMessage = "{2}到{1}个字符")]
        [Display(Name = "邀请码")]
        public string RefereeCode { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        [Column("password", TypeName = "varchar(50)")]
        [StringLength(50, MinimumLength = 0, ErrorMessage = "{2}到{1}个字符")]
        [Display(Name = "密码")]
        public string Password { get; set; }

        /// <summary>
        /// 注册ip
        /// </summary>
        [Column("register_ip", TypeName = "varchar(50)")]
        [Required(ErrorMessage = "必填", AllowEmptyStrings = true)]
        [StringLength(50, MinimumLength = 0, ErrorMessage = "{2}到{1}个字符")]
        [Display(Name = "注册ip")]
        public string RegisterIp { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        [Column("add_time", TypeName = "bigint")]
        [Required(ErrorMessage = "必填", AllowEmptyStrings = true)]
        [Display(Name = "添加时间")]
        public long AddTime { get; set; }
        /// <summary>
        /// 状态,1：启用，0：禁用，2：限制登录，3：限制交易
        /// </summary>
        [Column("status", TypeName = "int")]
        [Required(ErrorMessage = "必填", AllowEmptyStrings = true)]
        [Display(Name = "状态")]
        public int Status { get; set; }
        /// <summary>
        /// 最后登录时间
        /// </summary>
        [Column("last_login_time", TypeName = "bigint")]
        [Display(Name = "最后登录时间")]
        public long? LastLoginTime { get; set; }
        /// <summary>
        /// QQ
        /// </summary>
        [Column("im_qq", TypeName = "varchar(50)")]
        [StringLength(50, MinimumLength = 0, ErrorMessage = "{2}到{1}个字符")]
        [Display(Name = "QQ")]
        public string ImQq { get; set; }

        #endregion Model
    }
}