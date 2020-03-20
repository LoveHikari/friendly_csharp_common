namespace FsLib.CreditCardUtils
{
    /// <summary>
    /// 卡号格式
    /// </summary>
    public enum CardNumberFormat
    {
        /// <summary>
        /// 无
        /// </summary>
        None = 0,
        /// <summary>
        /// 有效_仅Luhn
        /// </summary>
        Valid_LuhnOnly = 100,
        /// <summary>
        /// 有效_BIN检验
        /// </summary>
        Valid_BINTest = 101,
        /// <summary>
        /// 无效_BadString格式
        /// </summary>
        Invalid_BadStringFormat = 200,
        /// <summary>
        /// 无效_Luhn失败
        /// </summary>
        Invalid_LuhnFailure = 201
    }
}