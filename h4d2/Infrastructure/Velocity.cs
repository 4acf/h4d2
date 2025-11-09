namespace H4D2.Infrastructure;

public class Velocity
{
    public double X;
    public double Y;
    public double Z;
    public double HypotenuseSquared => (X * X) + (Y * Y) + (Z * Z);

    public Velocity(double x, double y, double z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public Velocity()
    {
        X = 0;
        Y = 0;
        Z = 0;
    }

    public void Stop()
    {
        X = 0;
        Y = 0;
        Z = 0;
    }
    
    public ReadonlyVelocity ReadonlyCopy() => new(this);
}

public readonly struct ReadonlyVelocity
{
    public readonly double X;
    public readonly double Y;
    public readonly double Z;

    public ReadonlyVelocity(Velocity velocity)
    {
        X = velocity.X;
        Y = velocity.Y;
        Z = velocity.Z;
    }
    
    public ReadonlyVelocity()
    {
        X = 0;
        Y = 0;
        Z = 0;
    }
}