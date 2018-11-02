using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web;

/***
 * title:加解密工具类
 * date:2016-4-26  2016-06-02更新
 * author:YUXiaoWei
 ***/
namespace System
{
    /// <summary>
    /// 加解密工具类
    /// </summary>
    public class DEncryptHelper
    {
        #region DES加密解密
        /// <summary>
        /// DES加密
        /// </summary>
        /// <param name="strSource"></param>
        /// <returns></returns>
        public static string DESEncrypt(string strSource)
        {
            byte[] DESKey = System.Text.ASCIIEncoding.ASCII.GetBytes("xhVs6DRXLfUGxw+AhtfQdpQGoa+8SA9d");
            return DESEncrypt(strSource, DESKey);
        }
        /// <summary>
        /// DES加密
        /// </summary>
        /// <param name="strSource">待加密字串</param>
        /// <param name="key">32位Key值</param>
        /// <returns>加密后的字符串</returns>
        public static string DESEncrypt(string strSource, byte[] key)
        {
            SymmetricAlgorithm sa = Rijndael.Create();
            sa.Key = key;
            sa.Mode = CipherMode.ECB;
            sa.Padding = PaddingMode.Zeros;
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, sa.CreateEncryptor(), CryptoStreamMode.Write);
            byte[] byt = Encoding.Unicode.GetBytes(strSource);
            cs.Write(byt, 0, byt.Length);
            cs.FlushFinalBlock();
            cs.Close();
            return Convert.ToBase64String(ms.ToArray());
        }
        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="strSource"></param>
        /// <returns></returns>
        public static string DESDecrypt(string strSource)
        {
            byte[] DESKey = System.Text.ASCIIEncoding.ASCII.GetBytes("xhVs6DRXLfUGxw+AhtfQdpQGoa+8SA9d");
            return DESDecrypt(strSource, DESKey);
        }
        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="strSource">待解密的字串</param>
        /// <param name="key">32位Key值</param>
        /// <returns>解密后的字符串</returns>
        public static string DESDecrypt(string strSource, byte[] key)
        {
            SymmetricAlgorithm sa = Rijndael.Create();
            sa.Key = key;
            sa.Mode = CipherMode.ECB;
            sa.Padding = PaddingMode.Zeros;
            ICryptoTransform ct = sa.CreateDecryptor();
            byte[] byt = Convert.FromBase64String(strSource);
            MemoryStream ms = new MemoryStream(byt);
            CryptoStream cs = new CryptoStream(ms, ct, CryptoStreamMode.Read);
            StreamReader sr = new StreamReader(cs, Encoding.Unicode);
            return sr.ReadToEnd();
        }
        #endregion
        #region 3DES加密解密
        /// <summary>
        /// 默认密钥和矢量的3DES加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Encrypt3DES(string str)
        {
            //密钥
            string sKey = "xhVs6DRXLfUGxw+AhtfQdpQGoa+8SA9d";
            // //矢量,可为空
            string sIV = "4vHKRj3yfzU=";
            // //构造对称算法
            SymmetricAlgorithm mCSP = new TripleDESCryptoServiceProvider();
            ICryptoTransform ct;
            MemoryStream ms;
            CryptoStream cs;
            byte[] byt;
            mCSP.Key = Convert.FromBase64String(sKey);
            mCSP.IV = Convert.FromBase64String(sIV);
            mCSP.Mode = System.Security.Cryptography.CipherMode.ECB;
            mCSP.Padding = System.Security.Cryptography.PaddingMode.PKCS7;
            ct = mCSP.CreateEncryptor(mCSP.Key, mCSP.IV);
            byt = Encoding.UTF8.GetBytes(str);
            ms = new MemoryStream();
            cs = new CryptoStream(ms, ct, CryptoStreamMode.Write);
            cs.Write(byt, 0, byt.Length);
            cs.FlushFinalBlock();
            cs.Close();
            return Convert.ToBase64String(ms.ToArray());
        }
        /// <summary>
        /// 带指定密钥和矢量的3DES加密
        /// </summary>
        /// <param name="str"></param>
        /// <param name="sKey">32个字符</param>
        /// <param name="sIV">12个字符</param>
        /// <returns></returns>
        public string Encrypt3DES(string str, string sKey, string sIV)
        {
            SymmetricAlgorithm mCSP = new TripleDESCryptoServiceProvider();
            ICryptoTransform ct;
            MemoryStream ms;
            CryptoStream cs;
            byte[] byt;
            mCSP.Key = Convert.FromBase64String(sKey);
            mCSP.IV = Convert.FromBase64String(sIV);
            mCSP.Mode = System.Security.Cryptography.CipherMode.ECB;
            mCSP.Padding = System.Security.Cryptography.PaddingMode.PKCS7;
            ct = mCSP.CreateEncryptor(mCSP.Key, mCSP.IV);
            byt = Encoding.UTF8.GetBytes(str);
            ms = new MemoryStream();
            cs = new CryptoStream(ms, ct, CryptoStreamMode.Write);
            cs.Write(byt, 0, byt.Length);
            cs.FlushFinalBlock();
            cs.Close();
            return Convert.ToBase64String(ms.ToArray());
        }
        /// <summary>
        /// 默认密钥和矢量的3DES解密
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static string Decrypt3DES(string Value)
        {
            string sKey = "xhVs6DRXLfUGxw+AhtfQdpQGoa+8SA9d";//32个字符
            string sIV = "4vHKRj3yfzU=";//12个字符  
            SymmetricAlgorithm mCSP = new TripleDESCryptoServiceProvider();
            ICryptoTransform ct;
            MemoryStream ms;
            CryptoStream cs;
            byte[] byt;
            mCSP.Key = Convert.FromBase64String(sKey);
            mCSP.IV = Convert.FromBase64String(sIV);
            mCSP.Mode = System.Security.Cryptography.CipherMode.ECB;
            mCSP.Padding = System.Security.Cryptography.PaddingMode.PKCS7;
            ct = mCSP.CreateDecryptor(mCSP.Key, mCSP.IV);
            byt = Convert.FromBase64String(Value);
            ms = new MemoryStream();
            cs = new CryptoStream(ms, ct, CryptoStreamMode.Write);
            cs.Write(byt, 0, byt.Length);
            cs.FlushFinalBlock();
            cs.Close();
            return Encoding.UTF8.GetString(ms.ToArray());
        }
        /// <summary>
        /// 带指定密钥和矢量的3DES解密
        /// </summary>
        /// <param name="Value"></param>
        /// <param name="sKey">32个字符</param>
        /// <param name="sIV">12个字符</param>
        /// <returns></returns>
        public string Decrypt3DES(string str, string sKey, string sIV)
        {
            SymmetricAlgorithm mCSP = new TripleDESCryptoServiceProvider();
            ICryptoTransform ct;
            MemoryStream ms;
            CryptoStream cs;
            byte[] byt;
            mCSP.Key = Convert.FromBase64String(sKey);
            mCSP.IV = Convert.FromBase64String(sIV);
            mCSP.Mode = System.Security.Cryptography.CipherMode.ECB;
            mCSP.Padding = System.Security.Cryptography.PaddingMode.PKCS7;
            ct = mCSP.CreateDecryptor(mCSP.Key, mCSP.IV);
            byt = Convert.FromBase64String(str);
            ms = new MemoryStream();
            cs = new CryptoStream(ms, ct, CryptoStreamMode.Write);
            cs.Write(byt, 0, byt.Length);
            cs.FlushFinalBlock();
            cs.Close();
            return Encoding.UTF8.GetString(ms.ToArray());
        }
        #endregion
        #region 一个简单的加密解密方法,只支持英文
        public static string EnCryptEnStr(string str) //倒序加1加密
        {
            byte[] by = new byte[str.Length];
            for (int i = 0;
            i <= str.Length - 1;
            i++)
            {
                by[i] = (byte)((byte)str[i] + 1);
            }
            str = "";
            for (int i = by.Length - 1;
            i >= 0;
            i--)
            {
                str += ((char)by[i]).ToString();
            }
            return str;
        }
        public static string DeCryptEnStr(string str) //顺序减1解码
        {
            byte[] by = new byte[str.Length];
            for (int i = 0;
            i <= str.Length - 1;
            i++)
            {
                by[i] = (byte)((byte)str[i] - 1);
            }
            str = "";
            for (int i = by.Length - 1;
            i >= 0;
            i--)
            {
                str += ((char)by[i]).ToString();
            }
            return str;
        }
        #endregion
        #region 一个简单的加密解密方法,在上一个的基础上支持中文
        public static string EnCryptCnStr(string str)
        {
            string htext = ""; // blank text
            for (int i = 0; i < str.Length; i++)
            {
                htext = htext + (char)(str[i] + 10 - 1 * 2);
            }
            return htext;
        }
        public static string DeCryptCnStr(string str)
        {
            string dtext = "";
            for (int i = 0; i < str.Length; i++)
            {
                dtext = dtext + (char)(str[i] - 10 + 1 * 2);
            }
            return dtext;
        }
        #endregion
        #region Url地址编码解码
        /// <summary>
        /// 编码Url地址
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string UrlEncode(string url)
        {
            byte[] mByte = null;
            mByte = System.Text.Encoding.GetEncoding("GB2312").GetBytes(url);
            return System.Web.HttpUtility.UrlEncode(mByte);
        }
        /// <summary>
        /// 解码Url地址
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string UrlDecode(string url)
        {
            return HttpUtility.UrlDecode(url, System.Text.Encoding.GetEncoding("GB2312"));
        }
        #endregion

        #region DES加密解密
        /// <summary>
        /// DES加密
        /// </summary>
        /// <param name="data">加密数据</param>
        /// <param name="key">8位字符的密钥字符串</param>
        /// <param name="iv">8位字符的初始化向量字符串</param>
        /// <returns></returns>
        public static string DESEncrypt(string data, string key, string iv)
        {
            byte[] byKey = System.Text.ASCIIEncoding.ASCII.GetBytes(key);
            byte[] byIV = System.Text.ASCIIEncoding.ASCII.GetBytes(iv);

            DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
            int i = cryptoProvider.KeySize;
            MemoryStream ms = new MemoryStream();
            CryptoStream cst = new CryptoStream(ms, cryptoProvider.CreateEncryptor(byKey, byIV), CryptoStreamMode.Write);

            StreamWriter sw = new StreamWriter(cst);
            sw.Write(data);
            sw.Flush();
            cst.FlushFinalBlock();
            sw.Flush();
            return Convert.ToBase64String(ms.GetBuffer(), 0, (int)ms.Length);
        }

        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="data">解密数据</param>
        /// <param name="key">8位字符的密钥字符串(需要和加密时相同)</param>
        /// <param name="iv">8位字符的初始化向量字符串(需要和加密时相同)</param>
        /// <returns></returns>
        public static string DESDecrypt(string data, string key, string iv)
        {
            byte[] byKey = System.Text.ASCIIEncoding.ASCII.GetBytes(key);
            byte[] byIV = System.Text.ASCIIEncoding.ASCII.GetBytes(iv);

            byte[] byEnc;
            try
            {
                byEnc = Convert.FromBase64String(data);
            }
            catch
            {
                return null;
            }

            DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
            MemoryStream ms = new MemoryStream(byEnc);
            CryptoStream cst = new CryptoStream(ms, cryptoProvider.CreateDecryptor(byKey, byIV), CryptoStreamMode.Read);
            StreamReader sr = new StreamReader(cst);
            return sr.ReadToEnd();
        }
        #endregion

        /// <summary>
        /// SHA1加密
        /// </summary>
        /// <param name="content"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string SHA1Encrypt(string content, string encoding = "utf-8")
        {
            SHA1 sha1 = new SHA1CryptoServiceProvider();
            //SHA1 sha1 = System.Security.Cryptography.SHA1.Create();
            byte[] bytesIn = System.Text.Encoding.GetEncoding(encoding).GetBytes(content);
            byte[] bytesOut = sha1.ComputeHash(bytesIn);
            sha1.Clear();
            sha1.Dispose();
            string result = BitConverter.ToString(bytesOut);
            result = result.Replace("-", "");
            return result;
        }
    }
}