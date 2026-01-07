using SkiaSharp;

namespace H4D2.Infrastructure;

public class Bitmap
{
    public readonly byte[] Data;
    public readonly int Width;
    public readonly int Height;

    protected readonly Camera? _camera;
    
    protected Bitmap(int width, int height, Camera camera)
    {
        int numBytes = width * height * 4;
        Data = new byte[numBytes];
        Width = width;
        Height = height;
        _camera = camera;
    }

    public Bitmap(SKBitmap bitmap)
    {
        _camera = null;
        Width = bitmap.Width;
        Height = bitmap.Height;
        int numBytes = Width * Height * 4;
        Data = new byte[numBytes];

        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                SKColor color = bitmap.GetPixel(x, y);
                int index = (y * Width * 4) + (x * 4);
                Data[index] = color.Red;
                Data[index + 1] = color.Green;
                Data[index + 2] = color.Blue;
                Data[index + 3] = color.Alpha;
            }
        }
    }
    
    public Bitmap(SKBitmap bitmap, int spriteSize, int row, int col)
    : this(bitmap, spriteSize, spriteSize, row, col)
    {
       
    }
    
    public Bitmap(SKBitmap bitmap, int spriteWidth, int spriteHeight, int row, int col)
    {
        _camera = null;
        Width = spriteWidth;
        Height = spriteHeight;
        int numBytes = spriteWidth * spriteHeight * 4;
        Data = new byte[numBytes];

        for (int i = 0; i < spriteHeight; i++)
        {
            for (int j = 0; j < spriteWidth; j++)
            {
                int x = (col * spriteWidth) + j;
                int y = (row * spriteHeight) + i;
                SKColor color = bitmap.GetPixel(x, y);

                int index = (i * spriteWidth * 4) + (j * 4);
                Data[index] = color.Red;
                Data[index + 1] = color.Green;
                Data[index + 2] = color.Blue;
                Data[index + 3] = color.Alpha;
            }
        }
    }

    public int ColorAt(int x, int y)
    {
        int index = _GetBytespaceIndex(Width, x, y);
        if (IsOutOfBounds(index))
            return 0x0;
        return RGB.ToInt(Data[index],  Data[index + 1], Data[index + 2]);
    }
    
    public bool IsOutOfBounds(int index)
    {
        return index < 0 || index >= Data.Length;
    }
    
    protected static int _GetBytespaceIndex(int width, int x, int y)
    {
        return ((y * width) + x) * 4;
    }
}