﻿using System.Reflection;
using Hikari.Common.Mime;

namespace Hikari.Common.IO.FileDetector.Detectors;

[FormatCategory(FormatCategory.Image)]
internal sealed class PngDetector : AbstractSignatureDetector
{
    private static readonly SignatureInformation[] PngSignatureInfo = {
        new () { Position = 0, Signature = new byte [] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A } },
    };

    public override string Extension => "png";

    protected override SignatureInformation[] SignatureInformations => PngSignatureInfo;

    public override string MimeType => new MimeMapper().GetMimeFromExtension("." + Extension);

    public override List<FormatCategory> FormatCategories => GetType().GetCustomAttributes<FormatCategoryAttribute>().Select(a => a.Category).ToList();

    public override string ToString() => "Portable Network Graphics Detector";
}
