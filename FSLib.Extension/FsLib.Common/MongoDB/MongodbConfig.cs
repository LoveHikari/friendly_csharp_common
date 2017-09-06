using System.Collections.Generic;
using System.Xml.Serialization;

/******************************************************************************************************************
 * 
 * 
 * 标  题： 读取配置文件管理类(版本：Version1.0.0)
 * 作  者： YuXiaoWei
 * 日  期： 2017/03/07
 * 修  改：
 * 参  考： http://www.cnblogs.com/qtqq/p/4715120.html
 * 说  明： 暂无...
 * 备  注： 暂无...
 * 调用示列：
 * 
 * 
 * ***************************************************************************************************************/
namespace System.MongoDB
{
    /****************************************
    * 配置格式如下:
    * <?xml version="1.0" encoding="utf-8" ?>
    * <ServiceConfig>
    * <mongodbs>
    *  <Item dbName="myDb" hostName="mongodb://127.0.0.1:27017"></Item>
    *  <Item dbName="myDb1" hostName="mongodb://127.0.0.1:27017"></Item>
    *  <Item dbName="myDb2" hostName="mongodb://127.0.0.1:27017"></Item>
    * </mongodbs>
    * </ServiceConfig>
    *****************************************/
    /// <summary>
    /// Xml序列化对象类
    /// </summary>
    internal class ServiceConfig
    {
        [XmlArray, XmlArrayItem("Item")]
        public List<MongodbConfig> Mongodbs { get; set; }
    }

    [XmlRoot]
    internal class MongodbConfig
    {
        [XmlAttribute("dbName")]
        public string DbName { get; set; }
        [XmlAttribute("hostName")]
        public string HostName { get; set; }
    }
}