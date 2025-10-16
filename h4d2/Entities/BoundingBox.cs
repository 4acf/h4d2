using H4D2.Levels;

namespace H4D2.Entities;

public class BoundingBox
{
    private int _xOffset;
    private int _yOffset;
    private int _width;
    private int _height;
    
    public BoundingBox(int xOffset, int yOffset, int width, int height)
    {
        _xOffset = xOffset;
        _yOffset = yOffset;
        _width = width;
        _height = height;
    }

    public bool IsOutOfLevelBounds(Level level, double xPosition, double yPosition)
    {
        double x0 = xPosition + _xOffset;
        if (x0 < 0) return true;
        
        double y0 = yPosition - _yOffset - _height;
        if(y0 < 0) return true;
        
        double x1 = xPosition + _xOffset + _width;
        if (x1 >= level.Width) return true;
        
        double y1 = yPosition - _yOffset;
        if (y1 >= level.Height) return true;

        return false;
    }
}