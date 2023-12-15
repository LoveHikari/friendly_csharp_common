using Microsoft.EntityFrameworkCore;

namespace Hikari.Common.Web.AspNetCore.EntityFrameworkCore;

internal class NpgsqlProvider
{
    public static DbContextOptionsBuilder Use(DbContextOptionsBuilder options, string connectionString, string assemblyName)
    {
        return options.UseNpgsql(connectionString,
                    builder => { builder.MigrationsAssembly(assemblyName).UseRelationalNulls(); });
    }
}