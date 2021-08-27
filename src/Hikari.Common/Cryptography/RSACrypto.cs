using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

/******************************************************************************************************************
 * 
 * 
 * 标  题：RSA密码学类(版本：Version1.0.0)
 * 作　者：YuXiaoWei
 * 日　期：2018/02/26
 * 修　改：
 * 参　考：
 * 说　明： 暂无...
 * 备　注： 暂无...
 * 
 * 
 * ***************************************************************************************************************/
namespace Hikari.Common.Cryptography
{
    /// <summary>
    /// rsa密码学，包括签名和验签
    /// </summary>
    public class RSACrypto : CryptoBase
    {
        /// <summary>
        /// 读取秘钥文件
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public string ReadKeyFile(string file)
        {
            StringBuilder sb = new StringBuilder();
            var list = File.ReadAllLines(file);
            foreach (var item in list)
            {
                if (string.IsNullOrEmpty(item) || item.Trim().StartsWith("--"))
                {
                    continue;
                }
                sb.Append(item);
            }
            return sb.ToString();
        }

        /// <summary>
        /// 私钥解密
        /// </summary>
        /// <param name="data"></param>
        /// <param name="pass">私钥</param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public override byte[] DecryptEx(byte[] data, string pass, string encoding = "utf-8")
        {
            using (RSACryptoServiceProvider rsa = DecodePemPrivateKey(pass))
            {
                var cipherbytes = rsa.Decrypt(data, false);

                return cipherbytes;
            }
        }
        /// <summary>
        /// 公钥加密
        /// </summary>
        /// <param name="data"></param>
        /// <param name="pass">公钥</param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public override byte[] EncryptEx(byte[] data, string pass, string encoding = "utf-8")
        {
            RSAParameters paraPub = ConvertFromPublicKey(pass);
            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportParameters(paraPub);
                var cipherbytes = rsa.Encrypt(data, false);
                return cipherbytes;
            }
        }

        /// <summary>
        /// 私钥签名
        /// </summary>
        /// <param name="content">待签名字符串</param>
        /// <param name="privateKey">私钥</param>
        /// <param name="charset">编码格式</param>
        /// <param name="signType">签名类型</param>
        /// <returns>签名后字符串</returns>
        public string Signature(string content, string privateKey, string charset = "utf-8", string signType = "MD5")
        {
            byte[] data = Encoding.GetEncoding(charset).GetBytes(content);
            RSACryptoServiceProvider rsa = DecodePemPrivateKey(privateKey);
            HashAlgorithm crypto = null;
            if (signType.ToLower() == "md5")
            {
                crypto = MD5.Create();
            }

            if (signType.ToLower() == "sha1")
            {
                crypto = SHA1.Create();;
            }

            using (crypto)
            {
                byte[] signData = rsa.SignData(data, crypto);
                return Convert.ToBase64String(signData);
            }

        }

        /// <summary>
        /// 公钥验签
        /// </summary>
        /// <param name="content">待验签字符串</param>
        /// <param name="signedString">签名</param>
        /// <param name="publicKey">公钥</param>
        /// <param name="charset">编码格式</param>
        /// <param name="signType">签名类型</param>
        /// <returns>true(通过)，false(不通过)</returns>
        public bool Verify(string content, string signedString, string publicKey, string charset = "utf-8", string signType = "MD5")
        {
            bool result = false;

            byte[] data = Encoding.GetEncoding(charset).GetBytes(content);
            byte[] signData = Convert.FromBase64String(signedString);
            RSAParameters paraPub = ConvertFromPublicKey(publicKey);
            using (RSACryptoServiceProvider rsaPub = new RSACryptoServiceProvider())
            {
                rsaPub.ImportParameters(paraPub);
                HashAlgorithm crypto = null;
                if (signType.ToLower() == "md5")
                {
                    crypto = MD5.Create();
                }

                if (signType.ToLower() == "sha1")
                {
                    crypto = SHA1.Create();
                }
                using (crypto)
                {
                    result = rsaPub.VerifyData(data, crypto, signData);
                    return result;
                }
            }

        }


        #region 私有方法
        private RSACryptoServiceProvider DecodePemPrivateKey(String pemstr)
        {
            byte[] pkcs8privatekey;
            pkcs8privatekey = Convert.FromBase64String(pemstr);
            if (pkcs8privatekey != null)
            {
                RSACryptoServiceProvider rsa = DecodePrivateKeyInfo(pkcs8privatekey);
                return rsa;
            }
            else
                return null;
        }
        private RSACryptoServiceProvider DecodePrivateKeyInfo(byte[] pkcs8)
        {

            byte[] SeqOID = { 0x30, 0x0D, 0x06, 0x09, 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x01, 0x01, 0x01, 0x05, 0x00 };
            byte[] seq = new byte[15];

            using (MemoryStream mem = new MemoryStream(pkcs8))
            {
                int lenstream = (int)mem.Length;
                using (BinaryReader binr = new BinaryReader(mem))  //wrap Memory Stream with BinaryReader for easy reading
                {


                    byte bt = 0;
                    ushort twobytes = 0;

                    try
                    {

                        twobytes = binr.ReadUInt16();
                        if (twobytes == 0x8130) //data read as little endian order (actual data order for Sequence is 30 81)
                            binr.ReadByte();    //advance 1 byte
                        else if (twobytes == 0x8230)
                            binr.ReadInt16();   //advance 2 bytes
                        else
                            return null;


                        bt = binr.ReadByte();
                        if (bt != 0x02)
                            return null;

                        twobytes = binr.ReadUInt16();

                        if (twobytes != 0x0001)
                            return null;

                        seq = binr.ReadBytes(15);       //read the Sequence OID
                        if (!CompareBytearrays(seq, SeqOID))    //make sure Sequence for OID is correct
                            return null;

                        bt = binr.ReadByte();
                        if (bt != 0x04) //expect an Octet string 
                            return null;

                        bt = binr.ReadByte();       //read next byte, or next 2 bytes is  0x81 or 0x82; otherwise bt is the byte count
                        if (bt == 0x81)
                            binr.ReadByte();
                        else
                            if (bt == 0x82)
                            binr.ReadUInt16();
                        //------ at this stage, the remaining sequence should be the RSA private key

                        byte[] rsaprivkey = binr.ReadBytes((int)(lenstream - mem.Position));
                        RSACryptoServiceProvider rsacsp = DecodeRSAPrivateKey(rsaprivkey);
                        return rsacsp;
                    }

                    catch (Exception)
                    {
                        return null;
                    }

                    finally { binr.Close(); }
                }
            }


        }
        private bool CompareBytearrays(byte[] a, byte[] b)
        {
            if (a.Length != b.Length)
                return false;
            int i = 0;
            foreach (byte c in a)
            {
                if (c != b[i])
                    return false;
                i++;
            }
            return true;
        }

        private RSACryptoServiceProvider DecodeRSAPrivateKey(byte[] privkey)
        {
            byte[] MODULUS, E, D, P, Q, DP, DQ, IQ;

            // ---------  Set up stream to decode the asn.1 encoded RSA private key  ------
            using (MemoryStream mem = new MemoryStream(privkey))
            {
                using (BinaryReader binr = new BinaryReader(mem))    //wrap Memory Stream with BinaryReader for easy reading
                {

                    byte bt = 0;
                    ushort twobytes = 0;
                    int elems = 0;
                    try
                    {
                        twobytes = binr.ReadUInt16();
                        if (twobytes == 0x8130) //data read as little endian order (actual data order for Sequence is 30 81)
                            binr.ReadByte();    //advance 1 byte
                        else if (twobytes == 0x8230)
                            binr.ReadInt16();   //advance 2 bytes
                        else
                            return null;

                        twobytes = binr.ReadUInt16();
                        if (twobytes != 0x0102) //version number
                            return null;
                        bt = binr.ReadByte();
                        if (bt != 0x00)
                            return null;


                        //------  all private key components are Integer sequences ----
                        elems = GetIntegerSize(binr);
                        MODULUS = binr.ReadBytes(elems);

                        elems = GetIntegerSize(binr);
                        E = binr.ReadBytes(elems);

                        elems = GetIntegerSize(binr);
                        D = binr.ReadBytes(elems);

                        elems = GetIntegerSize(binr);
                        P = binr.ReadBytes(elems);

                        elems = GetIntegerSize(binr);
                        Q = binr.ReadBytes(elems);

                        elems = GetIntegerSize(binr);
                        DP = binr.ReadBytes(elems);

                        elems = GetIntegerSize(binr);
                        DQ = binr.ReadBytes(elems);

                        elems = GetIntegerSize(binr);
                        IQ = binr.ReadBytes(elems);

                        // ------- create RSACryptoServiceProvider instance and initialize with public key -----
                        RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
                        RSAParameters RSAparams = new RSAParameters();
                        RSAparams.Modulus = MODULUS;
                        RSAparams.Exponent = E;
                        RSAparams.D = D;
                        RSAparams.P = P;
                        RSAparams.Q = Q;
                        RSAparams.DP = DP;
                        RSAparams.DQ = DQ;
                        RSAparams.InverseQ = IQ;
                        RSA.ImportParameters(RSAparams);
                        return RSA;
                    }
                    catch (Exception)
                    {
                        return null;
                    }
                    finally { binr.Close(); }
                }
            }

        }
        private int GetIntegerSize(BinaryReader binr)
        {
            byte bt = 0;
            byte lowbyte = 0x00;
            byte highbyte = 0x00;
            int count = 0;
            bt = binr.ReadByte();
            if (bt != 0x02)		//expect integer
                return 0;
            bt = binr.ReadByte();

            if (bt == 0x81)
                count = binr.ReadByte();	// data size in next byte
            else
                if (bt == 0x82)
            {
                highbyte = binr.ReadByte(); // data size in next 2 bytes
                lowbyte = binr.ReadByte();
                byte[] modint = { lowbyte, highbyte, 0x00, 0x00 };
                count = BitConverter.ToInt32(modint, 0);
            }
            else
            {
                count = bt;     // we already have the data size
            }



            while (binr.ReadByte() == 0x00)
            {	//remove high order zeros in data
                count -= 1;
            }
            binr.BaseStream.Seek(-1, SeekOrigin.Current);		//last ReadByte wasn't a removed zero, so back up a byte
            return count;
        }
        private RSAParameters ConvertFromPublicKey(string pemFileConent)
        {

            byte[] keyData = Convert.FromBase64String(pemFileConent);
            if (keyData.Length < 162)
            {
                throw new ArgumentException("pem file content is incorrect.");
            }
            byte[] pemModulus = new byte[128];
            byte[] pemPublicExponent = new byte[3];
            Array.Copy(keyData, 29, pemModulus, 0, 128);
            Array.Copy(keyData, 159, pemPublicExponent, 0, 3);
            RSAParameters para = new RSAParameters();
            para.Modulus = pemModulus;
            para.Exponent = pemPublicExponent;
            return para;
        }



        #endregion


    }
}
