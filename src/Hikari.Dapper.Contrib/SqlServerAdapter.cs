using System.Data;
using System.Data.Common;
using System.Reflection;
using System.Text;
using Dapper;
using Dapper.Contrib.Extensions;

namespace Hikari.Dapper.Contrib;

internal class SqlServerAdapter<T> : SqlAdapter<T>
{
    public override async Task<int> InsertAsync(DbConnection conn, T entity, IDbTransaction? dbTransaction)
    {
        string sql = GetInsertSql();
        return await conn.ExecuteScalarAsync<int>(sql, entity, dbTransaction);
    }

    public override int Insert(DbConnection conn, T entity, IDbTransaction? dbTransaction)
    {
        string sql = GetInsertSql();
        return conn.ExecuteScalar<int>(sql, entity, dbTransaction);
    }

    private string GetInsertSql()
    {
        var t = typeof(T);

        var table = t.GetCustomAttribute<TableAttribute>();
        var tableName = table != null ? table.Name : t.Name;

        var stringBuilder = new StringBuilder();
        var sb = new StringBuilder();
        var propertyInfos = t.GetProperties();

        for (int i = 0; i < propertyInfos.Count(); i++)
        {
            PropertyInfo propertyInfo = propertyInfos[i];

            var keys = propertyInfo.GetCustomAttributes(false).OfType<KeyAttribute>();
            if (!keys.Any())
            {
                sb.Append(GetColumnName(propertyInfo));
                stringBuilder.AppendFormat("@{0}", propertyInfo.Name);
                if (i < propertyInfos.Count() - 1)
                {
                    sb.Append(", ");
                    stringBuilder.Append(", ");
                }
            }
        }

        string sql = $"INSERT INTO {tableName} ({sb}) values ({stringBuilder});select @@IDENTITY;";
        return sql;
    }
}