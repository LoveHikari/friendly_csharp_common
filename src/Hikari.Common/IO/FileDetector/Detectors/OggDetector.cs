using System.Reflection;
using Hikari.Common.Mime;

namespace Hikari.Common.IO.FileDetector.Detectors;

[FormatCategory(FormatCategory.Audio)]
[FormatCategory(FormatCategory.Video)]
internal sealed class OggDetector : AbstractSignatureDetector
{
    private static readonly SignatureInformation[] OggSignatureInfo = {
        new () { Position = 0, Signature = "OggS"u8.ToArray() },
    };

    public override string Extension => "ogg";

    public override string MimeType => new MimeMapper().GetMimeFromExtension("." + Extension);

    public override List<FormatCategory> FormatCategories => GetType().GetCustomAttributes<FormatCategoryAttribute>().Select(a => a.Category).ToList();

    protected override SignatureInformation[] SignatureInformations => OggSignatureInfo;

    public override string ToString() => "OGG Detector";
}
