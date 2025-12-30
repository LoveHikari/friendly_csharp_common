using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace Hikari.Common.EntityFrameworkCore.DatabaseProvider;

internal class MySqlProvider
{
    public static DbContextOptionsBuilder Use(DbContextOptionsBuilder options, string connectionString, string assemblyName)
    {
        return options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 4, 6)),
            builder =>
            {
               builder.MigrationsAssembly(assemblyName).UseRelationalNulls();
            });
    }
}