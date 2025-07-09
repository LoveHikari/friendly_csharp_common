using System.Reflection;
using Hikari.Common.Mime;

namespace Hikari.Common.IO.FileDetector.Detectors;

[FormatCategory(FormatCategory.Audio)]
internal sealed class MP3Detector : AbstractSignatureDetector
{
    private static readonly SignatureInformation[] Mp3SignatureInfo = {
        new () { Position = 0, Signature = new byte [] { 0xFF, 0xFB } },
        new () { Position = 0, Signature = "ID3"u8.ToArray() },
    };

    public override string Extension => "mp3";

    public override string MimeType => new MimeMapper().GetMimeFromExtension("." + Extension);

    public override List<FormatCategory> FormatCategories => GetType().GetCustomAttributes<FormatCategoryAttribute>().Select(a => a.Category).ToList();

    protected override SignatureInformation[] SignatureInformations => Mp3SignatureInfo;

    public override string ToString() => "MP3 Detector";
}
