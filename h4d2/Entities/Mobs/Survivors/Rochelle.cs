using H4D2.Levels;
using H4D2.Weapons;

namespace H4D2.Entities.Mobs.Survivors;
using Cfg = SurvivorConfig;

public class Rochelle : Survivor
{
    public Rochelle(Level level, int xPosition, int yPosition) 
        : base(level, Cfg.Rochelle, xPosition, yPosition, Cfg.BlackSkinColor)
    {
        _weapon = new Uzi(level, this);
    }
}