using Hikari.Common.IdGenerator.Contract;

namespace Hikari.Common.IdGenerator.Core;

/// <summary>
/// 雪花漂移算法（支持数据中心ID和秒级时间戳）
/// </summary> 
internal class SnowWorkerM3 : SnowWorkerM1
{
    /// <summary>
    /// 数据中心ID（默认0）
    /// </summary>
    protected readonly uint DataCenterId = 0;

    /// <summary>
    /// 数据中心ID长度（默认0）
    /// </summary>
    protected readonly byte DataCenterIdBitLength = 0;

    /// <summary>
    /// 时间戳类型（0-毫秒，1-秒），默认0
    /// </summary>
    protected readonly byte TimestampType = 0;


    public SnowWorkerM3(IdGeneratorOptions options) : base(options)
    {
        // 秒级时间戳类型
        TimestampType = options.TimestampType;

        // DataCenter相关
        DataCenterId = options.DataCenterId;
        DataCenterIdBitLength = options.DataCenterIdBitLength;

        if (TimestampType == 1)
        {
            TopOverCostCount = 0;
        }
        _TimestampShift = (byte)(DataCenterIdBitLength + WorkerIdBitLength + SeqBitLength);
    }

    protected override long CalcId(long useTimeTick)
    {
        var result = ((useTimeTick << _TimestampShift) +
            ((long)DataCenterId << DataCenterIdBitLength) +
            ((long)WorkerId << SeqBitLength) +
            (long)_CurrentSeqNumber);

        _CurrentSeqNumber++;
        return result;
    }

    protected override long CalcTurnBackId(long useTimeTick)
    {
        var result = ((useTimeTick << _TimestampShift) +
            ((long)DataCenterId << DataCenterIdBitLength) +
            ((long)WorkerId << SeqBitLength) +
            _TurnBackIndex);

        _TurnBackTimeTick--;
        return result;
    }

    protected override long GetCurrentTimeTick()
    {
        return TimestampType == 0 ?
            (long)(DateTime.UtcNow - BaseTime).TotalMilliseconds :
            (long)(DateTime.UtcNow - BaseTime).TotalSeconds;
    }

}