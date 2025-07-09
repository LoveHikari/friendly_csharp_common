﻿using System.Reflection;
using System.Text;
using Hikari.Common.Mime;

namespace Hikari.Common.IO.FileDetector.Detectors;

[FormatCategory(FormatCategory.Document)]
internal sealed class CompoundHWPDetector : AbstractCompoundFileDetailDetector
{
    public override IEnumerable<string> Chunks
    {
        get
        {
            yield return "FileHeader";
        }
    }

    public override string Extension => "hwp";

    protected override bool IsValidChunk(string chunkName, byte[] chunkData)
    {
        return chunkName == "FileHeader" && Encoding.UTF8.GetString(chunkData, 0, 18) == "HWP Document File\0";
    }

    public override string MimeType => new MimeMapper().GetMimeFromExtension("." + Extension);

    public override List<FormatCategory> FormatCategories => GetType().GetCustomAttributes<FormatCategoryAttribute>().Select(a => a.Category).ToList();

    public override string ToString() => "Hancom Compound File Binary Format Type HWP Document Detector";
}
