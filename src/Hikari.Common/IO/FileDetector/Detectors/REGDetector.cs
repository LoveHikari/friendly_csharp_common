﻿using System.Reflection;
using System.Text.RegularExpressions;
using Hikari.Common.Mime;

namespace Hikari.Common.IO.FileDetector.Detectors;

[FormatCategory(FormatCategory.Document)]
[FormatCategory(FormatCategory.System)]
internal sealed class REGDetector : AbstractRegexSignatureDetector
{
    public override string Precondition => "txt";

    public override string Extension => "reg";

    protected override Regex Signature => new("^Windows Registry Editor Version [0-9]+.[0-9][0-9]\r?\n");

    public override string MimeType => new MimeMapper().GetMimeFromExtension("." + Extension);

    public override List<FormatCategory> FormatCategories => GetType().GetCustomAttributes<FormatCategoryAttribute>().Select(a => a.Category).ToList();

    public override string ToString() => "Windows Registry Detector";
}
