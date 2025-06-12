using Microsoft.EntityFrameworkCore;

namespace Hikari.Common.EntityFrameworkCore.DatabaseProvider;

internal class MySqlProvider
{
    public static DbContextOptionsBuilder Use(DbContextOptionsBuilder options, string connectionString, string assemblyName)
    {
        return options.UseMySQL(connectionString,
            builder =>
            {
                builder.MigrationsAssembly(assemblyName).UseRelationalNulls();
                //builder.ServerVersion(new Version(5, 7, 17), ServerType.MySql);
            });
    }
}