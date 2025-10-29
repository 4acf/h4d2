using H4D2.Levels;
using H4D2.Weapons;

namespace H4D2.Entities.Mobs.Survivors;
using Cfg = SurvivorConfig;

public class Nick : Survivor
{
    public Nick(Level level, int xPosition, int yPosition) 
        : base(level, Cfg.Nick, xPosition, yPosition, Cfg.WhiteSkinColor)
    {
        _weapon = new Deagle(level, this);
    }    
}