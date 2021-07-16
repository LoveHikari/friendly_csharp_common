using System;
using System.Security.Cryptography;

namespace Hikari.Common.Cryptography
{
    /// <summary>
    /// AES加解密
    /// </summary>
    public class AESCrypto
    {
        private readonly Aes _aes;
        private readonly System.Text.Encoding _encoding;
        public AESCrypto(string key, string iv = "", CipherMode mode = CipherMode.ECB, PaddingMode padding = PaddingMode.PKCS7, string encoding = "utf-8")
        {
            _encoding = System.Text.Encoding.GetEncoding(encoding);
            _aes = Aes.Create();
            _aes.Key = _encoding.GetBytes(key);
            _aes.IV = _encoding.GetBytes(iv);
            _aes.Mode = mode;
            _aes.Padding = padding;
        }
        public AESCrypto(byte[] key, byte[] iv = null, CipherMode mode = CipherMode.ECB, PaddingMode padding = PaddingMode.PKCS7, string encoding = "utf-8")
        {
            _encoding = System.Text.Encoding.GetEncoding(encoding);
            _aes = Aes.Create();
            _aes.Key = key;
            if(iv != null) _aes.IV = iv;
            _aes.Mode = mode;
            _aes.Padding = padding;
        }
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="data">要加密的数据</param>
        /// <returns>密文</returns>
        public byte[] Encrypt(byte[] data)
        {
            using (_aes)
            {
                ICryptoTransform cTransform = _aes.CreateEncryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(data, 0, data.Length);
                return resultArray;
            }

        }
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="data">要加密的数据</param>
        /// <returns>密文</returns>
        public byte[] Encrypt(string data)
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
        /// <returns>明文</returns>
        public byte[] Decrypt(byte[] data)
        {
            using (_aes)
            {
                ICryptoTransform cTransform = _aes.CreateDecryptor();
                return cTransform.TransformFinalBlock(data, 0, data.Length);
            }

        }
        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="data">要解密的数据</param>
        /// <returns>明文</returns>
        public byte[] Decrypt(string data)
        {
            byte[] bytes = Convert.FromBase64String(data);
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