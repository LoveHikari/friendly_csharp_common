using System.Reflection;
using Hikari.Common.Mime;

namespace Hikari.Common.IO.FileDetector.Detectors;

[FormatCategory(FormatCategory.Document)]
internal sealed class PFXDetector : AbstractSignatureDetector
{
    private static readonly SignatureInformation[] PfxSignatureInfo = {
        new() { Position = 0, Signature = new byte [] { 0x30, 0x82, 0x06 } },
    };

    public override string Extension => "pfx";

    protected override SignatureInformation[] SignatureInformations => PfxSignatureInfo;

    public override string MimeType => new MimeMapper().GetMimeFromExtension("." + Extension);

    public override List<FormatCategory> FormatCategories => GetType().GetCustomAttributes<FormatCategoryAttribute>().Select(a => a.Category).ToList();

    public override string ToString() => "Microsoft Personal inFormation eXchange Certificate Detector";
}
