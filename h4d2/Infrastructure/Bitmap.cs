using SkiaSharp;

namespace H4D2.Infrastructure;

public class Bitmap
{
    public readonly byte[] Data;
    public readonly int Width;
    public readonly int Height;

    public Bitmap(int width, int height)
    {
        int numBytes = width * height * 4;
        Data = new byte[numBytes];
        Width = width;
        Height = height;
    }
    
    public Bitmap(SKBitmap bitmap, int spriteSize, int row, int col)
    {
        int numBytes = spriteSize * spriteSize * 4;
        Data = new byte[numBytes];

        for (int i = 0; i < spriteSize; i++)
        {
            for (int j = 0; j < spriteSize; j++)
            {
                int x = (col * spriteSize) + j;
                int y = (row * spriteSize) + i;
                SKColor color = bitmap.GetPixel(x, y);

                int index = (i * spriteSize * 4) + (j * 4);
                Data[index] = color.Red;
                Data[index + 1] = color.Green;
                Data[index + 2] = color.Blue;
                Data[index + 3] = color.Alpha;
            }
        }
        
        Width = spriteSize;
        Height = spriteSize;
    }

    public void Clear()
    {
        for (int i = 0; i < Data.Length; i += 4)
        {
            Data[i] = 0x5c;
            Data[i + 1] = 0x5b;
            Data[i + 2] = 0x56;
            Data[i + 3] = 0xff;
        }
    }

    public void Draw(Bitmap bitmap, int x, int y)
    {
        for (int i = 0; i < bitmap.Height; i++)
        {
            for (int j = 0; j < bitmap.Width; j++)
            {
                int parentIndex = _GetBytespaceIndex(Width, x + j, y - i - 1);
                int childIndex = _GetBytespaceIndex(bitmap.Width, j, i);

                if (parentIndex < 0 || childIndex < 0 || parentIndex >= Data.Length || childIndex >= bitmap.Data.Length)
                {
                    continue;
                }

                if (bitmap.Data[childIndex + 3] == 0)
                {
                    continue;
                }
                
                Data[parentIndex] = bitmap.Data[childIndex];
                Data[parentIndex + 1] = bitmap.Data[childIndex + 1];
                Data[parentIndex + 2] = bitmap.Data[childIndex + 2];
                Data[parentIndex + 3] = bitmap.Data[childIndex + 3];
            }
        }
    }
    
    private int _GetBytespaceIndex(int width, int x, int y)
    {
        return ((y * width) + x) * 4;
    }
}