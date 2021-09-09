using System;
using System.Security.Cryptography;

namespace Hikari.Common.Security
{
    /// <summary>
    /// 摘要算法类
    /// 调用方法 new SecureHelper(data).Md5().DigestHex()
    /// </summary>
    public class SecureHelper
    {
        private HashAlgorithm _hashAlgorithm;  // 加密算法
        private readonly byte[] _buffer;  // 原文
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="data">要加密的内容</param>
        public SecureHelper(byte[] data)
        {
            _buffer = data;
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="data">要加密的内容</param>
        /// <param name="encoding">内容编码</param>
        public SecureHelper(string data, string encoding = "utf-8")
        {
            _buffer = System.Text.Encoding.GetEncoding(encoding).GetBytes(data);
        }
        /// <summary>
        /// md5
        /// </summary>
        /// <returns></returns>
        public SecureHelper Md5()
        {
            _hashAlgorithm = MD5.Create();

            return this;
        }
        /// <summary>
        /// Sha1
        /// </summary>
        /// <returns></returns>
        public SecureHelper Sha1()
        {
            _hashAlgorithm = SHA1.Create();

            return this;
        }
        /// <summary>
        /// Sha256
        /// </summary>
        /// <returns></returns>
        public SecureHelper Sha256()
        {
            _hashAlgorithm = SHA256.Create();
            return this;
        }
        /// <summary>
        /// HmacMd5
        /// </summary>
        /// <param name="key">密码</param>
        /// <returns></returns>
        public SecureHelper HmacMd5(string key)
        {
            var hmac = HMAC.Create("HMACMD5");
            hmac.Key = System.Text.Encoding.UTF8.GetBytes(key);
            _hashAlgorithm = hmac;
            return this;
        }
        /// <summary>
        /// HmacSha1
        /// </summary>
        /// <param name="key">密码</param>
        /// <returns></returns>
        public SecureHelper HmacSha1(string key)
        {
            var hmac = HMAC.Create("HMACSHA1");
            hmac.Key = System.Text.Encoding.UTF8.GetBytes(key);
            _hashAlgorithm = hmac;
            return this;
        }
        /// <summary>
        /// HmacSha256
        /// </summary>
        /// <param name="key">密码</param>
        /// <returns></returns>
        public SecureHelper HmacSha256(string key)
        {
            var hmac = HMAC.Create("HMACSHA256");
            hmac.Key = System.Text.Encoding.UTF8.GetBytes(key);
            _hashAlgorithm = hmac;
            return this;
        }
        /// <summary>
        /// 生成摘要
        /// </summary>
        /// <returns>摘要bytes</returns>
        public byte[] Digest()
        {
            return _hashAlgorithm.ComputeHash(_buffer);
        }

        /// <summary>
        /// 生成摘要，并转为16进制字符串
        /// </summary>
        /// <returns>摘要</returns>
        public string DigestHex()
        {
            var hash = Digest();
            string t2 = System.BitConverter.ToString(hash);
            t2 = t2.Replace("-", "").ToLower();
            return t2;
        }
        /// <summary>
        /// 生成摘要，并转为base64字符串
        /// </summary>
        /// <returns></returns>
        public string DigestBase64()
        {
            var hash = Digest();
            return Convert.ToBase64String(hash);
        }
    }
}