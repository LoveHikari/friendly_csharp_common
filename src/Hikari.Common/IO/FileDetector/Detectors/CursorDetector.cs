﻿using System.Reflection;
using Hikari.Common.Mime;

namespace Hikari.Common.IO.FileDetector.Detectors;

[FormatCategory(FormatCategory.Image)]
internal sealed class CursorDetector : AbstractSignatureDetector
{
    private static readonly SignatureInformation[] CurSignatureInfo = {
        new() { Position = 0, Signature = new byte [] { 0x00, 0x00, 0x02, 0x00 } },
    };

    public override string Extension => "cur";

    protected override SignatureInformation[] SignatureInformations => CurSignatureInfo;

    public override string MimeType => new MimeMapper().GetMimeFromExtension("." + Extension);

    public override List<FormatCategory> FormatCategories => GetType().GetCustomAttributes<FormatCategoryAttribute>().Select(a => a.Category).ToList();

    public override string ToString() => "Cursor Detector";
}
