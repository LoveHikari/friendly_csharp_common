using System.Security.Cryptography;

namespace System.Cryptography
{
    public class AesHelper
    {
        /// <summary>
        /// AES的加密函数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static byte[] AESEncrypt(byte[] key, byte[] bytes)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.Mode = CipherMode.ECB;
                aes.Padding = PaddingMode.PKCS7;
                ICryptoTransform cTransform = aes.CreateEncryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(bytes, 0, bytes.Length);
                return resultArray;
            }

        }
        /// <summary>
        /// AES的解密函数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static byte[] AESDecrypt(byte[] key, byte[] bytes)
        {
            Aes aes = Aes.Create();
            aes.Key = key;
            aes.Mode = CipherMode.ECB;
            aes.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = aes.CreateDecryptor();
            return cTransform.TransformFinalBlock(bytes, 0, bytes.Length);
        }
    }
}