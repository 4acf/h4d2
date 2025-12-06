namespace H4D2.Infrastructure;

public readonly struct SpriteDirection
{
    public readonly int Offset;
    public readonly bool XFlip;

    public SpriteDirection(int offset, bool xFlip)
    {
        Offset = offset;
        XFlip = xFlip;
    }
}

public static class Direction
{
    private const int _e4 = 1;
    private const int _n4 = 2;
    private const int _w4 = 1;
    private const int _s4 = 0;

    private const int _e8 = 2;
    private const int _ne8 = 3;
    private const int _n8 = 4;
    private const int _nw8 = 3;
    private const int _w8 = 2;
    private const int _sw8 = 1;
    private const int _s8 = 0;
    private const int _se8 = 1;

    public static SpriteDirection Cardinal(double directionRadians)
    {
        int direction = 0;
        bool xFlip = false;
        double degrees = MathHelpers.RadiansToDegrees(directionRadians);
        switch (degrees)
        {
            case < 90:
                direction = _n4;
                xFlip = false;
                break;
            case < 180:
                direction = _w4;
                xFlip = true;
                break;
            case < 270:
                direction = _s4;
                xFlip = false;
                break;
            default:
                direction = _e4;
                xFlip = false;
                break;
        }
        return new SpriteDirection(direction, xFlip);
    }

    public static SpriteDirection Intercardinal(double directionRadians)
    {
        int direction = 0;
        bool xFlip = false;
        double degrees = MathHelpers.RadiansToDegrees(directionRadians);
        switch (degrees)
        {
            case < 45:
                direction = _ne8;
                xFlip = false;
                break;
            case < 90:
                direction = _n8;
                xFlip = false;
                break;
            case < 135:
                direction = _nw8;
                xFlip = true;
                break;
            case < 180:
                direction = _w8;
                xFlip = true;
                break;
            case < 225:
                direction = _sw8;
                xFlip = true;
                break;
            case < 270:
                direction = _s8;
                xFlip = false;
                break;
            case < 315:
                direction = _se8;
                xFlip = false;
                break;
            default:
                direction = _e8;
                xFlip = false;
                break;
        }
        return new SpriteDirection(direction, xFlip);
    }
}