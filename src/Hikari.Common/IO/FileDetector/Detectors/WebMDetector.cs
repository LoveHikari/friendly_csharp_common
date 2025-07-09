﻿using System.Reflection;
using System.Text;
using Hikari.Common.Mime;

namespace Hikari.Common.IO.FileDetector.Detectors;

[FormatCategory(FormatCategory.Video)]
[FormatCategory(FormatCategory.Audio)]
internal sealed class WebMDetector : AbstractSignatureDetector
{
    private static readonly SignatureInformation[] WebmSignatureInfo = {
        new() { Position = 0, Signature = new byte [] { 0x1A, 0x45, 0xDF, 0xA3 } },
    };

    public override string Extension => "webm";

    public override string MimeType => new MimeMapper().GetMimeFromExtension("." + Extension);

    public override List<FormatCategory> FormatCategories => GetType().GetCustomAttributes<FormatCategoryAttribute>().Select(a => a.Category).ToList();

    protected override SignatureInformation[] SignatureInformations => WebmSignatureInfo;

    public override string ToString() => "WebM Video Detector";

    public override bool Detect(Stream stream)
    {
        if (base.Detect(stream))
        {
            stream.Position = 0x1F;
            byte[] buffer = new byte[4];
            stream.Read(buffer, 0, 4);
            return Encoding.GetEncoding("ascii").GetString(buffer, 0, 4) == "webm";
        }
        return false;
    }
}
