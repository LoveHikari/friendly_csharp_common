using System.Data;
using System.Data.Common;
using System.Reflection;
using Dapper;
using Dapper.Contrib.Extensions;

namespace Hikari.Dapper.Contrib;

internal abstract class SqlAdapter<T> : ISqlAdapter<T>
{
    /// <summary>
    /// 通过PropertyInfo找到ColumnAttribute类型的属性注释
    /// </summary>
    /// <param name="property"></param>
    /// <returns></returns>
    public string GetColumnName(PropertyInfo property)
    {
        if (SqlMapper.GetTypeMap(typeof(T)).GetType() != typeof(DefaultTypeMap))
        {
            var proName = property.GetCustomAttributes(false).OfType<ColumnAttribute>().Select(t => t.Name).FirstOrDefault();
            return string.IsNullOrWhiteSpace(proName) ? property.Name : proName;
        }
        else
        {
            return property.Name;
        }

    }

    public abstract Task<int> InsertAsync(DbConnection conn, T entity, IDbTransaction? dbTransaction);

    public abstract int Insert(DbConnection conn, T entity, IDbTransaction? dbTransaction);
}