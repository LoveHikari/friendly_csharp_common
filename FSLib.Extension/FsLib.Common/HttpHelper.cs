﻿using System.Collections;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;

/******************************************************************************************************************
 * 
 * 
 * 标   题： 获得http信息(版本：Version1.0.1)
 * 作　者：YuXiaoWei
 * 日　期：2016/10/18
 * 修　改：添加了上传图片方法(Version1.0.1)
 * 参　考：http://www.cnblogs.com/zjfree/archive/2011/03/10/1980325.html http://www.cnblogs.com/xssxss/archive/2012/07/03/2574554.html
 * 说　明： 暂无...
 * 备　注： 暂无...
 * 
 * 
 * ***************************************************************************************************************/
namespace System
{
    /// <summary>
    /// 获得http信息
    /// </summary>
    public class HttpHelper
    {
        #region GetHttp

        /// <summary>
        /// GetHttpWebRequest方式获得html，推荐
        /// </summary>
        /// <param name="url">网址</param>
        /// <param name="chareset">编码</param>
        /// <param name="proxy">http代理设置</param>
        /// <c>！注意：有时候请求会重定向，但我们就需要从重定向url获取东西，像QQ登录成功后获取sid，但上面的会自动根据重定向地址跳转。我们可以用:
        ///     request.AllowAutoRedirect = false;设置重定向禁用，你就可以从headers的Location属性中获取重定向地址</c>
        /// <returns>html代码</returns>
        public static string GetHttpWebRequest(string url, string chareset, WebProxy proxy)
        {
            return HttpRequest(url, "GET", "", chareset, null, proxy, null, "").retHtml;
        }


        /// <summary>
        /// PostHttpWebRequest方式获得html，推荐
        /// </summary>
        /// <param name="url">网址</param>
        /// <param name="chareset">编码</param>
        /// <param name="proxy">http代理设置</param>
        /// <c>在post的时候有时也用的到cookie，像登录163发邮件时候就需要发送cookie，所以在外部一个cookie属性随时保存 CookieContainer cookie = new CookieContainer();</c>
        /// <returns>html代码</returns>
        public static string PostHttpWebRequest(string url, string chareset = "utf-8", WebProxy proxy = null)
        {
            return HttpRequest(url, "POST", "", chareset, null, proxy, null, "").retHtml;
        }

        /// <summary>
        /// GetWebRequest方式获得html
        /// </summary>
        /// <param name="url">网址</param>
        /// <param name="chareset">编码</param>
        /// <returns>html代码</returns>
        public static string GetWebRequest(string url, string chareset = "utf-8")
        {
            Uri uri = new Uri(url);
            WebRequest myReq = WebRequest.Create(uri);
            WebResponse result = myReq.GetResponse();
            Stream receviceStream = result.GetResponseStream();
            StreamReader readerOfStream = new StreamReader(receviceStream, System.Text.Encoding.GetEncoding(chareset));
            string strHTML = readerOfStream.ReadToEnd();
            readerOfStream.Close();
            receviceStream.Close();
            result.Close();
            myReq.Abort();
            return strHTML;
        }
        /// <summary>
        /// GetWebClient方式获得html
        /// </summary>
        /// <param name="url">网址</param>
        /// <param name="chareset">编码</param>
        /// <returns>html代码</returns>
        public static string GetWebClient(string url, string chareset = "utf-8")
        {
            string strHTML = "";
            WebClient myWebClient = new WebClient();
            myWebClient.Proxy = null;  //将其默认代理设置为空
            Stream myStream = myWebClient.OpenRead(url);
            StreamReader sr = new StreamReader(myStream, System.Text.Encoding.GetEncoding(chareset));
            strHTML = sr.ReadToEnd();
            sr.Close();
            myStream.Close();
            myWebClient.Dispose();
            return strHTML;
        }

        #endregion

        #region http请求

        /// <summary>
        /// http POST 请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="postDataStr">请求主体</param>
        /// <param name="chareset">编码，默认utf-8</param>
        /// <param name="headerItem"></param>
        /// <param name="cookies">cookie容器</param>
        /// <returns>响应的页面, 响应的cookie</returns>
        public static (string retHtml, string cookies) HttpPostRequest(string url, string postDataStr, string chareset = "utf-8", Hashtable headerItem = null, CookieContainer cookies = null)
        {
            return HttpRequest(url, "POST", postDataStr, chareset, headerItem, null, cookies, "");
        }

        /// <summary>
        /// http POST 请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="postDataStr">请求主体</param>
        /// <param name="chareset">编码，默认utf-8</param>
        /// <param name="headerItem"></param>
        /// <param name="cookie">cookie容器</param>
        /// <returns>响应的页面, 响应的cookie</returns>
        public static (string retHtml, string cookies) PostHttpWebRequest(string url, string postDataStr, string chareset = "utf-8", Hashtable headerItem = null, string cookie = "")
        {
            return HttpRequest(url, "POST", postDataStr, chareset, headerItem, null, null, cookie);
        }


        /// <summary>
        /// http GET 请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="chareset">编码，默认utf-8</param>
        /// <param name="headerItem">请求头</param>
        /// <param name="cookies">cookie容器</param>
        /// <returns>响应的页面, 响应的cookie</returns>
        public static (string retHtml, string cookies) GetHttpWebRequest(string url, string chareset, Hashtable headerItem, CookieContainer cookies)
        {
            return HttpRequest(url, "GET", "", chareset, headerItem, null, cookies, "");
        }



        /// <summary>
        /// http GET 请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="chareset">编码，默认utf-8</param>
        /// <param name="headerItem">请求头</param>
        /// <param name="cookie">cookie容器</param>
        /// <returns>响应的页面</returns>
        public static (string retHtml, string cookies) GetHttpWebRequest(string url, string chareset = "utf-8", Hashtable headerItem = null, string cookie = "")
        {
            return HttpRequest(url, "GET", "", chareset, headerItem, null, null, cookie);
        }



        #endregion

        /// <summary>
        /// 描述:格式化网页源码
        /// </summary>
        /// <param name="htmlContent"></param>
        /// <returns></returns>
        public static string FormatHtml(string htmlContent)
        {
            string result = htmlContent.Replace("»", "").Replace(" ", "")
                .Replace("©", "").Replace("\r", "").Replace("\t", "")
                .Replace("\n", "").Replace("&", "%26");
            return result;
        }
        ///<summary>
        ///清除HTML标记
        ///</summary>
        ///<param name="htmlstring">包括HTML的源码</param>
        ///<returns>已经去除后的文字</returns>
        public static string NoHtml(string htmlstring)
        {
            //删除脚本   
            htmlstring = Regex.Replace(htmlstring, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);

            //删除HTML   
            Regex regex = new Regex("<.+?>", RegexOptions.IgnoreCase);
            htmlstring = regex.Replace(htmlstring, "");
            htmlstring = Regex.Replace(htmlstring, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"-->", "", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);

            htmlstring = Regex.Replace(htmlstring, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(nbsp|#160);", "   ", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&#(\d+);", "", RegexOptions.IgnoreCase);

            htmlstring.Replace("<", "");
            htmlstring.Replace(">", "");
            htmlstring.Replace("\r\n", "");

            return htmlstring;
        }
        /// <summary> 
        /// 上传图片文件 
        /// </summary> 
        /// <param name="url">提交的地址</param> 
        /// <param name="fileformname">文本域的名称  比如：name="file"，那么fileformname=file  </param> 
        /// <param name="filepath">上传的文件路径  比如： c:\12.jpg </param> 
        /// <param name="headerItem">header</param> 
        /// <param name="cookie">cookie数据</param> 
        /// <returns>响应源码</returns> 
        public static string HttpUploadFile(string url, string fileformname, string filepath, Hashtable headerItem, string cookie)
        {
            string fileName = System.IO.Path.GetFileName(filepath);
            // 这个可以是改变的，也可以是下面这个固定的字符串 
            string boundary = DateTime.Now.Ticks.ToString("x");//元素分割标记

            // 创建request对象 
            HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(url);
            webrequest.Method = "POST";
            webrequest.CookieContainer = GetCookieContainer(url, cookie);
            SetHeaderValue(ref webrequest, headerItem);
            webrequest.ContentType = $"multipart/form-data; boundary=---------------------------{boundary}";

            // 构造发送数据 
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("-----------------------------" + boundary);
            sb.AppendLine("Content-Disposition: form-data; name=\"localUrl\"");
            sb.AppendLine();
            sb.AppendLine(filepath);
            sb.AppendLine("-----------------------------" + boundary);
            sb.AppendLine($"Content-Disposition: form-data; name=\"{fileformname}\"; filename=\"{fileName}\"");
            sb.AppendLine("Content-Type: image/png");
            sb.AppendLine();

            string postHeader = sb.ToString();
            byte[] postHeaderBytes = Encoding.GetEncoding("gb2312").GetBytes(postHeader);

            //构造尾部数据 
            byte[] boundaryBytes = Encoding.GetEncoding("gb2312").GetBytes("\r\n-----------------------------" + boundary + "--\r\n");

            FileStream fileStream = new FileStream(filepath, FileMode.Open, FileAccess.Read);
            long length = postHeaderBytes.Length + fileStream.Length + boundaryBytes.Length;
            webrequest.ContentLength = length;

            Stream requestStream = webrequest.GetRequestStream();

            // 输入头部数据 
            requestStream.Write(postHeaderBytes, 0, postHeaderBytes.Length);

            // 输入文件流数据 
            byte[] buffer = new Byte[checked((uint)Math.Min(4096, (int)fileStream.Length))];
            int bytesRead = 0;
            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                requestStream.Write(buffer, 0, bytesRead);

            // 输入尾部数据 
            requestStream.Write(boundaryBytes, 0, boundaryBytes.Length);
            WebResponse responce = webrequest.GetResponse();
            Stream s = responce.GetResponseStream();
            StreamReader sr = new StreamReader(s);

            // 返回数据流(源码) 
            return sr.ReadToEnd();
        }
        #region 百度
        /// <summary>
        /// 百度url解密
        /// </summary>
        /// <param name="url">百度url</param>
        /// <returns>真实url</returns>
        public static string BaiduUrlDecrypt(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url.Replace("&amp;", "&"));
            request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; MyIE2; .NET CLR 1.1.4322)";
            request.Method = "HEAD";
            request.AllowAutoRedirect = false;

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string realurl = response.GetResponseHeader("Location");
            response.Close();
            request.Abort();

            return realurl;


        }

        #endregion

        #region 私有方法
        /// <summary>
        /// 设置请求头
        /// </summary>
        /// <param name="request">HttpWebRequest对象</param>
        /// <param name="headerItem">header的属性对象</param>
        /// <example>调用说明：SetHeaderValue(request.Headers, headerItem);</example>
        private static void SetHeaderValue(ref HttpWebRequest request, Hashtable headerItem)
        {
            request.ContentType = "text/html";
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/45.0.2454.101 Safari/537.36";
            request.Accept = "*/*";
            if (headerItem != null)
            {
                if (headerItem.ContainsKey("Content-Type"))
                {
                    request.ContentType = headerItem["Content-Type"].ToString();
                    headerItem.Remove("Content-Type");
                }
                if (headerItem.ContainsKey("User-Agent"))
                {
                    request.UserAgent = headerItem["User-Agent"].ToString();
                    headerItem.Remove("User-Agent");
                }
                if (headerItem.ContainsKey("Accept"))
                {
                    request.Accept = headerItem["Accept"].ToString();
                    headerItem.Remove("Accept");
                }

                var property = typeof(WebHeaderCollection).GetProperty("InnerCollection",
                    System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                if (property != null)
                {
                    var collection = property.GetValue(request.Headers, null) as System.Collections.Specialized.NameValueCollection;
                    foreach (var pair in headerItem.Keys)
                    {
                        if (collection != null) collection[pair.ToString()] = headerItem[pair].ToString();
                    }
                }
            }

        }
        /// <summary>
        /// 获得cookie容器
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="cookie">cookie字符串</param>
        private static CookieContainer GetCookieContainer(string url, string cookie)
        {
            CookieContainer cookieContainer = new CookieContainer();
            if (cookie == null)
            {
                return cookieContainer;
            }
            string[] ss = cookie.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            CookieCollection cookieCollection = new CookieCollection();
            foreach (string s in ss)
            {
                string name = s.Trim().Split('=')[0];
                string value = s.Trim().Split('=')[1];
                cookieCollection.Add(new Cookie(name, value));
            }

            cookieContainer.Add(new Uri(url), cookieCollection);
            return cookieContainer;
        }
        /// <summary>
        /// gzip解压
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        /// <remarks>https://wenku.baidu.com/view/dd321f1a59eef8c75fbfb352.html</remarks>
        private static byte[] Decompress(Stream stream)
        {
            MemoryStream ms = new MemoryStream();

            GZipStream g = new GZipStream(stream, CompressionMode.Decompress);

            byte[] buf = new byte[1024 * 1024];
            int count;
            do
            {
                count = g.Read(buf, 0, buf.Length);
                ms.Write(buf, 0, count);
            } while (count > 0);

            return ms.ToArray();
        }

        /// <summary>
        /// http 请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="qequest">请求方法，GET、POST</param>
        /// <param name="dataStr">请求主体</param>
        /// <param name="chareset">编码，默认utf-8</param>
        /// <param name="headerItem"></param>
        /// <param name="proxy">http代理设置</param>
        /// <param name="cookies">cookie容器</param>
        /// <param name="cookie">cookie</param>
        /// <returns>响应的页面, 响应的cookie</returns>
        /// <c>！注意：有时候请求会重定向，但我们就需要从重定向url获取东西，像QQ登录成功后获取sid，但上面的会自动根据重定向地址跳转。我们可以用:
        ///     request.AllowAutoRedirect = false;设置重定向禁用，你就可以从headers的Location属性中获取重定向地址</c>
        private static (string retHtml, string cookies) HttpRequest(string url, string qequest, string dataStr, string chareset, Hashtable headerItem, WebProxy proxy, CookieContainer cookies, string cookie)
        {
            HttpWebRequest request;
            if (qequest == "GET")
                request = (HttpWebRequest)WebRequest.Create(url + (dataStr == "" ? "" : "?") + dataStr);
            else
                request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = qequest;
            if (qequest == "POST")
                request.ContentLength = Encoding.GetEncoding(chareset).GetByteCount(dataStr);
            SetHeaderValue(ref request, headerItem);
            request.Proxy = proxy;  //将其默认代理设置为空
            //CookieContainer cookie = new CookieContainer();

            request.CookieContainer = cookies ?? GetCookieContainer(url, cookie);
            request.Timeout = 90000;
            if (qequest == "POST")
            {
                Stream myRequestStream = request.GetRequestStream();
                myRequestStream.Write(Encoding.GetEncoding(chareset).GetBytes(dataStr), 0, Encoding.GetEncoding(chareset).GetByteCount(dataStr));
                myRequestStream.Close();
            }
            request.ServicePoint.Expect100Continue = false;  //这个在Post的时候，一定要加上，如果服务器返回错误，他还会继续再去请求，不会使用之前的错误数据，做返回数据

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            response.Cookies = request.CookieContainer?.GetCookies(response.ResponseUri);
            cookie = request.CookieContainer?.GetCookieHeader(response.ResponseUri);


            using (Stream myResponseStream = response.GetResponseStream())
            {
                if (request.Headers[HttpRequestHeader.AcceptEncoding]?.IndexOf("gzip", StringComparison.CurrentCultureIgnoreCase) > -1)
                {
                    string retString = System.Text.Encoding.GetEncoding(chareset).GetString(Decompress(myResponseStream));
                    request.Abort();
                    return (retString, cookie);
                }
                else
                {
                    using (StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding(chareset)))
                    {
                        string retString = myStreamReader.ReadToEnd();
                        request.Abort();
                        return (retString, cookie);
                    }
                }

            }


        }

        #endregion
    }

}