﻿using System.Reflection;
using System.Text;
using Hikari.Common.Mime;

namespace Hikari.Common.IO.FileDetector.Detectors;

[FormatCategory(FormatCategory.Image)]
internal sealed class PKMDetector : AbstractSignatureDetector
{
    private static readonly SignatureInformation[] PkmSignatureInfo = {
        new() { Position = 0, Signature = Encoding.ASCII.GetBytes ( "PKM 10" ) },
    };

    public override string Extension => "pkm";

    public override string MimeType => new MimeMapper().GetMimeFromExtension("." + Extension);

    public override List<FormatCategory> FormatCategories => GetType().GetCustomAttributes<FormatCategoryAttribute>().Select(a => a.Category).ToList();

    protected override SignatureInformation[] SignatureInformations => PkmSignatureInfo;

    public override string ToString() => "PKM Detector";
}
