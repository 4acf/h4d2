namespace H4D2.Infrastructure;

public class ShadowBitmap
{
    public readonly bool[] Data;
    public readonly int Width;
    public readonly int Height;

    public ShadowBitmap(int width, int height)
    {
        int numBytes = width * height;
        Data = new bool[numBytes];
        Width = width;
        Height = height;
    }

    public void Clear()
    {
        for (int i = 0; i < Data.Length; i++)
        {
            Data[i] = false;
        }
    }

    public void Fill(int x0, int y0, int x1, int y1)
    {
        for (int i = y0; i <= y1; i++)
        {
            for (int j = x0; j <= x1; j++)
            {
                int index = _GetBytespaceIndex(Width, j, i);
                if (_IsOutOfBounds(index)) continue;
                
                int expectedY = y0 + (i - y0);
                int actualY = index / Width;
                if (expectedY != actualY) continue;

                Data[index] = true;
            }
        }
    }

    public void SetPixel(int x, int y)
    {
        Fill(x, y, x, y);
    }
    
    private bool _IsOutOfBounds(int index)
    {
        return index < 0 || index >= Data.Length;
    }
    
    private static int _GetBytespaceIndex(int width, int x, int y)
    {
        return ((y * width) + x);
    }
}