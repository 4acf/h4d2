namespace H4D2.Infrastructure;

public static class MathHelpers
{
    public static double Distance(double x0, double y0, double x1, double y1)
    {
        double term1 = Math.Pow(x1 - x0, 2);
        double term2 = Math.Pow(y1 - y0, 2);
        return Math.Sqrt(term1 + term2);
    }
    
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

    public static double GaussianRandom(double mean, double standardDeviation)
    {
        double u1 = 1.0 - RandomSingleton.Instance.NextDouble();
        double u2 = 1.0 - RandomSingleton.Instance.NextDouble();
        double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);
        return mean + standardDeviation * randStdNormal;
    }
}