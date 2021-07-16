using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Hikari.Common.Web.AspNetCore
{
    /// <summary>
    /// 配置管理器
    /// <see cref="https://blog.csdn.net/hofmann/article/details/118565090"/>
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never), System.ComponentModel.Browsable(false)]
    public static class ConfigurationManager
    {
        private static IConfiguration _configuration;
        static ConfigurationManager()
        {
            // Microsoft.Extensions.Configuration扩展包提供的
            var builder = new ConfigurationBuilder()
                .SetBasePath(System.Environment.CurrentDirectory)
                .Add(new JsonConfigurationSource { Path = "appsettings.json", Optional = false, ReloadOnChange = true })
                .Add(new JsonConfigurationSource { Path = "appsettings.Development.json", Optional = true, ReloadOnChange = true });

            _configuration = builder.Build();

        }
        /// <summary>
        /// 依赖注入添加配置
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void AddConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            _configuration = configuration;
        }
        /// <summary>
        /// 获取应用设置
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T GetAppSettings<T>(string key) where T : class, new()
        {
            var appConfig = new ServiceCollection()
                .AddOptions()
                .Configure<T>(_configuration.GetSection(key))
                .BuildServiceProvider()
                .GetService<IOptions<T>>()
                ?.Value;

            return appConfig;
        }
        ///// <summary>
        ///// 应用设置
        ///// </summary>
        //public static IConfiguration AppSettings => _configuration;
        /// <summary>
        /// 获取应用设置
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Get(string key)
        {
            return _configuration[key];
        }
    }
}
