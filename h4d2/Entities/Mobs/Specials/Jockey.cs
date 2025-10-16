using H4D2.Levels;

namespace H4D2.Entities.Mobs.Specials;

public class Jockey : Special
{
    public Jockey(Level level, int xPosition, int yPosition) 
        : base(level, new BoundingBox(6, 11, 6, 5), 4, 325, 250, xPosition, yPosition)
    {
        
    }    
}