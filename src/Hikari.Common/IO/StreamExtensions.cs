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
    /// <summary>
    /// 生成摘要，并转为16进制字符串
    /// </summary>
    /// <param name="this"></param>
    /// <returns>摘要</returns>
    public static string DigestHex(this byte[] @this)
    {
        string t2 = System.BitConverter.ToString(@this);
        t2 = t2.Replace("-", "").ToLower();
        return t2;
    }
    /// <summary>
    /// 生成摘要，并转为base64字符串
    /// </summary>
    /// <param name="this"></param>
    /// <returns></returns>
    public static string DigestBase64(this byte[] @this)
    {
        return Convert.ToBase64String(@this);
    }
    /// <summary>
    /// 保存到文件
    /// </summary>
    /// <param name="this"></param>
    /// <param name="filePath">文件路径</param>
    /// <returns></returns>
    public static async Task SaveToFileAsync(this Stream @this, string filePath)
    {
        async Task CopyStreamAsync(Stream input, Stream output)
        {
            byte[] buffer = new byte[1024];
            int len;
            while ((len = await input.ReadAsync(buffer, 0, buffer.Length)) > 0)
            {
                await output.WriteAsync(buffer, 0, len);
            }
        }

        await using var fileStream = File.Create(filePath);
        @this.Seek(0, SeekOrigin.Begin);
        await CopyStreamAsync(@this, fileStream);
    }
    /// <summary>
    /// 保存到文件
    /// </summary>
    /// <param name="this"></param>
    /// <param name="filePath">文件路径</param>
    public static void SaveToFile(this Stream @this, string filePath)
    {
        void CopyStream(Stream input, Stream output)
        {
            byte[] buffer = new byte[1024];
            int len;
            while ((len = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, len);
            }
        }
        using var fileStream = File.Create(filePath);
        @this.Seek(0, SeekOrigin.Begin);
        CopyStream(@this, fileStream);
    }
}