#if !NETSTANDARD2_0
/******************************************************************************************************************
 * 
 * 
 * 标  题： 数据工厂类,实现了跨数据库的操作,多个数据库helper (版本：Version1.0.0)
 * 作  者： YuXiaoWei
 * 日  期： 2017-01-04
 * 修  改：
 * 参  考： http://www.cnblogs.com/Relict/archive/2011/12/19/2293460.html
 * 说  明： 暂无...
 * 备  注： 数据库连接默认在根目录下Intel\dell.xml下，dell文件格式为<?xml version="1.0" encoding="utf-8"?>
 *                                                              <SQlConn>
 *                                                                 <local ConnStr="server=121.41.101.4,5533;uid=kidsnet;pwd=1D#g2!hj3kYt4rwg5r#o6hfd7sr@;database=nynet" providerName="System.Data.SqlClient" />
 *                                                              </SQlConn>
 *         修改数据库连接位置需添加配置节：<appSettings><add key="dbpath" value="E:\Intel\\dell.xml"/></appSettings>
 *         SQLite需要在app.config文件中注册： <system.data>
                                            <DbProviderFactories>
                                            <remove invariant="System.Data.SQLite"/>
                                            <add name="SQLite Data Provider" invariant="System.Data.SQLite" description=".Net Framework Data Provider for SQLite" type="System.Data.SQLite.SQLiteFactory, System.Data.SQLite" />
                                            </DbProviderFactories>
                                            </system.data>
 * 调用示列：
 *
 * 
 * ***************************************************************************************************************/
namespace System.DBHelper.CrDB
{
    /// <summary>
    /// 数据工厂类,实现了跨数据库的操作
    /// </summary>
    public class DBHelper2 : CrDB
    {
        /// <summary>
        /// dell.xml 关键字
        /// </summary>
        private string _dbName;

        /// <summary>
        /// 属性,dell.xml 关键字
        /// </summary>
        public string DbName
        {
            get
            {
                return _dbName;
            }
            set
            {
                _dbName = value;
                GetConnStr(_dbName);
            }
        }

        /// <summary>
        /// 数据工厂类
        /// </summary>
        public DBHelper2()
        {
            DbName = "local";
        }
        /// <summary>
        /// 数据工厂类
        /// </summary>
        /// <param name="dbName">dell.xml 关键字</param>
        public DBHelper2(string dbName)
        {
            DbName = dbName;
        }
    }
}
#endif
