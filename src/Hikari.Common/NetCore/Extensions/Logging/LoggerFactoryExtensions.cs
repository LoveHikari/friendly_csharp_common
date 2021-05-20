using Microsoft.AspNetCore.Builder;

namespace Hikari.Common.NetCore.Extensions.Logging
{
    /// <summary>
    /// nlog扩展
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never), System.ComponentModel.Browsable(false)]
    public static class LoggerFactoryExtensions
    {
        /// <summary>
        /// 添加nlog
        /// </summary>
        /// <param name="app"></param>
        /// <param name="nlogName">nlog文件名字</param>
        public static void UseNlog(this IApplicationBuilder app, string nlogName = "nlog.config")
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);//这是为了防止中文乱码
            NLog.Web.NLogBuilder.ConfigureNLog(System.IO.Path.Combine(System.Environment.CurrentDirectory, nlogName));//读取Nlog配置文件
        }
    }
}