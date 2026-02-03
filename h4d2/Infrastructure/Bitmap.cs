using SkiaSharp;

namespace H4D2.Infrastructure;

public class Bitmap
{
    public readonly int Width;
    public readonly int Height;

    protected readonly Camera? _camera;
    protected readonly byte[] _data;
    
    protected Bitmap(int width, int height, Camera camera)
    {
        int numBytes = width * height * 4;
        _data = new byte[numBytes];
        Width = width;
        Height = height;
        _camera = camera;
    }

    public Bitmap(SKBitmap bitmap) 
        : this(bitmap, bitmap.Width, bitmap.Height, 0, 0)
    {

    }

    public Bitmap(SKBitmap bitmap, int spriteSize, int col)
        : this(bitmap, spriteSize, spriteSize, 0, col)
    {
        
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
        _data = new byte[numBytes];

        for (int i = 0; i < spriteHeight; i++)
        {
            for (int j = 0; j < spriteWidth; j++)
            {
                int x = (col * spriteWidth) + j;
                int y = (row * spriteHeight) + i;
                SKColor color = bitmap.GetPixel(x, y);

                int index = (i * spriteWidth * 4) + (j * 4);
                _data[index] = color.Red;
                _data[index + 1] = color.Green;
                _data[index + 2] = color.Blue;
                _data[index + 3] = color.Alpha;
            }
        }
    }

    public int ColorAt(int x, int y)
    {
        int index = _GetBytespaceIndex(Width, x, y);
        if (IsOutOfBounds(index))
            return 0x0;
        return RGB.ToInt(_data[index],  _data[index + 1], _data[index + 2]);
    }

    public byte ByteAt(int index)
    {
        if (IsOutOfBounds(index))
            throw new IndexOutOfRangeException();
        return _data[index];
    }
    
    public bool IsOutOfBounds(int index)
    {
        return index < 0 || index >= _data.Length;
    }
    
    protected static int _GetBytespaceIndex(int width, int x, int y)
    {
        return ((y * width) + x) * 4;
    }
}