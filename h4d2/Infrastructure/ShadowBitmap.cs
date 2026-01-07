namespace H4D2.Infrastructure;

public class ShadowBitmap
{
    public readonly int Width;
    public readonly int Height;

    private readonly Camera? _camera;
    private readonly bool[] _data;

    
    public ShadowBitmap(int width, int height, Camera? camera = null)
    {
        int numBytes = width * height;
        _data = new bool[numBytes];
        Width = width;
        Height = height;
        _camera = camera;
    }

    public void Clear()
    {
        for (int i = 0; i < _data.Length; i++)
        {
            _data[i] = false;
        }
    }

    public void Fill(int x0, int y0, int x1, int y1)
    {
        if (_camera != null)
        {
            x0 += _camera.XOffset;
            x1 += _camera.XOffset;
            y0 += _camera.YOffset;
            y1 += _camera.YOffset;
        }
        
        for (int i = y0; i <= y1; i++)
        {
            for (int j = x0; j <= x1; j++)
            {
                int index = _GetBytespaceIndex(Width, j, i);
                if (_IsOutOfBounds(index)) continue;
                
                int expectedY = y0 + (i - y0);
                int actualY = index / Width;
                if (expectedY != actualY) continue;

                _data[index] = true;
            }
        }
    }

    public void SetPixel(int x, int y)
    {
        Fill(x, y, x, y);
    }
    
    private bool _IsOutOfBounds(int index)
    {
        return index < 0 || index >= _data.Length;
    }
    
    public bool IsSet(int index)
    {
        if (_IsOutOfBounds(index))
            throw new IndexOutOfRangeException();
        return _data[index];
    }
    
    private static int _GetBytespaceIndex(int width, int x, int y)
    {
        return ((y * width) + x);
    }
}