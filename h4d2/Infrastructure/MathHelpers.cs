namespace H4D2.Infrastructure;

public static class MathHelpers
{
    public static int RadiansToDegrees(double radians)
    {
        return (int)(radians * 180 / Math.PI) % 360;
    }
}