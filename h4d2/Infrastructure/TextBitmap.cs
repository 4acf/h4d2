using SkiaSharp;

namespace H4D2.Infrastructure;

public class TextBitmap
{
    public readonly int Width;
    public readonly int Height;

    private readonly bool[] _data;
    
    public TextBitmap(SKBitmap bitmap, int character, int[] widths, int height)
    {
        int width = widths[character];
        int numBytes = width * height;
        _data = new bool[numBytes];
        
        int xOffset = 0;
        for (int i = 0; i < character; i++)
        {
            xOffset += widths[i];
        }

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                int x = xOffset + j;
                int y = i;
                SKColor color = bitmap.GetPixel(x, y);

                int index = i * width + j;
                _data[index] = color.Alpha != 0;
            }
        }
        
        Width = width;
        Height = height;
    }

    public bool IsOutOfBounds(int index)
    {
        return index < 0 || index >= _data.Length;
    }

    public bool IsSet(int index)
    {
        if (IsOutOfBounds(index))
            throw new IndexOutOfRangeException();
        return _data[index];
    }
    
    public int GetBytespaceIndex(int x, int y)
    {
        return ((y * Width) + x);
    }
}