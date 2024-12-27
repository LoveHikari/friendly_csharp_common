using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;

/************
 *  1. 功能：处理数据类型转换，数制转换、编码转换相关的类
 *  2. 作者：周兆坤 
 *  3. 创建日期：2010-3-19
 *  4. 最后修改日期：2010-3-19
**/
namespace Hikari.Common
{
    /// <summary>
    /// 处理数据类型转换，数制转换、编码转换相关的类
    /// </summary>    
    public class ConvertHelper
    {
        /// <summary>
        /// 实现各进制数间的转换。ConvertBase("15",10,16)表示将十进制数15转换为16进制的数。
        /// </summary>
        /// <param name="value">要转换的值,即原值</param>
        /// <param name="from">原值的进制,只能是2,8,10,16四个值。</param>
        /// <param name="to">要转换到的目标进制，只能是2,8,10,16四个值。</param>
        public static string ConvertBase(string value, int from, int to)
        {
            int intValue = Convert.ToInt32(value, from);  //先转成10进制
            string result = Convert.ToString(intValue, to);  //再转成目标进制
            if (to == 2)
            {
                int resultLength = result.Length;  //获取二进制的长度
                switch (resultLength)
                {
                    case 7:
                        result = "0" + result;
                        break;
                    case 6:
                        result = "00" + result;
                        break;
                    case 5:
                        result = "000" + result;
                        break;
                    case 4:
                        result = "0000" + result;
                        break;
                    case 3:
                        result = "00000" + result;
                        break;
                }
            }
            return result;
        }

        /// <summary>
        /// 大端序字节数组转十进制
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static int BigEndianToInt32(byte[]? bytes)
        {
            if (bytes == null || bytes.Length == 0)
            {
                return 0;
            }

            int result = 0;

            // 大端序按位计算
            for (int i = 0; i < bytes.Length; i++)
            {
                result = (result << 8) | bytes[i];
            }

            return result;
        }

        /// <summary>
        /// 小端序字节数组转十进制
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static int LittleEndianToToInt32(byte[]? bytes)
        {
            if (bytes == null || bytes.Length == 0)
            {
                return 0;
            }

            int result = 0;

            for (int i = 0; i < bytes.Length; i++)
            {
                result |= bytes[i] << (8 * i); // 按照小端序的位移计算
            }

            return result;
        }
        /// <summary>
        /// 字节数组转16进制字符串
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string ByteToHexStr(byte[] bytes) {
            var hexString = string.Empty;
            if (bytes.Length <= 0) return hexString;
            foreach (var item in bytes) {
                hexString += item.ToString("X2");
            }

            return hexString.ToUpper().Replace(" ", "");
        }
        /// <summary>
        /// 将XML字符串转换为DataSet
        /// </summary>
        /// <param name="xmlData">XML字符</param>
        /// <returns>DataSet:相同结点生成一个DataTable</returns>
        public static DataSet XmlToDataSet(string xmlData)
        {
            DataSet xmlDs = new DataSet();
            StringReader stream = new StringReader(xmlData);
            using XmlTextReader reader = new XmlTextReader(stream);
            xmlDs.ReadXml(reader);
            return xmlDs;
        }
        
        /// <summary>
        /// 解密base64,解决了base64长度不是4的倍数的问题
        /// </summary>
        /// <param name="s">待解密字符串</param>
        /// <param name="encode">编码</param>
        /// <returns></returns>
        public static string FromBase64String(string s, string encode)
        {
            var bytes = FromBase64String(s);
            return System.Text.Encoding.GetEncoding(encode).GetString(bytes);
        }
        /// <summary>
        /// 解密base64,解决了base64长度不是4的倍数的问题
        /// </summary>
        /// <param name="s">待解密字符串</param>
        /// <returns></returns>
        public static byte[] FromBase64String(string s)
        {
            int length = s.Length;
            if (length % 4 != 0)
            {
                s += "=";
            }

            var bytes = Convert.FromBase64String(s);
            return bytes;
        }
        /// <summary>
        /// 加密为base64，去掉了最后一个‘=’补位
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static string ToBase64String(byte[] b)
        {
            string s = Convert.ToBase64String(b);
            s = s.TrimEnd('=');
            return s;
        }

        /// <summary>
        /// 加密为base64，去掉了最后一个‘=’补位
        /// </summary>
        /// <param name="s"></param>
        /// <param name="encode"></param>
        /// <returns></returns>
        public static string ToBase64String(string s, string encode)
        {
            byte[] b = System.Text.Encoding.GetEncoding(encode).GetBytes(s);
            return ToBase64String(b);
        }

        /// <summary>
        /// 数字转ip地址
        /// </summary>
        /// <param name="ipInt">ip数字</param>
        /// <returns>ip地址</returns>
        public static string IntToIp(long ipInt)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(ipInt >> 0x18 & 0xff).Append(".");
            sb.Append(ipInt >> 0x10 & 0xff).Append(".");
            sb.Append(ipInt >> 0x8 & 0xff).Append(".");
            sb.Append(ipInt & 0xff);
            return sb.ToString();
        }
        /// <summary>
        /// ip地址转数字
        /// </summary>
        /// <param name="ip">ip地址</param>
        /// <returns>ip数字</returns>
        public static long IpToInt(string ip)
        {
            char[] separator = { '.' };
            string[] items = ip.Split(separator);
            return long.Parse(items[0]) << 24
                   | long.Parse(items[1]) << 16
                   | long.Parse(items[2]) << 8
                   | long.Parse(items[3]);
        }

    }
}
