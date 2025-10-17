using H4D2.Levels;

namespace H4D2.Entities.Mobs.Specials;

public class Smoker : Special
{
    public Smoker(Level level, int xPosition, int yPosition) 
        : base(level, new BoundingBox(true, 6, 4, 6, 12), 2, 250, 210, xPosition, yPosition)
    {
        
    }
}