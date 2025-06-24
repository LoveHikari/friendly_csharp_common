using System;
using System.Security.Cryptography;

namespace Hikari.Common.Cryptography
{
    /// <summary>
    /// 3DES加解密
    /// </summary>
    public class TripleDesCrypto : CryptoBase
    {
        private readonly byte[] _key;
        private readonly byte[]? _iv;
        private readonly CipherMode _mode;
        private readonly PaddingMode _padding;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        /// <param name="mode"></param>
        /// <param name="padding"></param>
        /// <param name="encoding"></param>
        public TripleDesCrypto(string key, string iv = "", CipherMode mode = CipherMode.ECB, PaddingMode padding = PaddingMode.PKCS7, string encoding = "utf-8") : base(encoding)
        {
            _key = _encoding.GetBytes(key);
            _iv = !string.IsNullOrWhiteSpace(iv) ? _encoding.GetBytes(iv) : null;
            _mode = mode;
            _padding = padding;
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        /// <param name="mode"></param>
        /// <param name="padding"></param>
        /// <param name="encoding"></param>
        public TripleDesCrypto(byte[] key, byte[]? iv = null, CipherMode mode = CipherMode.ECB, PaddingMode padding = PaddingMode.PKCS7, string encoding = "utf-8") : base(encoding)
        {
            _key = key;
            _iv = iv;
            _mode = mode;
            _padding = padding;
        }
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="data">要加密的数据</param>
        /// <returns>密文</returns>
        public override byte[] Encrypt(byte[] data)
        {
            using var desAlg = GetDes();
            ICryptoTransform cTransform = desAlg.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(data, 0, data.Length);
            return resultArray;
        }


        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="data">要解密的数据</param>
        /// <returns>明文</returns>
        public override byte[] Decrypt(byte[] data)
        {
            using var desAlg = GetDes();
            // 创建解密器
            ICryptoTransform decryptor = desAlg.CreateDecryptor();

            // 使用 TransformFinalBlock 解密
            byte[] decryptedBytes = decryptor.TransformFinalBlock(data, 0, data.Length);
            return decryptedBytes;

        }


        private TripleDES GetDes()
        {
            TripleDES desAlg = TripleDES.Create();
            int targetLength;

            if (_key.Length <= 24)
            {
                targetLength = 24;
            }
            else
            {
                // 如果输入长度超过 24，则截取前 24 字节
                targetLength = 24;
            }

            // 创建目标长度的数组
            byte[] resultArray = new byte[targetLength];

            // 将输入数组复制到目标数组中，超出目标长度的部分将被截取，未达到目标长度的部分用0填充
            Array.Copy(_key, resultArray, Math.Min(_key.Length, targetLength));

            desAlg.Key = resultArray;
            if (_iv != null)
            {
                // 创建一个新的 8 字节数组，并用默认值 0 填充
                byte[] expandedArray = new byte[8];
                // 将 _iv 复制到 expandedArray 中
                // 如果不足 16 字节则填充 0），如果长度超过 8 字节则截取前 8 字节
                Array.Copy(_iv, expandedArray, Math.Min(_iv.Length, expandedArray.Length));
                desAlg.IV = expandedArray;
            }

            desAlg.Padding = _padding;
            desAlg.Mode = _mode;
            return desAlg;
        }
    }
}