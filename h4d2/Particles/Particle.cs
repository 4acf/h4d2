using H4D2.Infrastructure;
using H4D2.Levels;

namespace H4D2.Particles;

public abstract class Particle : Isometric
{
    protected const double _baseFramerate = 60.0;
    
    protected Particle(Level level, Position position)
        : base(level, position)
    {

    }
    
    public abstract void Update(double elapsedTime);
}