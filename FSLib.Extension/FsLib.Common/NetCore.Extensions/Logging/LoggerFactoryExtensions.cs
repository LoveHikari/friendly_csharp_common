using Microsoft.AspNetCore.Builder;
using NLog.Extensions.Logging;

namespace System.NetCore.Extensions.Logging
{
    /// <summary>
    /// nlog扩展
    /// </summary>
    public static class LoggerFactoryExtensions
    {
        /// <summary>
        /// 添加nlog
        /// </summary>
        /// <param name="loggerFactory"></param>
        /// <param name="nlogName">nlog文件名字</param>
        public static void UseNlog(this Microsoft.Extensions.Logging.ILoggerFactory loggerFactory, string nlogName = "nlog.config")
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);//这是为了防止中文乱码
            loggerFactory.AddNLog();//添加NLog
            NLog.Web.NLogBuilder.ConfigureNLog(System.IO.Path.Combine(System.Environment.CurrentDirectory, nlogName));//读取Nlog配置文件
        }
    }
}