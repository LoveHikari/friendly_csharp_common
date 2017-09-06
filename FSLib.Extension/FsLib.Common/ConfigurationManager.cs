#if NETSTANDARD2_0
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace System
{
    public class ConfigurationManager
    {
        private static readonly IConfiguration Config;
        static ConfigurationManager()
        {
            // Microsoft.Extensions.Configuration扩展包提供的
            var builder = new ConfigurationBuilder()
                .Add(new JsonConfigurationSource {Path = "appsettings.json", Optional = false, ReloadOnChange = true});
            Config = builder.Build();
        }
        public static T GetAppSettings<T>(string key) where T : class, new()
        {
            var appconfig = new ServiceCollection()
                .AddOptions()
                .Configure<T>(Config.GetSection(key))
                .BuildServiceProvider()
                .GetService<IOptions<T>>()
                .Value;

            return appconfig;
        }

        public static IConfiguration AppSettings => Config;

        public static string Get(string key)
        {
            return Config[key];
        }
    }
}
#endif
