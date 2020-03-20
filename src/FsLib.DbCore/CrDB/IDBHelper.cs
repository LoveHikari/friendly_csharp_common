using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace FsLib.DbCore.CrDB
{
    /// <summary>
    /// 跨数据库的操作类的接口
    /// </summary>
    interface IDBHelper
    {
        #region List<DBParam>

        #region 执行非查询语句,并返回受影响的记录行数 ExecuteNonQuery(string sql)

        /// <summary>
        /// 执行非查询语句,并返回受影响的记录行数
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns>受影响记录行数</returns>
        int ExecuteNonQuery(string sql);

        /// <summary>
        /// 执行非查询语句,并返回受影响的记录行数
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="cmdtype">命令类型</param>
        /// <returns>受影响记录行数</returns>
        int ExecuteNonQuery(string sql, CommandType cmdtype);

        /// <summary>
        /// 执行非查询语句,并返回受影响的记录行数
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>受影响记录行数</returns>
        int ExecuteNonQuery(string sql, List<DBParam> parameters);

        /// <summary>
        ///批量执行SQL语句 
        /// </summary>
        /// <param name="sqlList">SQL列表</param>
        /// <returns></returns>
        bool ExecuteNonQuery(List<string> sqlList);

        /// <summary>
        /// 执行非查询语句,并返回受影响的记录行数
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="cmdtype">命令类型</param>
        /// <param name="parameters">参数</param>
        /// <returns>受影响记录行数</returns>
        int ExecuteNonQuery(string sql, CommandType cmdtype, List<DBParam> parameters);
        #endregion

        #region 执行非查询语句,并返回首行首列的值 ExecuteScalar(string sql)

        /// <summary>
        /// 执行非查询语句,并返回首行首列的值
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns>Object</returns>
        object ExecuteScalar(string sql);

        /// <summary>
        /// 执行非查询语句,并返回首行首列的值
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="cmdtype">命令类型</param>
        /// <returns>Object</returns>
        object ExecuteScalar(string sql, CommandType cmdtype);

        /// <summary>
        /// 执行非查询语句,并返回首行首列的值
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>Object</returns>
        object ExecuteScalar(string sql, List<DBParam> parameters);

        /// <summary>
        /// 执行非查询语句,并返回首行首列的值
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="cmdtype">命令类型</param>
        /// <param name="parameters">参数</param>
        /// <returns>Object</returns>
        object ExecuteScalar(string sql, CommandType cmdtype, List<DBParam> parameters);
        #endregion

        #region 执行查询，并以DataReader返回结果集  ExecuteReader(string sql)

        /// <summary>
        /// 执行查询，并以DataReader返回结果集
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns>IDataReader</returns>
        DbDataReader ExecuteReader(string sql);

        /// <summary>
        /// 执行查询，并以DataReader返回结果集
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="cmdtype">命令类型</param>
        /// <returns>IDataReader</returns>
        DbDataReader ExecuteReader(string sql, CommandType cmdtype);

        /// <summary>
        /// 执行查询，并以DataReader返回结果集
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>IDataReader</returns>
        DbDataReader ExecuteReader(string sql, List<DBParam> parameters);

        /// <summary>
        /// 执行查询，并以DataReader返回结果集
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="cmdtype">命令类型</param>
        /// <param name="parameters">参数</param>
        /// <returns>IDataReader</returns>
        DbDataReader ExecuteReader(string sql, CommandType cmdtype, List<DBParam> parameters);
        #endregion

        #region 执行查询，并以DataSet返回结果集 ExecuteDataSet(string sql)

        /// <summary>
        /// 执行查询，并以DataSet返回结果集
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns>DataSet</returns>
        DataSet ExecuteDataSet(string sql);

        /// <summary>
        /// 执行查询，并以DataSet返回结果集
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="cmdtype">命令类型</param>
        /// <returns>DataSet</returns>
        DataSet ExecuteDataSet(string sql, CommandType cmdtype);

        /// <summary>
        /// 执行查询，并以DataSet返回结果集
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>DataSet</returns>
        DataSet ExecuteDataSet(string sql, List<DBParam> parameters);

        /// <summary>
        /// 执行查询，并以DataSet返回结果集
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="cmdtype">命令类型</param>
        /// <param name="parameters">参数</param>
        /// <returns>DataSet</returns>
        DataSet ExecuteDataSet(string sql, CommandType cmdtype, List<DBParam> parameters);

        /// <summary>
        /// 执行查询,并以DataSet返回指定记录的结果集
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="startIndex">开始索引</param>
        /// <param name="recordCount">显示记录</param>
        /// <returns>DataSet</returns>
        DataSet ExecuteDataSet(string sql, int startIndex, int recordCount);
        #endregion

        #region 执行查询，并以DataView返回结果集   ExecuteDataView(string sql)

        /// <summary>
        /// 执行查询，并以DataView返回结果集
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns>DataView</returns>
        DataView ExecuteDataView(string sql);

        /// <summary>
        /// 执行查询，并以DataView返回结果集
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="cmdtype">命令类型</param>
        /// <returns>DataView</returns>
        DataView ExecuteDataView(string sql, CommandType cmdtype);

        /// <summary>
        /// 执行查询，并以DataView返回结果集
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>DataView</returns>
        DataView ExecuteDataView(string sql, List<DBParam> parameters);

        /// <summary>
        /// 执行查询，并以DataView返回结果集
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="cmdtype">命令类型</param>
        /// <param name="parameters">参数</param>
        /// <returns>DataView</returns>
        DataView ExecuteDataView(string sql, CommandType cmdtype, List<DBParam> parameters);

        /// <summary>
        /// 执行查询,并以DataView返回指定记录的结果集
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="startIndex">开始索引</param>
        /// <param name="recordCount">显示记录</param>
        /// <returns>DataView</returns>
        DataView ExecuteDataView(string sql, int startIndex, int recordCount);
        #endregion

        #region 执行查询，并以DataTable返回结果集   ExecuteDataTable(string sql)

        /// <summary>
        /// 执行查询，并以DataTable返回结果集
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns>DataTable</returns>
        DataTable ExecuteDataTable(string sql);

        /// <summary>
        /// 执行查询，并以DataTable返回结果集
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="cmdtype">命令类型</param>
        /// <returns>DataTable</returns>
        DataTable ExecuteDataTable(string sql, CommandType cmdtype);

        /// <summary>
        /// 执行查询，并以DataTable返回结果集
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>DataTable</returns>
        DataTable ExecuteDataTable(string sql, List<DBParam> parameters);

        /// <summary>
        /// 执行查询，并以DataTable返回结果集
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="cmdtype">命令类型</param>
        /// <param name="parameters">参数</param>
        /// <returns>DataTable</returns>
        DataTable ExecuteDataTable(string sql, CommandType cmdtype, List<DBParam> parameters);

        /// <summary>
        /// 执行查询,并以DataTable返回指定记录的结果集
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="startIndex">开始索引</param>
        /// <param name="recordCount">显示记录</param>
        /// <returns>DataTable</returns>
        DataTable ExecuteDataTable(string sql, int startIndex, int recordCount);

        /// <summary>
        /// 执行查询,返回以空行填充的指定条数记录集
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="sizeCount">显示记录条数</param>
        /// <returns>DataTable</returns>
        DataTable ExecuteDataTable(string sql, int sizeCount);

        #endregion

        #endregion

    }
}