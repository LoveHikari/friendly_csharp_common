using System.IO;
using System.Net;
using System.Web;

/******************************************************************************************************************
 * 
 * 
 * 标  题： http下载类(版本：Version1.0.0)
 * 作  者： YuXiaoWei
 * 日  期： 2016/12/14
 * 修  改：
 * 参  考： http://www.jb51.net/article/57068.htm
 * 说  明： 暂无...
 * 备  注： 暂无...
 * 调用示列：
 * 
 * 
 * ***************************************************************************************************************/
namespace System
{
    /// <summary>
    /// http下载类
    /// </summary>
    public class HttpDownload
    {
        /// <summary>
        /// Http下载文件
        /// </summary>
        /// <param name="url">下载地址</param>
        /// <param name="path">保存路径</param>
        /// <returns>cookie</returns>
        public static string HttpDownloadFile(string url, string path)
        {
            path = FileHelper.FilePathProcess(path);
            string dir = System.IO.Path.GetDirectoryName(path);
            if (!System.IO.Directory.Exists(dir))
            {
                System.IO.Directory.CreateDirectory(dir);
            }
            
            // 设置参数
            HttpWebRequest request = WebRequest.Create(HttpUtility.HtmlEncode(url)) as HttpWebRequest;

            //发送请求并获取相应回应数据
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;

            string cookie = request.CookieContainer?.GetCookieHeader(request.RequestUri);
            //直到request.GetResponse()程序才开始向目标网页发送Post请求
            using (Stream responseStream = response.GetResponseStream())
            {
                //创建本地文件写入流
                using (Stream stream = new FileStream(path, FileMode.Create))
                {
                    byte[] bArr = new byte[1024];
                    int size = responseStream.Read(bArr, 0, (int)bArr.Length);
                    while (size > 0)
                    {
                        stream.Write(bArr, 0, size);
                        size = responseStream.Read(bArr, 0, (int)bArr.Length);
                    }
                    //Bitmap bitmap = new Bitmap((Image)new Bitmap(stream));
                }
            }
            return cookie;
        }
    }
}