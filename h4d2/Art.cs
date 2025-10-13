using SkiaSharp;

namespace H4D2;

public static class Art
{
    public const int SpriteSize = 16;
    public static readonly Bitmap[][] Survivors = _LoadSurvivors();
    public static readonly Bitmap[][] Common = _LoadCommons();
    public static readonly Bitmap[][] Uncommon = _LoadUncommons();
    public static readonly Bitmap[][] Special = _LoadSpecials();
    
    private static Bitmap[][] _LoadBitmaps(string resourceName, int rows, int columns)
    {
        var result = new Bitmap[rows][];
        for (int i = 0; i < rows; i++)
        {
            result[i] = new Bitmap[columns];
        }

        SKBitmap fullResourceBitmap = ResourceLoader.LoadEmbeddedResource($"h4d2.res.{resourceName}");
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                result[i][j] = new Bitmap(
                    fullResourceBitmap,
                    SpriteSize,
                    i,
                    j
                );
            }
        }
        
        return result;
    }
    private static Bitmap[][] _LoadSurvivors() => _LoadBitmaps("survivor.png", 8, 9);
    private static Bitmap[][] _LoadCommons() => _LoadBitmaps("common.png", 9, 9);
    private static Bitmap[][] _LoadUncommons() => _LoadBitmaps("uncommon.png", 5, 9);
    private static Bitmap[][] _LoadSpecials() => _LoadBitmaps("special.png", 8, 9);
}