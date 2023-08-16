
using SkiaSharp;

namespace Hikari.Common.SkiaSharp;

/// <summary>
/// <see cref="SKImage"/> 扩展方法
/// </summary>
[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never), System.ComponentModel.Browsable(false)]
public static class ImageExtensions
{
    /// <summary>
    /// 图片转二进制
    /// </summary>
    /// <param name="image">图片对象</param>  
    /// <returns>二进制</returns>  
    public static byte[] ToBytes(this SKImage image)
    {
        using SKData data = image.Encode();
        return data.ToArray();

    }
    /// <summary>
    /// 图片转二进制
    /// </summary>
    /// <param name="image">图片对象</param>  
    /// <returns>二进制</returns>  
    public static async Task<byte[]> ToBytesAsync(this SKImage image)
    {
        byte[] bytes = await Task.Run(() =>
        {
            using var data = image.Encode();
            return data.ToArray();
        });
        return bytes;
    }

    /// <summary>
    /// 二进制数组转图片对象
    /// </summary>
    /// <param name="bytes"></param>
    /// <returns></returns>
    public static SKImage ToImage(this byte[] bytes)
    {
        SKImage image = SKImage.FromEncodedData(bytes);
        return image;
    }
    /// <summary>
    /// 二进制数组转图片对象
    /// </summary>
    /// <param name="bytes"></param>
    /// <returns></returns>
    public static async Task<SKImage> ToImageAsync(this byte[] bytes)
    {
        SKImage image  = await Task.Run(() =>
        {
            SKImage image = SKImage.FromEncodedData(bytes);
            return image;
        });
        return image;
    }
}