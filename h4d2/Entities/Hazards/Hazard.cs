using H4D2.Infrastructure;
using H4D2.Levels;

namespace H4D2.Entities.Hazards;

public class Hazard : Entity
{
    public readonly int Damage;
    protected double _timeToLiveSeconds;
    
    protected Hazard(Level level, Position position, HazardConfig config)
        : base(level, position, config.BoundingBox)
    {
        Damage = config.Damage;
        _timeToLiveSeconds = config.TimeToLiveSeconds;
    }

    public override void Update(double elapsedTime)
    {
        _timeToLiveSeconds -= elapsedTime;
        if (_timeToLiveSeconds <= 0)
        {
            Removed = true;
        }
    }
}