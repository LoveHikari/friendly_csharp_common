using System.Reflection;
using Hikari.Common.Mime;

namespace Hikari.Common.IO.FileDetector.Detectors;

[FormatCategory(FormatCategory.Image)]
internal sealed class Jpeg2000Detector : AbstractSignatureDetector
{
    private static readonly SignatureInformation[] JpegSignatureInfo = {
        new() { Position = 0, Signature = new byte [] { 0x00, 0x00, 0x00, 0x0C, 0x6A, 0x50, 0x20, 0x20 } },
    };

    public override string Extension => "jp2";

    protected override SignatureInformation[] SignatureInformations => JpegSignatureInfo;

    public override string MimeType => new MimeMapper().GetMimeFromExtension("." + Extension);

    public override List<FormatCategory> FormatCategories => GetType().GetCustomAttributes<FormatCategoryAttribute>().Select(a => a.Category).ToList();

    public override string ToString() => "JPEG2000 Detector";
}
