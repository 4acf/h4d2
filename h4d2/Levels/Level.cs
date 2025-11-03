using H4D2.Entities;
using H4D2.Entities.Mobs.Zombies.Commons;
using H4D2.Entities.Mobs.Survivors;
using H4D2.Entities.Mobs.Zombies;
using H4D2.Entities.Mobs.Zombies.Uncommons;
using H4D2.Entities.Pickups.Consumables;
using H4D2.Entities.Pickups.Throwable;
using H4D2.Entities.Projectiles;
using H4D2.Infrastructure;
using H4D2.Particles;

namespace H4D2.Levels;

public class Level
{
    public const int Padding = 32;
    private const double _levelResetCooldownSeconds = 8.0;
    private const int _minZombies = 20;
    private const int _minSpawnWaveSize = 5;
    private const int _maxSpawnWaveSize = 20;
    
    public readonly int Width;
    public readonly int Height;
    private double _resetSecondsLeft;
    public bool CanReset => _resetSecondsLeft <= 0;
    public bool IsGameOver => GetLivingSurvivors().Count == 0; 
    private List<Entity> _entities;
    private List<Particle> _particles;
    
    public Level(int width, int height)
    {
        Width = width;
        Height = height;
        _resetSecondsLeft = _levelResetCooldownSeconds;
        
        _entities = new List<Entity>();
        _particles = new List<Particle>();
        
        _entities.Add(new Louis   (this, new Position(32, 120)));
        _entities.Add(new Francis (this, new Position(64, 120)));
        _entities.Add(new Zoey    (this, new Position(96, 120)));
        _entities.Add(new Bill    (this, new Position(128, 120)));
        _entities.Add(new Rochelle(this, new Position(160, 120)));
        _entities.Add(new Ellis   (this, new Position(192, 120)));
        _entities.Add(new Nick    (this, new Position(224, 120)));
        _entities.Add(new Coach   (this, new Position(256, 120)));
        
        _entities.Add(new FirstAidKit   (this, new Position(32, 192)));
        _entities.Add(new Pills         (this, new Position(64, 192)));
        _entities.Add(new Adrenaline    (this, new Position(96, 192)));
        
        _entities.Add(new Molotov (this, new Position(128, 192)));
        _entities.Add(new PipeBomb(this, new Position(160, 192)));
        _entities.Add(new BileBomb(this, new Position(192, 192)));
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
    
    public void Update(double elapsedTime)
    {
        if (IsGameOver)
            _resetSecondsLeft -= elapsedTime;
        _UpdateEntities(elapsedTime);
        _UpdateParticles(elapsedTime);
    }
    
    public void Render(Bitmap screen)
    {
        _RenderBackground(screen);
        _RenderShadows(screen);
        _RenderParticles(screen);
        _RenderEntities(screen);
    }
    
    private void _UpdateEntities(double elapsedTime)
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

        _SpawnZombies();
    }

    private void _UpdateParticles(double elapsedTime)
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
    
    private void _RenderBackground(Bitmap screen)
    {
        
    }
    
    private void _RenderShadows(Bitmap screen)
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
    
    private void _RenderEntities(Bitmap screen)
    {
        _entities = _entities.OrderByDescending(e => e.FootPosition.Y).ToList();
        foreach (Entity entity in _entities)
        {
            entity.Render(screen);
        }
    }

    private void _RenderParticles(Bitmap screen)
    {
        foreach (Particle particle in _particles)
        {
            if(!particle.Removed)
                particle.Render(screen);
        }
    }

    // these functions are somewhat temporary until i add real levels
    private void _SpawnZombies()
    {
        List<Zombie> zombies = GetLivingZombies();
        if (zombies.Count < _minZombies)
        {
            int randomNewZombies = 
                RandomSingleton.Instance.Next(_minSpawnWaveSize, _maxSpawnWaveSize);
            for (int i = 0; i < randomNewZombies; i++)
            {
                Zombie zombie = _CreateRandomLevelZombie(_RandomZombieSpawnPosition());
                _entities.Add(zombie);
            }
        }
    }

    private Position _RandomZombieSpawnPosition()
    {
        double x = 0;
        double y = 0;
        int random = RandomSingleton.Instance.Next(2);
        if (random == 0)
        {
            // NS
            x = RandomSingleton.Instance.NextDouble() * Width;
            y = RandomSingleton.Instance.Next(2) == 0 ? 
                Height + Padding - 1 : 
                0;
        }
        else
        {
            // WE
            x = RandomSingleton.Instance.Next(2) == 0 ?
               -Padding + 1 :
               Width;
            y = RandomSingleton.Instance.NextDouble() * Height;
        }

        return new Position(x, y);
    }
    
    private Zombie _CreateRandomLevelZombie(Position position)
    {
        int random = RandomSingleton.Instance.Next(20);
        if (random != 0) 
            return new Common(this, position);
        int randomUncommon = RandomSingleton.Instance.Next(5);
        return randomUncommon switch
        {
            0 => new Hazmat(this, position),
            1 => new Clown(this, position),
            2 => new Mudman(this, position),
            3 => new Worker(this, position),
            4 => new Riot(this, position),
            _ => new Common(this, position)
        };
    }
}