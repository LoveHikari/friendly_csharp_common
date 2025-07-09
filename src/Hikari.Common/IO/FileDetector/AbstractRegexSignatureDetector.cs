﻿using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Hikari.Common.Mime;

namespace Hikari.Common.IO.FileDetector;

public abstract class AbstractRegexSignatureDetector : IDetector
{
    public abstract string Extension { get; }

    public virtual string Precondition => null;

    public virtual string MimeType => new MimeMapper().GetMimeFromExtension("." + Extension);

    public virtual List<FormatCategory> FormatCategories => GetType().GetCustomAttributes<FormatCategoryAttribute>().Select(a => a.Category).ToList();

    protected abstract Regex Signature { get; }

    public virtual bool Detect(Stream stream)
    {
        int readBufferSize = Signature.ToString().Length * 8;
        char[] buffer = new char[readBufferSize];
        var encodings = new[]
        {
            Encoding.ASCII,
            Encoding.UTF8,
            Encoding.UTF32,
            Encoding.Unicode,
            Encoding.BigEndianUnicode
        };
        foreach (var encoding in encodings)
        {
            stream.Position = 0;
            var reader = new StreamReader(stream, encoding, true, readBufferSize, true);
            reader.ReadBlock(buffer, 0, readBufferSize);
            if (Signature.IsMatch(new string(buffer)))
            {
                return true;
            }
        }

        return false;
    }
}
