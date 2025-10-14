using System.Reflection;
using SkiaSharp;

namespace H4D2.Infrastructure;

public static class ResourceLoader
{
    public static SKBitmap LoadEmbeddedResource(string resourceName)
    {
        Assembly assembly = Assembly.GetExecutingAssembly();
        using Stream? stream = assembly.GetManifestResourceStream(resourceName);
        return stream == null ? 
            throw new Exception($"Resource not found: {resourceName}") : 
            SKBitmap.Decode(stream);
    }
}