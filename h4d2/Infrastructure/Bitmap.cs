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
        for (int i = 0; i < Data.Length; i++)
        {
            Data[i] = 0;
        }
    }

    public void Draw(Bitmap bitmap, int x, int y, bool flip = false)
    {
        for (int i = 0; i < bitmap.Height; i++)
        {
            for (int j = 0; j < bitmap.Width; j++)
            {
                int k = j;
                if (flip)
                {
                    k = bitmap.Width - j - 1;
                }
                
                int parentIndex = _GetBytespaceIndex(Width, x + j, y - i - 1);
                int childIndex = _GetBytespaceIndex(bitmap.Width, k, i);
                
                if (IsOutOfBounds(parentIndex) || bitmap.IsOutOfBounds(childIndex)) continue;
                if (bitmap.Data[childIndex + 3] == 0) continue;

                int expectedY = y - i - 1;
                int actualY = parentIndex / (Width * 4);
                if (expectedY != actualY) continue;
                
                Data[parentIndex] = bitmap.Data[childIndex];
                Data[parentIndex + 1] = bitmap.Data[childIndex + 1];
                Data[parentIndex + 2] = bitmap.Data[childIndex + 2];
                Data[parentIndex + 3] = bitmap.Data[childIndex + 3];
            }
        }
    }

    public void DrawShadows(ShadowBitmap shadows)
    {
        if (Width != shadows.Width || Height != shadows.Height)
            throw new ArgumentException("Shadow bitmap dimensions do not match base bitmap dimensions.");

        for (int i = 0; i < Data.Length; i += 4)
        {
            int j = i / 4;
            bool set = shadows.Data[j];
            if (!set) continue;
            Data[i] = MathHelpers.ByteLerp(Data[i], Art.ShadowColor, Art.ShadowBlend);
            Data[i + 1] = MathHelpers.ByteLerp(Data[i + 1], Art.ShadowColor, Art.ShadowBlend);
            Data[i + 2] = MathHelpers.ByteLerp(Data[i + 2], Art.ShadowColor, Art.ShadowBlend);
            Data[i + 3] = 0xff;
        }
    }
    
    public void Fill(int x0, int y0, int x1, int y1, int color)
    {
        FillBlend(x0, y0, x1, y1, color, 0);
    }
    
    public void FillBlend(int x0, int y0, int x1, int y1, int color, double blend)
    {
        byte r = (byte)(color >> 16 & 0xff);
        byte g = (byte)(color >> 8 & 0xff);
        byte b = (byte)(color & 0xff);

        for (int i = y0; i <= y1; i++)
        {
            for (int j = x0; j <= x1; j++)
            {
                int index = _GetBytespaceIndex(Width, j, i);
                if (IsOutOfBounds(index)) continue;
                
                int expectedY = y0 + (i - y0);
                int actualY = index / (Width * 4);
                if (expectedY != actualY) continue;
                
                Data[index] = MathHelpers.ByteLerp(Data[index], r, blend);
                Data[index + 1] = MathHelpers.ByteLerp(Data[index + 1], g, blend);
                Data[index + 2] = MathHelpers.ByteLerp(Data[index + 2], b, blend);
                Data[index + 3] = 0xff;
            }
        }
    }
    
    public void SetPixel(int x, int y, int color)
    {
        SetPixelBlend(x, y, color, 0);
    }

    public void SetPixelBlend(int x, int y, int color, double blend)
    {
        FillBlend(x, y, x, y, color, blend);
    }
    
    public bool IsOutOfBounds(int index)
    {
        return index < 0 || index >= Data.Length;
    }
    
    private static int _GetBytespaceIndex(int width, int x, int y)
    {
        return ((y * width) + x) * 4;
    }
    
}