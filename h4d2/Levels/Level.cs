using H4D2.Entities;
using H4D2.Entities.Mobs.Zombies.Commons;
using H4D2.Entities.Mobs.Zombies.Specials;
using H4D2.Entities.Mobs.Survivors;
using H4D2.Entities.Mobs.Zombies.Uncommons;
using H4D2.Infrastructure;

namespace H4D2.Levels;

public class Level
{
    public readonly int Width;
    public readonly int Height;
    private List<Entity> _entities;
    
    public Level(int width, int height)
    {
        Width = width;
        Height = height;
        
        _entities = new List<Entity>();
        
        for (int i = 0; i < 10; i++)
        {
            _entities.Add(new Common(this, i * 32, 64));
        }
        
        _entities.Add(new Riot  (this, 0, 128));
        _entities.Add(new Worker(this, 32, 128));
        _entities.Add(new Mudman(this, 64, 128));
        _entities.Add(new Clown (this, 96, 128));
        _entities.Add(new Hazmat(this, 128, 128));
        
        _entities.Add(new Witch  (this, 0, 196));
        _entities.Add(new Tank   (this, 32, 196));
        _entities.Add(new Spitter(this, 64, 196));
        _entities.Add(new Jockey (this, 96, 196));
        _entities.Add(new Charger(this, 128, 196));
        _entities.Add(new Smoker (this, 160, 196));
        _entities.Add(new Boomer (this, 192, 196));
        _entities.Add(new Hunter (this, 224, 196));
        
        _entities.Add(new Louis   (this, 0, 16));
        _entities.Add(new Francis (this, 32, 16));
        _entities.Add(new Zoey    (this, 64, 16));
        _entities.Add(new Bill    (this, 96, 16));
        _entities.Add(new Rochelle(this, 128, 16));
        _entities.Add(new Ellis   (this, 160, 16));
        _entities.Add(new Nick    (this, 192, 16));
        _entities.Add(new Coach   (this, 224, 16));
    }
    
    public bool ContainsBlockingEntity(Entity e1, double xPosition, double yPosition)
    {
        foreach (Entity e2 in _entities)
        {
            if (e2 != e1 &&
                e2.BoundingBox.IsBlocking &&
                e1.IsIntersecting(e2, xPosition, yPosition)
            )
                return true;
        }
        return false;
    }

    public Entity? GetNearestHealthySurvivor(double xPosition, double yPosition)
    {
        Entity? result = null;
        double lowestDistance = double.MaxValue;
        foreach (Survivor survivor in _entities.OfType<Survivor>())
        {
            if (survivor.Health <= 0) continue;
            double distance = MathHelpers.Distance(xPosition, yPosition, survivor.XPosition, survivor.YPosition);
            if (distance < lowestDistance)
            {
                result = survivor;
                lowestDistance = distance;
            }
        }
        return result;
    }
    
    public void UpdateEntities(double elapsedTime)
    {
        foreach (Entity entity in _entities)
        {
            entity.Update(elapsedTime);
        }
    }
    
    public void RenderBackground(Bitmap screen)
    {
        
    }
    
    public void RenderShadows(Bitmap screen)
    {
        foreach (Entity entity in _entities)
        {
            entity.RenderShadow(screen);
        }
    }
    
    public void RenderEntities(Bitmap screen)
    {
        foreach (Entity entity in _entities)
        {
            entity.Render(screen);
        }
    }
}