using System.IO;
using System.Xml.Serialization;

/******************************************************************************************************************
 * 
 * 
 * 标  题： XML 帮助类(版本：Version1.0.0)
 * 作  者： YuXiaoWei
 * 日  期： 2017/06/02
 * 修  改：
 * 参  考： 
 * 说  明： 暂无...
 * 备  注： 暂无...
 * 调用示列：
 *
 * 
 * ***************************************************************************************************************/
namespace System
{
    /// <summary>
	/// XML序列化支持类
	/// </summary>
	public class XmlHelper
    {
        #region 序列化

        /// <summary>
        /// 序列化对象到文件
        /// </summary>
        /// <param name="objectToSerialize">要序列化的对象</param>
        /// <param name="fliePath">保存到的目标文件路径</param>
        public static void XmlSerilizeToFile(object objectToSerialize, string fliePath)
        {
            string dir = Path.GetDirectoryName(fliePath);
            if (!string.IsNullOrWhiteSpace(dir))
            {
                if (!System.IO.Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
            }
            

            using (var stream = new FileStream(fliePath, FileMode.Create))
            {
                XmlSerializeToStream(objectToSerialize, stream);
                stream.Dispose();
            }
        }

        /// <summary>
        /// 序列化对象为文本
        /// </summary>
        /// <param name="objectToSerialize">要序列化的对象</param>
        /// <returns>保存信息的 <see cref="T:System.String"/></returns>
        public static string XmlSerializeToString(object objectToSerialize)
        {
            if (objectToSerialize == null)
                return null;

            using (var ms = XmlSerializeToStream(objectToSerialize))
            {
                ms.Dispose();
                return System.Text.Encoding.UTF8.GetString(ms.ToArray());
            }
        }

        /// <summary>
        /// 序列化指定对象为一个内存流
        /// </summary>
        /// <param name="objectToSerialize">要序列化的对象</param>
        /// <returns>保存序列化信息的 <see cref="T:System.IO.MemoryStream"/></returns>
        public static MemoryStream XmlSerializeToStream(object objectToSerialize)
        {
            if (objectToSerialize == null)
                return null;

            var result = new MemoryStream();
            XmlSerializeToStream(objectToSerialize, result);

            return result;
        }

        #endregion


        #region 反序列化

        /// <summary>
        /// 从流中反序列化对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="stream">流对象</param>
        /// <returns>反序列化结果</returns>
        public static T XmlDeserialize<T>(Stream stream) where T : class
        {
            T res = XmlDeserialize(stream, typeof(T)) as T;

            return res;
        }

        /// <summary>
        /// 反序列化文本或文件为对象
        /// </summary>
        /// <typeparam name="T">反序列化的对象类型</typeparam>
        /// <param name="content">文件路径或xml文本</param>
        /// <returns>反序列化对象</returns>
        public static T XmlDeserialize<T>(string content) where T : class
        {
            return (T)XmlDeserialize(content, typeof(T));
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 序列化指定对象到指定流中
        /// </summary>
        /// <param name="objectToSerialize">要序列化的对象</param>
        /// <param name="stream">目标流</param>
        private static void XmlSerializeToStream(object objectToSerialize, Stream stream)
        {
            if (objectToSerialize == null || stream == null)
                return;

            var xso = new XmlSerializer(objectToSerialize.GetType());
            xso.Serialize(stream, objectToSerialize);
        }
        /// <summary>
        /// 从文件中反序列化指定类型的对象
        /// </summary>
        /// <param name="objType">反序列化的对象类型</param>
        /// <param name="filePath">文件路径</param>
        /// <returns>对象</returns>
        private static object XmlDeserializeFromFile(string filePath, System.Type objType)
        {
            using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                object res = XmlDeserialize(stream, objType);
                stream.Dispose();
                return res;
            }
        }
        /// <summary>
        /// 从指定的字符串或文件中反序列化对象
        /// </summary>
        /// <param name="type">目标类型</param>
        /// <param name="content">文件路径或XML文本</param>
        /// <returns>反序列化的结果</returns>
        private static object XmlDeserialize(string content, Type type)
        {
            content = content.Trim();

            if (string.IsNullOrEmpty(content))
                return null;
            if (content[0] == '<')
            {
                using (var ms = new MemoryStream())
                {
                    byte[] buffer = System.Text.Encoding.Unicode.GetBytes(content);
                    ms.Write(buffer, 0, buffer.Length);
                    ms.Seek(0, SeekOrigin.Begin);

                    return XmlDeserialize(ms, type);
                }
            }
            else
            {
                return XmlDeserializeFromFile(content, type);
            }
        }

        /// <summary>
        /// 从流中反序列化出指定对象类型的对象
        /// </summary>
        /// <param name="objType">对象类型</param>
        /// <param name="stream">流对象</param>
        /// <returns>反序列结果</returns>
        private static object XmlDeserialize(Stream stream, System.Type objType)
        {
            var xso = new XmlSerializer(objType);
            object res = xso.Deserialize(stream);

            return res;
        }

        #endregion

    }
}
