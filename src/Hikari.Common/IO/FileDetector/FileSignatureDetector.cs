using System.Reflection;

namespace Hikari.Common.IO.FileDetector;

public static class FileSignatureDetector
{
    private static List<IDetector> Detectors { get; set; } = [];

    public static IReadOnlyList<IDetector> Registered => Detectors;

    public static void AddDetector<T>() where T : IDetector
    {
        var instance = Activator.CreateInstance<T>();
        AddDetector(instance);
    }

    public static void AddDetector(IDetector instance)
    {
        if (!Detectors.Contains(instance))
        {
            Detectors.Add(instance);
        }
    }

    static FileSignatureDetector()
    {
        var type = typeof(IDetector);
        foreach (var item in AppDomain.CurrentDomain.GetAssemblies().Where(a => !a.FullName.StartsWith(["System", "Microsoft"])).SelectMany(a => GetLoadableTypes(a)))
        {
            if (type.IsAssignableFrom(item) && !item.IsAbstract && !item.IsInterface && item.GetTypeInfo().DeclaredConstructors.First().GetParameters().Length == 0)
            {
                AddDetector(Activator.CreateInstance(item) as IDetector);
            }
        }
    }

    public static IDetector DetectFiletype(this FileInfo file)
    {
        using var stream = file.OpenRead();
        return DetectFiletype(stream);
    }

    public static IDetector DetectFiletype(this Stream stream)
    {
        if (stream.CanSeek && stream.Length > 0)
        {
            foreach (var detector in Detectors)
            {
                stream.Position = 0;
                if (detector.Detect(stream))
                {
                    stream.Position = 0;
                    return detector;
                }
            }
        }
        return new NoneDetector();
    }
    /// <summary>
	/// 获取可加载的程序集类型信息
	/// </summary>
	/// <param name="assembly"></param>
	/// <returns></returns>
	/// <exception cref="ArgumentNullException"></exception>
	private static IEnumerable<Type> GetLoadableTypes(Assembly assembly)
    {
        if (assembly == null) throw new ArgumentNullException(nameof(assembly));
        try
        {
            return assembly.GetTypes();
        }
        catch (ReflectionTypeLoadException e)
        {
            return e.Types.Where(t => t != null);
        }
    }
}