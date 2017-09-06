/***
 * title:数据工厂类,实现了跨数据库的操作,多个数据库helper
 * date:2016-04-29
 * author:YUXiaoWei
 * 参考：http://www.cnblogs.com/Relict/archive/2011/12/19/2293460.html
 * 备注：<connectionStrings><add name="ConnectionString" connectionString="SERVER=(local);uid=sa;pwd=123456;DATABASE=ESSoft" providerName="System.Data.SqlClient" /></connectionStrings>
 ***/
namespace System.DBHelper.CrDB
{
    /// <summary>
    /// 数据工厂类,实现了跨数据库的操作
    /// </summary>
    public class DBHelper : CrDB
    {
        /// <summary>
        /// Webconfig配置连接字符串
        /// </summary>
        private string _configString;

        /// <summary>
        /// 属性,Webconfig配置连接字符串
        /// </summary>
        public string ConfigString
        {
            get
            {
                return _configString;
            }
            set
            {
                _configString = value;
                GetConnectionString(_configString);
            }
        }

        /// <summary>
        /// 数据工厂类
        /// </summary>
        public DBHelper()
        {
            ConfigString = "ConnectionString";
        }
        /// <summary>
        /// 数据工厂类
        /// </summary>
        /// <param name="configString">web.config 关键字</param>
        public DBHelper(string configString)
        {
            this.ConfigString = configString;
        }

        /// <summary>
        /// 数据工厂类
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        /// <param name="providerName">数据提供者</param>
        public DBHelper(string connectionString, string providerName)
        {
            base.ProviderName = providerName;
            base.ConnectionString = connectionString;
        }
    }
}