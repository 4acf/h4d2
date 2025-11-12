using H4D2.Infrastructure;
using H4D2.Levels;

namespace H4D2.Entities.Hazards;

public abstract class Hazard : Entity
{
    public readonly int Damage;
    protected readonly CountdownTimer _despawnTimer;
    
    protected Hazard(Level level, Position position, HazardConfig config)
        : base(level, position, config.BoundingBox)
    {
        Damage = config.Damage;
        _despawnTimer = new CountdownTimer(config.Duration);
    }

    public override void Update(double elapsedTime)
    {
        _despawnTimer.Update(elapsedTime);
        if (_despawnTimer.IsFinished)
        {
            Removed = true;
        }
    }
}