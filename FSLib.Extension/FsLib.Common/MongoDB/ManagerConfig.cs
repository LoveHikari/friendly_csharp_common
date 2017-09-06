using System.IO;
using System.Xml.Serialization;

namespace System.MongoDB
{
    /// <summary>
    /// 读取配置文件管理类
    /// </summary>
    public class ManagerConfig
    {
        public static string ConfigPath;
        //加载配置文件
        public ManagerConfig(string configPath = "./config.xml")
        {
            ConfigPath = configPath;
        }
        //xml序列化后的对象
        private ServiceConfig _settings;
        internal ServiceConfig ServiceSettings => _settings ?? (_settings = Load());

        //加载xml序列化为ServiceConfig对象
        internal ServiceConfig Load()
        {
            if (File.Exists(ConfigPath))
            {
                using (FileStream fs = new FileStream(ConfigPath, FileMode.Open))
                {
                    XmlSerializer xs = new XmlSerializer(typeof(ServiceConfig));
                    //序列化为一个对象
                    _settings = (ServiceConfig)xs.Deserialize(fs);
                }
            }
            else
            {
                throw new Exception("数据库配置文件不存在，请检查");
                //_settings = new ServiceConfig();
            }

            return _settings;
        }
    }
}