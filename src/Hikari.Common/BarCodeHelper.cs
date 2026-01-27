using SkiaSharp;
using ZXing;
using ZXing.Common;
using ZXing.SkiaSharp;

namespace Hikari.Common;
/// <summary>
/// 条码帮助类
/// </summary>
public class BarCodeHelper
{
    /// <summary>
    ///  生成条形码
    /// </summary>
    /// <param name="content">条码信息</param>
    /// <param name="width">宽度</param>
    /// <param name="height">高度</param>
    /// <param name="format">格式</param>
    public static Stream CreateBarCode(string content, int width = 120, int height = 60, BarcodeFormat format = BarcodeFormat.CODE_128)
    {

        var writer = new BarcodeWriter
        {
            Format = format
        };
        EncodingOptions options = new EncodingOptions()
        {
            Width = width,
            Height = height,
            Margin = 2,
        };
        writer.Options = options;
        var map = writer.Write(content);

        using SKImage image = SKImage.FromBitmap(map);
        using SKData data = image.Encode(SKEncodedImageFormat.Png, 100);
        var stream = new MemoryStream();
        data.SaveTo(stream);
        stream.Position = 0; // 重置位置，便于读取
        return stream;
    }
}