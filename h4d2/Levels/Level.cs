using H4D2.Entities;
using H4D2.Entities.Mobs.Zombies.Commons;
using H4D2.Entities.Mobs.Zombies.Specials;
using H4D2.Entities.Mobs.Survivors;
using H4D2.Entities.Mobs.Zombies;
using H4D2.Entities.Mobs.Zombies.Uncommons;
using H4D2.Entities.Projectiles;
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
        
        _entities.Add(new Riot  (this, 32, 16));
        _entities.Add(new Worker(this, 64, 16));
        _entities.Add(new Mudman(this, 96, 16));
        _entities.Add(new Clown (this, 128, 16));
        _entities.Add(new Hazmat(this, 160, 16));
        
        _entities.Add(new Witch  (this, 32, 196));
        _entities.Add(new Tank   (this, 64, 196));
        _entities.Add(new Spitter(this, 96, 196));
        _entities.Add(new Jockey (this, 128, 196));
        _entities.Add(new Charger(this, 160, 196));
        _entities.Add(new Smoker (this, 192, 196));
        _entities.Add(new Boomer (this, 224, 196));
        _entities.Add(new Hunter (this, 256, 196));
        
        _entities.Add(new Louis   (this, 32, 120));
        _entities.Add(new Francis (this, 64, 120));
        _entities.Add(new Zoey    (this, 96, 120));
        _entities.Add(new Bill    (this, 128, 120));
        _entities.Add(new Rochelle(this, 160, 120));
        _entities.Add(new Ellis   (this, 192, 120));
        _entities.Add(new Nick    (this, 224, 120));
        _entities.Add(new Coach   (this, 256, 120));
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

    public List<Survivor> GetLivingSurvivors()
    {
        return _entities
            .OfType<Survivor>()
            .Where(s => s.Health > 0)
            .ToList();
    }
    
    public Survivor? GetNearestLivingSurvivor(double xPosition, double yPosition)
    {
        Survivor? result = null;
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

    public List<Zombie> GetLivingZombies()
    {
        return _entities
            .OfType<Zombie>()
            .Where(z => z.Health > 0)
            .ToList();
    }
    
    public Zombie? GetNearestLivingZombie(double xPosition, double yPosition)
    {
        Zombie? result = null;
        double lowestDistance = double.MaxValue;
        foreach (Zombie zombie in _entities.OfType<Zombie>())
        {
            if (zombie.Health <= 0) continue;
            double distance = MathHelpers.Distance(xPosition, yPosition, zombie.XPosition, zombie.YPosition);
            if (distance < lowestDistance)
            {
                result = zombie;
                lowestDistance = distance;
            }
        }
        return result;
    }

    public void AddBullet(Bullet bullet)
    {
        _entities.Add(bullet);
    }
    
    public void UpdateEntities(double elapsedTime)
    {
        var indexesToRemove = new List<int>();
        for (int i = 0; i < _entities.Count; i++)
        {
            if (_entities[i].Removed)
            {
                indexesToRemove.Add(i);
            }
            else
            {
                _entities[i].Update(elapsedTime);
            }
        }

        for (int i = indexesToRemove.Count - 1; i >= 0; i--)
        {
            _entities.RemoveAt(indexesToRemove[i]);
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