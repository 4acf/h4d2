using H4D2.Entities;
using H4D2.Entities.Hazards;
using H4D2.Entities.Mobs;
using H4D2.Entities.Mobs.Zombies.Commons;
using H4D2.Entities.Mobs.Survivors;
using H4D2.Entities.Mobs.Zombies;
using H4D2.Entities.Mobs.Zombies.Specials;
using H4D2.Entities.Mobs.Zombies.Specials.Pinners;
using H4D2.Entities.Mobs.Zombies.Uncommons;
using H4D2.Entities.Pickups.Consumables;
using H4D2.Entities.Pickups.Throwable;
using H4D2.Entities.Projectiles;
using H4D2.Entities.Projectiles.ThrowableProjectiles;
using H4D2.Infrastructure;
using H4D2.Infrastructure.H4D2;
using H4D2.Levels.LevelElements;
using H4D2.Particles;
using H4D2.Particles.Clouds;

namespace H4D2.Levels;

public enum Tile
{
    Floor,
    Wall,
    ZombieWall,
    SurvivorFloor
}

public class Level
{
    public const int Padding = 0;
    private const double _levelResetCooldownSeconds = 8.0;
    private const int _minZombiesAlive = 20;
    private const int _minSpawnWaveSize = 5;
    private const int _maxSpawnWaveSize = 15;
    private const int _maxZombiesAlive = 50;
    private const int _maxParticles = 5000;
    
    private const int _wallColor = 0x0;
    private const int _floorColor = 0xffffff;
    private const int _healthPickupColor = 0xff0000;
    private const int _zombieSpawnColor = 0x00ff00;
    private const int _throwablePickupColor = 0x0000ff;
    private const int _survivorSpawnColor = 0xff00ff;
    
    public readonly int Width;
    public readonly int Height;
    public readonly CollisionManager<CollisionGroup> CollisionManager;
    private readonly CountdownTimer _levelResetTimer;
    public bool CanReset => _levelResetTimer.IsFinished;
    public bool IsGameOver => GetLivingMobs<Survivor>().Count == 0;
    private readonly List<Entity> _entities;
    private readonly List<Particle> _particles;
    private readonly List<LevelElement> _levelElements;
    private readonly Tile[] _tiles;
    
    public Level(Bitmap levelBitmap, CollisionManager<CollisionGroup> collisionManager, Camera camera)
    {
        _levelResetTimer = new CountdownTimer(_levelResetCooldownSeconds);
        
        _entities = [];
        _particles = [];
        _levelElements = [];
        CollisionManager = collisionManager;
        
        Width = levelBitmap.Width;
        Height = levelBitmap.Height;
        _tiles = new Tile[Width * Height];
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                if (x < 0 || y < 0 || x >= levelBitmap.Width || y >= levelBitmap.Height)
                {
                    _tiles[(y * Width) + x] = Tile.Wall;
                    continue;
                }

                int color = levelBitmap.ColorAt(x, y);
                int tileIndex = (y * Width) + x; 
                switch (color)
                {
                    case _wallColor:
                        _tiles[tileIndex] = Tile.Wall;
                        break;
                    case _zombieSpawnColor:
                        _tiles[tileIndex] = Tile.ZombieWall;
                        break;
                    case _survivorSpawnColor:
                        _tiles[tileIndex] = Tile.SurvivorFloor;
                        // temporary hardcoded values
                        camera.MoveX(-((320 / 2) - (H4D2Art.TileSize / 2)) + ((x + y) * (H4D2Art.TileSize / 2)));
                        camera.MoveY(-H4D2Art.TileCenterOffset);
                        camera.MoveY(((x - y) * H4D2Art.TileIsoHalfHeight) - (240 / 2));
                        break;
                    default:
                        _tiles[tileIndex] = Tile.Floor;
                        break;
                }
            }
        }

        _entities.Add(new Coach(this, new Position(0, 0)));
    }
    
    public Entity? GetFirstCollidingEntity(Entity e1, ReadonlyPosition position, Entity? exclude)
    {
        foreach (Entity e2 in _entities)
        {
            if (e2 == exclude)
                continue;
            
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

    private Survivor? _GetNearestSurvivor(ReadonlyPosition position, Func<Survivor, bool> predicate)
    {
        Survivor? result = null;
        double lowestDistance = double.MaxValue;
        foreach (Survivor survivor in _entities.OfType<Survivor>())
        {
            if (survivor.Removed || !survivor.IsAlive || !predicate(survivor)) continue;
            double distance = ReadonlyPosition.Distance(position, survivor.Position);
            if (distance < lowestDistance)
            {
                result = survivor;
                lowestDistance = distance;
            }
        }
        return result;
    }

    public Survivor? GetNearestBiledSurvivor(ReadonlyPosition position)
        => _GetNearestSurvivor(position, s => s.IsBiled);
    
    public Survivor? GetNearestUnpinnedSurvivor(ReadonlyPosition position)
        => _GetNearestSurvivor(position, s => !s.IsPinned);
    
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
        if(_particles.Count < _maxParticles)
            _particles.Add(particle);
    }

    public void AddSpecial(Special special)
    {
        _entities.Add(special);
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

    public void ExplodeBile(Boomer boomer, double radius)
    {
        List<Survivor> survivors = GetLivingMobs<Survivor>();
        foreach (Survivor survivor in survivors)
        {
            double distance = ReadonlyPosition.Distance(boomer.CenterMass, survivor.CenterMass);
            if (distance <= radius && !survivor.IsBiled)
            {
                survivor.Biled();
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
        _entities.Sort(Comparators.EntityUpdating);
        var indicesToRemove = new List<int>();
        for (int i = 0; i < _entities.Count; i++)
        {
            if (_entities[i].Removed)
            {
                indicesToRemove.Add(i);
            }
            else
            {
                _entities[i].Update(elapsedTime);
            }
        }

        for (int i = indicesToRemove.Count - 1; i >= 0; i--)
        {
            _entities.RemoveAt(indicesToRemove[i]);
        }

        //_ReplenishZombies();
    }

    private void _UpdateParticles(double elapsedTime)
    {
        var indicesToRemove = new List<int>();
        for (int i = 0; i < _particles.Count; i++)
        {
            if (_particles[i].Removed)
            {
                indicesToRemove.Add(i);
            }
            else
            {
                _particles[i].Update(elapsedTime);
            }
        }

        for (int i = indicesToRemove.Count - 1; i >= 0; i--)
        {
            _particles.RemoveAt(indicesToRemove[i]);
        }
    }
    
    private void _RenderBackground(Bitmap screen)
    {
        //screen.Clear(0x2b2b2b);
        screen.Clear(0x312422);
        _levelElements.Clear();
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                int index = (y * Width) + x;
                int xScreenPos = (y * 12) + (x * 12);
                int yScreenPos = (y * -6) + (x * 6);
                
                Tile tile = _tiles[index];
                switch (tile)
                {
                    case Tile.SurvivorFloor:
                        screen.Draw(H4D2Art.Floors[2], xScreenPos, yScreenPos);
                        break;
                    case Tile.Wall:
                        _levelElements.Add(new Wall(this, new Position(0, 0)));
                        screen.Draw(H4D2Art.Floors[2], xScreenPos, yScreenPos);
                        break;
                    case Tile.ZombieWall:
                        _levelElements.Add(new ZombieWall(this, new Position(0, 0)));
                        screen.Draw(H4D2Art.Floors[2], xScreenPos, yScreenPos);
                        break;
                    case Tile.Floor:
                    default:
                    {
                        Bitmap floorBitmap = (x + y) % 2 == 0 ? 
                            H4D2Art.Floors[0] :
                            H4D2Art.Floors[1];
                        screen.Draw(floorBitmap, xScreenPos, yScreenPos);
                        break;
                    }
                }
            }
        }
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
        _entities.Sort(Comparators.EntityRendering);
        foreach (Entity entity in _entities)
        {
            entity.Render(screen);
        }
    }

    private void _RenderParticles(Bitmap screen)
    {
        _particles.Sort(Comparators.Particle);
        foreach (Particle particle in _particles)
        {
            if(!particle.Removed)
                particle.Render(screen);
        }
    }

    private void _RenderLevelElements(Bitmap screen)
    {
        foreach (LevelElement levelElement in _levelElements)
        {
            levelElement.Render(screen);
        }
    }
    
    public void SpawnZombies()
    {
        int randomNewZombies = 
            RandomSingleton.Instance.Next(_minSpawnWaveSize, _maxSpawnWaveSize);
        randomNewZombies = Math.Min(randomNewZombies, _maxZombiesAlive);
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
        if (zombies.Count < _minZombiesAlive)
        {
            SpawnZombies();
        }
    }

    private Position _RandomZombieSpawnPosition()
    {
        double x = 0;
        double y = 0;
        if (Probability.OneIn(2))
        {
            // NS
            x = RandomSingleton.Instance.NextDouble() * Width;
            y = Probability.OneIn(2) ? 
                Height + Padding - 1 : 
                0;
        }
        else
        {
            // WE
            x = Probability.OneIn(2) ?
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