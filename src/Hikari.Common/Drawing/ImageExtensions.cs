using System.Drawing;
using System.Drawing.Imaging;

namespace Hikari.Common.Drawing;

/// <summary>
/// <see cref="Image"/> 扩展方法
/// </summary>
[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never), System.ComponentModel.Browsable(false)]
public static class ImageExtensions
{
    /// <summary>
    /// 图片转二进制
    /// </summary>
    /// <param name="image">图片对象</param>  
    /// <returns>二进制</returns>  
    public static byte[] ToBytes(this Image image)
    {
        //将Image转换成流数据，并保存为byte[]   
        using MemoryStream mstream = new MemoryStream();
        var imageFormat = image.RawFormat.Equals(ImageFormat.MemoryBmp) ? ImageFormat.Bmp : image.RawFormat;
        image.Save(mstream, imageFormat);
        byte[] byData = new Byte[mstream.Length];
        mstream.Position = 0;
        var read = mstream.Read(byData, 0, byData.Length);
        mstream.Flush();
        return byData;

    }
    /// <summary>
    /// 图片转二进制
    /// </summary>
    /// <param name="image">图片对象</param>  
    /// <returns>二进制</returns>  
    public static async Task<byte[]> ToBytesAsync(this Image image)
    {
        //将Image转换成流数据，并保存为byte[]   
        await using MemoryStream mstream = new MemoryStream();
        var imageFormat = image.RawFormat.Equals(ImageFormat.MemoryBmp) ? ImageFormat.Bmp : image.RawFormat;
        image.Save(mstream, imageFormat);
        byte[] byData = new Byte[mstream.Length];
        mstream.Position = 0;
        var read = await mstream.ReadAsync(byData, 0, byData.Length);
        mstream.Flush();
        return byData;

    }

    /// <summary>
    /// 二进制数组转图片对象
    /// </summary>
    /// <param name="bytes"></param>
    /// <returns></returns>
    public static Image ToImage(this byte[] bytes)
    {
        using MemoryStream ms = new MemoryStream(bytes);
        Image image = Image.FromStream(ms);
        ms.Flush();
        return image;
    }
    /// <summary>
    /// 二进制数组转图片对象
    /// </summary>
    /// <param name="bytes"></param>
    /// <returns></returns>
    public static async Task<Image> ToImageAsync(this byte[] bytes)
    {
        await using MemoryStream ms = new MemoryStream(bytes);
        Image image = Image.FromStream(ms);
        await ms.FlushAsync();
        return image;
    }
}