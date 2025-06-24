using System;

namespace Hikari.Common.Cryptography
{
    /// <summary>
    /// 密码学基类
    /// </summary>
    public abstract class CryptoBase
    {
        protected System.Text.Encoding _encoding;

        protected CryptoBase(string encoding = "utf-8")
        {
            _encoding = System.Text.Encoding.GetEncoding(encoding);
        }
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="data">要加密的数据</param>
        /// <param name="pass">密钥</param>
        /// <param name="encoding">编码</param>
        /// <returns>密文</returns>
        public abstract Byte[] Encrypt(Byte[] data);

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="data">要加密的数据</param>
        /// <param name="pass">密钥</param>
        /// <param name="encoding">编码</param>
        /// <returns>密文</returns>
        public Byte[] Encrypt(string data)
        {
            byte[] bytes = _encoding.GetBytes(data);
            return this.Encrypt(bytes);
        }
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="data">要加密的数据</param>
        /// <returns>密文</returns>
        public string EncryptBase64(string data)
        {
            byte[] bytes = _encoding.GetBytes(data);
            bytes = this.Encrypt(bytes);
            return Convert.ToBase64String(bytes);
        }
        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="data">要解密的数据</param>
        /// <param name="pass">密码</param>
        /// <param name="encoding">编码</param>
        /// <returns>明文</returns>
        public abstract Byte[] Decrypt(Byte[] data);

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="data">要解密的数据</param>
        /// <param name="pass">密码</param>
        /// <param name="encoding">编码</param>
        /// <returns>明文</returns>
        public Byte[] Decrypt(string data)
        {
            byte[] bytes = _encoding.GetBytes(data);
            return this.Decrypt(bytes);
        }
        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="data">要解密的数据</param>
        /// <returns>明文</returns>
        public string DecryptStr(string data)
        {
            var bytes = Convert.FromBase64String(data);
            bytes = Decrypt(bytes);
            return _encoding.GetString(bytes);
        }
    }
}