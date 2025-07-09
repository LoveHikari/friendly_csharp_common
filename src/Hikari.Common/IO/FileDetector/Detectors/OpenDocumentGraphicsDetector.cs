﻿using System.IO.Compression;
using System.Reflection;
using System.Text;
using Hikari.Common.Mime;

namespace Hikari.Common.IO.FileDetector.Detectors;

[FormatCategory(FormatCategory.Document)]
internal sealed class OpenDocumentGraphicsDetector : AbstractZipDetailDetector
{
    public override IEnumerable<string> Files
    {
        get
        {
            yield return "mimetype";
        }
    }

    public override string Precondition => "zip";

    public override string Extension => "odg";

    public override string MimeType => new MimeMapper().GetMimeFromExtension("." + Extension);

    public override List<FormatCategory> FormatCategories => GetType().GetCustomAttributes<FormatCategoryAttribute>().Select(a => a.Category).ToList();

    protected override bool IsValid(string filename, ZipArchiveEntry entry)
    {
        if (filename == "mimetype")
        {
            using Stream mimetypeStream = entry.Open();
            byte[] buffer = new byte["application/vnd.oasis.opendocument.graphics".Length];
            if (mimetypeStream.Read(buffer, 0, buffer.Length) != buffer.Length)
            {
                return false;
            }

            if (Encoding.GetEncoding("ascii").GetString(buffer, 0, buffer.Length) != "application/vnd.oasis.opendocument.graphics")
            {
                return false;
            }
        }

        return base.IsValid(filename, entry);
    }

    public override string ToString() => "OpenDocument Graphics File Detector";
}
