﻿using System;
using System.Collection;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
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
namespace System
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

        private readonly CookieContainer _cookieContainer = new CookieContainer();
        /// <summary>
        /// 请求错误代码
        /// </summary>
        public HttpStatusCode ErrorStatusCode;
        /// <summary>
        /// HttpClient封装
        /// </summary>
        public HttpClientHelper1()
        {
            var handler = new HttpClientHandler
            {
                CookieContainer = _cookieContainer,
                UseCookies = true,
            };
            this._client = new HttpClient(handler);
        }
        /// <summary>
        /// 发生一个get请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="chareset">编码</param>
        /// <param name="headerItem">请求头</param>
        /// <returns></returns>
        public async Task<string> GetAsync(string url, string chareset = "utf-8", IDictionary<string, string> headerItem = null)
        {
            var response = await SendAsync(url, HttpMethod.Get, null, headerItem);

            string html = "";

            if (response.IsSuccessStatusCode)
            {
                var bytes = await response.Content.ReadAsByteArrayAsync();
                html = System.Text.Encoding.GetEncoding(chareset).GetString(bytes);
            }

            return html;
        }

        /// <summary>
        /// 发生一个post请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="param">请求参数</param>
        /// <param name="chareset">编码</param>
        /// <param name="headerItem">请求头</param>
        /// <returns></returns>
        public async Task<string> PostAsync(string url, IDictionary<string, object> param, string chareset = "utf-8", IDictionary<string, string> headerItem = null)
        {
            var response = await SendAsync(url, HttpMethod.Post, param, headerItem);

            string html = "";

            if (response.IsSuccessStatusCode)
            {
                var bytes = await response.Content.ReadAsByteArrayAsync();
                html = System.Text.Encoding.GetEncoding(chareset).GetString(bytes);
            }

            return html;
        }

        /// <summary>
        /// 发生一个HTTP请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="method">请求方式</param>
        /// <param name="param">请求参数</param>
        /// <param name="chareset">编码</param>
        /// <param name="headerItem">请求头</param>
        /// <returns></returns>
        public async Task<string> SendAsync(string url, HttpMethod method, IDictionary<string, object> param, string chareset = "utf-8", IDictionary<string, string> headerItem = null)
        {
            var response = await SendAsync(url, method, param, headerItem);

            string html = "";

            if (response.IsSuccessStatusCode)
            {
                var bytes = await response.Content.ReadAsByteArrayAsync();
                html = System.Text.Encoding.GetEncoding(chareset).GetString(bytes);
            }

            return html;

        }
        /// <summary>
        /// 发生一个HTTP请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="method">请求方式</param>
        /// <param name="param">请求参数</param>
        /// <param name="headerItem">请求头</param>
        /// <returns></returns>
        private async Task<HttpResponseMessage> SendAsync(string url, HttpMethod method, IDictionary<string, object> param, IDictionary<string, string> headerItem = null)
        {
            var request = new HttpRequestMessage(method, new Uri(url));

            SetHttpContent(request, headerItem, param);
            SetHeaders(request, headerItem);

            var response = await this._client.SendAsync(request);

            this.ErrorStatusCode = response.StatusCode;

            return response;

        }
        /// <summary>
        /// 获得cookies
        /// </summary>
        /// <returns></returns>
        public string GetCookies()
        {
            var cookies = _cookieContainer.GetCookieHeader(_client.BaseAddress);
            return cookies;
        }
        /// <summary>
        /// 设置请求内容
        /// </summary>
        /// <param name="request">请求对象</param>
        /// <param name="headerItem">请求头</param>
        /// <param name="param">请求参数</param>
        private void SetHttpContent(HttpRequestMessage request, IDictionary<string, string> headerItem, IDictionary<string, object> param)
        {
            var contentType = headerItem?["Content-Type"] ?? "";
            HttpContent content = new ByteArrayContent(Array.Empty<byte>());
            if (param != null)
            {
                if (contentType.Contains("form-data"))
                {
                    var data = new MultipartFormDataContent();

                    foreach (KeyValuePair<string, object> pair in param)
                    {
                        data.Add(new StringContent(pair.Value.ToString()), pair.Key);

                    }
                    content = data;
                }
                else if (contentType.Contains("stream"))
                {
                    HttpContent data = null;
                    foreach (KeyValuePair<string, object> pair in param)
                    {
                        if (pair.Key.Contains("file"))
                        {
                            // 读文件流
                            FileStream fs = new FileStream(pair.Value.ToString(), FileMode.Open, FileAccess.Read, FileShare.Read);
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
                }
                else
                {
                    IEnumerable<KeyValuePair<string, string>> dic = param.Select(kv => new KeyValuePair<string, string>(kv.Key, kv.Value.ToString()));
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
        /// <param name="headerItem">请求头</param>
        private void SetHeaders(HttpRequestMessage request, IDictionary<string, string> headerItem)
        {
            request.Content.Headers.Clear();
            request.Headers.Clear();
            headerItem ??= new Dictionary<string, string>();

            if (!headerItem.ContainsKey("User-Agent"))
            {
                headerItem.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36");
            }
            if (!headerItem.ContainsKey("Content-Type"))
            {
                headerItem.Add("Content-Type", "text/plain; charset=utf-8");
            }
            if (!headerItem.ContainsKey("Accept"))
            {
                headerItem.Add("Accept", "*/*");
            }
            foreach (KeyValuePair<string, string> pair in headerItem)
            {
                if (_wellKnownContentHeaders.Contains(pair.Key))
                {
                    request.Content.Headers.Add(pair.Key, pair.Value);
                }
                else
                {
                    request.Headers.Add(pair.Key, pair.Value);
                }


            }
        }
    }
}