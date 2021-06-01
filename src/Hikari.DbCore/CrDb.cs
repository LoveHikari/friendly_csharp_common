using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using System.Xml;

namespace Hikari.DbCore
{
    /// <summary>
    /// 数据库的操作类
    /// </summary>
    public class CrDb
    {
        /// <summary>
        /// 连接字符串
        /// </summary>
        private readonly string _connectionString;
        /// <summary>
        /// 数据提供者
        /// </summary>
        private readonly DbProviderEnum _dbProvider;

        private readonly DbConnection _dbConnection;

        private DbTransaction _dbTransaction;


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        /// <param name="dbProvider">数据提供者</param>
        public CrDb(string connectionString, DbProviderEnum dbProvider = DbProviderEnum.SqlServer)
        {
            _connectionString = connectionString;
            _dbProvider = dbProvider;
            _dbConnection = CreateConnection();
            _dbConnection.Open();
        }

        #region 私有方法

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
        /// 创建数据库连接
        /// </summary>
        /// <returns></returns>
        private DbConnection CreateConnection()
        {
            DbConnection con = GetDbProviderFactory().CreateConnection();
            con.ConnectionString = _connectionString;
            return con;
        }
        /// <summary>
        /// 创建执行命令对象
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="commandType"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private DbCommand CreateCommand(string sql, CommandType commandType, List<DbParam> parameters)
        {
            DbCommand command = GetDbProviderFactory().CreateCommand();
            command.Connection = _dbConnection;
            command.CommandText = sql;
            command.CommandType = commandType;
            if (_dbTransaction != null)
            {
                command.Transaction = _dbTransaction;
            }
            
            if (parameters is { Count: > 0 })
            {
                foreach (DbParam param in parameters)
                {
                    DbParameter sqlp = GetDbProviderFactory().CreateParameter();
                    sqlp.ParameterName = param.FieldName;
                    sqlp.DbType = param.DbType;
                    sqlp.Size = param.Size;

                    if (param.DbValue == null)
                    {
                        sqlp.Value = Convert.DBNull;
                    }
                    else
                    {
                        if (param.DbValue.GetType().ToString() == "System.DateTime")
                        {
                            sqlp.Value = DateTime.MinValue == (DateTime)param.DbValue ? Convert.DBNull : param.DbValue;
                        }
                        else
                        {
                            sqlp.Value = param.DbValue;
                        }
                    }
                    command.Parameters.Add(sqlp);
                }

            }
            return command;
        }
        /// <summary>
        /// 创建数据适配器
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="commandType">命令类型</param>
        /// <param name="parameters">参数</param>
        /// <returns>数据适配器实例</returns>
        private DbDataAdapter CreateAdapter(string sql, CommandType commandType, List<DbParam> parameters)
        {
            using DbCommand command = GetDbProviderFactory().CreateCommand();
            command.Connection = _dbConnection;
            command.CommandText = sql;
            command.CommandType = commandType;
            if (_dbTransaction is not  null)
            {
                command.Transaction = _dbTransaction;
            }
            if (parameters is { Count: > 0 })
            {
                foreach (DbParam param in parameters)
                {
                    DbParameter sqlp = GetDbProviderFactory().CreateParameter();
                    sqlp.ParameterName = param.FieldName;
                    sqlp.DbType = param.DbType;
                    sqlp.Size = param.Size;
                    sqlp.Direction = param.Direction;

                    if (param.DbValue == null)
                    {
                        sqlp.Value = Convert.DBNull;
                    }
                    else
                    {
                        if (param.DbValue.GetType().ToString() == "System.DateTime")
                        {
                            sqlp.Value = DateTime.MinValue == (DateTime)param.DbValue ? Convert.DBNull : param.DbValue;
                        }
                        else
                        {
                            sqlp.Value = param.DbValue;
                        }
                    }
                    command.Parameters.Add(sqlp);
                }
            }
            DbDataAdapter da = GetDbProviderFactory().CreateDataAdapter();
            da.SelectCommand = command;

            return da;
        }
        #endregion


        /// <summary>
        /// 开始一个事务
        /// </summary>
        /// <returns></returns>
        public async Task BeginTransactionAsync()
        {
            _dbTransaction = await _dbConnection.BeginTransactionAsync();
        }
        /// <summary>
        /// 事务提交
        /// </summary>
        /// <returns></returns>
        public async Task CommitAsync()
        {
            if (_dbTransaction != null)
                await _dbTransaction.CommitAsync();
        }
        /// <summary>
        /// 事务回滚
        /// </summary>
        /// <returns></returns>
        public async Task RollbackAsync()
        {
            if (_dbTransaction != null)
                await _dbTransaction.RollbackAsync();

        }
        #region 执行过程

        #region 执行非查询语句,并返回受影响的记录行数 ExecuteNonQuery(string sql)
        /// <summary>
        /// 执行非查询语句,并返回受影响的记录行数
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="commandType">命令类型</param>
        /// <returns>受影响记录行数</returns>
        public async Task<int> ExecuteNonQueryAsync(string sql, CommandType commandType = CommandType.Text)
        {
            List<DbParam> parameters = new List<DbParam>();
            return await ExecuteNonQueryAsync(sql, parameters, commandType);
        }

        /// <summary>
        /// 执行非查询语句,并返回受影响的记录行数
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">参数</param>
        /// <param name="commandType">命令类型</param>
        /// <returns>受影响记录行数</returns>
        public async Task<int> ExecuteNonQueryAsync(string sql, List<DbParam> parameters, CommandType commandType = CommandType.Text)
        {
            await using DbCommand command = CreateCommand(sql, commandType, parameters);
            int result = await command.ExecuteNonQueryAsync();
            return result;
        }
        #endregion

        #region 执行非查询语句,并返回首行首列的值 ExecuteScalar(string sql)
        /// <summary>
        /// 执行非查询语句,并返回首行首列的值
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="commandType">命令类型</param>
        /// <returns>Object</returns>
        public async Task<object> ExecuteScalarAsync(string sql, CommandType commandType = CommandType.Text)
        {
            List<DbParam> parameters = new List<DbParam>();
            return await ExecuteScalarAsync(sql, parameters, commandType);
        }

        /// <summary>
        /// 执行非查询语句,并返回首行首列的值
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">参数</param>
        /// <param name="commandType">命令类型</param>
        /// <returns>Object</returns>
        public async Task<object> ExecuteScalarAsync(string sql, List<DbParam> parameters, CommandType commandType = CommandType.Text)
        {
            await using DbCommand command = CreateCommand(sql, commandType, parameters);
            var result = await command.ExecuteScalarAsync();

            return result;
        }
        #endregion

        #region 执行查询，并以DataReader返回结果集  ExecuteReader(string sql)
        /// <summary>
        /// 执行查询，并以DataReader返回结果集
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="commandType">命令类型</param>
        /// <returns>IDataReader</returns>
        public async Task<DbDataReader> ExecuteReaderAsync(string sql, CommandType commandType = CommandType.Text)
        {
            List<DbParam> parameters = new List<DbParam>();
            return await ExecuteReaderAsync(sql, parameters, commandType);
        }

        /// <summary>
        /// 执行查询，并以DataReader返回结果集
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">参数</param>
        /// <param name="commandType">命令类型</param>
        /// <returns>IDataReader</returns>
        public async Task<DbDataReader> ExecuteReaderAsync(string sql, List<DbParam> parameters, CommandType commandType = CommandType.Text)
        {
            await using DbCommand command = CreateCommand(sql, commandType, parameters);
            var result = await command.ExecuteReaderAsync(CommandBehavior.CloseConnection);

            return result;
        }
        #endregion

        #region 执行查询，并以DataSet返回结果集 ExecuteDataSet(string sql)

        /// <summary>
        /// 执行查询，并以DataSet返回结果集
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="commandType">命令类型</param>
        /// <returns>DataSet</returns>
        public virtual DataSet ExecuteDataSet(string sql, CommandType commandType = CommandType.Text)
        {
            List<DbParam> parameters = new List<DbParam>();
            return ExecuteDataSet(sql, parameters, commandType);
        }

        /// <summary>
        /// 执行查询，并以DataSet返回结果集
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="commandType">命令类型</param>
        /// <param name="parameters">参数</param>
        /// <returns>DataSet</returns>
        public virtual DataSet ExecuteDataSet(string sql, List<DbParam> parameters, CommandType commandType = CommandType.Text)
        {
            DataSet result = new DataSet();
            using DbDataAdapter dataAdapter = CreateAdapter(sql, commandType, parameters);
            dataAdapter.Fill(result);
            dataAdapter.Dispose();
            return result;
        }
        /// <summary>
        /// 执行查询,并以DataSet返回指定记录的结果集
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="startIndex">开始索引</param>
        /// <param name="recordCount">显示记录</param>
        /// <returns>DataSet</returns>
        public virtual DataSet ExecuteDataSet(string sql, int startIndex, int recordCount)
        {
            DataSet ds = new DataSet();
            DataTable dt = ExecuteDataSet(sql).Tables[0];
            if (startIndex > dt.Rows.Count)
            {
                ds.Tables.Add(dt.Clone());
                return ds;
            }
            DataTable newTable = dt.Clone();
            DataRow[] rows = dt.Select("1=1");
            int topItem = startIndex + recordCount - 1;
            for (int i = startIndex; i < topItem; i++)
            {
                newTable.ImportRow((DataRow)rows[i]);
            }
            ds.Tables.Add(newTable);
            return ds;
        }
        #endregion

        #region 执行查询，并以DataView返回结果集   ExecuteDataView(string sql)

        /// <summary>
        /// 执行查询，并以DataView返回结果集
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="commandType">命令类型</param>
        /// <returns>DataView</returns>
        public DataView ExecuteDataView(string sql, CommandType commandType = CommandType.Text)
        {
            List<DbParam> parameters = new List<DbParam>();
            DataView dv = ExecuteDataSet(sql, parameters, commandType).Tables[0].DefaultView;
            return dv;
        }

        /// <summary>
        /// 执行查询，并以DataView返回结果集
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">参数</param>
        /// <param name="commandType">命令类型</param>
        /// <returns>DataView</returns>
        public DataView ExecuteDataView(string sql, List<DbParam> parameters, CommandType commandType = CommandType.Text)
        {
            DataView dv = ExecuteDataSet(sql, parameters, commandType).Tables[0].DefaultView;
            return dv;
        }
        /// <summary>
        /// 执行查询,并以DataView返回指定记录的结果集
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="startIndex">开始索引</param>
        /// <param name="recordCount">显示记录</param>
        /// <returns>DataView</returns>
        public DataView ExecuteDataView(string sql, int startIndex, int recordCount)
        {
            return ExecuteDataSet(sql, startIndex, recordCount).Tables[0].DefaultView;
        }
        #endregion

        #region 执行查询，并以DataTable返回结果集   ExecuteDataTable(string sql)
        /// <summary>
        /// 执行查询，并以DataTable返回结果集
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="commandType">命令类型</param>
        /// <returns>DataTable</returns>
        public DataTable ExecuteDataTable(string sql, CommandType commandType = CommandType.Text)
        {
            List<DbParam> parameters = new List<DbParam>();
            DataTable dt = ExecuteDataSet(sql, parameters, commandType).Tables[0];
            return dt;
        }
        /// <summary>
        /// 执行查询，并以DataTable返回结果集
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">参数</param>
        /// <param name="commandType">命令类型</param>
        /// <returns>DataTable</returns>
        public DataTable ExecuteDataTable(string sql, List<DbParam> parameters, CommandType commandType = CommandType.Text)
        {
            DataTable dt = ExecuteDataSet(sql, parameters, commandType).Tables[0];
            return dt;
        }
        /// <summary>
        /// 执行查询,并以DataTable返回指定记录的结果集
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="startIndex">开始索引</param>
        /// <param name="recordCount">显示记录</param>
        /// <returns>DataTable</returns>
        public DataTable ExecuteDataTable(string sql, int startIndex, int recordCount)
        {
            return ExecuteDataSet(sql, startIndex, recordCount).Tables[0];
        }
        /// <summary>
        /// 执行查询,返回以空行填充的指定条数记录集
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="sizeCount">显示记录条数</param>
        /// <returns>DataTable</returns>
        public DataTable ExecuteDataTable(string sql, int sizeCount)
        {
            DataTable dt = ExecuteDataSet(sql).Tables[0];
            int b = sizeCount - dt.Rows.Count;
            if (dt.Rows.Count < sizeCount)
            {
                for (int i = 0; i < b; i++)
                {
                    DataRow dr = dt.NewRow();
                    dt.Rows.Add(dr);
                }
            }
            return dt;
        }
        #endregion

        #endregion
    }
}