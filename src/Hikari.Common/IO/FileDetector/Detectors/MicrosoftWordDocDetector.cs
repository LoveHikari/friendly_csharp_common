using System.Reflection;
using Hikari.Common.Mime;

namespace Hikari.Common.IO.FileDetector.Detectors;

[FormatCategory(FormatCategory.Document)]
internal sealed class MicrosoftWordDocDetector : AbstractCompoundFileDetailDetector
{
    public override IEnumerable<string> Chunks
    {
        get
        {
            yield return "WordDocument";
        }
    }

    public override string Extension => "doc";

    public override string MimeType => new MimeMapper().GetMimeFromExtension("." + Extension);

    public override List<FormatCategory> FormatCategories => GetType().GetCustomAttributes<FormatCategoryAttribute>().Select(a => a.Category).ToList();

    protected override bool IsValidChunk(string chunkName, byte[] chunkData)
    {
        return chunkName == "WordDocument";
    }

    public override string ToString() => "Microsoft Office Word Document(DOC) Detector";
}
