using H4D2.Infrastructure;
using H4D2.Levels;
using H4D2.Weapons;

namespace H4D2.Entities.Mobs.Survivors;
using Cfg = SurvivorConfig;

public class Bill : Survivor
{
    public Bill(Level level, Position position) 
        : base(level, position, Cfg.Bill, Cfg.WhiteSkinColor)
    {
        _weapon = new M16(level, this);
    }
}