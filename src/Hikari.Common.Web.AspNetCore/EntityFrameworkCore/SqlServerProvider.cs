using Microsoft.EntityFrameworkCore;

namespace Hikari.Common.Web.AspNetCore.EntityFrameworkCore;

internal class SqlServerProvider
{
    public static DbContextOptionsBuilder Use(DbContextOptionsBuilder options, string connectionString, string assemblyName)
    {
        return options.UseSqlServer(connectionString,
                    builder => { builder.MigrationsAssembly(assemblyName).UseRelationalNulls(); });
    }
}