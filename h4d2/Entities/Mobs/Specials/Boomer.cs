using H4D2.Levels;

namespace H4D2.Entities.Mobs.Specials;

public class Boomer : Special
{
    public Boomer(Level level, int xPosition, int yPosition) 
        : base(level, new BoundingBox(3, 7, 10, 9), 1, 50, 175, xPosition, yPosition)
    {
        
    }
}