using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Hikari.Common.Mime;

namespace Hikari.Common.IO.FileDetector.Detectors;

[FormatCategory(FormatCategory.Archive)]
internal sealed class POSIXtarDetector : IDetector
{
    public string Precondition => null;

    public string Extension => "tar";

    public bool Detect(Stream stream)
    {
        stream.Position = 100;
        byte[] mode = new byte[8];
        stream.Read(mode, 0, 8);
        return Regex.IsMatch(Encoding.GetEncoding("ascii").GetString(mode, 0, 8), "[0-7][0-7][0-7][0-7][0-7][0-7][0-7][\0]");
    }

    public string MimeType => new MimeMapper().GetMimeFromExtension("." + Extension);

    public List<FormatCategory> FormatCategories => GetType().GetCustomAttributes<FormatCategoryAttribute>().Select(a => a.Category).ToList();

    public override string ToString() => "pre-POSIX Tar(TAR) Detector";
}
