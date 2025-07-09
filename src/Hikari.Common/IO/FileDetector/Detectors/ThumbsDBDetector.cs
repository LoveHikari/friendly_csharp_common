using System.Reflection;
using Hikari.Common.Mime;

namespace Hikari.Common.IO.FileDetector.Detectors;

[FormatCategory(FormatCategory.System)]
internal sealed class ThumbsDBDetector : AbstractSignatureDetector
{
    private static readonly SignatureInformation[] ThumbdbSignatureInfo = {
        new () { Position = 0, Signature = new byte [] { 0xFD, 0xFF, 0xFF, 0xFF } },
    };

    public override string Extension => "db";

    public override string MimeType => new MimeMapper().GetMimeFromExtension("." + Extension);

    public override List<FormatCategory> FormatCategories => GetType().GetCustomAttributes<FormatCategoryAttribute>().Select(a => a.Category).ToList();

    protected override SignatureInformation[] SignatureInformations => ThumbdbSignatureInfo;

    public override string ToString() => "Thumbs.db Detector";
}
