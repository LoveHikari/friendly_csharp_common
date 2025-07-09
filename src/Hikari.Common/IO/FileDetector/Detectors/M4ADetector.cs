using System.Reflection;
using Hikari.Common.Mime;

namespace Hikari.Common.IO.FileDetector.Detectors;

[FormatCategory(FormatCategory.Audio)]
internal sealed class M4ADetector : AbstractISOBaseMediaFileDetailDetector
{
    public override string Extension => "m4a";

    protected override IEnumerable<string> NextSignature
    {
        get
        {
            yield return "M4A ";
        }
    }

    public override string MimeType => new MimeMapper().GetMimeFromExtension("." + Extension);

    public override List<FormatCategory> FormatCategories => GetType().GetCustomAttributes<FormatCategoryAttribute>().Select(a => a.Category).ToList();

    public override string ToString() => "MP4 Contained Advanced Audio Coding(AAC) Decoder";
}
