using Microsoft.AspNetCore.Builder;
using NLog;
using NLog.Web;

namespace Hikari.Common.Web.AspNetCore.Logging
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
        public static void UseNlog(this IApplicationBuilder app)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);//这是为了防止中文乱码
            // NLog.Web.NLogBuilder.ConfigureNLog(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, nlogName));//读取Nlog配置文件
            NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();//读取Nlog配置文件
        }
    }
}