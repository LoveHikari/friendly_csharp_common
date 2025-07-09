﻿using System.Reflection;
using Hikari.Common.Mime;

namespace Hikari.Common.IO.FileDetector.Detectors;

[FormatCategory(FormatCategory.Document)]
internal sealed class DOCXDetector : AbstractZipDetailDetector
{
    public override IEnumerable<string> Files
    {
        get
        {
            yield return "[Content_Types].xml";
            yield return "_rels/.rels";
            yield return "word/_rels/document.xml.rels";
        }
    }

    public override string Precondition => "zip";

    public override string Extension => "docx";

    public override string MimeType => new MimeMapper().GetMimeFromExtension("." + Extension);

    public override List<FormatCategory> FormatCategories => GetType().GetCustomAttributes<FormatCategoryAttribute>().Select(a => a.Category).ToList();

    public override string ToString() => "Microsoft Word Open XML Document(DOCX) Detector";
}