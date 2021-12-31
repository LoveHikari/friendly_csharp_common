using System.Data;
using System.Data.Common;
using System.Reflection;

namespace Hikari.Dapper.Contrib;

internal interface ISqlAdapter<T>
{
    Task<int> InsertAsync(DbConnection conn, T entity, IDbTransaction? dbTransaction);
    int Insert(DbConnection conn, T entity, IDbTransaction? dbTransaction);
}