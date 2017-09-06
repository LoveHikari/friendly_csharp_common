using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace System.Serialization
{
    /// <summary>
    /// Binary序列化帮助类，序列化为二进制文件(bat)
    /// </summary>
    public class BinaryFormatterHelper
    {
        #region 序列化

        /// <summary>
        /// 序列化到文件
        /// </summary>
        /// <param name="obj">要序列化的对象</param>
        /// <param name="fliePath">保存到的目标文件路径</param>
        public static void SerilizeToFile(object obj, string fliePath)
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
                BinaryFormatter b = new BinaryFormatter();
                b.Serialize(stream, obj);
                stream.Dispose();
            }
        }

        /// <summary>
        /// 序列化对象为文本
        /// </summary>
        /// <param name="obj">要序列化的对象</param>
        /// <returns>保存信息的 <see cref="T:System.String"/></returns>
        public static string SerializeToString(object obj)
        {
            if (obj == null) return null;

            using (var stream = new MemoryStream())
            {
                BinaryFormatter b = new BinaryFormatter();
                b.Serialize(stream, obj);
                stream.Dispose();
                return System.Text.Encoding.UTF8.GetString(stream.ToArray());
            }
        }

        /// <summary>
        /// 序列化指定对象为一个内存流
        /// </summary>
        /// <param name="obj">要序列化的对象</param>
        /// <returns>保存序列化信息的 <see cref="T:System.IO.MemoryStream"/></returns>
        public static MemoryStream SerializeToStream(object obj)
        {
            if (obj == null) return null;

            var stream = new MemoryStream();
            BinaryFormatter b = new BinaryFormatter();
            b.Serialize(stream, obj);
            return stream;
        }

        #endregion

        #region 反序列化

        /// <summary>
        /// 从流中反序列化对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="stream">流对象</param>
        /// <returns>反序列化结果</returns>
        public static T Deserialize<T>(Stream stream) where T : class
        {
            BinaryFormatter b = new BinaryFormatter();
            T res = b.Deserialize(stream) as T;
            return res;
        }

        /// <summary>
        /// 反序列化文本或文件为对象
        /// </summary>
        /// <typeparam name="T">反序列化的对象类型</typeparam>
        /// <param name="filePath">文件路径</param>
        /// <returns>反序列化对象</returns>
        public static T DeserializeForFile<T>(string filePath) where T : class
        {
            using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                BinaryFormatter b = new BinaryFormatter();
                T res = b.Deserialize(stream) as T;
                stream.Dispose();
                return res;
            }
        }
        /// <summary>
        /// 反序列化文本或文件为对象
        /// </summary>
        /// <typeparam name="T">反序列化的对象类型</typeparam>
        /// <param name="content">文本</param>
        /// <returns>反序列化对象</returns>
        public static T Deserialize<T>(string content) where T : class
        {
            using (var ms = new MemoryStream())
            {
                byte[] buffer = System.Text.Encoding.Unicode.GetBytes(content);
                ms.Write(buffer, 0, buffer.Length);
                ms.Seek(0, SeekOrigin.Begin);

                BinaryFormatter b = new BinaryFormatter();
                T res = b.Deserialize(ms) as T;
                ms.Dispose();
                return res;
            }
        }
        #endregion
    }
}