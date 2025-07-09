using System.Reflection;
using System.Text.RegularExpressions;
using Hikari.Common.Mime;

namespace Hikari.Common.IO.FileDetector.Detectors;

[FormatCategory(FormatCategory.Document)]
internal sealed class VisualStudioSolutionDetector : AbstractRegexSignatureDetector
{
    public override string Precondition => "txt";

    public override string Extension => "sln";

    public override string MimeType => new MimeMapper().GetMimeFromExtension("." + Extension);

    public override List<FormatCategory> FormatCategories => GetType().GetCustomAttributes<FormatCategoryAttribute>().Select(a => a.Category).ToList();

    protected override Regex Signature => new("\r\nMicrosoft Visual Studio Solution File, Format Version [0-9]+.[0-9]+\r\n");

    public override string ToString() => "Microsoft Visual Studio Solution Detector";
}
