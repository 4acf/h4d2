using H4D2.Entities;
using H4D2.Entities.Hazards;
using H4D2.Entities.Mobs;
using H4D2.Entities.Mobs.Zombies.Commons;
using H4D2.Entities.Mobs.Survivors;
using H4D2.Entities.Mobs.Zombies;
using H4D2.Entities.Mobs.Zombies.Uncommons;
using H4D2.Entities.Pickups.Consumables;
using H4D2.Entities.Pickups.Throwable;
using H4D2.Entities.Projectiles;
using H4D2.Entities.Projectiles.ThrowableProjectiles;
using H4D2.Infrastructure;
using H4D2.Infrastructure.H4D2;
using H4D2.Particles;
using H4D2.Particles.Clouds;

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
    public readonly CollisionManager<CollisionGroup> CollisionManager;
    private readonly CountdownTimer _levelResetTimer;
    public bool CanReset => _levelResetTimer.IsFinished;
    public bool IsGameOver => GetLivingMobs<Survivor>().Count == 0;
    private List<Entity> _entities;
    private List<Particle> _particles;
    
    public Level(int width, int height, CollisionManager<CollisionGroup> collisionManager)
    {
        Width = width;
        Height = height;
        _levelResetTimer = new CountdownTimer(_levelResetCooldownSeconds);
        
        _entities = new List<Entity>();
        _particles = new List<Particle>();
        CollisionManager = collisionManager;
        
        _entities.Add(new Louis   (this, new Position(32, 120)));
        _entities.Add(new Francis (this, new Position(64, 120)));
        _entities.Add(new Zoey    (this, new Position(96, 120)));
        _entities.Add(new Bill    (this, new Position(128, 120)));
        _entities.Add(new Rochelle(this, new Position(160, 120)));
        _entities.Add(new Ellis   (this, new Position(192, 120)));
        _entities.Add(new Nick    (this, new Position(224, 120)));
        _entities.Add(new Coach   (this, new Position(256, 120)));
        
        _entities.Add(new Molotov(this, new Position(32, 192)));
        _entities.Add(new PipeBomb(this, new Position(64, 192)));
        _entities.Add(new BileBomb(this, new Position(96, 192)));
        _entities.Add(new Molotov(this, new Position(128, 192)));
        _entities.Add(new PipeBomb(this, new Position(160, 192)));
        _entities.Add(new BileBomb(this, new Position(192, 192)));
        _entities.Add(new Molotov(this, new Position(224, 192)));
        _entities.Add(new PipeBomb(this, new Position(256, 192)));
        
        _entities.Add(new FirstAidKit(this, new Position(32, 32)));
        _entities.Add(new Pills(this, new Position(64, 32)));
        _entities.Add(new Adrenaline(this, new Position(96, 32)));
        _entities.Add(new FirstAidKit(this, new Position(128, 32)));
        _entities.Add(new Pills(this, new Position(160, 32)));
        _entities.Add(new Adrenaline(this, new Position(192, 32)));
        _entities.Add(new FirstAidKit(this, new Position(224, 32)));
        _entities.Add(new Pills(this, new Position(256, 32)));
    }
    
    public Entity? GetFirstCollidingEntity(Entity e1, ReadonlyPosition position)
    {
        foreach (Entity e2 in _entities)
        {
            if (e2 != e1 &&
                e1.BoundingBox.CanCollideWith(CollisionManager, e2.BoundingBox) &&
                e1.IsIntersecting(e2, position)
            )
                return e2;
        }
        return null;
    }

    public List<T> GetLivingMobs<T>() where T : Mob
    {
        return _entities
            .OfType<T>()
            .Where(t => t.IsAlive)
            .ToList();
    }

    public T? GetNearestEntity<T>(ReadonlyPosition position, T? exclude = null) where T : Entity
    {
        T? result = null;
        double lowestDistance = double.MaxValue;
        foreach (T t in _entities.OfType<T>())
        {
            if (exclude != null && t == exclude) continue;
            if (t.Removed) continue;
            double distance = ReadonlyPosition.Distance(position, t.Position);
            if (distance < lowestDistance)
            {
                result = t;
                lowestDistance = distance;
            }
        }
        return result;
    }
    
    public void AddProjectile(Projectile projectile)
    {
        _entities.Add(projectile);
    }

    public void AddHazard(Hazard hazard)
    {
        _entities.Add(hazard);
    }
    
    public void AddParticle(Particle particle)
    {
        _particles.Add(particle);
    }
    
    public void Explode(Grenade grenade)
    {
        AddParticle(new Explosion(this, grenade.Position.MutableCopy(), Grenade.SplashRadius));
        List<Zombie> zombies = GetLivingMobs<Zombie>();
        foreach (Zombie zombie in zombies)
        {
            double distance = ReadonlyPosition.Distance(grenade.Position, zombie.CenterMass);
            if (distance <= Grenade.SplashRadius)
            {
                zombie.HitBy(grenade);
            }
        }
    }

    public void Explode(PipeBombProjectile pipeBomb)
    {
        AddParticle(new Explosion(this, pipeBomb.Position.MutableCopy(), PipeBombProjectile.SplashRadius));
        List<Zombie> zombies = GetLivingMobs<Zombie>();
        foreach (Zombie zombie in zombies)
        {
            double distance = ReadonlyPosition.Distance(pipeBomb.Position, zombie.CenterMass);
            if (distance <= PipeBombProjectile.SplashRadius)
            {
                zombie.HitBy(pipeBomb);
            }
        }
    }
    
    public void Update(double elapsedTime)
    {
        if (IsGameOver)
            _levelResetTimer.Update(elapsedTime);
        _UpdateEntities(elapsedTime);
        _UpdateParticles(elapsedTime);
    }
    
    public void Render(Bitmap screen, ShadowBitmap shadows)
    {
        _RenderBackground(screen);
        _RenderShadows(screen, shadows);
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

        _ReplenishZombies();
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
        screen.Fill(0, 0, Width, Height, 0x5c5b56);
    }
    
    private void _RenderShadows(Bitmap screen, ShadowBitmap shadows)
    {
        foreach (Entity entity in _entities)
        {
            entity.RenderShadow(shadows);
        }

        foreach (Particle particle in _particles)
        {
            particle.RenderShadow(shadows);
        }

        screen.DrawShadows(shadows, H4D2Art.ShadowColor, H4D2Art.ShadowBlend);
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
        _particles.Sort(Particle.Comparator);
        foreach (Particle particle in _particles)
        {
            if(!particle.Removed)
                particle.Render(screen);
        }
    }

    public void SpawnZombies()
    {
        int randomNewZombies = 
            RandomSingleton.Instance.Next(_minSpawnWaveSize, _maxSpawnWaveSize);
        for (int i = 0; i < randomNewZombies; i++)
        {
            Zombie zombie = _CreateRandomLevelZombie(_RandomZombieSpawnPosition());
            _entities.Add(zombie);
        }
    }
    
    // these functions are somewhat temporary until i add real levels
    private void _ReplenishZombies()
    {
        List<Zombie> zombies = GetLivingMobs<Zombie>();
        if (zombies.Count < _minZombies)
        {
            SpawnZombies();
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