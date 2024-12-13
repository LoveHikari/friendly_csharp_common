using Microsoft.Extensions.DependencyInjection;

namespace Hikari.Mvvm.Ioc;
/// <summary>
/// 依赖注入器
/// </summary>
public class BootStrapper
{
    private static ServiceProvider? _serviceProvider;
    /// <summary>
    /// 获取服务提供者
    /// </summary>
    /// <returns></returns>
    public IServiceCollection Inject()
    {
        
        IServiceCollection services = new ServiceCollection();
        //services.AddEntityFrameworkMySQL().AddDbContext<AppDbContext>(options => options.UseMySQL(connectionStrings, b => b.EnableRetryOnFailure()), ServiceLifetime.Transient);
        //services.AddTransient(typeof(IDbContext), typeof(AppDbContext));
        //services.AddTransient(typeof(IUnitOfWork), typeof(UnitOfWork));
        //services.Scan(selector => selector.FromAssembliesOf(typeof(BaseRepository<>)).AddClasses().AsImplementedInterfaces().WithLifetime(ServiceLifetime.Transient));
        //services.Scan(selector => selector.FromAssembliesOf(typeof(BaseService)).AddClasses().AsImplementedInterfaces().WithLifetime(ServiceLifetime.Transient));

        //services.AddTransient<Analysis.Analysis>();

        _serviceProvider = services.BuildServiceProvider();
        return services;

    }
    /// <summary>
    /// 解析服务
    /// </summary>
    /// <typeparam name="T">服务类型</typeparam>
    /// <returns></returns>
    public static T? Resolve<T>()
    {
        if (_serviceProvider != null)
        {
            return (T?)_serviceProvider.GetService(typeof(T));
        }

        return default;
    }
}