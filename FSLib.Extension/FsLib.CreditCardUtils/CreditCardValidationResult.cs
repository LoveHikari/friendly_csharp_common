namespace FsLib.CreditCardUtils
{
    /// <summary>
    /// 信用卡验证结果
    /// </summary>
    public class CreditCardValidationResult
    {
        /// <summary>
        /// 卡号格式
        /// </summary>
        public CardNumberFormat CardNumberFormat { get; set; }
        /// <summary>
        /// 卡类型
        /// </summary>
        public string[] CardTypes { get; set; }
    }
}