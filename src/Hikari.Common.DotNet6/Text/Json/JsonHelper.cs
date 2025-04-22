using System.Collections;
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
namespace Hikari.Common.Text.Json
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
}