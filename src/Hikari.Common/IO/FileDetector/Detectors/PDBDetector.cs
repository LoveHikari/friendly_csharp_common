﻿using System.Reflection;
using System.Text.RegularExpressions;
using Hikari.Common.Mime;

namespace Hikari.Common.IO.FileDetector.Detectors;

[FormatCategory(FormatCategory.Document)]
internal sealed class PDBDetector : AbstractRegexSignatureDetector
{
    public override string Extension => "pdb";

    protected override Regex Signature => new("^Microsoft C\\/C\\+\\+ MSF [0-9].[0-9][0-9]\r\n");

    public override string MimeType => new MimeMapper().GetMimeFromExtension("." + Extension);

    public override List<FormatCategory> FormatCategories => GetType().GetCustomAttributes<FormatCategoryAttribute>().Select(a => a.Category).ToList();

    public override string ToString() => "Microsoft Program Database Detector";
}
