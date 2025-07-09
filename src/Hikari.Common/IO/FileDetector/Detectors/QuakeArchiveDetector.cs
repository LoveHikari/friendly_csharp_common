using System.Reflection;
using Hikari.Common.Mime;

namespace Hikari.Common.IO.FileDetector.Detectors;

[FormatCategory(FormatCategory.Archive)]
internal sealed class QuakeArchiveDetector : AbstractSignatureDetector
{
    private static readonly SignatureInformation[] PakSignatureInfo = {
        new() { Position = 0, Signature = "PACK"u8.ToArray() },
    };

    public override string Extension => "pak";

    protected override SignatureInformation[] SignatureInformations => PakSignatureInfo;

    public override string MimeType => new MimeMapper().GetMimeFromExtension("." + Extension);

    public override List<FormatCategory> FormatCategories => GetType().GetCustomAttributes<FormatCategoryAttribute>().Select(a => a.Category).ToList();

    public override string ToString() => "Quake™ Package Detector";
}
