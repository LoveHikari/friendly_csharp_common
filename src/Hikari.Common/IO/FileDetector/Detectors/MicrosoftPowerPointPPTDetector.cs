using System.Reflection;
using Hikari.Common.Mime;

namespace Hikari.Common.IO.FileDetector.Detectors;

[FormatCategory(FormatCategory.Document)]
internal sealed class MicrosoftPowerPointPPTDetector : AbstractCompoundFileDetailDetector
{
    public override IEnumerable<string> Chunks
    {
        get
        {
            yield return "PowerPoint Document";
        }
    }

    public override string Extension => "ppt";

    public override string MimeType => new MimeMapper().GetMimeFromExtension("." + Extension);

    public override List<FormatCategory> FormatCategories => GetType().GetCustomAttributes<FormatCategoryAttribute>().Select(a => a.Category).ToList();

    protected override bool IsValidChunk(string chunkName, byte[] chunkData)
    {
        return chunkName == "PowerPoint Document";
    }

    public override string ToString() => "Microsoft Office PowerPoint Document(PPT) Detector";
}
