using H4D2.Infrastructure;
using H4D2.Levels;

namespace H4D2.Particles;

public abstract class Particle : Isometric
{
    protected double _xVelocity;
    protected double _yVelocity;
    protected double _zVelocity;
    
    protected Particle(Level level, double xPosition, double yPosition, double zPosition)
        : base(level, xPosition, yPosition, zPosition)
    {
        _xVelocity = 0;
        _yVelocity = 0;
        _zVelocity = 0;
    }
    
    public abstract void Update(double elapsedTime);
}