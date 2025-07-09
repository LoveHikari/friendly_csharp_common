using System.Reflection;
using Hikari.Common.Mime;

namespace Hikari.Common.IO.FileDetector.Detectors;

[FormatCategory(FormatCategory.Video)]
internal sealed class M4VDetector : AbstractISOBaseMediaFileDetailDetector
{
    public override string Extension => "m4v";

    protected override IEnumerable<string> NextSignature
    {
        get
        {
            yield return "mp42";
        }
    }

    public override string MimeType => new MimeMapper().GetMimeFromExtension("." + Extension);

    public override List<FormatCategory> FormatCategories => GetType().GetCustomAttributes<FormatCategoryAttribute>().Select(a => a.Category).ToList();

    public override string ToString() => "MP4 Contained H.264(AVC) Decoder";
}
