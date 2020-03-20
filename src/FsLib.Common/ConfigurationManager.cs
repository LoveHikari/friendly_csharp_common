using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace System
{
    /// <summary>
    /// 配置管理器
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never), System.ComponentModel.Browsable(false)]
    public class ConfigurationManager
    {
        private static readonly IConfiguration Config;
        static ConfigurationManager()
        {
            // Microsoft.Extensions.Configuration扩展包提供的
            var builder = new ConfigurationBuilder()
                .SetBasePath(System.Environment.CurrentDirectory)
                .Add(new JsonConfigurationSource { Path = "appsettings.json", Optional = false, ReloadOnChange = true })
                .Add(new JsonConfigurationSource { Path = "appsettings.Development.json", Optional = true, ReloadOnChange = true });

            Config = builder.Build();
        }
        /// <summary>
        /// 获取应用设置
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T GetAppSettings<T>(string key) where T : class, new()
        {
            var appconfig = OptionsConfigurationServiceCollectionExtensions.Configure<T>(new ServiceCollection()
                    .AddOptions(), Config.GetSection(key))
                .BuildServiceProvider()
                .GetService<IOptions<T>>()
                .Value;

            return appconfig;
        }
        /// <summary>
        /// 应用设置
        /// </summary>
        public static IConfiguration AppSettings => Config;

        public static string Get(string key)
        {
            return Config[key];
        }
    }
}
