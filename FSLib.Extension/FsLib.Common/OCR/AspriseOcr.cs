using System.Drawing;
using System.Runtime.InteropServices;

/******************************************************************************************************************
 * 
 * 
 * 标  题： AspriseOCR 识别类(版本：Version1.0.0)
 * 作  者： YuXiaoWei
 * 日  期： 2016/12/28
 * 修  改：
 * 参  考： http://www.cnblogs.com/slyzly/articles/2121420.html
 * 说  明： 暂无...
 * 备  注： 暂无...
 * 调用示列：
 * 
 * 
 * ***************************************************************************************************************/
namespace System.OCR
{
    /// <summary>
    /// Asprise 方式识别
    /// </summary>
    public class AspriseOcr
    {
        #region DllImport
        /// <summary>
        /// 精确图像文件路径及格式，该功能将以字符串形式返回图片内容，如果类型参数设置为-1，Asprise OCR将自动决定文件格式。Asprise
        /// OCR支持的图片格式较广泛，如.bmp,.ico,.jpg,.jpeg,.png,.pic,.jng,.gif等多达30种图片格式。
        /// </summary>
        /// <param name="imagePath">图片文件路径</param>
        /// <param name="imageFileType">如果类型参数设置为-1，Asprise OCR将自动决定文件格式</param>
        /// <returns></returns>
        [DllImport("AspriseOCR.dll", EntryPoint = "OCR", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr OCR(string imagePath, int imageFileType);
        /// <summary>
        /// 图片的部分区域实施OCR任务，其中(startX, startY)对应图像的左上方区域，(width, height)对应区域的宽度和高度。
        /// </summary>
        /// <param name="imagePath">图片文件路径</param>
        /// <param name="imageFileType">如果类型参数设置为-1，Asprise OCR将自动决定文件格式</param>
        /// <param name="startX">开始x坐标，为0即可</param>
        /// <param name="startY">开始y坐标，为0即可</param>
        /// <param name="width">图片的宽</param>
        /// <param name="height">图片的高</param>
        /// <returns></returns>
        [DllImport("AspriseOCR.dll", EntryPoint = "OCRpart", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr OCRpart(string imagePath, int imageFileType, int startX, int startY, int width, int height);
        /// <summary>
        /// 识别图片中的条形码，当有多个条形码时，会以换行符分割
        /// </summary>
        /// <param name="imagePath"></param>
        /// <param name="imageFileType"></param>
        /// <returns></returns>
        [DllImport("AspriseOCR.dll", EntryPoint = "OCRBarCodes", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr OCRBarCodes(string imagePath, int imageFileType);
        /// <summary>
        /// 识别图片中条形码的部分区域
        /// </summary>
        /// <param name="imagePath"></param>
        /// <param name="imageFileType"></param>
        /// <param name="startX"></param>
        /// <param name="startY"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        [DllImport("AspriseOCR.dll", EntryPoint = "OCRpartBarCodes", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr OCRpartBarCodes(string imagePath, int imageFileType, int startX, int startY, int width, int height);
        #endregion
        /// <summary>
        /// 识别图片，无法识别中文
        /// </summary>
        /// <param name="imgPath">图片路径</param>
        /// <returns></returns>
        public static string DiscernImage(string imgPath)
        {
            int startX = 0;
            int startY = 0;

            Image img = Image.FromFile(imgPath);
            int width = img.Width;
            int height = img.Height;
            img.Dispose();
            return Marshal.PtrToStringAnsi(OCRpart(imgPath, -1, startX, startY, width, height));  //将返回string,并以"\r\n"结尾
        }
    }
}