namespace Hikari.Common.Cryptography
{
    /// <summary>
    /// 密码学基类
    /// </summary>
    public abstract class CryptoBase
    {
        /// <summary>
        /// 编码
        /// </summary>
        protected System.Text.Encoding Encoding;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="encoding"></param>
        protected CryptoBase(string encoding = "utf-8")
        {
            Encoding = System.Text.Encoding.GetEncoding(encoding);
        }
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="data">要加密的数据</param>
        /// <returns>密文</returns>
        public abstract Byte[] Encrypt(Byte[] data);

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="data">要加密的数据</param>
        /// <returns>密文</returns>
        public Byte[] Encrypt(string data)
        {
            byte[] bytes = Encoding.GetBytes(data);
            return this.Encrypt(bytes);
        }
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="data">要加密的数据</param>
        /// <returns>密文</returns>
        public string EncryptToBase64String(string data)
        {
            byte[] bytes = Encoding.GetBytes(data);
            bytes = this.Encrypt(bytes);
            return Convert.ToBase64String(bytes);
        }
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="data">要加密的数据</param>
        /// <returns>密文</returns>
        public string EncryptToHexString(string data)
        {
            byte[] bytes = Encoding.GetBytes(data);
            bytes = this.Encrypt(bytes);
            return bytes.ToHexString();
        }
        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="data">要解密的数据</param>
        /// <returns>明文</returns>
        public abstract Byte[]? Decrypt(Byte[] data);

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="data">要解密的数据</param>
        /// <returns>明文</returns>
        public Byte[]? Decrypt(string data)
        {
            byte[] bytes = Encoding.GetBytes(data);
            return this.Decrypt(bytes);
        }
        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="data">要解密的数据</param>
        /// <returns>明文</returns>
        public string DecryptFromBase64String(string data)
        {
            var bytes = Convert.FromBase64String(data);
            bytes = Decrypt(bytes);
            return Encoding.GetString(bytes);
        }
        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="data">要解密的数据</param>
        /// <returns>明文</returns>
        public string DecryptFromHexString(string data)
        {
            var bytes = data.FromHexString();
            bytes = Decrypt(bytes);
            return Encoding.GetString(bytes);
        }
    }
}