using System.Reflection;
using Hikari.Common.Mime;

namespace Hikari.Common.IO.FileDetector.Detectors;

[FormatCategory(FormatCategory.Executable)]
internal sealed class JavaClassDetector : AbstractSignatureDetector
{
    private static readonly SignatureInformation[] ClassSignatureInfo = {
        new() { Position = 0, Signature = new byte [] { 0xCA, 0xFE, 0xBA, 0xBE } },
    };

    public override string Extension => "class";

    protected override SignatureInformation[] SignatureInformations => ClassSignatureInfo;

    public override string MimeType => new MimeMapper().GetMimeFromExtension("." + Extension);

    public override List<FormatCategory> FormatCategories => GetType().GetCustomAttributes<FormatCategoryAttribute>().Select(a => a.Category).ToList();

    public override string ToString() => "Java Bytecode Detector";
}
