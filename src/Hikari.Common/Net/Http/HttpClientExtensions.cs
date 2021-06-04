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

            using var responseMessage = await client.GetAsync(new Uri(requestUri), HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
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
    }
}