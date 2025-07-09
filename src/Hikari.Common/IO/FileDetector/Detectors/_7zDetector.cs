﻿using System.Reflection;
using Hikari.Common.Mime;

namespace Hikari.Common.IO.FileDetector.Detectors;

[FormatCategory(FormatCategory.Archive)]
[FormatCategory(FormatCategory.Compression)]
internal sealed class _7zDetector : AbstractSignatureDetector
{
    private static readonly SignatureInformation[] _7ZSignatureInfo = {
        new() { Position = 0, Signature = new byte [] { 0x37, 0x7A, 0xBC, 0xAF, 0x27, 0x1C } },
    };

    public override string Extension => "7z";

    protected override SignatureInformation[] SignatureInformations => _7ZSignatureInfo;

    public override string MimeType => new MimeMapper().GetMimeFromExtension("." + Extension);

    public override List<FormatCategory> FormatCategories => GetType().GetCustomAttributes<FormatCategoryAttribute>().Select(a => a.Category).ToList();

    public override string ToString() => "7Z Detector";
}
