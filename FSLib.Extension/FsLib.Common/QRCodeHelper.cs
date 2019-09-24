using System.Text;
using ThoughtWorks.QRCode.Codec;
using ThoughtWorks.QRCode.Codec.Data;

namespace System
{
    /// <summary>
    /// 二维码帮助类
    /// </summary>
    public class QRCodeHelper
    {
        /// <summary>
        /// 解码二维码
        /// </summary>
        /// <param name="strbase64">待解码的二维码图片</param>
        /// <returns>扫码结果</returns>
        public static string DecodeQrCode(string strbase64)
        {
            QRCodeDecoder qrDecoder = new QRCodeDecoder();
            string txtMsg = qrDecoder.decode(new QRCodeBitmapImage(ImageHelper.Base64ToBitmap(strbase64).bitmap), Encoding.UTF8);
            return txtMsg;
        }
    }
}