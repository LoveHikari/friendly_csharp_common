using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;

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
            //根据图片文件的路径使用文件流打开，并保存为byte[]
            using (FileStream fs = new FileStream(imagePath, FileMode.Open))  //可以是其他重载方法
            {
                byte[] byData = new byte[fs.Length];
                fs.Read(byData, 0, byData.Length);
                return byData;
            }
        }
        /// <summary>
        /// 获取Image对象
        /// </summary>
        /// <param name="imagePath">图片地址</param>
        /// <returns></returns>
        public static Image<Rgba32> GetImage(string imagePath)
        {
            Image<Rgba32> image = Image.Load(imagePath);
            return image;
        }
        /// <summary>
        /// 二进制数组转图片对象
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static Image<Rgba32> BytesToImage(byte[] bytes)
        {
            if (bytes == null)
                return null;
            return Image.Load(bytes);
        }
        /// <summary>
        /// 图片转二进制
        /// </summary>
        /// <param name="image">图片对象</param>  
        /// <returns>二进制</returns>  
        public static byte[] ImageToBytes(Image<Rgba32> image)
        {
            return image.SavePixelData();
        }
        /// <summary>
        /// 图片转二进制
        /// </summary>
        /// <param name="image">图片对象</param>
        /// <param name="imageFormat">后缀名</param>
        /// <returns></returns>
        public static byte[] ImageToBytes(Image<Rgba32> image, IImageFormat imageFormat)
        {

            if (image == null) { return null; }
            byte[] data;
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, imageFormat);

                ms.Position = 0;
                data = new byte[ms.Length];
                ms.Read(data, 0, Convert.ToInt32(ms.Length));
                ms.Flush();

            }
            return data;
        }
        #endregion

        #region 旋转 http://www.cnblogs.com/linezero/p/ImageSharp.html
        /// <summary>
        /// 旋转
        /// </summary>
        /// <param name="oldPath">原图路径</param>
        /// <param name="newPath">要保存的路径</param>
        /// <param name="rotateType">图像旋转量</param>
        public static void KiRotate(string oldPath, string newPath, RotateType rotateType)
        {
            using (var image = Image.Load(oldPath))
            {
                image.Mutate(x => x.Rotate(rotateType));
                image.Save(newPath);
            }
        }
        /// <summary>
        /// 旋转
        /// </summary>
        /// <param name="original">原图对象</param>
        /// <param name="newPath">要保存的路径</param>
        /// <param name="rotateType">图像旋转量</param>
        public static void KiRotate(Image<Rgba32> original, string newPath, RotateType rotateType)
        {
            original.Mutate(x => x.Rotate(rotateType));
            original.Save(newPath);
        }

        #endregion

        #region 调整大小
        /// <summary>
        /// 缩放图片
        /// </summary>
        /// <param name="original">原始图片</param>
        /// <param name="width">新的宽度</param>
        /// <param name="height">新的高度</param>
        /// <returns>处理以后的图片</returns>
        public static Image<Rgba32> ResizeImage(Image<Rgba32> original, int width, int height)
        {
            original.Mutate(x => x.Resize(width, height));
            return original;
        }
        #endregion

        #region 剪裁

        /// <summary>
        /// 剪裁
        /// </summary>
        /// <param name="image">原始图片</param>
        /// <param name="startX">开始坐标X</param>
        /// <param name="startY">开始坐标Y</param>
        /// <param name="iWidth">宽度</param>
        /// <param name="iHeight">高度</param>
        /// <returns>剪裁后的图片</returns>
        public static Image<Rgba32> KiCut(Image<Rgba32> image, int startX, int startY, int iWidth, int iHeight)
        {
            Rectangle rectangle = new Rectangle(startX, startY, iWidth, iHeight);
            image.Mutate(x => x.Crop(rectangle));
            return image;
        }

        #endregion
        /// <summary>
        /// 将图片对象保存为图片文件
        /// </summary>
        /// <param name="image">图片对象</param>
        /// <param name="filePath">要保存的路径，可以是绝对路径或相对路径</param>
        /// <param name="imageFormat">要保存的图片格式，默认为jpg</param>
        public static void SaveImage(Image<Rgba32> image, string filePath, IImageFormat imageFormat = null)
        {
            string dir = System.IO.Path.GetDirectoryName(filePath);
            //如果文件夹不存在，则创建
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

            System.IO.FileStream fs = new System.IO.FileStream(filePath, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write);
            image.Save(fs, imageFormat ?? ImageFormats.Jpeg);
            fs.Close();
            image.Dispose();
        }


    }
}