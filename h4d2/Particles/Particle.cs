using H4D2.Infrastructure;

namespace H4D2.Particles;

public abstract class Particle : Isometric
{
    protected double _xVelocity;
    protected double _yVelocity;
    protected double _zVelocity;
    
    protected Particle(double xPosition, double yPosition, double zPosition)
        : base(xPosition, yPosition, zPosition)
    {
        _xVelocity = 0;
        _yVelocity = 0;
        _zVelocity = 0;
    }
    
    public abstract void Update(double elapsedTime);
}