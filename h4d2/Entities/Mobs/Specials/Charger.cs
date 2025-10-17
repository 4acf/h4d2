using H4D2.Levels;

namespace H4D2.Entities.Mobs.Specials;

public class Charger : Special
{
    public Charger(Level level, int xPosition, int yPosition) 
        : base(level, new BoundingBox(true, 3, 4, 10, 12), 3, 600, 250, xPosition, yPosition)
    {
        
    }
}