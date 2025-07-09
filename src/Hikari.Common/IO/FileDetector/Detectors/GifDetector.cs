using System.Reflection;
using Hikari.Common.Mime;

namespace Hikari.Common.IO.FileDetector.Detectors;

[FormatCategory(FormatCategory.Video)]
[FormatCategory(FormatCategory.Image)]
internal sealed class GifDetector : AbstractSignatureDetector
{
    private static readonly SignatureInformation[] GifSignatureInfo = {
        new() { Position = 0, Signature = "GIF87a"u8.ToArray() },
        new() { Position = 0, Signature = "GIF89a"u8.ToArray() },
    };

    public override string Extension => "gif";

    protected override SignatureInformation[] SignatureInformations => GifSignatureInfo;

    public override string MimeType => new MimeMapper().GetMimeFromExtension("." + Extension);

    public override List<FormatCategory> FormatCategories => GetType().GetCustomAttributes<FormatCategoryAttribute>().Select(a => a.Category).ToList();

    public override string ToString() => "Graphics Interchange Format Detector";
}
