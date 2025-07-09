﻿using System.Reflection;
using Hikari.Common.Mime;

namespace Hikari.Common.IO.FileDetector.Detectors;

[FormatCategory(FormatCategory.Video)]
[FormatCategory(FormatCategory.Executable)]
internal sealed class ShockwaveFlashDetector : AbstractSignatureDetector
{
    private static readonly SignatureInformation[] SwfSignatureInfo = {
        new() { Position = 0, Signature = "CWS"u8.ToArray() },
        new() { Position = 0, Signature = "FWS"u8.ToArray() },
    };

    public override string Extension => "swf";

    public override string MimeType => new MimeMapper().GetMimeFromExtension("." + Extension);

    public override List<FormatCategory> FormatCategories => GetType().GetCustomAttributes<FormatCategoryAttribute>().Select(a => a.Category).ToList();

    protected override SignatureInformation[] SignatureInformations => SwfSignatureInfo;

    public override string ToString() => "Shockwave Flash(SWF) Detector";
}
