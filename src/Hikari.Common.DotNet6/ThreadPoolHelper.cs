/******************************************************************************************************************
 * 
 * 
 * 标  题：线程池帮助类(版本：Version1.0.0)
 * 作  者：YuXiaoWei
 * 日  期：2016/11/01
 * 修  改：
 * 参  考： 
 * 说  明：调用BeginThreadPool()方法开始建立线程池并检查线程池，调用ThreadPoolEndFlag属性获得线程池状态，应在线程池使用之前调用
 * 备  注：暂无...
 * 
 * 
 * ***************************************************************************************************************/
/****************************************
 * 调用示列：
 *           ThreadPoolHelper.BeginThreadPool();
 *           ThreadPool.QueueUserWorkItem(new WaitCallback(test), obj);
 *           ThreadPoolHelper.CheckThreadPool();
 *          while (true)
 *          {
 *              Console.Out.WriteLine(ThreadPoolHelper.ThreadPoolEndFlag);
 *          }
 * ****************************************/
namespace Hikari.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class ThreadPoolHelper
    {
        private static RegisteredWaitHandle? _rhw;
        /// <summary>
        /// 线程池全部结束标志，结束为true
        /// </summary>
        public static bool ThreadPoolEndFlag { get; set; }
        /// <summary>
        /// 可用线程数
        /// </summary>
        public static int WorkerThreads { get; set; }
        /// <summary>
        /// 最大线程数
        /// </summary>
        public static int MaxWordThreads { get; set; }
        /// <summary>
        /// 开始线程池
        /// </summary>
        public static void BeginThreadPool()
        {
            BeginThreadPool(2, 10);
        }
        /// <summary>
        /// 开始线程池
        /// </summary>
        /// <param name="minThreads">最小线程数</param>
        /// <param name="maxThreads">最大线程数</param>
        public static void BeginThreadPool(int minThreads, int maxThreads)
        {
            
            ThreadPool.SetMaxThreads(maxThreads, maxThreads);
            ThreadPool.SetMinThreads(minThreads, minThreads);
            //_rhw = ThreadPool.RegisterWaitForSingleObject(new AutoResetEvent(false), CheckThreadPool, null, 1000, false);
        }
        /// <summary>
        /// 开始检查线程池
        /// </summary>
        public static void CheckThreadPool()
        {
            //检查线程是否结束
            _rhw = ThreadPool.RegisterWaitForSingleObject(new AutoResetEvent(false), CheckThreadPool, null, 1000, false);
        }

        /// <summary>
        /// 检查线程池的方法
        /// </summary>
        /// <param name="state"></param>
        /// <param name="timeout"></param>
        private static void CheckThreadPool(object? state, bool timeout)
        {
            ThreadPool.GetAvailableThreads(out var workerThreads, out _);
            ThreadPool.GetMaxThreads(out var maxWordThreads, out _);
            WorkerThreads = workerThreads;
            MaxWordThreads = maxWordThreads;
            System.Diagnostics.Debug.WriteLine($"现在是{DateTime.Now}，可用线程数为{workerThreads}，最大线程数为{maxWordThreads}");
            //当可用的线数与池程池最大的线程相等时表示线程池中所有的线程已经完成
            if (workerThreads == maxWordThreads)
            {
                //当执行此方法后CheckThreadPool将不再执行
                _rhw?.Unregister(null);
                //此处加入所有线程完成后的处理代码
                System.Diagnostics.Debug.WriteLine("所有线程结束!");
                ThreadPoolEndFlag = true;
            }

        }
    }
}