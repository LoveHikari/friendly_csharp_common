using Hikari.Common.IO;
using System.Net;
using System.Net.Http.Headers;
using System.Xml.Linq;

/******************************************************************************************************************
 * 
 * 
 * 标  题：发送Http请求(版本：Version1.0.0)
 * 作　者：YuXiaoWei
 * 日　期：2019/11/20
 * 修　改：
 * 参　考：https://www.cnblogs.com/xiaozhu39505/p/8033108.html
 * 说　明：相对于HttpHelper推荐使用这个类
 * 备　注：暂无...
 * 
 * 
 * ***************************************************************************************************************/
namespace Hikari.Common.Net.Http
{
    /// <summary>
    /// HttpClient帮助类
    /// </summary>
    public class HttpClientHelper
    {
        private readonly HttpClient _client;
        private readonly string[] _wellKnownContentHeaders =  // 这些是属于内容的标头
        {
            "Content-Disposition", "Content-Encoding", "Content-Language", "Content-Length", "Content-Location",
            "Content-MD5", "Content-Range", "Content-Type", "Expires", "Last-Modified"
        };

        private readonly CookieContainer _cookieContainer;
        private readonly WebProxy _webProxy;
        private IDictionary<string, string> _headerItem;
        private System.Text.Encoding _encoding;

        /// <summary>
        /// 请求错误代码
        /// </summary>
        public HttpStatusCode ErrorStatusCode { get; set; }
        /// <summary>
        /// 请求错误时内容
        /// </summary>
        public string ErrorContent { get; set; }
        /// <summary>
        /// HttpClient封装
        /// </summary>
        /// <param name="baseAddress">请求基址</param>
        public HttpClientHelper(string? baseAddress = null)
        {
            _cookieContainer = new ();
            _webProxy = new WebProxy();
            _headerItem = new Dictionary<string, string>();
            _encoding = System.Text.Encoding.GetEncoding("utf-8");

            var handler = new HttpClientHandler
            {
                CookieContainer = _cookieContainer,
                UseCookies = true,
                Proxy = _webProxy,
                UseProxy = true,
                ServerCertificateCustomValidationCallback = (message, cert, chain, error) => true  // 忽略https证书提醒
            };
            this._client = new HttpClient(handler);
            if (!string.IsNullOrWhiteSpace(baseAddress))
            {
                this._client.BaseAddress = new Uri(baseAddress);
            }
            
            this.ErrorContent = "";
        }
        /// <summary>
        /// 发生一个get请求
        /// </summary>
        /// <param name="url">请求url</param>
        /// <returns></returns>
        public async Task<string> GetAsync(string url)
        {
            var response = await GetHttpResponseMessageAsync(url, HttpMethod.Get, null);

            string html = "";

            if (response.IsSuccessStatusCode)
            {
                var bytes = await response.Content.ReadAsByteArrayAsync();
                html = _encoding.GetString(bytes);
            }

            return html;
        }
        /// <summary>
        /// 发生一个get请求
        /// </summary>
        /// <param name="url">请求url</param>
        /// <returns></returns>
        public string Get(string url)
        {
            var response = GetHttpResponseMessage(url, HttpMethod.Get, null);

            string html = "";

            if (response.IsSuccessStatusCode)
            {
                var bytes = response.Content.ReadAsStream().ToBytes();
                html = _encoding.GetString(bytes);
            }

            return html;
        }
        /// <summary>
        /// 发生一个post请求
        /// </summary>
        /// <param name="url">请求url</param>
        /// <param name="param">请求参数</param>
        /// <returns></returns>
        public async Task<string> PostAsync(string url, IDictionary<string, object> param)
        {
            var response = await GetHttpResponseMessageAsync(url, HttpMethod.Post, param);

            string html = "";

            if (response.IsSuccessStatusCode)
            {
                var bytes = await response.Content.ReadAsByteArrayAsync();
                html = _encoding.GetString(bytes);
            }

            return html;
        }
        /// <summary>
        /// 发生一个post请求
        /// </summary>
        /// <param name="url">请求url</param>
        /// <param name="param">请求参数</param>
        /// <returns></returns>
        public string Post(string url, IDictionary<string, object> param)
        {
            var response = GetHttpResponseMessage(url, HttpMethod.Post, param);

            string html = "";

            if (response.IsSuccessStatusCode)
            {
                var bytes = response.Content.ReadAsStream().ToBytes();
                html = _encoding.GetString(bytes);
            }

            return html;
        }

        /// <summary>
        /// 发生一个HTTP请求
        /// </summary>
        /// <param name="url">请求url</param>
        /// <param name="method">请求方式</param>
        /// <param name="param">请求参数</param>
        /// <returns></returns>
        public async Task<string> SendAsync(string url, HttpMethod method, IDictionary<string, object> param)
        {
            var response = await GetHttpResponseMessageAsync(url, method, param);

            string html = "";

            if (response.IsSuccessStatusCode)
            {
                var bytes = await response.Content.ReadAsByteArrayAsync();
                html = _encoding.GetString(bytes);
            }

            return html;

        }
        /// <summary>
        /// 发生一个HTTP请求
        /// </summary>
        /// <param name="url">请求url</param>
        /// <param name="method">请求方式</param>
        /// <param name="param">请求参数</param>
        /// <returns></returns>
        public string Send(string url, HttpMethod method, IDictionary<string, object> param)
        {
            var response = GetHttpResponseMessage(url, method, param);

            string html = "";

            if (response.IsSuccessStatusCode)
            {
                var bytes = response.Content.ReadAsStream().ToBytes();

                html = _encoding.GetString(bytes);
            }

            return html;

        }
        /// <summary>
        /// 发生一个HTTP请求
        /// </summary>
        /// <param name="url">请求url</param>
        /// <param name="method">请求方式</param>
        /// <param name="param">请求参数</param>
        /// <returns></returns>
        private async Task<HttpResponseMessage> GetHttpResponseMessageAsync(string url, HttpMethod method, IDictionary<string, object>? param)
        {
            var request = new HttpRequestMessage(method, new Uri(url));

            SetHttpContent(request, param);
            SetHeaders(request);

            var response = await this._client.SendAsync(request);

            this.ErrorStatusCode = response.StatusCode;
            this.ErrorContent = await response.Content.ReadAsStringAsync();

            return response;

        }
        /// <summary>
        /// 发生一个HTTP请求
        /// </summary>
        /// <param name="url">请求url</param>
        /// <param name="method">请求方式</param>
        /// <param name="param">请求参数</param>
        /// <returns></returns>
        private HttpResponseMessage GetHttpResponseMessage(string url, HttpMethod method, IDictionary<string, object>? param)
        {
            var request = new HttpRequestMessage(method, new Uri(url));

            SetHttpContent(request, param);
            SetHeaders(request);

            var response = this._client.Send(request);

            this.ErrorStatusCode = response.StatusCode;

            return response;

        }
        /// <summary>
        /// 获得cookies
        /// </summary>
        /// <returns></returns>
        public string GetCookies()
        {
            var cookies = _client.BaseAddress is not null ? _cookieContainer.GetCookieHeader(_client.BaseAddress) : "";

            return cookies;
        }
        /// <summary>
        /// 设置cookies
        /// </summary>
        /// <returns></returns>
        public void SetCookies(string cookieHeader)
        {
            if (_client.BaseAddress is not null)
            {
                _cookieContainer.SetCookies(_client.BaseAddress, cookieHeader);
            }

        }
        /// <summary>
        /// 设置代理
        /// </summary>
        /// <param name="proxyUrl"></param>
        public void SetWebProxy(string proxyUrl)
        {
            if (!string.IsNullOrWhiteSpace(proxyUrl))
            {
                _webProxy.Address = new Uri(proxyUrl);
                _webProxy.BypassProxyOnLocal = true;
            }
        }
        /// <summary>
        /// 设置请求头
        /// </summary>
        /// <param name="headerItem"></param>
        public void SetHeaderItem(IDictionary<string, string> headerItem)
        {
            _headerItem = headerItem;
        }
        /// <summary>
        /// 设置页面编码
        /// </summary>
        /// <param name="charset"></param>
        public void SetEncoding(string charset)
        {
            _encoding = System.Text.Encoding.GetEncoding(charset);
        }
        /// <summary>
        /// 设置请求内容
        /// </summary>
        /// <param name="request">请求对象</param>
        /// <param name="param">请求参数</param>
        private void SetHttpContent(HttpRequestMessage request, IDictionary<string, object>? param)
        {
            string contentType;
            if (!_headerItem.ContainsKey("Content-Type"))
            {
                contentType = "text/plain; charset=utf-8";
            }
            else
            {
                contentType = _headerItem["Content-Type"];
            }
            
            HttpContent? content = null;
            if (param != null)
            {
                if (contentType.Contains("form-data"))
                {
                    var data = new MultipartFormDataContent();
                    
                    foreach (KeyValuePair<string, object> pair in param)
                    {
                        if (pair.Key.ToLower().Contains("file"))
                        {
                            string fileName = Path.GetFileName(pair.Value.ToString())!;
                            // 读文件流
                            data.Add(new ByteArrayContent(File.ReadAllBytes(pair.Value.ToString()!)), pair.Key, fileName);
                        }
                        else
                        {
                            data.Add(new StringContent(pair.Value.ToString()!));
                        }

                    }
                    content = data;
                }
                else if (contentType.Contains("stream"))
                {
                    HttpContent? data = null;
                    foreach (KeyValuePair<string, object> pair in param)
                    {
                        if ( pair.Key.ToLower().Contains("file"))
                        {
                            // 读文件流
                            FileStream fs = new FileStream(pair.Value.ToString()!, FileMode.Open, FileAccess.Read, FileShare.Read);
                            data = new StreamContent(fs);//为文件流提供的HTTP容器
                            data.Headers.ContentType = MediaTypeHeaderValue.Parse(contentType);//设置媒体类型
                        }

                    }
                    content = data;
                }
                else if (contentType.Contains("json"))
                {
                    var value = System.Text.Json.JsonSerializer.Serialize(param);
                    var data = new StringContent(value);
                    content = data;
                    data.Headers.ContentType = MediaTypeHeaderValue.Parse(contentType);
                }
                else if (contentType.Contains("xml"))
                {
                    var value = new XElement("xml",
                        from keyValue in param
                        select new XElement(keyValue.Key, keyValue.Value)
                    ).ToString();
                    var data = new StringContent(value);
                    content = data;
                }
                else
                {
                    IEnumerable<KeyValuePair<string, string>> dic = param.Select(kv => new KeyValuePair<string, string>(kv.Key, kv.Value.ToString()!));
                    var data = new FormUrlEncodedContent(dic);
                    content = data;
                }
            }

            request.Content = content;
        }
        /// <summary>
        /// 设置请求头
        /// </summary>
        /// <param name="request">请求对象</param>
        private void SetHeaders(HttpRequestMessage request)
        {
            //request.Content.Headers.Clear();
            //request.Headers.Clear();

            //if (!request.Headers.Contains("Content-Type") && !headerItem.ContainsKey("Content-Type"))
            //{
            //    request.Headers.Add("Content-Type", "text/plain; charset=utf-8");
            //}
            if (!request.Headers.Contains("User-Agent") && !_headerItem.ContainsKey("User-Agent"))
            {
                request.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36");
            }
            //if (!headerItem.ContainsKey("Accept"))
            //{
            //    headerItem.Add("Accept", "*/*");
            //}
            foreach (KeyValuePair<string, string> pair in _headerItem)
            {
                if (!string.IsNullOrWhiteSpace(pair.Value))
                {
                    if (_wellKnownContentHeaders.Contains(pair.Key) && request.Content != null && !request.Content.Headers.Contains(pair.Key))
                    {
                        request.Content.Headers.Add(pair.Key, pair.Value);
                    }
                    else if (!_wellKnownContentHeaders.Contains(pair.Key) && !request.Headers.Contains(pair.Key))
                    {

                        request.Headers.Add(pair.Key, pair.Value);
                    }
                }
            }
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="requestUri">请求地址</param>
        /// <param name="path">保持文件路径带文件名</param>
        /// <param name="progress">下载进度通知</param>
        /// <param name="cancellationToken">线程取消令牌</param>
        /// <remarks>https://www.cnblogs.com/h82258652/p/10950580.html</remarks>
        /// <returns></returns>
        public async Task DownloadAsync(string requestUri, string path, IProgress<HttpDownloadProgress> progress, CancellationToken cancellationToken)
        {

            using var responseMessage = await _client.GetAsync(new Uri(requestUri), HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
            responseMessage.EnsureSuccessStatusCode();

            var content = responseMessage.Content;

            var headers = content.Headers;
            var contentLength = headers.ContentLength;
            await using var responseStream = await content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);

            int bufferSize = 1024;
            var buffer = new byte[bufferSize];
            int bytesRead;

            var downloadProgress = new HttpDownloadProgress();
            if (contentLength.HasValue)
            {
                downloadProgress.TotalBytesToReceive = (ulong)contentLength.Value;
            }
            progress.Report(downloadProgress);

            await using FileStream fileStream = File.Open(path, FileMode.Create);

            while ((bytesRead = await responseStream.ReadAsync(buffer, 0, bufferSize, cancellationToken).ConfigureAwait(false)) > 0)
            {

                await fileStream.WriteAsync(buffer.Take(bytesRead).ToArray(), cancellationToken);

                downloadProgress.BytesReceived += (ulong)bytesRead;
                progress?.Report(downloadProgress);
            }

            await fileStream.FlushAsync(cancellationToken);
        }
    }
}