using System.Reflection;
using Hikari.Common.Mime;

namespace Hikari.Common.IO.FileDetector.Detectors;

[FormatCategory(FormatCategory.Document)]
internal sealed class BinaryPropertyListDetector : AbstractSignatureDetector
{
    private static readonly SignatureInformation[] BplistSignatureInfo = {
        new () { Position = 0, Signature = "bplist"u8.ToArray() },
    };

    public override string Extension => "bplist";

    protected override SignatureInformation[] SignatureInformations => BplistSignatureInfo;

    public override string MimeType => new MimeMapper().GetMimeFromExtension("." + Extension);

    public override List<FormatCategory> FormatCategories => GetType().GetCustomAttributes<FormatCategoryAttribute>().Select(a => a.Category).ToList();

    public override string ToString() => "Apple Binary Property List Detector";
}
