using H4D2.Infrastructure;
using H4D2.Levels;
using H4D2.Particles.Clouds.Cloudlets;

namespace H4D2.Particles.Clouds;

public abstract class Cloud<T> : Particle where T : Cloudlet
{
    protected Cloud(Level level, Position position)
        : base(level, position)
    {
        
    }
}