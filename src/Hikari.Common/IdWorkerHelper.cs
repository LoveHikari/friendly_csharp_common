using System;

namespace Hikari.Common
{
    /// <summary>
    /// 雪花算法
    /// 调用案例：
    /// IdWorkerHelper helper = new IdWorkerHelper(1);
    /// var v = helper.NextId();
    /// <remarks>https://www.cnblogs.com/zhaoshujie/p/12010052.html</remarks>
    /// </summary>
    public class IdWorkerHelper
    {
        //机器ID
        private readonly long _workerId;
        private readonly long _twepoch; //唯一时间，这是一个避免重复的随机量，自行设定不要大于当前时间戳
        private readonly int _workerIdShift; //机器码数据左移位数，就是后面计数器占用的位数
        private readonly int _timestampLeftShift; //时间戳左移动位数就是机器码和计数器总字节数
        private readonly long _sequenceMask; //一微秒内可以产生计数，如果达到该值则等到下一微妙在进行生成
        
        private long _lastTimestamp;
        private long _sequence;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="workerId">机器码</param>
        public IdWorkerHelper(long workerId)
        {
            int workerIdBits = 4; // 机器码字节数。4个字节用来保存机器码(定义为Long类型会出现，最大偏移64位，所以左移64位没有意义)
            long maxWorkerId = -1L ^ -1L << workerIdBits;  // 最大机器ID
            int sequenceBits = 10;  //计数器字节数，10个字节用来保存计数码

            _twepoch = 687888001020L;
            _sequence = 0L;
            _workerIdShift = sequenceBits;
            _timestampLeftShift = sequenceBits + workerIdBits;
            _sequenceMask = -1L ^ -1L << sequenceBits;
            _lastTimestamp = -1L;

            if (workerId > maxWorkerId || workerId < 0)
                throw new Exception($"worker Id can't be greater than {workerId} or less than 0 ");
            _workerId = workerId;
        }
        /// <summary>
        /// 下一个id
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public long NextId()
        {
            lock (this)
            {
                long timestamp = TimeGen();
                if (this._lastTimestamp == timestamp)
                { //同一微妙中生成ID
                    _sequence = (_sequence + 1) & _sequenceMask; //用&运算计算该微秒内产生的计数是否已经到达上限
                    if (_sequence == 0)
                    {
                        //一微妙内产生的ID计数已达上限，等待下一微妙
                        timestamp = TillNextMillis(this._lastTimestamp);
                    }
                }
                else
                { //不同微秒生成ID
                    this._sequence = 0; //计数清0
                }
                if (timestamp < _lastTimestamp)
                { //如果当前时间戳比上一次生成ID时时间戳还小，抛出异常，因为不能保证现在生成的ID之前没有生成过
                    throw new Exception($"Clock moved backwards.  Refusing to generate id for {this._lastTimestamp - timestamp} milliseconds");
                }
                this._lastTimestamp = timestamp; //把当前时间戳保存为最后生成ID的时间戳
                long nextId = (timestamp - _twepoch << _timestampLeftShift) | _workerId << _workerIdShift | _sequence;
                return nextId;
            }
        }

        /// <summary>
        /// 获取下一微秒时间戳
        /// </summary>
        /// <param name="lastTimestamp"></param>
        /// <returns></returns>
        private long TillNextMillis(long lastTimestamp)
        {
            long timestamp = TimeGen();
            while (timestamp <= lastTimestamp)
            {
                timestamp = TimeGen();
            }
            return timestamp;
        }

        /// <summary>
        /// 生成当前时间戳
        /// </summary>
        /// <returns></returns>
        private long TimeGen()
        {
            return DateTime.Now.ToUnixTimeMilliseconds();
        }
    }
}