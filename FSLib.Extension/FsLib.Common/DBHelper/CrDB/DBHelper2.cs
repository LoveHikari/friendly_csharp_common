#if !NETSTANDARD2_0
/******************************************************************************************************************
 * 
 * 
 * ��  �⣺ ���ݹ�����,ʵ���˿����ݿ�Ĳ���,������ݿ�helper (�汾��Version1.0.0)
 * ��  �ߣ� YuXiaoWei
 * ��  �ڣ� 2017-01-04
 * ��  �ģ�
 * ��  ���� http://www.cnblogs.com/Relict/archive/2011/12/19/2293460.html
 * ˵  ���� ����...
 * ��  ע�� ���ݿ�����Ĭ���ڸ�Ŀ¼��Intel\dell.xml�£�dell�ļ���ʽΪ<?xml version="1.0" encoding="utf-8"?>
 *                                                              <SQlConn>
 *                                                                 <local ConnStr="server=121.41.101.4,5533;uid=kidsnet;pwd=1D#g2!hj3kYt4rwg5r#o6hfd7sr@;database=nynet" providerName="System.Data.SqlClient" />
 *                                                              </SQlConn>
 *         �޸����ݿ�����λ����������ýڣ�<appSettings><add key="dbpath" value="E:\Intel\\dell.xml"/></appSettings>
 *         SQLite��Ҫ��app.config�ļ���ע�᣺ <system.data>
                                            <DbProviderFactories>
                                            <remove invariant="System.Data.SQLite"/>
                                            <add name="SQLite Data Provider" invariant="System.Data.SQLite" description=".Net Framework Data Provider for SQLite" type="System.Data.SQLite.SQLiteFactory, System.Data.SQLite" />
                                            </DbProviderFactories>
                                            </system.data>
 * ����ʾ�У�
 *
 * 
 * ***************************************************************************************************************/
namespace System.DBHelper.CrDB
{
    /// <summary>
    /// ���ݹ�����,ʵ���˿����ݿ�Ĳ���
    /// </summary>
    public class DBHelper2 : CrDB
    {
        /// <summary>
        /// dell.xml �ؼ���
        /// </summary>
        private string _dbName;

        /// <summary>
        /// ����,dell.xml �ؼ���
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
        /// ���ݹ�����
        /// </summary>
        public DBHelper2()
        {
            DbName = "local";
        }
        /// <summary>
        /// ���ݹ�����
        /// </summary>
        /// <param name="dbName">dell.xml �ؼ���</param>
        public DBHelper2(string dbName)
        {
            DbName = dbName;
        }
    }
}
#endif
