using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace System.NetCore.Extensions.EntityFrameworkCore
{
    /// <summary>
    /// <see cref="DbContextOptionsBuilder"/> 扩展类
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never), System.ComponentModel.Browsable(false)]
    public static class DbContextOptionsBuilderExtensions
    {
        /// <summary>
        /// 设置数据库配置
        /// </summary>
        /// <param name="options">数据上下文选项构建器</param>
        /// <param name="connectionString">连接字符串</param>
        /// <param name="dbTypeName">数据库类型名称SqlServer，MySql，Sqlite，Npgsql</param>
        /// <param name="assemblyName">数据模型所在的程序集名称</param>
        /// <returns></returns>
        public static DbContextOptionsBuilder SetOptionsBuilder(this DbContextOptionsBuilder options, string connectionString, string dbTypeName,string assemblyName)
        {
            DbContextOptionsBuilder optionsBuilder = null;
            switch (dbTypeName.ToLower())
            {
                case "SqlServer":
                    optionsBuilder = options.UseSqlServer(connectionString, builder =>
                        {
                            builder.MigrationsAssembly(assemblyName).UseRelationalNulls().UseRowNumberForPaging();
                        });
                    break;
                case "MySql":
                    optionsBuilder = options.UseMySQL(connectionString, builder =>
                    {
                        builder.MigrationsAssembly(assemblyName).UseRelationalNulls();
                        //builder.ServerVersion(new Version(5, 7, 17), ServerType.MySql);
                    });
                    break;
                case "Sqlite":
                    optionsBuilder = options.UseSqlite(connectionString, builder =>
                    {
                        builder.MigrationsAssembly(assemblyName).UseRelationalNulls();
                    });
                    break;
                case "Npgsql":
                    optionsBuilder = options.UseNpgsql(connectionString, builder =>
                    {
                        builder.MigrationsAssembly(assemblyName).UseRelationalNulls();
                    });
                    break;
                default:
                    throw new System.Exception("未实现的数据库");
            }

            return optionsBuilder;
        }
    }
}