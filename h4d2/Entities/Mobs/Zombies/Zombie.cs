using H4D2.Levels;

namespace H4D2.Entities.Mobs.Zombies;

public abstract class Zombie : Mob
{
    protected Entity? _target;
    
    protected Zombie(Level level, BoundingBox boundingBox, int health, double speed, int xPosition, int yPosition)
        : base(level, boundingBox, health, speed, xPosition, yPosition)
    {
        _target = null;
    }
}