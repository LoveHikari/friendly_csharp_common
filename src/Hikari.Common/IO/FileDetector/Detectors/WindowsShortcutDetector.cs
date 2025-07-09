using System.Reflection;
using Hikari.Common.Mime;

namespace Hikari.Common.IO.FileDetector.Detectors;

[FormatCategory(FormatCategory.System)]
internal sealed class WindowsShortcutDetector : AbstractSignatureDetector
{
    private static readonly SignatureInformation[] LnkSignatureInfo = {
        new() { Position = 0, Signature = new byte [] { 0x4C, 0x00, 0x00, 0x00, 0x01, 0x14, 0x02, 0x00 } },
    };

    public override string Extension => "lnk";

    public override string MimeType => new MimeMapper().GetMimeFromExtension("." + Extension);

    public override List<FormatCategory> FormatCategories => GetType().GetCustomAttributes<FormatCategoryAttribute>().Select(a => a.Category).ToList();

    protected override SignatureInformation[] SignatureInformations => LnkSignatureInfo;

    public override string ToString() => "Windows Shortcut File Detector";
}
