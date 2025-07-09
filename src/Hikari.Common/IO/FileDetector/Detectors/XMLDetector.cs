using System.Reflection;
using System.Text.RegularExpressions;
using Hikari.Common.Mime;

namespace Hikari.Common.IO.FileDetector.Detectors;

[FormatCategory(FormatCategory.Document)]
internal sealed class XMLDetector : AbstractRegexSignatureDetector
{
    public override string Precondition => "txt";

    public override string Extension => "xml";

    public override string MimeType => new MimeMapper().GetMimeFromExtension("." + Extension);

    public override List<FormatCategory> FormatCategories => GetType().GetCustomAttributes<FormatCategoryAttribute>().Select(a => a.Category).ToList();

    protected override Regex Signature => new("^<\\?xml[ \t\n\r]+version=\"[0-9]+\\.[0-9]+\"[ \t\n\r]+([a-zA-Z0-9]+=\"[a-zA-Z0-9\\-_]+\"[ \t\n\r]*)*[ \t\n\r]+\\?>");

    public override string ToString() => "eXtensible Markup Language Document Detector";
}
