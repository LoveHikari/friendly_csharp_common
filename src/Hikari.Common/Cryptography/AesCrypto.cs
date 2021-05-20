using System.Security.Cryptography;

namespace Hikari.Common.Cryptography
{
    /// <summary>
    /// AES加解密
    /// </summary>
    public class AESCrypto : CryptoBase
    {
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="data">要加密的数据</param>
        /// <param name="pass">密钥</param>
        /// <param name="encoding">编码</param>
        /// <returns>密文</returns>
        public override byte[] EncryptEx(byte[] data, string pass, string encoding = "utf-8")
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = System.Text.Encoding.GetEncoding(encoding).GetBytes(pass);
                aes.Mode = CipherMode.ECB;
                aes.Padding = PaddingMode.PKCS7;
                ICryptoTransform cTransform = aes.CreateEncryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(data, 0, data.Length);
                return resultArray;
            }

        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="data">要解密的数据</param>
        /// <param name="pass">密码</param>
        /// <param name="encoding">编码</param>
        /// <returns>明文</returns>
        public override byte[] DecryptEx(byte[] data, string pass, string encoding = "utf-8")
        {
            Aes aes = Aes.Create();
            aes.Key = System.Text.Encoding.GetEncoding(encoding).GetBytes(pass);
            aes.Mode = CipherMode.ECB;
            aes.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = aes.CreateDecryptor();
            return cTransform.TransformFinalBlock(data, 0, data.Length);
        }
    }
}