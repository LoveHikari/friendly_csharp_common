using System;

namespace Hikari.Common.Cryptography
{
    /// <summary>
    /// RC4加解密
    /// </summary>
    public class RC4Crypto : CryptoBase
    {
        private readonly byte[] _pass;
        /// <summary>
        /// RC4加解密
        /// </summary>
        /// <param name="pass">加密密码</param>
        /// <param name="encoding">编码</param>
        public RC4Crypto(string pass, string encoding = "utf-8") : base(encoding)
        {
            _pass = Encoding.GetBytes(pass);
        }
        /// <summary>
        /// RC4加解密
        /// </summary>
        /// <param name="pass">加密密码</param>
        /// <param name="encoding">编码</param>
        public RC4Crypto(byte[] pass, string encoding = "utf-8") : base(encoding)
        {
            _pass = pass;
        }
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="data">待加密数据</param>
        /// <returns>加密字符串</returns>
        public override Byte[] Encrypt(Byte[] data)
        {
            Byte[] output = new Byte[data.Length];
            Int64 i = 0;
            Int64 j = 0;
            Byte[] mBox = GetKey(_pass, 256);

            // 加密
            for (Int64 offset = 0; offset < data.Length; offset++)
            {
                i = (i + 1) % mBox.Length;
                j = (j + mBox[i]) % mBox.Length;
                (mBox[i], mBox[j]) = (mBox[j], mBox[i]);
                Byte a = data[offset];
                //Byte b = mBox[(mBox[i] + mBox[j] % mBox.Length) % mBox.Length];
                // mBox[j] 一定比 mBox.Length 小，不需要在取模
                Byte b = mBox[(mBox[i] + mBox[j]) % mBox.Length];
                output[offset] = (Byte)((Int32)a ^ (Int32)b);
            }

            return output;
        }
        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="data">待解密数据</param>
        /// <returns>解密字符串</returns>
        public override byte[] Decrypt(byte[] data)
        {

            return Encrypt(data);
        }
        /// <summary>
        /// 打乱密码
        /// </summary>
        /// <param name="pass">密码</param>
        /// <param name="kLen">密码箱长度</param>
        /// <returns>打乱后的密码</returns>
        private Byte[] GetKey(Byte[] pass, Int32 kLen)
        {
            Byte[] mBox = new Byte[kLen];

            for (Int64 i = 0; i < kLen; i++)
            {
                mBox[i] = (Byte)i;
            }
            Int64 j = 0;
            for (Int64 i = 0; i < kLen; i++)
            {
                j = (j + mBox[i] + pass[i % pass.Length]) % kLen;
                (mBox[i], mBox[j]) = (mBox[j], mBox[i]);
            }
            return mBox;
        }

    }
}