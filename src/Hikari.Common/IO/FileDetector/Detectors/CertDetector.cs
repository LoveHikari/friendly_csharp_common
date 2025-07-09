﻿using System.Reflection;
using System.Text;
using Hikari.Common.Mime;

namespace Hikari.Common.IO.FileDetector.Detectors;

[FormatCategory(FormatCategory.Document)]
internal sealed class CertDetector : AbstractSignatureDetector
{
    private static readonly SignatureInformation[] CrtSignatureInfo = {
        new() { Position = 0, Signature = Encoding.GetEncoding ( "ascii" ).GetBytes ( "-----BEGIN CERTIFICATE-----" ) },
    };

    public override string Precondition => "txt";

    public override string Extension => "crt";

    protected override SignatureInformation[] SignatureInformations => CrtSignatureInfo;

    public override string MimeType => new MimeMapper().GetMimeFromExtension("." + Extension);

    public override List<FormatCategory> FormatCategories => GetType().GetCustomAttributes<FormatCategoryAttribute>().Select(a => a.Category).ToList();

    public override string ToString() => "Certificate Detector";
}
