using H4D2.Infrastructure;
using H4D2.Levels;
using H4D2.Particles.DebrisParticles.Granules;

namespace H4D2.Particles.DebrisParticles.Emitters;

public abstract class Emitter<T> : Debris where T : Granule
{
    protected readonly Func<Level, Position, ReadonlyVelocity, T>
        _factory;
    
    protected Emitter(
        Level level,
        Position position,
        EmitterConfig config,
        Func<Level, Position, ReadonlyVelocity, T> factory
    )
        : base(level, position, config)
    {
        _factory = factory;
    }

    public override void Update(double elapsedTime)
    {
        base.Update(elapsedTime);
        if (Probability.OneIn(2))
            return;
        Granule granule = _factory(_level, _position.Copy(), _velocity.ReadonlyCopy());
        _level.AddParticle(granule);
    }
}