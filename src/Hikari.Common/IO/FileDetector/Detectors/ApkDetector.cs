using System.Reflection;
using Hikari.Common.Mime;

namespace Hikari.Common.IO.FileDetector.Detectors;

[FormatCategory(FormatCategory.Archive)]
[FormatCategory(FormatCategory.Compression)]
[FormatCategory(FormatCategory.Executable)]
internal sealed class ApkDetector : AbstractZipDetailDetector
{
    public override IEnumerable<string> Files
    {
        get
        {
            yield return "classes.dex";
            yield return "AndroidManifest.xml";
        }
    }

    public override string Precondition => "zip";

    public override string Extension => "apk";

    public override string MimeType => new MimeMapper().GetMimeFromExtension("." + Extension);

    public override List<FormatCategory> FormatCategories => GetType().GetCustomAttributes<FormatCategoryAttribute>().Select(a => a.Category).ToList();

    public override string ToString() => "Android Package(APK) Detector";
}
