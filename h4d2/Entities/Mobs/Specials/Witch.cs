using H4D2.Levels;

namespace H4D2.Entities.Mobs.Specials;

public class Witch : Special
{
    public Witch(Level level, int xPosition, int yPosition) 
        : base(level, new BoundingBox(6, 6, 6, 10), 7, 1000, 300, xPosition, yPosition)
    {
        
    }    
}