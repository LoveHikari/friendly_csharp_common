﻿using System.Reflection;
using System.Xml;
using Hikari.Common.Mime;

namespace Hikari.Common.IO.FileDetector.Detectors;

[FormatCategory(FormatCategory.Document)]
internal sealed class ConfigurationDetector : IDetector
{
    public string Precondition => "txt";

    public string Extension => "config";

    public string MimeType => new MimeMapper().GetMimeFromExtension("." + Extension);

    public List<FormatCategory> FormatCategories => GetType().GetCustomAttributes<FormatCategoryAttribute>().Select(a => a.Category).ToList();

    public bool Detect(Stream stream)
    {
        try
        {
            var reader = XmlReader.Create(stream, new XmlReaderSettings()
            {
            });
            if (reader.Read() && reader.IsStartElement() && reader.Name == "configuration")
            {
                return true;
            }
        }
        catch
        {
        }

        return false;
    }

    public override string ToString() => "Microsoft .NET Configuration File Detector";
}
