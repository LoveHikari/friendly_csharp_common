using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Mime;
using SkiaSharp;

namespace Hikari.Common
{
    /// <summary>
    /// 验证码帮助类
    /// </summary>
    public class CaptchaHelper
    {
        /// <summary>
        /// 创建验证码
        /// </summary>
        /// <returns>随机表达式，表达式结果，图片base64</returns>
        public static (string randomCode, int value, string pic) CreateCaptcha()
        {
            var captchaCode = GetCaptchaCode();
            byte[] bytes = CreateCaptchaImage(captchaCode.randomCode);
            string pic = Convert.ToBase64String(bytes);
            return (captchaCode.randomCode, captchaCode.value, pic);
        }

        /// <summary>
        /// 获得表达式和表达式结果
        /// </summary>
        /// <returns>随机表达式，表达式结果</returns>
        private static (string randomCode, int value) GetCaptchaCode()
        {
            int value = 0;
            char[] operators = { '+', '-', '*' };
            string randomCode = string.Empty;
            Random random = new Random();

            int first = random.Next() % 10;
            int second = random.Next() % 10;
            char operatorChar = operators[random.Next(0, operators.Length)];
            switch (operatorChar)
            {
                case '+': value = first + second; break;
                case '-':
                    // 第1个数要大于第二个数
                    if (first < second)
                    {
                        int temp = first;
                        first = second;
                        second = temp;
                    }
                    value = first - second;
                    break;
                case '*': value = first * second; break;
            }

            char code = (char)('0' + (char)first);
            randomCode += code;
            randomCode += operatorChar;
            code = (char)('0' + (char)second);
            randomCode += code;
            randomCode += "=?";
            return (randomCode, value);
        }
        /// <summary>
        /// 生成验证码图片
        /// </summary>
        /// <param name="randomCode">随机码</param>
        /// <returns>图片</returns>
        private static byte[] CreateCaptchaImage(string randomCode)
        {
            const int randAngle = 45; //随机转动角度
            int mapwidth = (int)(randomCode.Length * 16);
            SKBitmap map = new SKBitmap(mapwidth, 28);//创建图片背景
            using SKCanvas canvas = new SKCanvas(map);
            canvas.Clear(SKColors.AliceBlue);//清除画面，填充背景

            Random random = new Random();

            //背景噪点生成，为了在白色背景上显示，尽量生成深色
            int intRed = random.Next(256);
            int intGreen = random.Next(256);
            int intBlue = (intRed + intGreen > 400) ? 0 : 400 - intRed - intGreen;
            intBlue = (intBlue > 255) ? 255 : intBlue;

            var blackPen = new SKColor((byte)intRed, (byte)intGreen, (byte)intBlue);
            for (int i = 0; i < 50; i++)
            {
                int x = random.Next(0, map.Width);
                int y = random.Next(0, map.Height);
                canvas.DrawPoint(x, y, new SKPaint() {Color = blackPen});
            }
            //绘制干扰曲线
            for (int i = 0; i < 2; i++)
            {
                SKPoint p1 = new SKPoint(0, random.Next(map.Height));
                SKPoint p2 = new SKPoint(random.Next(map.Width), random.Next(map.Height));
                SKPoint p3 = new SKPoint(random.Next(map.Width), random.Next(map.Height));
                SKPoint p4 = new SKPoint(map.Width, random.Next(map.Height));
                SKPoint[] p = { p1, p2, p3, p4 };
                using SKPaint pen = new SKPaint { Color = SKColors.Gray, StrokeWidth = 1 };
                canvas.DrawPoints(SKPointMode.Polygon, p, pen);
            }

            //文字距中
            using SKPaint paint = new SKPaint { IsAntialias = true };
            paint.TextAlign = SKTextAlign.Center;
            paint.TextSize = 14;

            //定义颜色
            SKColor[] colors = { SKColors.Black, SKColors.Red, SKColors.DarkBlue, SKColors.Green, SKColors.Orange, SKColors.Brown, SKColors.DarkCyan, SKColors.Purple };
            //定义字体
            string[] fonts = { "Verdana", "Microsoft Sans Serif", "Comic Sans MS", "Arial", "宋体" };
            int cindex = random.Next(7);

            //验证码旋转，防止机器识别
            char[] chars = randomCode.ToCharArray();//拆散字符串成单字符数组
            foreach (char t in chars)
            {
                int findex = random.Next(5);
                using SKTypeface typeface = SKTypeface.FromFamilyName(fonts[findex], SKFontStyle.Bold);
                paint.Typeface = typeface;
                paint.Color = colors[cindex];
                SKPoint dot = new SKPoint(14, 14);
                float angle = random.NextSingle() * 2 * randAngle - randAngle;  // 转动的度数
                if (t == '+' || t == '-' || t == '*')
                {
                    //加减乘运算符不进行旋转
                    canvas.Translate(dot.X, dot.Y);//移动光标到指定位置
                    canvas.DrawText(t.ToString(), 1, 1, paint);
                    canvas.Translate(-2, -dot.Y);//移动光标到指定位置，每个字符紧凑显示，避免被软件识别
                }
                else
                {
                    canvas.Translate(dot.X, dot.Y);//移动光标到指定位置
                    canvas.RotateDegrees(angle);
                    canvas.DrawText(t.ToString(), 1, 1, paint);
                    canvas.RotateDegrees(-angle);//转回去
                    canvas.Translate(-2, -dot.Y);//移动光标到指定位置，每个字符紧凑显示，避免被软件识别
                }
            }

            //生成图片
            using SKData encodedData = map.Encode(SKEncodedImageFormat.Png, 100);
            return encodedData.ToArray();
        }
        ///// <summary>
        ///// 生成验证码图片
        ///// </summary>
        ///// <param name="randomCode">随机码</param>
        ///// <returns>图片</returns>
        //private static byte[] CreateCaptchaImage2(string randomCode)
        //{
        //    const int randAngle = 45; //随机转动角度
        //    int mapwidth = (int)(randomCode.Length * 16);
        //    Bitmap map = new Bitmap(mapwidth, 28);//创建图片背景
        //    Graphics graph = Graphics.FromImage(map);
        //    graph.Clear(Color.AliceBlue);//清除画面，填充背景

        //    Random random = new Random();
        //    //背景噪点生成，为了在白色背景上显示，尽量生成深色
        //    //int intRed = random.Next(256);
        //    //int intGreen = random.Next(256);
        //    //int intBlue = (intRed + intGreen > 400) ? 0 : 400 - intRed - intGreen;
        //    //intBlue = (intBlue > 255) ? 255 : intBlue;

        //    //Pen blackPen = new Pen(Color.FromArgb(intRed, intGreen, intBlue), 0);
        //    //for (int i = 0; i < 50; i++)
        //    //{
        //    //    int x = random.Next(0, map.Width);
        //    //    int y = random.Next(0, map.Height);
        //    //    graph.DrawRectangle(blackPen, x, y, 1, 1);
        //    //}
        //    //绘制干扰曲线
        //    for (int i = 0; i < 2; i++)
        //    {
        //        Point p1 = new Point(0, random.Next(map.Height));
        //        Point p2 = new Point(random.Next(map.Width), random.Next(map.Height));
        //        Point p3 = new Point(random.Next(map.Width), random.Next(map.Height));
        //        Point p4 = new Point(map.Width, random.Next(map.Height));
        //        Point[] p = { p1, p2, p3, p4 };
        //        using (Pen pen = new Pen(Color.Gray, 1))
        //        {
        //            graph.DrawBeziers(pen, p);
        //        }
        //    }

        //    //文字距中
        //    using (StringFormat format = new StringFormat(StringFormatFlags.NoClip))
        //    {
        //        format.Alignment = StringAlignment.Center;
        //        format.LineAlignment = StringAlignment.Center;

        //        //定义颜色
        //        Color[] c = { Color.Black, Color.Red, Color.DarkBlue, Color.Green, Color.Orange, Color.Brown, Color.DarkCyan, Color.Purple };
        //        //定义字体
        //        string[] fonts = { "Verdana", "Microsoft Sans Serif", "Comic Sans MS", "Arial", "宋体" };
        //        int cindex = random.Next(7);

        //        //验证码旋转，防止机器识别
        //        char[] chars = randomCode.ToCharArray();//拆散字符串成单字符数组
        //        foreach (char t in chars)
        //        {
        //            int findex = random.Next(5);
        //            using (Font font = new Font(fonts[findex], 14, FontStyle.Bold))//字体样式(参数2为字体大小)
        //            {
        //                using (Brush brush = new SolidBrush(c[cindex]))
        //                {
        //                    Point dot = new Point(14, 14);
        //                    float angle = random.Next(-randAngle, randAngle);//转动的度数
        //                    if (t == '+' || t == '-' || t == '*')
        //                    {
        //                        //加减乘运算符不进行旋转
        //                        graph.TranslateTransform(dot.X, dot.Y);//移动光标到指定位置
        //                        graph.DrawString(t.ToString(), font, brush, 1, 1, format);
        //                        graph.TranslateTransform(-2, -dot.Y);//移动光标到指定位置，每个字符紧凑显示，避免被软件识别
        //                    }
        //                    else
        //                    {
        //                        graph.TranslateTransform(dot.X, dot.Y);//移动光标到指定位置
        //                        graph.RotateTransform(angle);
        //                        graph.DrawString(t.ToString(), font, brush, 1, 1, format);
        //                        graph.RotateTransform(-angle);//转回去
        //                        graph.TranslateTransform(-2, -dot.Y);//移动光标到指定位置，每个字符紧凑显示，避免被软件识别
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    //生成图片
        //    using (MemoryStream ms = new MemoryStream())
        //    {
        //        map.Save(ms, ImageFormat.Gif);

        //        graph.Dispose();
        //        map.Dispose();
        //        return ms.GetBuffer();
        //    }
        //}
    }
}