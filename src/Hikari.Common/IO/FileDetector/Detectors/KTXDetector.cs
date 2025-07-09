﻿using System.Reflection;
using Hikari.Common.Mime;

namespace Hikari.Common.IO.FileDetector.Detectors;

[FormatCategory(FormatCategory.Image)]
internal sealed class KTXDetector : AbstractSignatureDetector
{
    private static readonly SignatureInformation[] KtxSignatureInfo = {
        new() { Position = 0, Signature = new byte [] { 0xAB, 0x4B, 0x54, 0x58, 0x20, 0x31, 0x31, 0xBB, 0x0D, 0x0A, 0x1A, 0x0A } },
    };

    public override string Extension => "ktx";

    protected override SignatureInformation[] SignatureInformations => KtxSignatureInfo;

    public override string MimeType => new MimeMapper().GetMimeFromExtension("." + Extension);

    public override List<FormatCategory> FormatCategories => GetType().GetCustomAttributes<FormatCategoryAttribute>().Select(a => a.Category).ToList();

    public override string ToString() => "Khronos Texture Detector";
}
