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
            var position = new Position(i * 32, 64);
            _entities.Add(new Common(this, position));
        }
        
        _entities.Add(new Riot  (this, new Position(32, 16)));
        _entities.Add(new Worker(this, new Position(64, 16)));
        _entities.Add(new Mudman(this, new Position(96, 16)));
        _entities.Add(new Clown (this, new Position(128, 16)));
        _entities.Add(new Hazmat(this, new Position(160, 16)));
        
        _entities.Add(new Witch  (this, new Position(32, 196)));
        _entities.Add(new Tank   (this, new Position(64, 196)));
        _entities.Add(new Spitter(this, new Position(96, 196)));
        _entities.Add(new Jockey (this, new Position(128, 196)));
        _entities.Add(new Charger(this, new Position(160, 196)));
        _entities.Add(new Smoker (this, new Position(192, 196)));
        _entities.Add(new Boomer (this, new Position(224, 196)));
        _entities.Add(new Hunter (this, new Position(256, 196)));
        
        _entities.Add(new Louis   (this, new Position(32, 120)));
        _entities.Add(new Francis (this, new Position(64, 120)));
        _entities.Add(new Zoey    (this, new Position(96, 120)));
        _entities.Add(new Bill    (this, new Position(128, 120)));
        _entities.Add(new Rochelle(this, new Position(160, 120)));
        _entities.Add(new Ellis   (this, new Position(192, 120)));
        _entities.Add(new Nick    (this, new Position(224, 120)));
        _entities.Add(new Coach   (this, new Position(256, 120)));
    }
    
    public Entity? GetFirstCollidingEntity(Entity e1, ReadonlyPosition position)
    {
        foreach (Entity e2 in _entities)
        {
            if (e2 != e1 &&
                e1.BoundingBox.CanCollideWith(e2.BoundingBox) &&
                e1.IsIntersecting(e2, position)
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
    
    public Survivor? GetNearestLivingSurvivor(ReadonlyPosition position)
    {
        Survivor? result = null;
        double lowestDistance = double.MaxValue;
        foreach (Survivor survivor in _entities.OfType<Survivor>())
        {
            if (!survivor.IsAlive) continue;
            double distance = ReadonlyPosition.Distance(position, survivor.Position);
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
    
    public Zombie? GetNearestLivingZombie(ReadonlyPosition position)
    {
        Zombie? result = null;
        double lowestDistance = double.MaxValue;
        foreach (Zombie zombie in _entities.OfType<Zombie>())
        {
            if (!zombie.IsAlive) continue;
            double distance = ReadonlyPosition.Distance(position, zombie.Position);
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
        AddParticle(new Explosion(this, grenade.Position.MutableCopy(), Grenade.SplashRadius));
        List<Zombie> zombies = GetLivingZombies();
        foreach (Zombie zombie in zombies)
        {
            double distance = ReadonlyPosition.Distance(grenade.Position, zombie.CenterMass);
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
        _entities = _entities.OrderByDescending(e => e.Position.Y).ToList();
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