using System.Security.Cryptography;
using System.Text;

namespace Hikari.Common.Security
{
    /// <summary>
    /// sha256加密类
    /// </summary>
    public class Sha256Encrypt
    {
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="data">要加密的数据</param>
        /// <param name="encoding">编码</param>
        /// <returns>密文</returns>
        public string Encrypt(string data, string encoding = "utf-8")
        {
            byte[] bytes = System.Text.Encoding.GetEncoding(encoding).GetBytes(data);
            byte[] hash = SHA256.Create().ComputeHash(bytes);

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                builder.Append(hash[i].ToString("X2"));
            }

            return builder.ToString();
        }
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="data">要加密的数据</param>
        /// <returns>密文</returns>
        public string Encrypt(byte[] data)
        {
            byte[] hash = SHA256.Create().ComputeHash(data);

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                builder.Append(hash[i].ToString("X2"));
            }

            return builder.ToString();
        }
    }
}