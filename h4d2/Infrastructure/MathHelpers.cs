namespace H4D2.Infrastructure;

public static class MathHelpers
{
    public static int RadiansToDegrees(double radians)
    {
        return (int)(radians * 180 / Math.PI) % 360;
    }

    public static double NormalizeRadians(double radians)
    {
        while (radians < 0) radians += 2 * Math.PI;
        while (radians >= 2 * Math.PI) radians -= 2 * Math.PI;
        return radians;
    }
    
    public static byte ByteLerp(byte a, byte b, double t)
    {
        return (byte)((t * a) + (1 - t) * b);
    }

    public static double Distance(double x0, double y0, double x1, double y1)
    {
        double term1 = Math.Pow(x1 - x0, 2);
        double term2 = Math.Pow(y1 - y0, 2);
        return Math.Sqrt(term1 + term2);
    }

    public static double Distance(double x0, double y0, double z0, double x1, double y1, double z1)
    {
        double term1 = Math.Pow(x1 - x0, 2);
        double term2 = Math.Pow(y1 - y0, 2);
        double term3 = Math.Pow(z1 - z0, 2);
        return Math.Sqrt(term1 + term2 + term3);
    }

    public static double ClampDouble(double value, double min, double max)
    {
        if (value < min) return min;
        if (value > max) return max;
        return value;
    }
}