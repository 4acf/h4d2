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
    private static readonly SpriteDirection[] _cardinalDirections =
    [
        new (2, false), // N
        new (1, true),  // W
        new (0, false), // S
        new (1, false)  // E
    ];

    private static readonly SpriteDirection[] _intercardinalDirections =
    [
        new (3, false),  // NE
        new (4, false),  // N
        new (3, true),   // NW
        new (2, true),   // W
        new (1, true),   // SW
        new (0, false),  // S
        new (1, false),  // SE
        new (2, false)   // E
    ];
    
    public static SpriteDirection Cardinal(double directionRadians)
    {
        double degrees = MathHelpers.RadiansToDegrees(directionRadians);
        int index = (int)(degrees / 90);
        return _cardinalDirections[index];
    }

    public static SpriteDirection Intercardinal(double directionRadians)
    {
        double degrees = MathHelpers.RadiansToDegrees(directionRadians);
        int index = (int)((degrees + 22.5) / 45) % 8;
        return _intercardinalDirections[index];
    }
}