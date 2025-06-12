using Microsoft.EntityFrameworkCore;

namespace Hikari.Common.EntityFrameworkCore.DatabaseProvider
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
        public static DbContextOptionsBuilder SetOptionsBuilder(this DbContextOptionsBuilder options, string connectionString, DbTypeEnum dbTypeName, string assemblyName)
        {
            return dbTypeName switch
            {
                DbTypeEnum.SqlServer => SqlServerProvider.Use(options, connectionString, assemblyName),
                DbTypeEnum.MySql => MySqlProvider.Use(options, connectionString, assemblyName),
                DbTypeEnum.Sqlite => SqliteProvider.Use(options, connectionString, assemblyName),
                DbTypeEnum.Npgsql => NpgsqlProvider.Use(options, connectionString, assemblyName),
                _ => throw new System.Exception("未实现的数据库")
            };

        }
    }
}