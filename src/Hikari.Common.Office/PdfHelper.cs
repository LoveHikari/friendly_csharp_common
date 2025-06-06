using iText.Html2pdf;
using iText.Kernel.Pdf;
using iText.Layout.Font;
using iText.StyledXmlParser.Css.Media;

namespace Hikari.Common.Office;
/// <summary>
/// pdf帮助类
/// </summary>
public class PdfHelper
{
    /// <summary>
    /// html转为pdf
    /// </summary>
    /// <param name="htmlContent">html内容</param>
    /// <param name="pdfPath">pdf文件路径</param>
    /// <param name="fontProvider">字体，默认微软雅黑</param>
    public static void HtmlToPdf(string htmlContent, string pdfPath, FontProvider? fontProvider = null)
    {
        var pdfDir = Path.GetDirectoryName(pdfPath);
        if (!Directory.Exists(pdfDir))
        {
            if (pdfDir != null) Directory.CreateDirectory(pdfDir);
        }

        using var writer = new PdfWriter(pdfPath);
        using var pdf = new PdfDocument(writer);
        // 设置 PDF 属性（可选）
        pdf.SetTagged(); // 启用可访问性标签
        
        ConverterProperties properties = new ConverterProperties();
        fontProvider ??= new FontProvider();

        // 加载常规和加粗字体
        fontProvider.AddFont(@"C:\Windows\Fonts\msyh.ttc,0");
        fontProvider.AddFont(@"C:\Windows\Fonts\msyhbd.ttc,0");
        properties.SetFontProvider(fontProvider);
        properties.SetMediaDeviceDescription(new MediaDeviceDescription(MediaType.PRINT));

        // 转换 HTML 到 PDF
        HtmlConverter.ConvertToPdf(htmlContent, pdf, properties);
    }
}