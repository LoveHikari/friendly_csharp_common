using System;
using Hikari.Common.DateTimeExt;

namespace Hikari.Common.IdGenerator;
/// <summary>
/// Twitter的分布式自增ID雪花算法
/// 调用案例：
/// IdWorker idworker = IdWorker.Singleton;
/// var v = idworker.nextId();
/// <remarks>https://www.cnblogs.com/zhaoshujie/p/12010052.html</remarks>
/// </summary>
public class IdWorker
{
    //起始的时间戳
    private static readonly long START_STMP = 1480166465631L;

    //每一部分占用的位数
    private static readonly int SEQUENCE_BIT = 12; //序列号占用的位数
    private static readonly int MACHINE_BIT = 5;   //机器标识占用的位数
    private static readonly int DATACENTER_BIT = 5;//数据中心占用的位数

    //每一部分的最大值
    private static readonly long MAX_DATACENTER_NUM = -1L ^ (-1L << DATACENTER_BIT);
    private static readonly long MAX_MACHINE_NUM = -1L ^ (-1L << MACHINE_BIT);
    private static readonly long MAX_SEQUENCE = -1L ^ (-1L << SEQUENCE_BIT);

    //每一部分向左的位移
    private static readonly int MACHINE_LEFT = SEQUENCE_BIT;
    private static readonly int DATACENTER_LEFT = SEQUENCE_BIT + MACHINE_BIT;
    private static readonly int TIMESTMP_LEFT = DATACENTER_LEFT + DATACENTER_BIT;

    private readonly long _datacenterId = 1;  //数据中心
    private readonly long _machineId = 1;     //机器标识
    private long _sequence = 0L; //序列号
    private long _lastStmp = -1L;//上一次时间戳

    #region 单例:完全懒汉
    private static readonly Lazy<IdWorker> Lazy = new Lazy<IdWorker>(() => new IdWorker());
    /// <summary>
    /// 单例:完全懒汉
    /// </summary>
    public static IdWorker Singleton => Lazy.Value;

    private IdWorker() { }
    #endregion
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="cid">数据中心</param>
    /// <param name="mid">机器标识</param>
    /// <exception cref="Exception"></exception>
    public IdWorker(long cid, long mid)
    {
        if (cid > MAX_DATACENTER_NUM || cid < 0) throw new Exception($"中心Id应在(0,{MAX_DATACENTER_NUM})之间");
        if (mid > MAX_MACHINE_NUM || mid < 0) throw new Exception($"机器Id应在(0,{MAX_MACHINE_NUM})之间");
        _datacenterId = cid;
        _machineId = mid;
    }

    /// <summary>
    /// 产生下一个ID
    /// </summary>
    /// <returns></returns>
    public long NextId()
    {
        long currStmp = GetNewstmp();
        if (currStmp < _lastStmp) throw new Exception("时钟倒退，Id生成失败！");

        if (currStmp == _lastStmp)
        {
            //相同毫秒内，序列号自增
            _sequence = (_sequence + 1) & MAX_SEQUENCE;
            //同一毫秒的序列数已经达到最大
            if (_sequence == 0L) currStmp = GetNextMill();
        }
        else
        {
            //不同毫秒内，序列号置为0
            _sequence = 0L;
        }

        _lastStmp = currStmp;

        return (currStmp - START_STMP) << TIMESTMP_LEFT       //时间戳部分
                      | _datacenterId << DATACENTER_LEFT       //数据中心部分
                      | _machineId << MACHINE_LEFT             //机器标识部分
                      | _sequence;                             //序列号部分
    }

    private long GetNextMill()
    {
        long mill = GetNewstmp();
        while (mill <= _lastStmp)
        {
            mill = GetNewstmp();
        }
        return mill;
    }

    private long GetNewstmp()
    {
        return (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
    }
}

//两种测试方法，均为500并发，生成5000个Id：

//Machine1() 模拟1台主机，单例模式获取实例
//Machine5() 模拟5台主机，创建5个实例
//static void Main(string[] args)
//{
//    //Machine1();
//    Machine5();
//    Console.ReadLine();
//}

//static void Machine1()
//{
//    for (int j = 0; j < 500; j++)
//    {
//        Task.Run(() =>
//        {
//            IdWorker idworker = IdWorker.Singleton;
//            for (int i = 0; i < 10; i++)
//            {
//                Console.WriteLine(idworker.nextId());
//            }
//        });
//    }
//}

//static void Machine5()
//{
//    List<IdWorker> workers = new List<IdWorker>();
//    Random random = new Random();
//    for (int i = 0; i < 5; i++)
//    {
//        workers.Add(new IdWorker(1, i + 1));
//    }
//    for (int j = 0; j < 500; j++)
//    {
//        Task.Run(() =>
//        {
//            for (int i = 0; i < 10; i++)
//            {
//                int mid = random.Next(0, 5);
//                Console.WriteLine(workers[mid].nextId());
//            }
//        });
//    }
//}