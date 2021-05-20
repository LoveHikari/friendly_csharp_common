using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

/******************************************************************************************************************
 * 
 * 
 * 标  题： Dictionary 的可序列化类(版本：Version1.0.0)
 * 作  者： YuXiaoWei
 * 日  期： 2017/06/02
 * 修  改：
 * 参  考： http://www.cnblogs.com/darrenji/p/4391515.html , http://blog.163.com/tuchengju@126/blog/static/38071165201411294454161/
 * 说  明： 暂无...
 * 备  注： 暂无...
 * 调用示列：
 *
 * 
 * ***************************************************************************************************************/
namespace Hikari.Common
{
    /// <summary>
    /// <see cref="Dictionary{TKey,TValue}"/> 的可序列化形式
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    [Serializable]
    public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, IXmlSerializable
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SerializableDictionary() { }
        /// <summary>
        /// 写出到xml
        /// </summary>
        /// <param name="write"></param>
        public void WriteXml(XmlWriter write)       // Serializer  
        {
            //键的xml序列化器
            XmlSerializer keySerializer = new XmlSerializer(typeof(TKey));
            //值的xml序列化器
            XmlSerializer valueSerializer = new XmlSerializer(typeof(TValue));

            foreach (KeyValuePair<TKey, TValue> kv in this)
            {
                write.WriteStartElement("SerializableDictionary");
                write.WriteStartElement("key");
                keySerializer.Serialize(write, kv.Key);
                write.WriteEndElement();
                write.WriteStartElement("value");
                valueSerializer.Serialize(write, kv.Value);
                write.WriteEndElement();
                write.WriteEndElement();
            }
        }
        /// <summary>
        /// 读取xml
        /// </summary>
        /// <param name="reader"></param>
        public void ReadXml(XmlReader reader)       // Deserializer  
        {
            //键的xml序列化器
            XmlSerializer keySerializer = new XmlSerializer(typeof(TKey));
            //值的xml序列化器
            XmlSerializer valueSerializer = new XmlSerializer(typeof(TValue));
            //判断xml中当前节点是否为null
            bool wasEmpty = reader.IsEmptyElement;
            reader.Read();
            if (!wasEmpty)
            {
                while (reader.NodeType != System.Xml.XmlNodeType.EndElement)
                {
                    //先读键
                    reader.ReadStartElement("SerializableDictionary");
                    reader.ReadStartElement("key");
                    //反序列化成键的类型
                    TKey tk = (TKey)keySerializer.Deserialize(reader);
                    reader.ReadEndElement();
                    //再读值
                    reader.ReadStartElement("value");
                    TValue vl = (TValue)valueSerializer.Deserialize(reader);
                    reader.ReadEndElement();
                    reader.ReadEndElement();
                    this.Add(tk, vl);
                    //读下一个节点
                    reader.MoveToContent();

                }
                reader.ReadEndElement();
            }

        }
        /// <summary>
        /// 获取架构
        /// </summary>
        /// <returns></returns>
        public XmlSchema GetSchema()
        {
            return null;
        }
    }
}