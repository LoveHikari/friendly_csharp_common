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
    public sealed class ConvertHelper
    {
        /// <summary>
        /// 指定字符串的固定长度，如果字符串小于固定长度，
        /// 则在字符串的前面补足零
        /// </summary>
        /// <param name="text">原始字符串</param>
        /// <param name="limitedLength">字符串的固定长度</param>
        /// <returns></returns>
        public static string RepairZero(string text, int limitedLength)
        {
            //补足0的字符串
            string temp = "";

            //补足0
            for (int i = 0; i < limitedLength - text.Length; i++)
            {
                temp += "0";
            }

            //连接text
            temp += text;

            //返回补足0的字符串
            return temp;
        }
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
        /// 将byte[]转换成int
        /// </summary>
        /// <param name="data">需要转换成整数的byte数组</param>
        /// <returns></returns>
        public static int BytesToInt32(byte[] data)
        {
            //如果传入的字节数组长度小于4,则返回0
            if (data.Length < 4)
            {
                return 0;
            }

            //定义要返回的整数
            int num = 0;

            //如果传入的字节数组长度大于4,需要进行处理
            if (data.Length >= 4)
            {
                //创建一个临时缓冲区
                byte[] tempBuffer = new byte[4];

                //将传入的字节数组的前4个字节复制到临时缓冲区
                Buffer.BlockCopy(data, 0, tempBuffer, 0, 4);

                //将临时缓冲区的值转换成整数，并赋给num
                num = BitConverter.ToInt32(tempBuffer, 0);
            }

            //返回整数
            return num;
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
        /// 匿名类型转强类型
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="obj">匿名对象</param>
        /// <param name="func"></param>
        /// <para>Cast(new { Name = "Tom", Age = 25 }, () => new { Name = "", Age = 0 });</para>
        /// <returns></returns>
        public static T Cast<T>(object obj, Func<T> func) where T : class
        {
            return (T)obj;
        }
        /// <summary>
        /// 匿名类型转强类型
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="obj">匿名对象</param>
        /// <param name="type">匿名类型</param>
        /// <para>Cast(new { Name = "Tom", Age = 25 }, new { Name = "", Age = 0 });</para>
        /// <returns></returns>
        public static T Cast<T>(object obj, T type) where T : class
        {
            return (T)obj;
        }

        /// <summary>
        /// 返回一个指定类型的对象，该对象的值等效于指定的对象。
        /// </summary>
        /// <param name="value">需要转化的对象</param>
        /// <param name="conversionType">转化后的类型</param>
        /// <returns>转化后的对象</returns>
        public static object ChangeType(object value, Type conversionType)
        {
            if (value == null)
                return null;
            if (conversionType.IsGenericType && conversionType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                System.ComponentModel.NullableConverter nullableConverter = new System.ComponentModel.NullableConverter(conversionType);
                conversionType = nullableConverter.UnderlyingType;
            }

            try
            {
                return Convert.ChangeType(value, conversionType);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 以最大的可能性返回一个指定类型的对象，该对象的值等效于指定的对象。
        /// </summary>
        /// <typeparam name="T">转化后的类型</typeparam>
        /// <param name="value">需要转化的对象</param>
        /// <returns>转化后的对象</returns>
        public static T ChangeType<T>(object value)
        {
            if (value == null)
            {
                return default(T);
            }
            Type conversionType = typeof(T);
            object obj = conversionType.Assembly.CreateInstance(conversionType.FullName);

            Type oldType = value.GetType();
            PropertyInfo[] propertyInfos = conversionType.GetProperties();
            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                var v = oldType.GetProperty(propertyInfo.Name)?.GetValue(value);

                propertyInfo.SetValue(obj, ChangeType(v, propertyInfo.PropertyType), null);


            }

            return (T)obj;
        }
        /// <summary>
        /// 以最大的可能性返回一个指定类型的对象列表，该对象的值等效于指定的对象列表。
        /// </summary>
        /// <typeparam name="T">转化后的类型</typeparam>
        /// <param name="value">需要转化的对象</param>
        /// <returns>转化后的对象</returns>
        public static List<T> ChangeType<T>(IEnumerable<object> value)
        {
            if (value == null)
            {
                return default;
            }

            List<T> list = new List<T>();
            foreach (object t in value)
            {
                list.Add(ChangeType<T>(t));
            }

            return list;
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
    }
}
