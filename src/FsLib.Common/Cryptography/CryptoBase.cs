namespace System.Cryptography
{
    /// <summary>
    /// 密码学基类
    /// </summary>
    public abstract class CryptoBase
    {
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="data">要加密的数据</param>
        /// <param name="pass">密钥</param>
        /// <param name="encoding">编码</param>
        /// <returns>密文</returns>
        public abstract Byte[] EncryptEx(Byte[] data, String pass, string encoding = "utf-8");

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="data">要加密的数据</param>
        /// <param name="pass">密钥</param>
        /// <param name="encoding">编码</param>
        /// <returns>密文</returns>
        public Byte[] EncryptEx(string data, String pass, string encoding = "utf-8")
        {
            byte[] bytes = System.Text.Encoding.GetEncoding(encoding).GetBytes(data);
            return this.EncryptEx(bytes, pass, encoding);
        }
        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="data">要解密的数据</param>
        /// <param name="pass">密码</param>
        /// <param name="encoding">编码</param>
        /// <returns>明文</returns>
        public abstract Byte[] DecryptEx(Byte[] data, String pass, string encoding = "utf-8");

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="data">要解密的数据</param>
        /// <param name="pass">密码</param>
        /// <param name="encoding">编码</param>
        /// <returns>明文</returns>
        public Byte[] DecryptEx(string data, String pass, string encoding = "utf-8")
        {
            byte[] bytes = System.Text.Encoding.GetEncoding(encoding).GetBytes(data);
            return this.DecryptEx(bytes, pass, encoding);
        }
    }
}