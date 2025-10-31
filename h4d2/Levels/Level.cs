using H4D2.Entities;
using H4D2.Entities.Mobs.Zombies.Commons;
using H4D2.Entities.Mobs.Zombies.Specials;
using H4D2.Entities.Mobs.Survivors;
using H4D2.Entities.Mobs.Zombies;
using H4D2.Entities.Mobs.Zombies.Uncommons;
using H4D2.Entities.Projectiles;
using H4D2.Infrastructure;
using H4D2.Particles;

namespace H4D2.Levels;

public class Level
{
    public readonly int Width;
    public readonly int Height;
    private List<Entity> _entities;
    private List<Particle> _particles;
    
    public Level(int width, int height)
    {
        Width = width;
        Height = height;
        
        _entities = new List<Entity>();
        _particles = new List<Particle>();
        
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
    
    public Entity? GetFirstCollidingEntity(Entity e1, double xPosition, double yPosition, double zPosition)
    {
        foreach (Entity e2 in _entities)
        {
            if (e2 != e1 &&
                e1.BoundingBox.CanCollideWith(e2.BoundingBox) &&
                e1.IsIntersecting(e2, xPosition, yPosition, zPosition)
            )
                return e2;
        }
        return null;
    }

    public List<Survivor> GetLivingSurvivors()
    {
        return _entities
            .OfType<Survivor>()
            .Where(s => s.IsAlive)
            .ToList();
    }
    
    public Survivor? GetNearestLivingSurvivor(double xPosition, double yPosition)
    {
        Survivor? result = null;
        double lowestDistance = double.MaxValue;
        foreach (Survivor survivor in _entities.OfType<Survivor>())
        {
            if (!survivor.IsAlive) continue;
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
            .Where(z => z.IsAlive)
            .ToList();
    }
    
    public Zombie? GetNearestLivingZombie(double xPosition, double yPosition)
    {
        Zombie? result = null;
        double lowestDistance = double.MaxValue;
        foreach (Zombie zombie in _entities.OfType<Zombie>())
        {
            if (!zombie.IsAlive) continue;
            double distance = MathHelpers.Distance(xPosition, yPosition, zombie.XPosition, zombie.YPosition);
            if (distance < lowestDistance)
            {
                result = zombie;
                lowestDistance = distance;
            }
        }
        return result;
    }

    public void AddProjectile(Projectile projectile)
    {
        _entities.Add(projectile);
    }

    public void AddParticle(Particle particle)
    {
        _particles.Add(particle);
    }

    public void Explode(Grenade grenade)
    {
        AddParticle(new Explosion(this, grenade.XPosition, grenade.YPosition, grenade.ZPosition, Grenade.SplashRadius));
        List<Zombie> zombies = GetLivingZombies();
        foreach (Zombie zombie in zombies)
        {
            (double zombieXPosition, double zombieYPosition, double zombieZPosition) = zombie.CenterMass;
            double distance = MathHelpers.Distance(grenade.XPosition, grenade.YPosition, grenade.ZPosition, zombieXPosition, zombieYPosition, zombieZPosition);
            if (distance <= Grenade.SplashRadius)
            {
                zombie.HitBy(grenade);
            }
        }
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

    public void UpdateParticles(double elapsedTime)
    {
        var indexesToRemove = new List<int>();
        for (int i = 0; i < _particles.Count; i++)
        {
            if (_particles[i].Removed)
            {
                indexesToRemove.Add(i);
            }
            else
            {
                _particles[i].Update(elapsedTime);
            }
        }

        for (int i = indexesToRemove.Count - 1; i >= 0; i--)
        {
            _particles.RemoveAt(indexesToRemove[i]);
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

        foreach (Particle particle in _particles)
        {
            particle.RenderShadow(screen);
        }
    }
    
    public void RenderEntities(Bitmap screen)
    {
        _entities = _entities.OrderByDescending(e => e.YPosition).ToList();
        foreach (Entity entity in _entities)
        {
            entity.Render(screen);
        }
    }

    public void RenderParticles(Bitmap screen)
    {
        foreach (Particle particle in _particles)
        {
            if(!particle.Removed)
                particle.Render(screen);
        }
    }
}