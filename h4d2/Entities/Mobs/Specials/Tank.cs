using H4D2.Levels;

namespace H4D2.Entities.Mobs.Specials;

public class Tank : Special
{
    public Tank(Level level, int xPosition, int yPosition) 
        : base(level, new BoundingBox(1, 5, 14, 11), 6, 6000, 210, xPosition, yPosition)
    {
        
    }
}