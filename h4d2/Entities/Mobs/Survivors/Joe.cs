using H4D2.Entities.Projectiles.ThrowableProjectiles;
using H4D2.Infrastructure;
using H4D2.Levels;

namespace H4D2.Entities.Mobs.Survivors;

public class Joe : Survivor
{
    private PipeBombProjectile? _explosionSource;
    
    public Joe(Level level, Position position)
        : base(level, position, SurvivorConfigs.Joe)
    {
        _weapon = null;
    }

    public override void Update(double elapsedTime)
    {
        base.Update(elapsedTime);
        const int expectedUpdatesPerSecond = 60;
        const int expectedUpdatesPerMinute = expectedUpdatesPerSecond * 60;
        if (Probability.OneIn(expectedUpdatesPerMinute))
            _Die();
    }

    protected override void _Die()
    {
        base._Die();
        _explosionSource = new PipeBombProjectile(_level, CenterMass.MutableCopy(), 0.0);
        _level.Explode(_explosionSource);
    }
}