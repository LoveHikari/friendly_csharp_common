using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Xml;

namespace System.DBHelper.CrDB
{
    /// <summary>
    /// 跨数据库的操作类，父类，不可new
    /// </summary>
    public abstract class CrDB : IDBHelper
    {

        private string _providerName;

        private string _connectionString;
        private DbCommand _dbCommand;
        /// <summary>
        /// 数据提供者
        /// </summary>
        public string ProviderName
        {
            get => _providerName;
            set => _providerName = value;
        }

        /// <summary>
        /// 连接字符串
        /// </summary>
        public string ConnectionString
        {
            get => _connectionString;
            set => _connectionString = value;
        }
        /// <summary>
        /// 数据库执行过程，只读
        /// </summary>
        public DbCommand DbCommand => _dbCommand;

        /// <summary>
        /// 构造函数，使该类不可new
        /// </summary>
        protected CrDB()
        {

        }

        #region 私有方法

        /// <summary>
        /// 返回数据工厂
        /// </summary>
        /// <returns></returns>
        private DbProviderFactory GetDbProviderFactory()
        {
            DbProviderFactory f;
            switch (_providerName)
            {
#if !NETSTANDARD2_0
                case "System.Data.SqlClient":
                    f = DbProviderFactories.GetFactory("System.Data.SqlClient");
                    break;
                case "System.Data.OracleClient":
                    f = DbProviderFactories.GetFactory("System.Data.OracleClient");
                    break;
                case "System.Data.OleDb":
                    f = DbProviderFactories.GetFactory("System.Data.OleDb");
                    break;
                case "MySql.Data.MySqlClient":
                    f = DbProviderFactories.GetFactory("MySql.Data.MySqlClient");
                    break;
                case "System.Data.SQLite":
                    f = DbProviderFactories.GetFactory("System.Data.SQLite");
                    break;
                case "Npgsql":
                    f = DbProviderFactories.GetFactory("Npgsql");
                    break;
                default:
                    f = DbProviderFactories.GetFactory("System.Data.SqlClient");
                    break;
#else
                case "System.Data.SqlClient":
                    f = System.Data.SqlClient.SqlClientFactory.Instance;
                    break;
                //case "System.Data.OracleClient":
                //    f = DbProviderFactories.GetFactory("System.Data.OracleClient");
                //    break;
                //case "System.Data.OleDb":
                //    f = DbProviderFactories.GetFactory("System.Data.OleDb");
                //    break;
                //case "MySql.Data.MySqlClient":
                //    f = DbProviderFactories.GetFactory("MySql.Data.MySqlClient");
                //    break;
                case "System.Data.SQLite":
                    f = System.Data.SQLite.SQLiteFactory.Instance;
                    break;
                case "Npgsql":
                    f = Npgsql.NpgsqlFactory.Instance;
                    break;
                default:
                    f = System.Data.SqlClient.SqlClientFactory.Instance;
                    break;
#endif
            }
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
        /// <param name="cmdType"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private DbCommand CreateCommand(string sql, CommandType cmdType, List<DBParam> parameters)
        {

            DbCommand command = GetDbProviderFactory().CreateCommand();
            command.Connection = CreateConnection();
            command.CommandText = sql;
            command.CommandType = cmdType;
            if (parameters != null && parameters.Count > 0)
            {
                foreach (DBParam param in parameters)
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
                            if (DateTime.MinValue == (DateTime)param.DbValue)
                            {
                                sqlp.Value = Convert.DBNull;
                            }
                            else
                            {
                                sqlp.Value = param.DbValue;
                            }
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
        /// <param name="cmdtype">命令类型</param>
        /// <param name="parameters">参数</param>
        /// <returns>数据适配器实例</returns>
        private DbDataAdapter CreateAdapter(string sql, CommandType cmdtype, List<DBParam> parameters)
        {
            DbConnection connection = CreateConnection();
            DbCommand command = GetDbProviderFactory().CreateCommand();
            command.Connection = connection;
            command.CommandText = sql;
            command.CommandType = cmdtype;
            if (parameters != null && parameters.Count > 0)
            {
                foreach (DBParam param in parameters)
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
                            if (DateTime.MinValue == (DateTime)param.DbValue)
                            {
                                sqlp.Value = Convert.DBNull;
                            }
                            else
                            {
                                sqlp.Value = param.DbValue;
                            }
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
#if !NETSTANDARD2_0
        /// <summary>
        /// 从XML文件获取数据库连接
        /// </summary>
        protected void GetConnStr(string dbName)
        {
            try
            {
                string str;
                if (string.IsNullOrEmpty(dbName))
                {
                    str = "local";
                }
                else
                {
                    str = dbName.Trim();
                }

                // 加载XML中的，根据数据库名称查找数据库连接
                XmlDocument xmldoc;
                if (System.Web.HttpRuntime.Cache["ConnXml"] == null)
                {
                    string sPath = System.Configuration.ConfigurationManager.AppSettings["dbpath"];
                    if (string.IsNullOrEmpty(sPath))
                    {
                        sPath = System.IO.Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase,
                        "Intel\\dell.xml");

                    }
                    if (!System.IO.File.Exists(sPath))
                    {
                        CreateDell(sPath);
                    }

                    xmldoc = new XmlDocument();
                    xmldoc.Load(sPath);
                    System.Web.HttpRuntime.Cache.Add("ConnXml", xmldoc, null, DateTime.Now.AddHours(1),
                        TimeSpan.Zero,
                        System.Web.Caching.CacheItemPriority.Default, null);
                }
                else
                {
                    xmldoc = (XmlDocument)System.Web.HttpRuntime.Cache["ConnXml"];
                }

                XmlNodeList elemList = xmldoc.GetElementsByTagName(str);
                XmlNode node = elemList[0];
                if (elemList.Count > 0)
                {
                    if (node.Attributes != null)
                    {
                        _connectionString = node.Attributes["ConnStr"].Value;
                        _providerName = node.Attributes["providerName"]?.Value;
                    }
                }
                else
                {
                    Exception ex = new Exception("没有找到指定连接字符串！", new NullReferenceException());
                    LogHelper.WriteError(ex);
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex);
                throw ex;
            }
        }
        /// <summary>
        /// 创建默认dell.xml
        /// </summary>
        /// <param name="path">dell.xml绝对路径</param>
        private void CreateDell(string path)
        {
            string m_dir = System.IO.Path.GetDirectoryName(path);
            if (!System.IO.Directory.Exists(m_dir))
            {
                System.IO.Directory.CreateDirectory(m_dir);
            }
            if (!System.IO.File.Exists(path))
            {
                System.IO.File.Create(path);
            }
            string s = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                        "<SQlConn>" +
                            "<local ConnStr=\"server=121.41.101.4,5533;uid=kidsnet;pwd=1D#g2!hj3kYt4rwg5r#o6hfd7sr@;database=nynet\" providerName=\"System.Data.SqlClient\" />" +
                        "</SQlConn>";
            System.IO.StreamWriter sw = System.IO.File.CreateText(path);
            sw.Write(s);
            sw.Flush();
            sw.Close();
            sw.Dispose();
        }
#endif
        /// <summary>
        /// 获得连接字符串
        /// </summary>
        /// <returns></returns>
        protected void GetConnectionString(string configString)
        {
#if !NETSTANDARD2_0
            System.Configuration.ConnectionStringSettingsCollection configStringCollention = System.Configuration.ConfigurationManager.ConnectionStrings;
            if (configStringCollention == null || configStringCollention.Count <= 0)
            {
                throw new Exception("web.config 中无连接字符串!");
            }
            System.Configuration.ConnectionStringSettings stringSettings = null;
            if (configString == string.Empty)
            {
                stringSettings = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"];
            }
            else
            {
                stringSettings = System.Configuration.ConfigurationManager.ConnectionStrings[configString];
            }
            _providerName = stringSettings.ProviderName;
            _connectionString = stringSettings.ConnectionString;
#else
if (configString == string.Empty)
            {
                _connectionString = ConfigurationManager.Get("ConnectionString");
            }
            else
            {
                _connectionString = ConfigurationManager.AppSettings[configString];
            }
#endif

        }

        /// <summary>
        /// 释放资源
        /// </summary>
        private void Dispose()
        {
            switch (_providerName)
            {
                case "System.Data.SqlClient":

                    break;
                case "System.Data.OracleClient":

                    break;
                case "System.Data.OleDb":

                    break;
                case "MySql.Data.MySqlClient":

                    break;
                case "System.Data.SQLite":
#if !NETSTANDARD2_0
                    System.Data.SQLite.SQLiteConnection.ClearAllPools();//清除連接池之後，數據庫文件才能使用
#endif

                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    break;
                default:

                    break;
            }
        }
        #endregion

        #region List<DBParam>

        #region 执行非查询语句,并返回受影响的记录行数 ExecuteNonQuery(string sql)
        /// <summary>
        /// 执行非查询语句,并返回受影响的记录行数
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns>受影响记录行数</returns>
        public int ExecuteNonQuery(string sql)
        {
            List<DBParam> parameters = new List<DBParam>();
            return ExecuteNonQuery(sql, CommandType.Text, parameters);
        }
        /// <summary>
        /// 执行非查询语句,并返回受影响的记录行数
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="cmdtype">命令类型</param>
        /// <returns>受影响记录行数</returns>
        public int ExecuteNonQuery(string sql, CommandType cmdtype)
        {
            List<DBParam> parameters = new List<DBParam>();
            return ExecuteNonQuery(sql, CommandType.Text, parameters);
        }
        /// <summary>
        /// 执行非查询语句,并返回受影响的记录行数
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>受影响记录行数</returns>
        public int ExecuteNonQuery(string sql, List<DBParam> parameters)
        {
            return ExecuteNonQuery(sql, CommandType.Text, parameters);
        }
        /// <summary>
        ///批量执行SQL语句 
        /// </summary>
        /// <param name="sqlList">SQL列表</param>
        /// <returns></returns>
        public bool ExecuteNonQuery(List<string> sqlList)
        {
            DbConnection con = CreateConnection();
            con.Open();
            bool iserror = false;
            string strerror = "";
            DbTransaction sqlTran = con.BeginTransaction();
            try
            {
                for (int i = 0; i < sqlList.Count; i++)
                {
                    DbCommand command = GetDbProviderFactory().CreateCommand();
                    command.Connection = con;
                    command.CommandText = sqlList[i].ToString();
                    command.Transaction = sqlTran;
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                iserror = true;
                strerror = ex.Message;
                LogHelper.WriteError(ex);
            }
            finally
            {
                if (iserror)
                {
                    sqlTran.Rollback();
                    throw new Exception(strerror);
                }
                else
                {
                    sqlTran.Commit();
                }
                con.Close();
            }
            return true;
        }
        /// <summary>
        /// 执行非查询语句,并返回受影响的记录行数
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="cmdtype">命令类型</param>
        /// <param name="parameters">参数</param>
        /// <returns>受影响记录行数</returns>
        public int ExecuteNonQuery(string sql, CommandType cmdtype, List<DBParam> parameters)
        {
            int result = 0;
            DbCommand command = CreateCommand(sql, cmdtype, parameters);
            try
            {
                command.Connection.Open();
                result = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex);
                throw new Exception(ex.Message);
            }
            finally
            {
                command.Connection.Close();
                command.Connection.Dispose();
                this.Dispose();
            }
            return result;
        }
        #endregion

        #region 执行非查询语句,并返回首行首列的值 ExecuteScalar(string sql)
        /// <summary>
        /// 执行非查询语句,并返回首行首列的值
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns>Object</returns>
        public object ExecuteScalar(string sql)
        {
            List<DBParam> parameters = new List<DBParam>();
            return ExecuteScalar(sql, CommandType.Text, parameters);
        }
        /// <summary>
        /// 执行非查询语句,并返回首行首列的值
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="cmdtype">命令类型</param>
        /// <returns>Object</returns>
        public object ExecuteScalar(string sql, CommandType cmdtype)
        {
            List<DBParam> parameters = new List<DBParam>();
            return ExecuteScalar(sql, CommandType.Text, parameters);
        }
        /// <summary>
        /// 执行非查询语句,并返回首行首列的值
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>Object</returns>
        public object ExecuteScalar(string sql, List<DBParam> parameters)
        {
            return ExecuteScalar(sql, CommandType.Text, parameters);
        }
        /// <summary>
        /// 执行非查询语句,并返回首行首列的值
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="cmdtype">命令类型</param>
        /// <param name="parameters">参数</param>
        /// <returns>Object</returns>
        public object ExecuteScalar(string sql, CommandType cmdtype, List<DBParam> parameters)
        {
            object result = null;
            DbCommand command = CreateCommand(sql, cmdtype, parameters);
            try
            {
                command.Connection.Open();
                result = command.ExecuteScalar();
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex);
                throw ex;
            }
            finally
            {
                command.Connection.Close();
                command.Connection.Dispose();
                this.Dispose();
            }
            return result;
        }
        #endregion

        #region 执行查询，并以DataReader返回结果集  ExecuteReader(string sql)
        /// <summary>
        /// 执行查询，并以DataReader返回结果集
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns>IDataReader</returns>
        public DbDataReader ExecuteReader(string sql)
        {
            List<DBParam> parameters = new List<DBParam>();
            return ExecuteReader(sql, CommandType.Text, parameters);
        }
        /// <summary>
        /// 执行查询，并以DataReader返回结果集
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="cmdtype">命令类型</param>
        /// <returns>IDataReader</returns>
        public DbDataReader ExecuteReader(string sql, CommandType cmdtype)
        {
            List<DBParam> parameters = new List<DBParam>();
            return ExecuteReader(sql, CommandType.Text, parameters);
        }
        /// <summary>
        /// 执行查询，并以DataReader返回结果集
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>IDataReader</returns>
        public DbDataReader ExecuteReader(string sql, List<DBParam> parameters)
        {
            return ExecuteReader(sql, CommandType.Text, parameters);
        }
        /// <summary>
        /// 执行查询，并以DataReader返回结果集
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="cmdtype">命令类型</param>
        /// <param name="parameters">参数</param>
        /// <returns>IDataReader</returns>
        public DbDataReader ExecuteReader(string sql, CommandType cmdtype, List<DBParam> parameters)
        {
            DbDataReader result;
            DbCommand command = CreateCommand(sql, cmdtype, parameters);
            try
            {
                command.Connection.Open();
                result = command.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex);
                throw ex;
            }
            finally
            {
                command.Connection.Close();
                command.Connection.Dispose();
                this.Dispose();
            }
            return result;
        }
        #endregion

        #region 执行查询，并以DataSet返回结果集 ExecuteDataSet(string sql)
        /// <summary>
        /// 执行查询，并以DataSet返回结果集
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns>DataSet</returns>
        public DataSet ExecuteDataSet(string sql)
        {
            List<DBParam> parameters = new List<DBParam>();
            return ExecuteDataSet(sql, CommandType.Text, parameters);
        }
        /// <summary>
        /// 执行查询，并以DataSet返回结果集
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="cmdtype">命令类型</param>
        /// <returns>DataSet</returns>
        public virtual DataSet ExecuteDataSet(string sql, CommandType cmdtype)
        {
            List<DBParam> parameters = new List<DBParam>();
            return ExecuteDataSet(sql, CommandType.Text, parameters);
        }
        /// <summary>
        /// 执行查询，并以DataSet返回结果集
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>DataSet</returns>
        public virtual DataSet ExecuteDataSet(string sql, List<DBParam> parameters)
        {
            return ExecuteDataSet(sql, CommandType.Text, parameters);
        }
        /// <summary>
        /// 执行查询，并以DataSet返回结果集
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="cmdtype">命令类型</param>
        /// <param name="parameters">参数</param>
        /// <returns>DataSet</returns>
        public virtual DataSet ExecuteDataSet(string sql, CommandType cmdtype, List<DBParam> parameters)
        {
            DataSet result = new DataSet();
            IDataAdapter dataAdapter = CreateAdapter(sql, cmdtype, parameters);
            try
            {
                dataAdapter.Fill(result);
                if (string.IsNullOrEmpty(_providerName) || _providerName == "System.Data.SqlClient")
                {
                    _dbCommand = ((System.Data.SqlClient.SqlDataAdapter)dataAdapter).SelectCommand;
                }

            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex);
                throw ex;
            }
            finally
            {
                ((DbDataAdapter)dataAdapter).Dispose();
                this.Dispose();
            }
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
        /// <returns>DataView</returns>
        public DataView ExecuteDataView(string sql)
        {
            List<DBParam> parameters = new List<DBParam>();
            DataView dv = ExecuteDataSet(sql, CommandType.Text, parameters).Tables[0].DefaultView;
            return dv;
        }
        /// <summary>
        /// 执行查询，并以DataView返回结果集
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="cmdtype">命令类型</param>
        /// <returns>DataView</returns>
        public DataView ExecuteDataView(string sql, CommandType cmdtype)
        {
            List<DBParam> parameters = new List<DBParam>();
            DataView dv = ExecuteDataSet(sql, cmdtype, parameters).Tables[0].DefaultView;
            return dv;
        }
        /// <summary>
        /// 执行查询，并以DataView返回结果集
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>DataView</returns>
        public DataView ExecuteDataView(string sql, List<DBParam> parameters)
        {
            DataView dv = ExecuteDataSet(sql, CommandType.Text, parameters).Tables[0].DefaultView;
            return dv;
        }
        /// <summary>
        /// 执行查询，并以DataView返回结果集
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="cmdtype">命令类型</param>
        /// <param name="parameters">参数</param>
        /// <returns>DataView</returns>
        public DataView ExecuteDataView(string sql, CommandType cmdtype, List<DBParam> parameters)
        {
            DataView dv = ExecuteDataSet(sql, cmdtype, parameters).Tables[0].DefaultView;
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
        /// <returns>DataTable</returns>
        public DataTable ExecuteDataTable(string sql)
        {
            List<DBParam> parameters = new List<DBParam>();
            DataTable dt = ExecuteDataSet(sql, CommandType.Text, parameters).Tables[0];
            return dt;
        }
        /// <summary>
        /// 执行查询，并以DataTable返回结果集
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="cmdtype">命令类型</param>
        /// <returns>DataTable</returns>
        public DataTable ExecuteDataTable(string sql, CommandType cmdtype)
        {
            List<DBParam> parameters = new List<DBParam>();
            DataTable dt = ExecuteDataSet(sql, cmdtype, parameters).Tables[0];
            return dt;
        }
        /// <summary>
        /// 执行查询，并以DataTable返回结果集
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>DataTable</returns>
        public DataTable ExecuteDataTable(string sql, List<DBParam> parameters)
        {
            DataTable dt = ExecuteDataSet(sql, CommandType.Text, parameters).Tables[0];
            return dt;
        }
        /// <summary>
        /// 执行查询，并以DataTable返回结果集
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="cmdtype">命令类型</param>
        /// <param name="parameters">参数</param>
        /// <returns>DataTable</returns>
        public DataTable ExecuteDataTable(string sql, CommandType cmdtype, List<DBParam> parameters)
        {
            DataTable dt = ExecuteDataSet(sql, cmdtype, parameters).Tables[0];
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