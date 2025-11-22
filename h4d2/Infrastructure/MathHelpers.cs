namespace H4D2.Infrastructure;

public static class MathHelpers
{
    public static double RadiansToDegrees(double radians)
    {
        return (radians * 180 / Math.PI) % 360;
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

    public static double ClampDouble(double value, double min, double max)
    {
        if (value < min) return min;
        if (value > max) return max;
        return value;
    }

    public static double GaussianRandom(double mean, double standardDeviation)
    {
        double u1 = 1.0 - RandomSingleton.Instance.NextDouble();
        double u2 = 1.0 - RandomSingleton.Instance.NextDouble();
        double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);
        return mean + standardDeviation * randStdNormal;
    }
}