﻿using System.Reflection;
using Hikari.Common.Mime;

namespace Hikari.Common.IO.FileDetector.Detectors;

[FormatCategory(FormatCategory.Compression)]
[FormatCategory(FormatCategory.Archive)]
internal sealed class ZipDetector : AbstractSignatureDetector
{
    private static readonly SignatureInformation[] ZipSignatureInfo = {
        new () { Position = 0, Signature = new byte [] { 0x50, 0x4b, 0x03, 0x04 } },
        new () { Position = 0, Signature = new byte [] { 0x50, 0x4b, 0x05, 0x06 } },
        new () { Position = 0, Signature = new byte [] { 0x50, 0x4b, 0x07, 0x08 } },
    };

    public override string Extension => "zip";

    public override string MimeType => new MimeMapper().GetMimeFromExtension("." + Extension);

    public override List<FormatCategory> FormatCategories => GetType().GetCustomAttributes<FormatCategoryAttribute>().Select(a => a.Category).ToList();

    protected override SignatureInformation[] SignatureInformations => ZipSignatureInfo;

    public override string ToString() => "ZIP Detector";
}
