using System.Reflection;
using SkiaSharp;

namespace H4D2.Infrastructure;

public static class Art
{
    private static class ResourceLoader
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

    public static Bitmap LoadBitmap(string resourceName)
    {
        return new Bitmap(ResourceLoader.LoadEmbeddedResource(resourceName));
    }
    
    public static Bitmap[][] LoadBitmaps(string resourceName, int spriteSize, int rows, int columns)
    {
        var result = new Bitmap[rows][];
        for (int i = 0; i < rows; i++)
        {
            result[i] = new Bitmap[columns];
        }

        SKBitmap fullResourceBitmap = ResourceLoader.LoadEmbeddedResource(resourceName);
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                result[i][j] = new Bitmap(
                    fullResourceBitmap,
                    spriteSize,
                    i,
                    j
                );
            }
        }
        
        return result;
    }
    
    public static Bitmap[][] LoadBitmaps(string resourceName, int spriteWidth, int spriteHeight, int rows, int columns)
    {
        var result = new Bitmap[rows][];
        for (int i = 0; i < rows; i++)
        {
            result[i] = new Bitmap[columns];
        }

        SKBitmap fullResourceBitmap = ResourceLoader.LoadEmbeddedResource(resourceName);
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                result[i][j] = new Bitmap(
                    fullResourceBitmap,
                    spriteWidth,
                    spriteHeight,
                    i,
                    j
                );
            }
        }
        
        return result;
    }

    public static TextBitmap[] LoadFontBitmaps(string resourceName, int characters, int[] widths, int height)
    {
        var result = new TextBitmap[characters];
        SKBitmap fullResourceBitmap = ResourceLoader.LoadEmbeddedResource(resourceName);
        for (int i = 0; i < characters; i++)
        {
            result[i] = new TextBitmap(fullResourceBitmap, i, widths, height);
        }
        return result;
    }
}