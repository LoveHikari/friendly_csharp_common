using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Text.Json;

/******************************************************************************************************************
 * 
 * 
 * 标  题： json 帮助类(版本：Version1.0.0)
 * 作  者： YuXiaoWei
 * 日  期： 2016/12/06
 * 修  改：
 * 参  考： http://blog.csdn.net/zhangqiang0551/article/details/49179357
 * 说  明： 暂无...
 * 备  注： 暂无...
 * 调用示列：
 *
 * 
 * ***************************************************************************************************************/
namespace Hikari.Common
{
    /// <summary>
    /// json 帮助类
    /// </summary>
    public class JsonHelper
    {
        /// <summary>
        /// json格式化
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        /// <remarks>http://www.cnblogs.com/unintersky/p/3884712.html</remarks>
        public static string JsonFormat(string str)
        {
            var obj = System.Text.Json.JsonSerializer.Deserialize<object>(str);
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
            };
            //格式化json字符串
            str = System.Text.Json.JsonSerializer.Serialize(obj, options);
            return str;
        }
        /// <summary>
        /// url参数序列化
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string UrlToJson(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return "";
            }
            IDictionary dictionary = new Dictionary<string, string>();
            string param = String2Json(Uri.UnescapeDataString(url.SplitRight("?")));
            string[] ps = param.Split('&');
            foreach (string p in ps)
            {
                var ss = p.Split('=');
                dictionary.Add(ss[0], ss[1]);
            }
            return System.Text.Json.JsonSerializer.Serialize(dictionary);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serializableObject"></param>
        /// <returns></returns>
        public static string Obj2Base64String(object serializableObject)
        {
            string returnedData;
            if (serializableObject == null)
                returnedData = String.Empty;
            else
            {
                byte[] resultBytes = null;
                System.Runtime.Serialization.Formatters.Binary.BinaryFormatter formatter
                    = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                MemoryStream stream = new MemoryStream();
                formatter.Serialize(stream, serializableObject);
                resultBytes = stream.ToArray();
                stream.Close();
                returnedData = Convert.ToBase64String(resultBytes);
            }
            return returnedData;
        }

        /// <summary>
        /// Deserializes base64 string to object.返序列化string 为 object
        /// </summary>
        /// <param name="deserializedString"></param>
        /// <returns></returns>
        public static object Base64String2Obj(string deserializedString)
        {
            object returnedData;
            if (deserializedString == String.Empty)
                returnedData = null;
            else
            {
                System.Runtime.Serialization.Formatters.Binary.BinaryFormatter formatter
                    = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                MemoryStream stream = new MemoryStream();
                byte[] middata = Convert.FromBase64String(deserializedString);
                stream.Write(middata, 0, middata.Length);
                //Pay attention to the following line. don't forget this line.      
                stream.Seek(0, SeekOrigin.Begin);
                returnedData = formatter.Deserialize(stream);
                stream.Close();
            }
            return returnedData;
        }

        /// <summary>
        /// DataTable转json
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string DataTableToJson(DataTable dt)
        {
            StringBuilder json = new StringBuilder();
            //Json.Append("[");
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    json.Append("{");
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        json.Append("\"" + dt.Columns[j].ColumnName.ToString() + "\":\"" + dt.Rows[i][j].ToString().Replace("\r\n", "").Replace("\r", "").Replace("\n", "").Replace("\"", "").Replace("“", "").Replace("”", "").Replace(" ", "").Replace("\\", "").Replace("	", "") + "\"");
                        if (j < dt.Columns.Count - 1)
                        {
                            json.Append(",");
                        }
                    }
                    json.Append("}");
                    if (i < dt.Rows.Count - 1)
                    {
                        json.Append("\r\n");
                    }
                }
            }
            //Json.Append("]");
            return json.ToString();
        }

        

        /// <summary>
        /// 过滤特殊字符
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private static string String2Json(String s)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < s.Length; i++)
            {
                char c = s.ToCharArray()[i];
                switch (c)
                {
                    case '\"':
                        sb.Append("\\\""); break;
                    case '\\':
                        sb.Append("\\\\"); break;
                    case '/':
                        sb.Append("\\/"); break;
                    case '\b':
                        sb.Append("\\b"); break;
                    case '\f':
                        sb.Append("\\f"); break;
                    case '\n':
                        sb.Append("\\n"); break;
                    case '\r':
                        sb.Append("\\r"); break;
                    case '\t':
                        sb.Append("\\t"); break;
                    default:
                        sb.Append(c); break;
                }
            }
            return sb.ToString();
        }


    }
    /// <summary>
    /// Json 扩展类
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never), System.ComponentModel.Browsable(false)]
    public static class JsonExtensions
    {
        /// <summary>
        /// 对象序列化为json
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns>json字符串</returns>
        public static string ToJson(this object obj)
        {
            return System.Text.Json.JsonSerializer.Serialize(obj);
        }
        /// <summary>
        /// json字符串反序列化为对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonStr"></param>
        /// <returns></returns>
        public static T ToEntity<T>(this string jsonStr) where T : class, new()
        {
            return System.Text.Json.JsonSerializer.Deserialize<T>(jsonStr);
        }
    }
}