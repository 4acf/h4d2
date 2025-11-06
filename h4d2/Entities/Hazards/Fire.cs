using H4D2.Infrastructure;
using H4D2.Levels;
using H4D2.Particles;

namespace H4D2.Entities.Hazards;

public class Fire : Hazard
{
    public Fire(Level level, Position position)
        : base(level, position, HazardConfigs.Fire)
    {
        var flame = new Flame(_level, _position.Copy());
        _level.AddParticle(flame);
    }
    
    
}