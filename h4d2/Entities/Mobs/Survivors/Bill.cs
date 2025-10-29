using H4D2.Levels;
using H4D2.Weapons;

namespace H4D2.Entities.Mobs.Survivors;
using Cfg = SurvivorConfig;

public class Bill : Survivor
{
    public Bill(Level level, int xPosition, int yPosition) 
        : base(level,Cfg.Bill, xPosition, yPosition, Cfg.WhiteSkinColor)
    {
        _weapon = new M16(level, this);
    }
}