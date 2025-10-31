using H4D2.Infrastructure;
using H4D2.Levels;
using H4D2.Weapons;

namespace H4D2.Entities.Mobs.Survivors;
using Cfg = SurvivorConfig;
public class Louis : Survivor
{
    public Louis(Level level, Position position) 
        : base(level, position, Cfg.Louis, Cfg.BlackSkinColor)
    {
        _weapon = new Uzi(level, this);
    }    
}