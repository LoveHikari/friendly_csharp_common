using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using ThoughtWorks.QRCode.Codec;
using ThoughtWorks.QRCode.Codec.Data;
using ZXing;
using ZXing.QrCode;
using ZXing.QrCode.Internal;
using ZXing.Windows.Compatibility;

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
            var barcodeBitmap = ImageHelper.Base64ToBitmap(strbase64);
            string text = DecodeQrCode(barcodeBitmap);
            return text;
        }
        /// <summary>
        /// 解码二维码
        /// </summary>
        /// <param name="barcodeBitmap">待解码的二维码图片</param>
        /// <returns>扫码结果</returns>
        public static string DecodeQrCode(Bitmap barcodeBitmap)
        {
            var barcodeReader = new ZXing.BarcodeReader()
            {
                Options = { CharacterSet = "utf-8" }
            };

            var coreCompatResult = barcodeReader.Decode(barcodeBitmap);
            return coreCompatResult?.Text;

        }
        /// <summary>
        /// 生成二维码
        /// </summary>
        /// <param name="content">内容文本</param>
        /// <param name="qRCodeEncodeMode">二维码编码方式</param>
        /// <param name="qRCodeErrorCorrect">纠错码等级</param>
        /// <param name="qRCodeVersion">二维码版本号 0-40</param>
        /// <param name="qRCodeScale">每个小方格的预设宽度（像素），正整数</param>
        /// <param name="size">图片尺寸（像素），0表示不设置</param>
        /// <param name="border">图片白边（像素），当size大于0时有效</param>
        /// <param name="codeEyeColor">定位点着色参数</param>
        /// <returns></returns>
        public static System.Drawing.Image EncodeQrCode(string content, QRCodeEncoder.ENCODE_MODE qRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE, QRCodeEncoder.ERROR_CORRECTION qRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.L, int qRCodeVersion = 8, int qRCodeScale = 4, int size = 0, int border = 0, Color codeEyeColor = default)
        {
            QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
            qrCodeEncoder.QRCodeEncodeMode = qRCodeEncodeMode;
            qrCodeEncoder.QRCodeErrorCorrect = qRCodeErrorCorrect;
            qrCodeEncoder.QRCodeScale = qRCodeScale;
            qrCodeEncoder.QRCodeVersion = qRCodeVersion;

            System.Drawing.Image image = qrCodeEncoder.Encode(content);

            #region 根据设定的目标图片尺寸调整二维码QRCodeScale设置，并添加边框
            if (size > 0)
            {
                //当设定目标图片尺寸大于生成的尺寸时，逐步增大方格尺寸
                #region 当设定目标图片尺寸大于生成的尺寸时，逐步增大方格尺寸
                while (image.Width < size)
                {
                    qrCodeEncoder.QRCodeScale++;
                    System.Drawing.Image imageNew = qrCodeEncoder.Encode(content);
                    if (imageNew.Width < size)
                    {
                        image = new System.Drawing.Bitmap(imageNew);
                        imageNew.Dispose();
                        imageNew = null;
                    }
                    else
                    {
                        qrCodeEncoder.QRCodeScale--; //新尺寸未采用，恢复最终使用的尺寸
                        imageNew.Dispose();
                        imageNew = null;
                        break;
                    }
                }
                #endregion

                //当设定目标图片尺寸小于生成的尺寸时，逐步减小方格尺寸
                #region 当设定目标图片尺寸小于生成的尺寸时，逐步减小方格尺寸
                while (image.Width > size && qrCodeEncoder.QRCodeScale > 1)
                {
                    qrCodeEncoder.QRCodeScale--;
                    System.Drawing.Image imageNew = qrCodeEncoder.Encode(content);
                    image = new System.Drawing.Bitmap(imageNew);
                    imageNew.Dispose();
                    imageNew = null;
                    if (image.Width < size)
                    {
                        break;
                    }
                }
                #endregion

                //根据参数设置二维码图片白边的最小宽度（按需缩小）
                #region 根据参数设置二维码图片白边的最小宽度
                if (image.Width <= size && border > 0)
                {
                    while (image.Width <= size && size - image.Width < border * 2 && qrCodeEncoder.QRCodeScale > 1)
                    {
                        qrCodeEncoder.QRCodeScale--;
                        System.Drawing.Image imageNew = qrCodeEncoder.Encode(content);
                        image = new System.Drawing.Bitmap(imageNew);
                        imageNew.Dispose();
                        imageNew = null;
                    }
                }
                #endregion

                //已经确认二维码图像，为图像染色修饰
                if (true)
                {
                    //定位点方块边长
                    int beSize = qrCodeEncoder.QRCodeScale * 3;

                    int bep1_l = qrCodeEncoder.QRCodeScale * 2;
                    int bep1_t = qrCodeEncoder.QRCodeScale * 2;

                    int bep2_l = image.Width - qrCodeEncoder.QRCodeScale * 5 - 1;
                    int bep2_t = qrCodeEncoder.QRCodeScale * 2;

                    int bep3_l = qrCodeEncoder.QRCodeScale * 2;
                    int bep3_t = image.Height - qrCodeEncoder.QRCodeScale * 5 - 1;

                    int bep4_l = image.Width - qrCodeEncoder.QRCodeScale * 7 - 1;
                    int bep4_t = image.Height - qrCodeEncoder.QRCodeScale * 7 - 1;

                    System.Drawing.Graphics graphic0 = System.Drawing.Graphics.FromImage(image);

                    // Create solid brush.
                    SolidBrush blueBrush = new SolidBrush(codeEyeColor);

                    // Fill rectangle to screen.
                    graphic0.FillRectangle(blueBrush, bep1_l, bep1_t, beSize, beSize);
                    graphic0.FillRectangle(blueBrush, bep2_l, bep2_t, beSize, beSize);
                    graphic0.FillRectangle(blueBrush, bep3_l, bep3_t, beSize, beSize);
                    graphic0.FillRectangle(blueBrush, bep4_l, bep4_t, qrCodeEncoder.QRCodeScale, qrCodeEncoder.QRCodeScale);
                }

                //当目标图片尺寸大于二维码尺寸时，将二维码绘制在目标尺寸白色画布的中心位置
                #region 如果目标尺寸大于生成的图片尺寸，将二维码绘制在目标尺寸白色画布的中心位置
                if (image.Width < size)
                {
                    //新建空白绘图
                    System.Drawing.Bitmap panel = new System.Drawing.Bitmap(size, size);
                    System.Drawing.Graphics graphic0 = System.Drawing.Graphics.FromImage(panel);
                    int p_left = 0;
                    int p_top = 0;
                    if (image.Width <= size) //如果原图比目标形状宽
                    {
                        p_left = (size - image.Width) / 2;
                    }
                    if (image.Height <= size)
                    {
                        p_top = (size - image.Height) / 2;
                    }

                    //将生成的二维码图像粘贴至绘图的中心位置
                    graphic0.DrawImage(image, p_left, p_top, image.Width, image.Height);
                    image = new System.Drawing.Bitmap(panel);
                    panel.Dispose();
                    panel = null;
                    graphic0.Dispose();
                    graphic0 = null;
                }
                #endregion
            }
            #endregion
            return image;
        }
        /// <summary>
        /// 生成二维码
        /// </summary>
        /// <param name="content">内容文本</param>
        /// <param name="size">图片尺寸（像素），0表示不设置</param>
        /// <param name="margin">二维码的边距,图片尺寸大于0生效</param>
        /// <param name="errorCorrection">纠错码等级</param>
        /// <returns></returns>
        public static System.Drawing.Image EncodeQrCode2(string content, int size = 10, int margin = 2, string errorCorrection = "L")
        {
            ZXing.BarcodeWriter barCodeWriter = new ZXing.BarcodeWriter();
            barCodeWriter.Format = BarcodeFormat.QR_CODE;
            QrCodeEncodingOptions options = new QrCodeEncodingOptions()
            {
                // DisableECI = true,
                CharacterSet = "utf-8", // 设置内容编码
                                        // QrVersion = 8,

            };
            errorCorrection = errorCorrection.ToUpper();
            IDictionary<string, int> dic = new Dictionary<string, int>()
            {
                {"L", 1 },
                {"M", 0 },
                {"Q", 3 },
                {"H", 2 }
            };
            if (!dic.ContainsKey(errorCorrection))
            {
                errorCorrection = "L";
            }

            
            if (size != 0)
            {
                int s;
                if (errorCorrection == "L" || errorCorrection == "M")
                {
                    s = size * 27;
                }
                else
                {
                    s = size * 31;
                }

                s += (margin - 1) * (size * 2);

                options.Width = s;
                options.Height = s;
                options.Margin = margin; // 设置二维码的边距,单位不是固定像素
            }


            options.ErrorCorrection = ErrorCorrectionLevel.forBits(dic[errorCorrection]);


            barCodeWriter.Options = options;
            ZXing.Common.BitMatrix bm = barCodeWriter.Encode(content);
            Bitmap result = barCodeWriter.Write(bm);

            return result;

        }
    }
}