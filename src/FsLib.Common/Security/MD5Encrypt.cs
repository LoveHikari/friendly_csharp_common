using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace System.Security
{
    /// <summary>
    /// MD5加密类
    /// </summary>
    public class MD5Encrypt
    {
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="data">要加密的数据</param>
        /// <param name="encoding">编码</param>
        /// <returns>密文</returns>
        public string Encrypt(string data, string encoding = "utf-8")
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] t = md5.ComputeHash(System.Text.Encoding.GetEncoding(encoding).GetBytes(data));
            string t2 = BitConverter.ToString(t);
            t2 = t2.Replace("-", "").ToLower();
            return t2;
        }
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="data">要加密的数据</param>
        /// <returns>密文</returns>
        public string Encrypt(byte[] data)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] t = md5.ComputeHash(data);
            string t2 = BitConverter.ToString(t);
            t2 = t2.Replace("-", "").ToLower();
            return t2;
        }
    }
}