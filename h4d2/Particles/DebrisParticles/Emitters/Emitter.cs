using H4D2.Infrastructure;
using H4D2.Levels;
using H4D2.Particles.DebrisParticles.Granules;

namespace H4D2.Particles.DebrisParticles.Emitters;

public abstract class Emitter<T> : Debris where T : Granule
{
    protected readonly Func<Level, Position, double, double, double, T>
        _factory;
    
    protected Emitter(
        Level level,
        Position position,
        EmitterConfig config,
        Func<Level, Position, double, double, double, T> factory
    )
        : base(level, position, config)
    {
        _factory = factory;
    }

    public override void Update(double elapsedTime)
    {
        base.Update(elapsedTime);
        if (RandomSingleton.Instance.Next(2) != 0)
            return;
        Granule granule = _factory(_level, _position.Copy(), _xVelocity, _yVelocity, _zVelocity);
        _level.AddParticle(granule);
    }
}