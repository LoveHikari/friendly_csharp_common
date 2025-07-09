﻿using System.Reflection;
using System.Text;
using Hikari.Common.Mime;

namespace Hikari.Common.IO.FileDetector;

public abstract class AbstractISOBaseMediaFileDetailDetector : IDetector
{
    protected abstract IEnumerable<string> NextSignature { get; }

    public abstract string Extension { get; }

    public virtual string Precondition => null;

    public virtual string MimeType => new MimeMapper().GetMimeFromExtension("." + Extension);

    public virtual List<FormatCategory> FormatCategories => GetType().GetCustomAttributes<FormatCategoryAttribute>().Select(a => a.Category).ToList();

    public virtual bool Detect(Stream stream)
    {
        var reader = new BinaryReader(stream, Encoding.UTF8, true);
        _ = reader.ReadInt32();
        if (reader.ReadByte() == 0x66 && reader.ReadByte() == 0x74 && reader.ReadByte() == 0x79 && reader.ReadByte() == 0x70)
        {
            foreach (var ns in NextSignature)
            {
                stream.Position = 8;
                var readed = Encoding.GetEncoding("ascii").GetString(reader.ReadBytes(ns.Length), 0, ns.Length);
                stream.Position = 0;
                if (ns == readed)
                {
                    return true;
                }
            }
        }

        return false;
    }
}
