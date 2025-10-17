using H4D2.Levels;

namespace H4D2.Entities.Mobs.Specials;

public class Hunter : Special
{
    public Hunter(Level level, int xPosition, int yPosition) 
        : base(level, new BoundingBox(true, 4, 7, 8, 9), 0, 250, 250, xPosition, yPosition)
    {
        
    }    
}