using H4D2.Levels;

namespace H4D2.Entities.Mobs.Specials;

public class Spitter : Special
{
    public Spitter(Level level, int xPosition, int yPosition) 
        : base(level, new BoundingBox(true, 6, 4, 6, 12), 5, 100, 210, xPosition, yPosition)
    {
        
    }    
}