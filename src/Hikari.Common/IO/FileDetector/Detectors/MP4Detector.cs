using System.Reflection;
using Hikari.Common.Mime;

namespace Hikari.Common.IO.FileDetector.Detectors;

[FormatCategory(FormatCategory.Video)]
[FormatCategory(FormatCategory.Audio)]
[FormatCategory(FormatCategory.Image)]
internal sealed class MP4Detector : AbstractISOBaseMediaFileDetailDetector
{
    public override string Extension => "mp4";

    protected override IEnumerable<string> NextSignature
    {
        get
        {
            yield return "isom";
        }
    }

    public override string MimeType => new MimeMapper().GetMimeFromExtension("." + Extension);

    public override List<FormatCategory> FormatCategories => GetType().GetCustomAttributes<FormatCategoryAttribute>().Select(a => a.Category).ToList();

    public override string ToString() => "MP4 Detector";
}
