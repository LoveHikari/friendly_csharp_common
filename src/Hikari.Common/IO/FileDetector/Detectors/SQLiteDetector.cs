﻿using System.Reflection;
using Hikari.Common.Mime;

namespace Hikari.Common.IO.FileDetector.Detectors;

[FormatCategory(FormatCategory.Document)]
internal sealed class SQLiteDetector : AbstractSignatureDetector
{
    private static readonly SignatureInformation[] SqliteSignatureInfo = {
        new () { Position = 0, Signature = new byte [] { 0x53, 0x51, 0x4C, 0x69, 0x74, 0x65, 0x20, 0x66, 0x6F, 0x72, 0x6D, 0x61, 0x74, 0x20, 0x33, 0x00 } },
    };

    public override string Extension => "sqlite";

    public override string MimeType => new MimeMapper().GetMimeFromExtension("." + Extension);

    public override List<FormatCategory> FormatCategories => GetType().GetCustomAttributes<FormatCategoryAttribute>().Select(a => a.Category).ToList();

    protected override SignatureInformation[] SignatureInformations => SqliteSignatureInfo;

    public override string ToString() => "SQLite Detector";
}
