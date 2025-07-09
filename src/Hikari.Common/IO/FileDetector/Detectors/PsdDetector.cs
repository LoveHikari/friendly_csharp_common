using System.Reflection;
using Hikari.Common.Mime;

namespace Hikari.Common.IO.FileDetector.Detectors;

[FormatCategory(FormatCategory.Image)]
internal sealed class PsdDetector : AbstractSignatureDetector
{
    private static readonly SignatureInformation[] PsdSignatureInfo = {
        new() { Position = 0, Signature = "8BPS"u8.ToArray() },
    };

    public override string Extension => "psd";

    protected override SignatureInformation[] SignatureInformations => PsdSignatureInfo;

    public override string MimeType => new MimeMapper().GetMimeFromExtension("." + Extension);

    public override List<FormatCategory> FormatCategories => GetType().GetCustomAttributes<FormatCategoryAttribute>().Select(a => a.Category).ToList();

    public override string ToString() => "Photoshop File(PSD) Detector";
}
