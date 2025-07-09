using System.Reflection;
using Hikari.Common.Mime;

namespace Hikari.Common.IO.FileDetector.Detectors;

[FormatCategory(FormatCategory.Compression)]
internal sealed class XarDetector : AbstractSignatureDetector
{
    private static readonly SignatureInformation[] XarSignatureInfo = {
        new() { Position = 0, Signature = "xar!"u8.ToArray() },
    };

    public override string Extension => "xar";

    public override string MimeType => new MimeMapper().GetMimeFromExtension("." + Extension);

    public override List<FormatCategory> FormatCategories => GetType().GetCustomAttributes<FormatCategoryAttribute>().Select(a => a.Category).ToList();

    protected override SignatureInformation[] SignatureInformations => XarSignatureInfo;

    public override string ToString() => "XAR Detector";
}
