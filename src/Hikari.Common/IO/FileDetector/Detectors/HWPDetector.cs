using System.Reflection;
using System.Text.RegularExpressions;
using Hikari.Common.Mime;

namespace Hikari.Common.IO.FileDetector.Detectors;

[FormatCategory(FormatCategory.Document)]
internal sealed class HWPDetector : AbstractRegexSignatureDetector
{
    public override string Extension => "hwp";

    protected override Regex Signature => new("^HWP Document File V[0-9]+.[0-9][0-9]");

    public override string MimeType => new MimeMapper().GetMimeFromExtension("." + Extension);

    public override List<FormatCategory> FormatCategories => GetType().GetCustomAttributes<FormatCategoryAttribute>().Select(a => a.Category).ToList();

    public override string ToString() => "Hancom HWP Document Detector";
}
