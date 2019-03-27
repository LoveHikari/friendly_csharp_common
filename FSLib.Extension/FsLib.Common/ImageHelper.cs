using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

/******************************************************************************************************************
 * 
 * 
 * 标  题： 图片处理类(版本：Version1.0.0)
 * 作  者： YuXiaoWei
 * 日  期： 2016/12/05
 * 修  改：
 * 参  考： 
 * 说  明： 暂无...
 * 备  注： 暂无...
 * 调用示列：
 *
 * 
 * ***************************************************************************************************************/

namespace System
{
    /// <summary>
    /// 图片处理类
    /// </summary>
    public class ImageHelper
    {
        #region 图片相关类型转换 http://keleyi.com/a/bjac/7531015d41ae2490.htm http://blog.csdn.net/smartsmile2012/article/details/46799417

        /// <summary>  
        /// 图片转二进制  
        /// </summary>  
        /// <param name="imagePath">图片地址</param>  
        /// <returns>二进制</returns>  
        public static byte[] GetPictureData(string imagePath)
        {
            byte[] byData;
            //根据图片文件的路径使用文件流打开，并保存为byte[]
            using (FileStream fs = new FileStream(imagePath, FileMode.Open))  //可以是其他重载方法
            {
                byData = new byte[fs.Length];
                fs.Read(byData, 0, byData.Length);
            }
            return byData;
        }
        /// <summary>
        /// 获取Image对象
        /// </summary>
        /// <param name="imagePath">图片地址</param>
        /// <returns></returns>
        public static Image GetImage(string imagePath)
        {
            Image image;
            using (FileStream fs = new FileStream(imagePath, FileMode.Open))  //可以是其他重载方法
            {
                byte[] bytes = new byte[fs.Length];
                fs.Read(bytes, 0, bytes.Length);
                using (MemoryStream ms = new MemoryStream(bytes))
                {
                    image = Image.FromStream(ms);
                    ms.Flush();

                }
            }
            return image;
        }
        /// <summary>
        /// 二进制数组转图片对象
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static Image BytesToImage(byte[] bytes)
        {
            if (bytes == null)
                return null;
            Image image;
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                image = Image.FromStream(ms);
                ms.Flush();
            }
            return image;
        }
        /// <summary>
        /// 图片转二进制
        /// </summary>
        /// <param name="image">图片对象</param>  
        /// <returns>二进制</returns>  
        public static byte[] ImageToBytes(Image image)
        {
            byte[] byData;
            //将Image转换成流数据，并保存为byte[]   
            using (MemoryStream mstream = new MemoryStream())
            {
                image.Save(mstream, ImageFormat.Bmp);
                byData = new Byte[mstream.Length];
                mstream.Position = 0;
                mstream.Read(byData, 0, byData.Length);
                mstream.Flush();
            }
            return byData;

        }
        /// <summary>
        /// 图片转二进制
        /// </summary>
        /// <param name="image">图片对象</param>
        /// <param name="imageFormat">后缀名</param>
        /// <returns></returns>
        public static byte[] ImageToBytes(Image image, ImageFormat imageFormat)
        {
            if (image == null) { return null; }
            byte[] data;
            using (MemoryStream ms = new MemoryStream())
            {
                using (Bitmap bitmap = new Bitmap(image))
                {
                    bitmap.Save(ms, imageFormat);
                    ms.Position = 0;
                    data = new byte[ms.Length];
                    ms.Read(data, 0, Convert.ToInt32(ms.Length));
                    ms.Flush();
                }
            }
            return data;
        }
        /// <summary>
        /// Bitmap转Image
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static Image BitmapToImage(Bitmap bitmap)
        {
            Image image;
            using (MemoryStream ms = new MemoryStream())
            {
                bitmap.Save(ms, ImageFormat.Png);
                image = Image.FromStream(ms);
                ms.Flush();
            }
            return image;
        }
        /// <summary>
        /// byte[] 转换 Bitmap
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static Bitmap BytesToBitmap(byte[] bytes)
        {
            Bitmap bitmap;
            using (MemoryStream stream = new MemoryStream(bytes))
            {
                bitmap = new Bitmap((Image)new Bitmap(stream));
            }

            return bitmap;
        }
        /// <summary>
        /// Bitmap转byte[]
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static byte[] BitmapToBytes(Bitmap bitmap)
        {
            byte[] byteImage;
            using (MemoryStream ms = new MemoryStream())
            {
                bitmap.Save(ms, ImageFormat.Png);
                byteImage = ms.ToArray();
                ms.Flush();
            }
            return byteImage;
        }
        /// <summary>
        /// base64转Bitmap
        /// </summary>
        /// <param name="imgStr">data:image/png;base64,iVBORw0KG</param>
        /// <returns></returns>
        public static (Bitmap bitmap, ImageFormat imageFormat) Base64ToBitmap(string imgStr)
        {
            string[] ss = imgStr.Split(',');
            string base64 = ss[1];
            //转图片
            byte[] bit = Convert.FromBase64String(base64);
            Bitmap bmp;
            using (MemoryStream ms = new MemoryStream(bit))
            {
                bmp = new Bitmap(ms);
            }

            string ifs = ss[0];
            ImageFormat imageFormat;
            if (ifs.Contains("image/png"))
            {
                imageFormat = ImageFormat.Png;
            }
            else
            {
                imageFormat = ImageFormat.Jpeg;
            }

            return (bmp, imageFormat);
        }
        #endregion

        #region 旋转 http://blog.csdn.net/jayzai/article/details/50332913 

        /// <summary>  
        /// 获取原图旋转角度(IOS和Android相机拍的照片)  
        /// </summary>  
        /// <param name="path">图片路径</param>
        /// <returns></returns>
        public static int ReadPictureDegree(string path)
        {
            int rotate = 0;
            using (var image = Image.FromFile(path))
            {
                foreach (var prop in image.PropertyItems)
                {
                    if (prop.Id == 0x112)
                    {
                        if (prop.Value[0] == 6)
                            rotate = 90;
                        if (prop.Value[0] == 8)
                            rotate = -90;
                        if (prop.Value[0] == 3)
                            rotate = 180;
                        prop.Value[0] = 1;
                    }
                }
            }
            return rotate;
        }
        /// <summary>  
        /// 获取原图旋转角度(IOS和Android相机拍的照片)  
        /// </summary>  
        /// <param name="image">图片对象</param>
        /// <returns></returns>
        public static int ReadPictureDegree(Image image)
        {
            int rotate = 0;
            foreach (var prop in image.PropertyItems)
            {
                if (prop.Id == 0x112)
                {
                    if (prop.Value[0] == 6)
                        rotate = 90;
                    if (prop.Value[0] == 8)
                        rotate = -90;
                    if (prop.Value[0] == 3)
                        rotate = 180;
                    prop.Value[0] = 1;
                }
            }

            return rotate;
        }

        /// <summary>
        /// 旋转
        /// </summary>
        /// <param name="oldPath">原图路径</param>
        /// <param name="newPath">要保存的路径</param>
        /// <param name="rotateFlipType">图像旋转量</param>  
        /// <returns></returns>  
        public static void KiRotate(string oldPath, string newPath, RotateFlipType rotateFlipType)
        {
            try
            {
                using (Bitmap bitmap = new Bitmap(oldPath))
                {
                    bitmap.RotateFlip(rotateFlipType);
                    bitmap.Save(newPath);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 旋转
        /// </summary>
        /// <param name="original">原图对象</param>
        /// <param name="newPath">要保存的路径</param>
        /// <param name="rotateFlipType">图像旋转量</param>  
        /// <returns></returns>  
        public static void KiRotate(Image original, string newPath, RotateFlipType rotateFlipType)
        {
            try
            {
                using (Bitmap bitmap = new Bitmap(original))
                {
                    bitmap.RotateFlip(rotateFlipType);
                    bitmap.Save(newPath);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region 调整大小 https://gist.github.com/nuintun/6f53784464d5ecda1be9

        /// <summary>
        /// Resize图片
        /// </summary>
        /// <param name="original">原始Bitmap</param>
        /// <param name="width">新的宽度</param>
        /// <param name="height">新的高度</param>
        /// <param name="mode">保留着，暂时未用</param>
        /// <returns>处理以后的图片</returns>
        public static Bitmap ResizeImage(Bitmap original, int width, int height, int mode)
        {
            try
            {
                Bitmap b = new Bitmap(width, height);
                Graphics g = Graphics.FromImage(b);

                // 插值算法的质量
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;

                g.DrawImage(original, new Rectangle(0, 0, width, height), new Rectangle(0, 0, original.Width, original.Height), GraphicsUnit.Pixel);
                g.Dispose();

                return b;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 缩放图片
        /// </summary>
        /// <param name="original">原始图片</param>
        /// <param name="width">新的宽度</param>
        /// <param name="height">新的高度</param>
        /// <returns>处理以后的图片</returns>
        public static Bitmap ResizeImage(Bitmap original, int width, int height)
        {
            try
            {
                // 生成新画布
                Bitmap image = new Bitmap(width, height);
                // 获取GDI+绘图图画
                Graphics graph = Graphics.FromImage(image);
                // 插值算法的质量
                graph.CompositingQuality = CompositingQuality.HighQuality;
                graph.SmoothingMode = SmoothingMode.HighQuality;
                graph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                // 缩放图片
                graph.DrawImage(original, new Rectangle(0, 0, width, height),
                    new Rectangle(0, 0, original.Width, original.Height), GraphicsUnit.Pixel);
                // 保存绘制结果
                graph.Save();
                // 释放画笔内存
                graph.Dispose();
                // 返回缩放图片
                return image;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 缩放并补白图片
        /// </summary>
        /// <param name="original">原始图片</param>
        /// <param name="width">新的宽度</param>
        /// <param name="height">新的高度</param>
        /// <param name="margin">补白宽度</param>
        /// <returns>处理以后的图片</returns>
        public static Bitmap FillerImage(Bitmap original, int width, int height, int margin)
        {
            try
            {
                // 生成新画布
                Bitmap image = new Bitmap(original.Width + 2 * margin, original.Height + 2 * margin);
                // 获取GDI+绘图图画
                Graphics graph = Graphics.FromImage(image);
                // 定义画笔
                SolidBrush brush = new SolidBrush(Color.Wheat);
                // 绘制背景色
                graph.FillRectangle(brush, new Rectangle(0, 0, image.Width, image.Height));
                // 叠加图片
                graph.DrawImageUnscaled(original, margin, margin);
                // 保存绘制结果
                graph.Save();
                // 释放GDI+绘图图画内存
                graph.Dispose();
                // 释放画笔内存
                brush.Dispose();
                // 缩放并返回处理后的图片
                return ResizeImage(image, width, height);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 剪裁 http://blog.csdn.net/ki1381/article/details/1584804

        /// <summary>
        /// 剪裁 -- 用GDI+
        /// </summary>
        /// <param name="b">原始Bitmap</param>
        /// <param name="startX">开始坐标X</param>
        /// <param name="startY">开始坐标Y</param>
        /// <param name="iWidth">宽度</param>
        /// <param name="iHeight">高度</param>
        /// <returns>剪裁后的Bitmap，失败返回null</returns>
        public static Bitmap KiCut(Bitmap b, int startX, int startY, int iWidth, int iHeight)
        {
            if (b == null)
            {
                return null;
            }

            int w = b.Width;
            int h = b.Height;

            if (startX >= w || startY >= h)
            {
                return null;
            }

            if (startX + iWidth > w)
            {
                iWidth = w - startX;
            }

            if (startY + iHeight > h)
            {
                iHeight = h - startY;
            }

            try
            {
                Bitmap bmpOut = new Bitmap(iWidth, iHeight, PixelFormat.Format24bppRgb);

                Graphics g = Graphics.FromImage(bmpOut);
                g.DrawImage(b, new Rectangle(0, 0, iWidth, iHeight), new Rectangle(startX, startY, iWidth, iHeight), GraphicsUnit.Pixel);
                g.Dispose();

                return bmpOut;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
        /// <summary>
        /// 将图片对象保存为图片文件
        /// </summary>
        /// <param name="image">图片对象</param>
        /// <param name="filePath">要保存的路径，可以是绝对路径或相对路径</param>
        /// <param name="imageFormat">要保存的图片格式，默认为jpg</param>
        public static void SaveImage(Bitmap image, string filePath, ImageFormat imageFormat = null)
        {
            string dir = System.IO.Path.GetDirectoryName(filePath);
            //如果文件夹不存在，则创建
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

            System.IO.FileStream fs = new System.IO.FileStream(filePath, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write);
            image.Save(fs, imageFormat ?? ImageFormat.Jpeg);
            fs.Close();
            image.Dispose();
        }


    }
}