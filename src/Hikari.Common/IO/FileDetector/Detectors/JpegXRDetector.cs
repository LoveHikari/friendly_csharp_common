using System.Reflection;
using Hikari.Common.Mime;

namespace Hikari.Common.IO.FileDetector.Detectors;

[FormatCategory(FormatCategory.Image)]
internal sealed class JpegXRDetector : AbstractSignatureDetector
{
    private static readonly SignatureInformation[] JpegSignatureInfo = {
        new() { Position = 0, Signature = new byte [] { 0x49, 0x49, 0xBC, 0x01 } },
    };

    public override string Extension => "hdp";

    protected override SignatureInformation[] SignatureInformations => JpegSignatureInfo;

    public override string MimeType => new MimeMapper().GetMimeFromExtension("." + Extension);

    public override List<FormatCategory> FormatCategories => GetType().GetCustomAttributes<FormatCategoryAttribute>().Select(a => a.Category).ToList();

    public override string ToString() => "HD Photo(JPEG XR) Detector";
}
