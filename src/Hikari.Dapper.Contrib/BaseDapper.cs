using Dapper;
using Dapper.Contrib.Extensions;
using System.Data;
using System.Data.Common;
using System.Reflection;
using System.Text;

namespace Hikari.Dapper.Contrib;

public class BaseDapper<T> where T : class, new()
{
    /// <summary>
    /// 连接字符串
    /// </summary>
    private readonly string _connectionString;
    private readonly DbProviderEnum? _dbProvider;  // 数据库提供者
    private readonly string _tableName;

    private IDbTransaction? _dbTransaction;  // 事务

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="connectionString">连接字符串</param>
    /// <param name="dbProvider">数据库提供者</param>
    public BaseDapper(string connectionString = "", DbProviderEnum? dbProvider = null)
    {
        _connectionString = string.IsNullOrWhiteSpace(connectionString) ? DapperMap.ConnectionString : connectionString;

        _dbProvider = dbProvider ?? DapperMap.DbProvider;

        var type = typeof(T);
        var table = type.GetCustomAttribute<TableAttribute>();
        _tableName = table != null ? table.Name : type.Name;

    }

    /// <summary>
    /// 创建数据库连接
    /// </summary>
    /// <returns></returns>
    public DbConnection CreateConnection()
    {
        DbConnection con = GetDbProviderFactory().CreateConnection()!;
        con.ConnectionString = _connectionString;
        return con;
    }
    /// <summary>
    /// 返回数据工厂
    /// </summary>
    /// <returns></returns>
    private DbProviderFactory GetDbProviderFactory()
    {
        DbProviderFactory f = _dbProvider switch
        {
            DbProviderEnum.SqlServer => System.Data.SqlClient.SqlClientFactory.Instance,
            DbProviderEnum.Oracle => System.Data.OracleClient.OracleClientFactory.Instance,
            DbProviderEnum.MySql => MySql.Data.MySqlClient.MySqlClientFactory.Instance,
            DbProviderEnum.Sqlite => System.Data.SQLite.SQLiteFactory.Instance,
            DbProviderEnum.Npgsql => Npgsql.NpgsqlFactory.Instance,
            _ => System.Data.SqlClient.SqlClientFactory.Instance
        };
        return f;
    }

    /// <summary>
    /// 查询数据
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<T> GetAsync(int id)
    {
        await using var conn = CreateConnection();
        var entity = await conn.GetAsync<T>(id, _dbTransaction);
        return entity;
    }
    /// <summary>
    /// 查询数据
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public T Get(int id)
    {
        using var conn = CreateConnection();
        var entity = conn.Get<T>(id, _dbTransaction);
        return entity;
    }
    /// <summary>
    /// 查询数据
    /// </summary>
    /// <returns></returns>
    public async Task<List<T>> GetAllAsync()
    {
        await using var conn = CreateConnection();
        var entitys = await conn.GetAllAsync<T>(_dbTransaction);
        return entitys.ToList();
    }
    /// <summary>
    /// 查询数据
    /// </summary>
    /// <returns></returns>
    public List<T> GetAll()
    {
        using var conn = CreateConnection();
        var entitys = conn.GetAll<T>(_dbTransaction);
        return entitys.ToList();
    }
    /// <summary>
    /// 查询数据
    /// </summary>
    /// <param name="param">查询条件</param>
    /// <returns></returns>
    public async Task<T?> GetAsync(object? param = null)
    {
        var entitys = await GetListAsync(param);
        return entitys.FirstOrDefault();
    }
    /// <summary>
    /// 查询数据
    /// </summary>
    /// <param name="param">查询条件</param>
    /// <returns></returns>
    public T? Get(object? param = null)
    {
        var entitys = GetList(param);
        return entitys.FirstOrDefault();
    }
    /// <summary>
    /// 查询数据
    /// </summary>
    /// <param name="param">查询条件</param>
    /// <returns></returns>
    public async Task<List<T>> GetListAsync(object? param = null)
    {
        await using var conn = CreateConnection();
        string sql = $"select * from {_tableName} where 1=1";
        if (param != null)
        {
            // 获得此模型的公共属性
            PropertyInfo[] propertys = param.GetType().GetProperties();
            //遍历该对象的所有属性 
            foreach (PropertyInfo pi in propertys)
            {
                string tempName = pi.Name;//将属性名称赋值给临时变量
                sql += " AND " + tempName + "=@" + tempName;
            }
        }

        var v = await conn.QueryAsync<T>(sql, param, _dbTransaction);
        return v.ToList();
    }
    /// <summary>
    /// 查询数据
    /// </summary>
    /// <param name="param">查询条件</param>
    /// <returns></returns>
    public List<T> GetList(object? param = null)
    {
        using var conn = CreateConnection();
        string sql = $"select * from {_tableName} where 1=1";
        if (param != null)
        {
            // 获得此模型的公共属性
            PropertyInfo[] propertys = param.GetType().GetProperties();
            //遍历该对象的所有属性 
            foreach (PropertyInfo pi in propertys)
            {
                string tempName = pi.Name;//将属性名称赋值给临时变量
                sql += " AND " + tempName + "=@" + tempName;
            }
        }

        return conn.Query<T>(sql, param, _dbTransaction).ToList();
    }


    /// <summary>
    /// 添加数据
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public async Task<int> InsertAsync(T entity)
    {
        await using var conn = CreateConnection();

        int id = await conn.InsertAsync(entity, _dbTransaction);
        return id;
    }
    /// <summary>
    /// 添加数据
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public int Insert(T entity)
    {
        using var conn = CreateConnection();

        int id = (int)conn.Insert(entity, _dbTransaction);
        return id;
    }
    /// <summary>
    /// 更新数据
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public async Task<bool> UpdateAsync(T entity)
    {
        await using var conn = CreateConnection();
        return await conn.UpdateAsync(entity, _dbTransaction);
    }
    /// <summary>
    /// 更新数据
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public bool Update(T entity)
    {
        using var conn = CreateConnection();
        return conn.Update(entity, _dbTransaction);
    }
    /// <summary>
    /// 删除数据
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public async Task<bool> DeleteAsync(T entity)
    {
        await using var conn = CreateConnection();
        return await conn.DeleteAsync(entity, _dbTransaction);
    }
    /// <summary>
    /// 删除数据
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public bool Delete(T entity)
    {
        using var conn = CreateConnection();
        return conn.Delete(entity, _dbTransaction);
    }
    /// <summary>
    /// 是否存在
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    public async Task<bool> AnyAsync(object? param = null)
    {
        await using var conn = CreateConnection();
        string sql = "select count(*) from dt_company where 1=1 ";
        if (param != null)
        {
            // 获得此模型的公共属性
            PropertyInfo[] propertys = param.GetType().GetProperties();
            //遍历该对象的所有属性 
            foreach (PropertyInfo pi in propertys)
            {
                string tempName = pi.Name;//将属性名称赋值给临时变量
                sql += " AND " + tempName + "=@" + tempName;
            }
        }

        var c = await conn.ExecuteScalarAsync<int>(sql, param);
        return c > 0;
    }
    /// <summary>
    /// 是否存在
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    public bool Any(object? param = null)
    {
        using var conn = CreateConnection();
        string sql = "select count(*) from dt_company where 1=1 ";
        if (param != null)
        {
            // 获得此模型的公共属性
            PropertyInfo[] propertys = param.GetType().GetProperties();
            //遍历该对象的所有属性 
            foreach (PropertyInfo pi in propertys)
            {
                string tempName = pi.Name;//将属性名称赋值给临时变量
                sql += " AND " + tempName + "=@" + tempName;
            }
        }

        var c = (int)conn.ExecuteScalar(sql, param);
        return c > 0;
    }

    /// <summary>
    /// 创建事务
    /// </summary>
    public async Task BeginTransactionAsync()
    {
        _dbTransaction = await CreateConnection().BeginTransactionAsync();
    }
    /// <summary>
    /// 创建事务
    /// </summary>
    public void BeginTransaction()
    {
        _dbTransaction = CreateConnection().BeginTransaction();
    }

    /// <summary>
    /// 事务提交
    /// </summary>
    /// <returns></returns>
    public void Commit()
    {
        _dbTransaction?.Commit();
    }
    /// <summary>
    /// 事务回滚
    /// </summary>
    public void Rollback()
    {
        _dbTransaction?.Rollback();
    }

}