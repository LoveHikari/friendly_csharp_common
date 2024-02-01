/******************************************************************************************************************
 * 
 * 
 * 说　明： 错误日志输出类(版本：Version1.0.0)
 * 作　者：YuXiaoWei
 * 日　期：2016/08/19
 * 修　改：
 * 参　考：
 * 备　注：
 * 
 * 
 * ***************************************************************************************************************/

namespace Hikari.Common
{
    /// <summary>
    /// 输出log
    /// </summary>
    public class LogHelper
    {
        private static readonly object _locker = new object();

        private static string _loginfoDir => GetLogger("loginfo");
        private static string _logerrorDir => GetLogger("logerror");

        /// <summary>
        /// 输出log
        /// </summary>
        /// <param name="info">消息</param>
        public static void WriteLog(string info)
        {
            lock (_locker)
            {
                string directPath = System.IO.Path.Combine(_loginfoDir, $"{DateTime.Now:yyyy-MM-dd}_LogInfo.log");
                using System.IO.StreamWriter sw = !System.IO.File.Exists(directPath) ? System.IO.File.CreateText(directPath) : System.IO.File.AppendText(directPath);    //判断文件是否存在如果不存在则创建，如果存在则添加。

                //把信息输出到文件
                sw.WriteLine(info);
                sw.WriteLine("***********************************************************************");
                sw.WriteLine();
            }
        }

        /// <summary>
        /// 将异常打印到LOG文件
        /// </summary>
        /// <param name="ex">异常</param>
        public static void WriteError(Exception ex)
        {
            lock (_locker)
            {
                string directPath = System.IO.Path.Combine(_logerrorDir, $"{DateTime.Now:yyyy-MM-dd}_LogError.log");

                using System.IO.StreamWriter sw = !System.IO.File.Exists(directPath) ? System.IO.File.CreateText(directPath) : System.IO.File.AppendText(directPath);    //判断文件是否存在如果不存在则创建，如果存在则添加。

                //把异常信息输出到文件
                sw.WriteLine("当前时间：" + DateTime.Now);
                sw.WriteLine("异常信息：" + ex.Message);
                sw.WriteLine("异常对象：" + ex.Source);
                sw.WriteLine("调用堆栈：\n" + ex.StackTrace.Trim());
                sw.WriteLine("触发方法：" + ex.TargetSite);
                sw.WriteLine("***********************************************************************");
                sw.WriteLine();
            }

        }
        /// <summary>
        /// 将异常打印到LOG文件
        /// </summary>
        /// <param name="ex">异常</param>
        /// <param name="tag">传入标签（这里用于标识函数由哪个线程调用）</param>
        public static void WriteError(Exception ex, string tag)
        {
            lock (_locker)
            {
                string directPath = System.IO.Path.Combine(_logerrorDir, $"{DateTime.Now:yyyy-MM-dd}_LogError.log");

                using System.IO.StreamWriter sw = !System.IO.File.Exists(directPath) ? System.IO.File.CreateText(directPath) : System.IO.File.AppendText(directPath);//判断文件是否存在如果不存在则创建，如果存在则添加。

                //把异常信息输出到文件
                sw.WriteLine(string.Concat('[', DateTime.Now, "] Tag:" + tag));
                sw.WriteLine("异常信息：" + ex.Message);
                sw.WriteLine("异常对象：" + ex.Source);
                sw.WriteLine("调用堆栈：\n" + ex.StackTrace.Trim());
                sw.WriteLine("触发方法：" + ex.TargetSite);
                sw.WriteLine("***********************************************************************");
                sw.WriteLine();
            }
        }
        /// <summary>
        /// 获得log目录
        /// </summary>
        /// <param name="logType"></param>
        /// <returns></returns>
        private static string GetLogger(string logType)
        {
            string logDir;
            if (logType == "loginfo")
            {
                logDir = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs", "LogInfo");  //如果日志文件为空，则默认在Debug目录下新建 Log\LogInfo目录
            }
            else
            {
                logDir = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs", "LogError");  //如果日志文件为空，则默认在Debug目录下新建 Log\LogError目录
            }

            if (!System.IO.Directory.Exists(logDir))   //判断文件夹是否存在，如果不存在则创建
            {
                System.IO.Directory.CreateDirectory(logDir);
            }
            return logDir;
        }
    }
}