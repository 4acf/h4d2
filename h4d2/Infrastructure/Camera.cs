namespace H4D2.Infrastructure;

public class Camera
{
    public int XOffset { get; private set; }
    public int YOffset { get; private set; }
    
    private bool _hasBounds => 
        (
            _lowerXBound != null &&
            _lowerYBound != null &&
            _upperXBound != null &&
            _upperYBound != null
        );
    private int? _lowerXBound;
    private int? _lowerYBound;
    private int? _upperXBound;
    private int? _upperYBound;
    
    public Camera()
    {
        XOffset = 0;
        YOffset = 0;
        _lowerXBound = null;
        _lowerYBound = null;
        _upperXBound = null;
        _upperYBound = null;
    }
    
    public Camera(int xOffset, int yOffset)
    {
        XOffset = xOffset;
        YOffset = yOffset;
        _lowerXBound = null;
        _lowerYBound = null;
        _upperXBound = null;
        _upperYBound = null;
    }

    public void InitBounds(int x0, int y0, int x1, int y1)
    {
        _lowerXBound = x0;
        _lowerYBound = y0;
        _upperXBound = x1;
        _upperYBound = y1;
    }
    
    public void MoveXY(int x, int y)
    {
        MoveX(x);
        MoveY(y);
    }

    public void MoveX(int x)
    {
        if (!_hasBounds)
            XOffset -= x;
        else
        {
            XOffset -= x;
            if (XOffset < _lowerXBound)
                XOffset = _lowerXBound.Value;
            if (XOffset > _upperXBound)
                XOffset = _upperXBound.Value;
        }
    }

    public void MoveY(int y)
    {
        if (!_hasBounds)
            YOffset -= y;
        else
        {
            YOffset -= y;
            if (YOffset < _lowerYBound)
                YOffset = _lowerYBound.Value;
            if (YOffset > _upperYBound)
                YOffset = _upperYBound.Value;
        }
    }
}