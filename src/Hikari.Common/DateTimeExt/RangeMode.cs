namespace Hikari.Common.DateTimeExt;

/// <summary>
/// 区间模式
/// </summary>
public enum RangeMode
{
    /// <summary>
    /// 开区间
    /// </summary>
    Open,  // (a,b)
    /// <summary>
    /// 左闭右开区间
    /// </summary>
    CloseOpen, // [a,b)
    /// <summary>
    /// 左开右闭区间
    /// </summary>
    OpenClose, // (a,b]
    /// <summary>
    /// 闭区间
    /// </summary>
    Close, // [a,b]
}