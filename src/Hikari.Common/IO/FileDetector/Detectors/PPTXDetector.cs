﻿using System.IO.Compression;
using System.Reflection;
using Hikari.Common.Mime;

namespace Hikari.Common.IO.FileDetector.Detectors;

[FormatCategory(FormatCategory.Document)]
internal sealed class PPTXDetector : AbstractZipDetailDetector
{
    public override IEnumerable<string> Files
    {
        get
        {
            yield return "[Content_Types].xml";
            yield return "_rels/.rels";
            yield return "ppt/_rels/presentation.xml.rels";
        }
    }

    public override string Precondition => "zip";

    public override string Extension => "pptx";

    public override string MimeType => new MimeMapper().GetMimeFromExtension("." + Extension);

    public override List<FormatCategory> FormatCategories => GetType().GetCustomAttributes<FormatCategoryAttribute>().Select(a => a.Category).ToList();

    protected override bool IsValid(string filename, ZipArchiveEntry entry)
    {
        if (filename == "[Content_Types].xml")
        {
            using StreamReader reader = new StreamReader(entry.Open());
            var text = reader.ReadToEnd();
            return text.IndexOf("<Override PartName=\"/ppt/presentation.xml\" ContentType=\"application/vnd.openxmlformats-officedocument.presentationml.presentation.main+xml\"/>") >= 0;
        }

        return base.IsValid(filename, entry);
    }

    public override string ToString() => "Microsoft PowerPoint Open XML Document(PPTX) Detector";
}
