using H4D2.Entities.Hazards;
using H4D2.Infrastructure;
using H4D2.Levels;

namespace H4D2.Entities.Mobs.Zombies.Uncommons;

public class Hazmat : Uncommon
{
    public Hazmat(Level level, Position position) 
        : base(level, position, UncommonConfigs.Hazmat)
    {
        
    }
    
    protected override void _Collide(Entity? entity)
    {
        if (entity is Fire)
            return;

        _velocity.Stop();
    }
}