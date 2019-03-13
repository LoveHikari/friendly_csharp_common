using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FsLib.FreeSql.Infrastructure
{
    /// <summary>
    /// <see cref="ServiceCollection"/> 扩展类
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never), System.ComponentModel.Browsable(false)]
    public static class EntityFrameworkCoreExtensions
    {
        /// <summary>
        /// 注入efCore
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="dbTypeName">数据库类型，SqlServer，MySql</param>
        public static IServiceCollection AddEntityFrameworkCore(this IServiceCollection services, string connectionString, string dbTypeName)
        {
            services.AddDbContextPool<AppDbContext>(options => SetOptionsBuilder(options, connectionString, dbTypeName));
            services.AddTransient(typeof(IDbContext), typeof(AppDbContext));
            services.AddTransient(typeof(IUnitOfWork), typeof(UnitOfWork));

            return services;
        }

        private static DbContextOptionsBuilder SetOptionsBuilder(DbContextOptionsBuilder options, string connectionString, string dbTypeName)
        {
            DbContextOptionsBuilder optionsBuilder = null;
            switch (dbTypeName.ToLower())
            {
                case "sqlserver":
                    optionsBuilder = options.UseSqlServer(connectionString, builder =>
                        {
                            builder.UseRelationalNulls().UseRowNumberForPaging();
                        });
                    break;
                case "mysql":
                    optionsBuilder = options.UseMySQL(connectionString, builder =>
                    {
                        builder.UseRelationalNulls();
                        //builder.ServerVersion(new Version(5, 7, 17), ServerType.MySql);
                    });
                    break;
                case "sqlite":
                    optionsBuilder = options.UseSqlite(connectionString, builder =>
                    {
                        builder.UseRelationalNulls();
                    });
                    break;
                default:
                    throw new System.Exception("未实现的数据库");
            }

            return optionsBuilder;
        }
    }
}