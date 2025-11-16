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
        int degrees = MathHelpers.RadiansToDegrees(directionRadians);
        switch (degrees)
        {
            case >= 315:
            case < 45:
                direction = _e4;
                xFlip = false;
                break;
            case < 135:
                direction = _n4;
                xFlip = false;
                break;
            case < 225:
                direction = _w4;
                xFlip = true;
                break;
            default:
                direction = _s4;
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
            case >= 337.5:
            case < 22.5:
                direction = _e8;
                xFlip = false;
                break;
            case < 67.5:
                direction = _ne8;
                xFlip = false;
                break;
            case < 112.5:
                direction = _n8;
                xFlip = false;
                break;
            case < 157.5:
                direction = _nw8;
                xFlip = true;
                break;
            case < 202.5:
                direction = _w8;
                xFlip = true;
                break;
            case < 247.5:
                direction = _sw8;
                xFlip = true;
                break;
            case < 292.5:
                direction = _s8;
                xFlip = false;
                break;
            default:
                direction = _se8;
                xFlip = false;
                break;
        }
        return new SpriteDirection(direction, xFlip);
    }
}