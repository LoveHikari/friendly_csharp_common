using System.Reflection;
using Hikari.Common.Mime;

namespace Hikari.Common.IO.FileDetector.Detectors;

[FormatCategory(FormatCategory.Video)]
[FormatCategory(FormatCategory.Audio)]
internal sealed class _3GPDetector : AbstractISOBaseMediaFileDetailDetector
{
    public override string Extension => "3gp";

    protected override IEnumerable<string> NextSignature
    {
        get
        {
            yield return "3gp";
        }
    }

    public override string ToString() => "3GPP Detector";

    public override string MimeType => new MimeMapper().GetMimeFromExtension("." + Extension);

    public override List<FormatCategory> FormatCategories => GetType().GetCustomAttributes<FormatCategoryAttribute>().Select(a => a.Category).ToList();
}
