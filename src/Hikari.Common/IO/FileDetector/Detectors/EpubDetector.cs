using System.IO.Compression;
using System.Reflection;
using System.Text;
using Hikari.Common.Mime;

namespace Hikari.Common.IO.FileDetector.Detectors;

[FormatCategory(FormatCategory.Document)]
internal sealed class EpubDetector : AbstractZipDetailDetector
{
    public override IEnumerable<string> Files
    {
        get
        {
            yield return "mimetype";
        }
    }

    public override string Precondition => "zip";

    public override string Extension => "epub";

    public override string MimeType => new MimeMapper().GetMimeFromExtension("." + Extension);

    public override List<FormatCategory> FormatCategories => GetType().GetCustomAttributes<FormatCategoryAttribute>().Select(a => a.Category).ToList();

    protected override bool IsValid(string filename, ZipArchiveEntry entry)
    {
        if (filename == "mimetype")
        {
            var mimetypeStream = entry.Open();
            byte[] buffer = new byte["application/epub+zip".Length];
            if (mimetypeStream.Read(buffer, 0, buffer.Length) != buffer.Length)
            {
                return false;
            }

            if (Encoding.GetEncoding("ascii").GetString(buffer, 0, buffer.Length) != "application/epub+zip")
            {
                return false;
            }
        }

        return base.IsValid(filename, entry);
    }

    public override string ToString() => "e-Pub Document File Detector";
}
