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

using System.Drawing;
using System.Net.Mime;
using MetadataExtractor;
using MetadataExtractor.Formats.Exif;
using SkiaSharp;

namespace Hikari.Common.SkiaSharp
{
    /// <summary>
    /// 图片处理类
    /// </summary>
    public class ImageHelper
    {
        #region 图片相关类型转换

        /// <summary>
        /// 读取图片
        /// </summary>
        /// <param name="imagePath">图片地址</param>
        /// <returns></returns>
        public static SKImage Read(string imagePath)
        {
            SKImage image = SKImage.FromEncodedData(File.ReadAllBytes(imagePath));

            return image;
        }
        /// <summary>
        /// 读取图片
        /// </summary>
        /// <param name="imagePath">图片地址</param>
        /// <returns></returns>
        public static async Task<SKImage> ReadAsync(string imagePath)
        {
            byte[] imageData = await File.ReadAllBytesAsync(imagePath);

            SKImage image = SKImage.FromEncodedData(imageData);

            return image;
        }
        /// <summary>
        /// 将图片对象保存为图片文件
        /// </summary>
        /// <param name="image">图片对象</param>
        /// <param name="filePath">要保存的路径，可以是绝对路径或相对路径</param>
        /// <param name="imageFormat">要保存的图片格式，默认为Bmp</param>
        public static void Save(SKImage image, string filePath, SKEncodedImageFormat? imageFormat = null)
        {
            string dir = System.IO.Path.GetDirectoryName(filePath)!;
            //如果文件夹不存在，则创建
            if (dir != "" && !System.IO.Directory.Exists(dir)) System.IO.Directory.CreateDirectory(dir);

            using System.IO.FileStream fs = new System.IO.FileStream(filePath, System.IO.FileMode.Create, System.IO.FileAccess.Write);
            image.Encode(imageFormat ?? SKEncodedImageFormat.Bmp, 100).SaveTo(fs);
        }
        /// <summary>
        /// base64转Bitmap
        /// </summary>
        /// <param name="imgStr">data:image/png;base64,iVBORw0KG</param>
        /// <returns></returns>
        public static SKImage Base64ToBitmap(string imgStr)
        {
            string[] ss = imgStr.Split(',');
            string base64 = imgStr;
            if (ss.Length > 1)
            {
                base64 = ss[1];
            }
            //转图片
            byte[] bytes = Convert.FromBase64String(base64);
            SKImage image = SKImage.FromEncodedData(bytes);
            return image;
        }
        #endregion

        #region 旋转

        /// <summary>  
        /// 获取原图旋转角度(IOS和Android相机拍的照片)  
        /// </summary>  
        /// <param name="path">图片路径</param>
        /// <returns></returns>
        public static int ReadPictureDegree(string path)
        {
            int rotationAngle = 0;
            var directories = ImageMetadataReader.ReadMetadata(path);
            var exifDirectory = directories.OfType<ExifIfd0Directory>().FirstOrDefault();
            if (exifDirectory != null && exifDirectory.TryGetInt32(ExifDirectoryBase.TagOrientation, out int orientationValue))
            {
                rotationAngle = orientationValue switch
                {
                    1 => 0,
                    3 => 180,
                    6 => 90,
                    8 => 270,
                    _ => 0
                };
            }

            return rotationAngle;
        }
        /// <summary>  
        /// 获取原图旋转角度(IOS和Android相机拍的照片)  
        /// </summary>  
        /// <param name="image">图片对象</param>
        /// <returns></returns>
        public static int ReadPictureDegree(SKImage image)
        {
            int rotationAngle = 0;
            using SKData data = image.Encode();
            using MemoryStream memoryStream = new MemoryStream(data.ToArray());
            var directories = ImageMetadataReader.ReadMetadata(memoryStream);
            var exifDirectory = directories.OfType<ExifIfd0Directory>().FirstOrDefault();
            if (exifDirectory != null && exifDirectory.TryGetInt32(ExifDirectoryBase.TagOrientation, out int orientationValue))
            {
                rotationAngle = orientationValue switch
                {
                    1 => 0,
                    3 => 180,
                    6 => 90,
                    8 => 270,
                    _ => 0
                };
            }

            return rotationAngle;
        }

        /// <summary>
        /// 旋转
        /// </summary>
        /// <param name="imagePath">原图路径</param>
        /// <param name="angle">图像旋转量</param>  
        /// <returns></returns>  
        public static SKImage KiRotate(string imagePath, double angle)
        {
            // 读取图像文件
            SKBitmap bitmap = SKBitmap.Decode(imagePath);

            double radians = Math.PI * angle / 180;
            float sine = (float)Math.Abs(Math.Sin(radians));
            float cosine = (float)Math.Abs(Math.Cos(radians));
            int originalWidth = bitmap.Width;
            int originalHeight = bitmap.Height;
            int rotatedWidth = (int)(cosine * originalWidth + sine * originalHeight);
            int rotatedHeight = (int)(cosine * originalHeight + sine * originalWidth);

            var rotatedBitmap = new SKBitmap(rotatedWidth, rotatedHeight);

            using var surface = new SKCanvas(rotatedBitmap);
            // 清空画布
            surface.Clear(SKColors.Transparent);
            surface.Translate(rotatedWidth / 2f, rotatedHeight / 2f);  // 设置旋转中心为图像中心
            surface.RotateDegrees((float)angle);  // 旋转图像
            surface.Translate(-originalWidth / 2f, -originalHeight / 2f);  // 恢复旋转中心到原
            surface.DrawBitmap(bitmap, new SKPoint(0, 0));  // 绘制旋转后的图像
            return SKImage.FromBitmap(rotatedBitmap);
        }
        /// <summary>
        /// 旋转
        /// </summary>
        /// <param name="original">原图对象</param>
        /// <param name="angle">图像旋转量</param>  
        /// <returns></returns>  
        public static SKImage KiRotate(SKImage original, double angle)
        {
            // 读取图像文件
            SKBitmap bitmap = SKBitmap.FromImage(original);

            double radians = Math.PI * angle / 180;
            float sine = (float)Math.Abs(Math.Sin(radians));
            float cosine = (float)Math.Abs(Math.Cos(radians));
            int originalWidth = bitmap.Width;
            int originalHeight = bitmap.Height;
            int rotatedWidth = (int)(cosine * originalWidth + sine * originalHeight);
            int rotatedHeight = (int)(cosine * originalHeight + sine * originalWidth);

            var rotatedBitmap = new SKBitmap(rotatedWidth, rotatedHeight);

            using var surface = new SKCanvas(rotatedBitmap);
            // 清空画布
            surface.Clear(SKColors.Transparent);
            surface.Translate(rotatedWidth / 2f, rotatedHeight / 2f);  // 设置旋转中心为图像中心
            surface.RotateDegrees((float)angle);  // 旋转图像
            surface.Translate(-originalWidth / 2f, -originalHeight / 2f);  // 恢复旋转中心到原
            surface.DrawBitmap(bitmap, new SKPoint(0, 0));  // 绘制旋转后的图像
            return SKImage.FromBitmap(rotatedBitmap);
        }

        #endregion

        #region 调整大小

        /// <summary>
        /// Resize图片
        /// </summary>
        /// <param name="original">原始Bitmap</param>
        /// <param name="width">新的宽度</param>
        /// <param name="height">新的高度</param>
        /// <returns>处理以后的图片</returns>
        public static SKImage ResizeImage(SKImage original, int width, int height)
        {
            // 读取图像文件
            SKBitmap bitmap = SKBitmap.FromImage(original);

            // 创建一个缩放后的图像
            SKBitmap scaledBitmap = new SKBitmap(width, height);

            // 使用SKBitmap.Resize方法进行缩放
            bitmap.ScalePixels(scaledBitmap, new SKSamplingOptions(SKFilterMode.Nearest));
            return SKImage.FromBitmap(scaledBitmap);
        }
        /// <summary>
        /// 缩放图片
        /// </summary>
        /// <param name="original">原始图片</param>
        /// <param name="scale">缩放比例</param>
        /// <returns>处理以后的图片</returns>
        public static SKImage ResizeImage(SKImage original, double scale)
        {
            // 读取图像文件
            SKBitmap bitmap = SKBitmap.FromImage(original);

            // 计算缩放后的尺寸
            int scaledWidth = (int)(bitmap.Width * scale);
            int scaledHeight = (int)(bitmap.Height * scale);

            // 创建一个缩放后的图像
            SKBitmap scaledBitmap = new SKBitmap(scaledWidth, scaledHeight);

            // 使用SKBitmap.Resize方法进行缩放
            bitmap.ScalePixels(scaledBitmap, new SKSamplingOptions(SKFilterMode.Nearest));

            return SKImage.FromBitmap(bitmap);
        }
        ///// <summary>
        ///// 缩放并补白图片
        ///// </summary>
        ///// <param name="original">原始图片</param>
        ///// <param name="width">新的宽度</param>
        ///// <param name="height">新的高度</param>
        ///// <param name="margin">补白宽度</param>
        ///// <returns>处理以后的图片</returns>
        //public static Bitmap FillerImage(Bitmap original, int width, int height, int margin)
        //{
        //    // 生成新画布
        //    using SKBitmap canvasBitmap = new SKBitmap(original.Width + 2 * margin, original.Height + 2 * margin);
        //    // 获取GDI+绘图图画
        //    using SKCanvas canvas = new SKCanvas(canvasBitmap);

        //    // 绘制背景色
        //    SKPaint paddingPaint = new SKPaint { Color = SKColors.White };
        //    SKRect paddingRect = new SKRect(0, 0, canvasBitmap.Width, canvasBitmap.Height);
        //    canvas.DrawRect(paddingRect, paddingPaint);
        //    canvas.DrawBitmap(original, sourceRect, destRect);
        //}
        #endregion

        #region 剪裁

        /// <summary>
        /// 剪裁
        /// </summary>
        /// <param name="original">原始Bitmap</param>
        /// <param name="startX">开始坐标X</param>
        /// <param name="startY">开始坐标Y</param>
        /// <param name="iWidth">宽度</param>
        /// <param name="iHeight">高度</param>
        /// <returns>剪裁后的Image，失败返回null</returns>
        public static SKImage KiCut(SKImage original, int startX, int startY, int iWidth, int iHeight)
        {
            using var skBitmap = SKBitmap.FromImage(original);
            using var pixmap = new SKPixmap(skBitmap.Info, skBitmap.GetPixels());
            var rectI = new SKRectI(startX, startY, iWidth + startX, iHeight + startY);

            var subset = pixmap.ExtractSubset(rectI);

            using var data = subset.Encode(SKPngEncoderOptions.Default);

            return SKImage.FromEncodedData(data);
        }

        #endregion



    }
}