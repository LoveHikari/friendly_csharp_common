﻿using System.Reflection;
using Hikari.Common.Mime;

namespace Hikari.Common.IO.FileDetector.Detectors;

[FormatCategory(FormatCategory.Image)]
public sealed class JpegDetector : AbstractSignatureDetector
{
    private static readonly SignatureInformation[] JpegSignatureInfo = {
        new() { Position = 0, Signature = new byte [] { 0xFF, 0xD8, 0xFF, 0xFE, 0x00 } },
        new () { Position = 0, Signature = new byte [] { 0xFF, 0xD8, 0xFF, 0xDB } },
        new () { Position = 0, Signature = new byte [] { 0xFF, 0xD8, 0xFF, 0xE0 } },
        new () { Position = 0, Signature = new byte [] { 0xFF, 0xD8, 0xFF, 0xE1 } },
        new () { Position = 0, Signature = new byte [] { 0xFF, 0xD8, 0xFF, 0xE2 } },
        new () { Position = 0, Signature = new byte [] { 0xFF, 0xD8, 0xFF, 0xE3 } },
        new () { Position = 0, Signature = new byte [] { 0xFF, 0xD8, 0xFF, 0xE8 } },
        new () { Position = 0, Signature = new byte [] { 0xFF, 0xD8, 0xFF, 0xEE } },
    };

    public override string Extension => "jpg";

    protected override SignatureInformation[] SignatureInformations => JpegSignatureInfo;

    public override string MimeType => new MimeMapper().GetMimeFromExtension("." + Extension);

    public override List<FormatCategory> FormatCategories => GetType().GetCustomAttributes<FormatCategoryAttribute>().Select(a => a.Category).ToList();

    public override string ToString() => "JPEG Detector";
}
