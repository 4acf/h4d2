namespace H4D2.Infrastructure;

public class Position
{
    public double X;
    public double Y;
    public double Z;

    public Position(double x, double y, double z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public Position(double x, double y)
    {
        X = x;
        Y = y;
        Z = 0;
    }

    private Position(Position position, double dx, double dy, double dz)
    {
        X = position.X + dx;
        Y = position.Y + dy;
        Z = position.Z + dz;
    }

    public Position Copy() => new(this, 0, 0, 0);
    public Position CopyAndTranslate(double dx, double dy, double dz) => new(this, dx, dy, dz);
    public ReadonlyPosition ReadonlyCopy() => new(this);
}

public readonly struct ReadonlyPosition
{
    public readonly double X;
    public readonly double Y;
    public readonly double Z;

    public ReadonlyPosition(double x, double y, double z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public ReadonlyPosition(double x, double y)
    {
        X = x;
        Y = y;
        Z = 0;
    }
    
    public ReadonlyPosition(Position position)
    {
        X = position.X;
        Y = position.Y;
        Z = position.Z;
    }
    
    public Position MutableCopy() => new(X, Y, Z);

    public static double Distance(ReadonlyPosition p1, ReadonlyPosition p2)
    {
        double term1 = Math.Pow(p2.X - p1.X, 2);
        double term2 = Math.Pow(p2.Y - p1.Y, 2);
        double term3 = Math.Pow(p2.Z - p1.Z, 2);
        return Math.Sqrt(term1 + term2 + term3);
    }
}