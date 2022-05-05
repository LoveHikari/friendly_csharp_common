using System;
using System.Collections.Generic;

namespace XUnitTestProject1;

/// <summary>
///  水电通知单数据传输对象
/// </summary>
public class WaterPowerNoticDto
{
    /// <summary>
    /// 企业id
    /// </summary>
    public int CompanyId { get; set; }
    /// <summary>
    /// 企业名称
    /// </summary>
    public string CompanyName { get; set; }
    /// <summary>
    /// 水费数据
    /// </summary>
    public WaterNotice WaterNoticeInfo { get; set; }

    /// <summary>
    /// 水费通知单
    /// </summary>
    public class WaterNotice
    {
        /// <summary>
        /// 公摊总量
        /// </summary>
        public decimal SharedTotal { get; set; }
        public List<WaterInfo> WaterInfoList { get; set; }
        /// <summary>
        /// 合计金额
        /// </summary>
        public decimal TotalAmount { get; set; }

        public class WaterInfo
        {
            /// <summary>
            /// 水费上月抄表读数
            /// </summary>
            public decimal BeforeMonthBm { get; set; }
            /// <summary>
            /// 水费当月抄表读数
            /// </summary>
            public decimal CurrentMonthBm { get; set; }
            /// <summary>
            /// 水用量
            /// </summary>
            public decimal Used { get; set; }

            /// <summary>
            /// 水单价（元/吨）
            /// </summary>
            public decimal Price { get; set; }
            /// <summary>
            /// 倍率
            /// </summary>
            public double MultiplyingPower { get; set; }
        }
    }
}