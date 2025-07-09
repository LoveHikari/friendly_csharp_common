using System.Reflection;
using System.Text.RegularExpressions;
using Hikari.Common.Mime;

namespace Hikari.Common.IO.FileDetector.Detectors;

[FormatCategory(FormatCategory.Document)]
internal sealed class InitializationDetector : AbstractRegexSignatureDetector
{
    public override string Precondition => "txt";

    public override string Extension => "ini";

    protected override Regex Signature => new("^(\\[(.*)\\]\r?\n((((.*)=(.*))*|(;(.*)))\r?\n)*)+");

    public override string MimeType => new MimeMapper().GetMimeFromExtension("." + Extension);

    public override List<FormatCategory> FormatCategories => GetType().GetCustomAttributes<FormatCategoryAttribute>().Select(a => a.Category).ToList();

    public override string ToString() => "Initialization File Detector";
}
