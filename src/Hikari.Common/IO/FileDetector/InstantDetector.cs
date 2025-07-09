using System.Reflection;
using Hikari.Common.Mime;

namespace Hikari.Common.IO.FileDetector;

internal class InstantDetector : IDetector
{
    public string Description { get; }

    public string Extension { get; }

    public string Precondition { get; }

    public string MimeType => new MimeMapper().GetMimeFromExtension("." + Extension);

    public List<FormatCategory> FormatCategories => GetType().GetCustomAttributes<FormatCategoryAttribute>().Select(a => a.Category).ToList();

    public event Func<Stream, bool> DetectionLogic;

    private int cachedHashcode;

    public InstantDetector(string extension, Func<Stream, bool> logic, string description, string precondition = null)
    {
        Extension = extension;
        Precondition = precondition;
        Description = description;
        DetectionLogic += logic;

        cachedHashcode = $"{Description}{Extension}{Precondition}".GetHashCode();
    }

    public bool Detect(Stream stream)
    {
        return DetectionLogic(stream);
    }

    public override string ToString()
    {
        return Description;
    }

    public override int GetHashCode()
    {
        return cachedHashcode;
    }
}
