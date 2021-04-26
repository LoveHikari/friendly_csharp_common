using System.Security.Cryptography;
using System.Text;

namespace System.Security
{
    /// <summary>
    /// sha1加密类
    /// </summary>
    public class Sha1Encrypt
    {
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="data">要加密的数据</param>
        /// <param name="encoding">编码</param>
        /// <returns>密文</returns>
        public string Encrypt(string data, string encoding = "utf-8")
        {
            SHA1 sha1 = new SHA1CryptoServiceProvider();
            byte[] bytes = System.Text.Encoding.GetEncoding(encoding).GetBytes(data);

            byte[] hash = sha1.ComputeHash(bytes);
            sha1.Dispose();
            string result = BitConverter.ToString(hash);
            result = result.Replace("-", "");
            return result;
        }
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="data">要加密的数据</param>
        /// <returns>密文</returns>
        public string Encrypt(byte[] data)
        {
            SHA1 sha1 = new SHA1CryptoServiceProvider();
            byte[] hash = sha1.ComputeHash(data);
            sha1.Dispose();
            string result = BitConverter.ToString(hash);
            result = result.Replace("-", "");
            return result;
        }
    }
}