namespace Hikari.Common.Net.Http
{
    /// <summary>
    /// 下载进度
    /// </summary>
    public struct HttpDownloadProgress
    {
        /// <summary>
        /// 已经下载的字节数
        /// </summary>
        public ulong BytesReceived { get; set; }
        /// <summary>
        /// 需要下载的字节数
        /// </summary>
        public ulong? TotalBytesToReceive { get; set; }
    }
}