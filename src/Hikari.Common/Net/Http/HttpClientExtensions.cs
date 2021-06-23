using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Hikari.Common.Net.Http
{
    /// <summary>
    /// <see cref="HttpClient"/> 扩展方法
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never), System.ComponentModel.Browsable(false)]
    public static class HttpClientExtensions
    {
        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="client"></param>
        /// <param name="requestUri">请求地址</param>
        /// <param name="path">保存文件路径带文件名</param>
        /// <param name="progress">下载进度通知</param>
        /// <param name="cancellationToken">线程取消令牌</param>
        /// <remarks>https://www.cnblogs.com/h82258652/p/10950580.html</remarks>
        /// <returns></returns>
        public static async Task GetByteArrayAsync(this HttpClient client, string requestUri, string path, IProgress<HttpDownloadProgress> progress, CancellationToken cancellationToken)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, new Uri(requestUri));
            request.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36");

            using var responseMessage = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
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
            progress?.Report(downloadProgress);

            await using FileStream fileStream = File.Open(path, FileMode.Create);

            while ((bytesRead = await responseStream.ReadAsync(buffer, 0, bufferSize, cancellationToken).ConfigureAwait(false)) > 0)
            {

                await fileStream.WriteAsync(buffer.Take(bytesRead).ToArray(), cancellationToken);

                downloadProgress.BytesReceived += (ulong)bytesRead;
                progress?.Report(downloadProgress);
            }

            await fileStream.FlushAsync(cancellationToken);
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="client"></param>
        /// <param name="requestUri">请求地址</param>
        /// <param name="path">保存文件路径带文件名</param>
        /// <returns></returns>
        public static async Task GetByteArrayAsync(HttpClient client, string requestUri, string path)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, new Uri(requestUri));
            request.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36");

            using var responseMessage = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);
            responseMessage.EnsureSuccessStatusCode();

            var content = responseMessage.Content;

            await using var responseStream = await content.ReadAsStreamAsync().ConfigureAwait(false);

            int bufferSize = 1024;
            var buffer = new byte[bufferSize];
            int bytesRead;

            await using FileStream fileStream = File.Open(path, FileMode.Create);

            while ((bytesRead = await responseStream.ReadAsync(buffer, 0, bufferSize).ConfigureAwait(false)) > 0)
            {

                await fileStream.WriteAsync(buffer.Take(bytesRead).ToArray());

            }

            await fileStream.FlushAsync();
        }
    }
}