namespace Hikari.Common.IO;

/// <summary>
/// <see cref="Stream"/> 扩展方法
/// </summary>
[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never), System.ComponentModel.Browsable(false)]
public static class StreamExtensions
{
    /// <summary>
    /// 将 Stream 转成 byte[]
    /// </summary>
    /// <param name="this"></param>
    /// <returns></returns>
    public static async Task<byte[]> ToBytesAsync(this Stream @this)
    {
        byte[] bytes = new byte[@this.Length];
        await @this.ReadExactlyAsync(bytes, 0, bytes.Length);
        // 设置当前流的位置为流的开始 
        @this.Seek(0, SeekOrigin.Begin);
        return bytes;
    }
    /// <summary>
    /// 将 Stream 转成 byte[]
    /// </summary>
    /// <param name="this"></param>
    /// <returns></returns>
    public static byte[] ToBytes(this Stream @this)
    {
        byte[] bytes = new byte[@this.Length];
        @this.ReadExactly(bytes, 0, bytes.Length);
        // 设置当前流的位置为流的开始 
        @this.Seek(0, SeekOrigin.Begin);
        return bytes;
    }
    /// <summary>
    /// 将 byte[] 转成 Stream 
    /// </summary>
    /// <param name="this"></param>
    /// <returns></returns>
    public static Stream ToStream(this byte[] @this)
    {
        Stream stream = new MemoryStream(@this);
        return stream;
    }
}