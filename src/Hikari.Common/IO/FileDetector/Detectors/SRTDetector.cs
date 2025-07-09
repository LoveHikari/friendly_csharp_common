﻿using System.Reflection;
using System.Text.RegularExpressions;
using Hikari.Common.Mime;

namespace Hikari.Common.IO.FileDetector.Detectors;

[FormatCategory(FormatCategory.Document)]
internal sealed class SRTDetector : AbstractRegexSignatureDetector
{
    public override string Precondition => "txt";

    public override string Extension => "srt";

    protected override Regex Signature => new("\\d+\r?\n(\\d\\d:\\d\\d:\\d\\d,\\d\\d\\d) --> (\\d\\d:\\d\\d:\\d\\d,\\d\\d\\d)\r?\n(.*)\r?\n\r?\n");

    public override string MimeType => new MimeMapper().GetMimeFromExtension("." + Extension);

    public override List<FormatCategory> FormatCategories => GetType().GetCustomAttributes<FormatCategoryAttribute>().Select(a => a.Category).ToList();

    public override string ToString() => "SubRip Subtitle Detector";
}
