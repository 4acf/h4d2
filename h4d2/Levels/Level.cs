using System.Collections.Immutable;
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

public class Level
{
    public const double TilePhysicalSize = 16;
   
    private const int _padding = 2;
    private const double _levelResetCooldownSeconds = 8.0;
    private const double _throwableSpawnCooldownSeconds = 30.0;
    private const double _zombieSpawnCooldownSeconds = 1.0 / 60.0;
    private const int _minZombiesAlive = 20;
    private const int _minSpawnWaveSize = 5;
    private const int _maxSpawnWaveSize = 15;
    private const int _maxZombiesAlive = 50;
    private const int _maxParticles = 5000;
    private const int _maxThrowablePickups = 3;
    private const double _mobSpawnXOffset = 5.5;
    private const double _mobSpawnYOffset = -(H4D2Art.TileCenterOffset - H4D2Art.SpriteSize) + 0.5;
    private const double _pickupXOffset = -7;
    private const double _pickupYOffset = -18;
    
    private const int _wallColor = 0x0;
    private const int _healthPickupColor = 0xff0000;
    private const int _zombieSpawnColor = 0x00ff00;
    private const int _survivorSpawnColor = 0x0000ff;

    private const int _tileRenderOffset = H4D2Art.TileSize - H4D2Art.TileIsoHalfHeight;
    public static readonly (double, double) TilePhysicalOffset 
        = Isometric.ScreenSpaceToWorldSpace(0, _tileRenderOffset);
    
    public readonly int Width;
    public readonly int Height;
    public readonly CollisionManager<CollisionGroup> CollisionManager;
    private readonly CountdownTimer _levelResetTimer;
    private readonly CountdownTimer _throwableSpawnTimer;
    private readonly CountdownTimer _zombieSpawnTimer;
    public bool CanReset => _levelResetTimer.IsFinished;
    public bool IsGameOver => GetLivingMobs<Survivor>().Count == 0;
    private readonly List<Entity> _entities;
    private readonly List<Particle> _particles;
    private readonly List<LevelElement> _levelElements;
    public readonly ImmutableArray<TileType> TileTypes;
    public readonly CostMap CostMap;
    private readonly List<int> _zombieSpawnLocations; 
    private readonly List<int> _healthPickupLocations;
    private readonly HashSet<int> _throwablePickupLocations;
    private readonly Queue<Zombie> _zombieSpawnQueue;
    
    public Level(Bitmap levelBitmap, CollisionManager<CollisionGroup> collisionManager, Camera camera)
    {
        _levelResetTimer = new CountdownTimer(_levelResetCooldownSeconds);
        _throwableSpawnTimer = new CountdownTimer(_throwableSpawnCooldownSeconds);
        _zombieSpawnTimer = new CountdownTimer(_zombieSpawnCooldownSeconds);
        _throwableSpawnTimer.Update(_throwableSpawnCooldownSeconds / 2.0);
        
        _entities = [];
        _particles = [];
        _levelElements = [];
        CollisionManager = collisionManager;
        
        Width = levelBitmap.Width + _padding;
        Height = levelBitmap.Height + _padding;
        _zombieSpawnLocations = [];
        _healthPickupLocations = [];
        _throwablePickupLocations = [];
        _zombieSpawnQueue = [];
        
        var tileTypes = new TileType[Width * Height];
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                int paddedX = x - (_padding / 2);
                int paddedY = y - (_padding / 2);
                
                if (paddedX < 0 || paddedY < 0 || paddedX >= levelBitmap.Width || paddedY >= levelBitmap.Height)
                {
                    tileTypes[(y * Width) + x] = TileType.EdgeWall;
                    continue;
                }

                int color = levelBitmap.ColorAt(paddedX, paddedY);
                int tileIndex = TileIndex(x, y); 
                switch (color)
                {
                    case _wallColor:
                        tileTypes[tileIndex] = TileType.Wall;
                        break;
                    case _zombieSpawnColor:
                        tileTypes[tileIndex] = TileType.ZombieWall;
                        _zombieSpawnLocations.Add(tileIndex);
                        break;
                    case _survivorSpawnColor:
                        tileTypes[tileIndex] = TileType.Floor;
                        _SpawnSurvivors(camera, x, y);
                        break;
                    case _healthPickupColor:
                        tileTypes[tileIndex] = TileType.Floor;
                        _healthPickupLocations.Add(tileIndex);
                        _SpawnHealthPickup(x, y);
                        break;
                    default:
                        tileTypes[tileIndex] = TileType.Floor;
                        break;
                }
            }
        }

        var tileBuilder = ImmutableArray.CreateBuilder<TileType>(Width * Height);
        foreach (var t in tileTypes)
        {
            tileBuilder.Add(t);
        }
        TileTypes = tileBuilder.MoveToImmutable();
        CostMap = new CostMap(this, TileTypes);
    }
    
    public int TileIndex(int x, int y)
    {
        return (y * Width) + x;
    }

    public int TileIndex(Tile tile)
    {
        return (tile.Y * Width) + tile.X;
    }
    
    public static Tile GetTilePosition(ReadonlyPosition position)
    {
        return GetTilePosition((position.X, position.Y));
    }

    public static Tile GetTilePosition((double x, double y) position)
    {
        int tileX = (int)Math.Floor((position.x + TilePhysicalOffset.Item1) / TilePhysicalSize);
        int tileY = (int)Math.Floor(-((position.y + TilePhysicalOffset.Item2) / TilePhysicalSize));
        return new Tile(tileX, tileY);
    }

    public Tile GetTileFromIndex(int tileIndex)
    {
        return new Tile(tileIndex % Width, tileIndex / Width);
    }
    
    public bool IsTileAdjacentToWall(int index)
    {
        Tile tile = GetTileFromIndex(index);
        return IsTileAdjacentToWall(tile);
    }
    
    public bool IsTileAdjacentToWall(int x, int y) => IsTileAdjacentToWall((x, y));
    public bool IsTileAdjacentToWall(Tile tile) => IsTileAdjacentToWall((tile.X, tile.Y));
    
    public bool IsTileAdjacentToWall((int x, int y) tile)
    {
        bool wallToN = IsWall(tile.x, tile.y - 1);
        if (wallToN) return true;
        bool wallToE = IsWall(tile.x + 1, tile.y);
        if (wallToE) return true;
        bool wallToS = IsWall(tile.x, tile.y + 1);
        if (wallToS) return true;
        bool wallToW = IsWall(tile.x - 1, tile.y);
        if (wallToW) return true;
        return false;
    }

    public bool IsWall(Tile tile)
    {
        return IsWall(tile.X, tile.Y);
    }
    
    public bool IsWall(int x, int y)
    {
        int index = TileIndex(x, y);
        if (index < 0 || index >= TileTypes.Length)
            return true;
        TileType tileType = TileTypes[index];
        return tileType == TileType.Wall || tileType ==  TileType.ZombieWall || tileType == TileType.EdgeWall;
    }
    
    // these functions are pretty bad right now so clean them up please
    public bool IsBlockedByWall(Entity entity, ReadonlyPosition destination)
    {
        var ne = entity.BoundingBox.NE(destination.X, destination.Y);
        var se = entity.BoundingBox.SE(destination.X, destination.Y);
        var sw = entity.BoundingBox.SW(destination.X, destination.Y);
        var nw  = entity.BoundingBox.NW(destination.X, destination.Y);

        Tile neTile = GetTilePosition(ne);
        int index = (neTile.Y * Width) + neTile.X;
        if (IsBlocked(index))
            return true;

        Tile seTile = GetTilePosition(se);
        index = (seTile.Y * Width) + seTile.X;
        if (IsBlocked(index))
            return true;
        
        Tile swTile = GetTilePosition(sw);
        index = (swTile.Y * Width) + swTile.X;
        if (IsBlocked(index))
            return true;
        
        Tile nwTile = GetTilePosition(nw);
        index = (nwTile.Y * Width) + nwTile.X;
        if (IsBlocked(index))
            return true;
        
        return false;
        
        bool IsBlocked(int i)
        {
            if (index < 0 || index >= TileTypes.Length)
                return true;
            if (TileTypes[i] == TileType.Wall || TileTypes[i] == TileType.EdgeWall)
                return true;
            if (entity is not Common && entity is not Uncommon && TileTypes[i] == TileType.ZombieWall)
                return true;
            return false;
        }
    }

    public bool IsBlockedByWall(ReadonlyPosition destination)
    {
        int x = (int)Math.Floor((destination.X + TilePhysicalOffset.Item1) / TilePhysicalSize);
        int y = (int)Math.Floor(-((destination.Y + TilePhysicalOffset.Item2) / TilePhysicalSize));
        int index = (y * Width) + x;
        if (index < 0 || index >= TileTypes.Length)
            return true;
        if (TileTypes[index] == TileType.Wall || TileTypes[index] == TileType.ZombieWall)
            return true;
        return false;
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

    public List<T> GetEntities<T>() where T : Entity
    {
        return _entities
            .OfType<T>()
            .Where(t => !t.Removed)
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

    public Survivor? GetNearestSurvivorInNeedOfHelp(Survivor survivor, ReadonlyPosition position)
        => _GetNearestSurvivor(position, s => s.NeedsHelp && s != survivor);
    
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
        _RenderIsometrics(screen);
    }
    
    private void _UpdateEntities(double elapsedTime)
    {
        _ReplenishZombies(elapsedTime);

        int numThrowablePickups = _entities
            .OfType<Throwable>()
            .Count(t => !t.Removed);
        if(numThrowablePickups < _maxThrowablePickups)
            _throwableSpawnTimer.Update(elapsedTime);
        if (_throwableSpawnTimer.IsFinished && _SpawnThrowable())
        { 
            _throwableSpawnTimer.Reset();
        }
        
        _entities.Sort(Comparators.EntityUpdating);
        var indicesToRemove = new List<int>();
        for (int i = 0; i < _entities.Count; i++)
        {
            if (_entities[i].Removed)
            {
                indicesToRemove.Add(i);
                if (_entities[i] is Throwable)
                    _throwablePickupLocations.Remove(
                        TileIndex(GetTilePosition(_entities[i].CenterMass))
                    );
            }
            else
            {
                _entities[i].Update(elapsedTime);
                if(_entities[i].Removed)
                    indicesToRemove.Add(i);
            }
        }

        for (int i = indicesToRemove.Count - 1; i >= 0; i--)
        {
            _entities.RemoveAt(indicesToRemove[i]);
        }
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
                if(_particles[i].Removed)
                    indicesToRemove.Add(i);
            }
        }

        for (int i = indicesToRemove.Count - 1; i >= 0; i--)
        {
            _particles.RemoveAt(indicesToRemove[i]);
        }
    }
    
    private void _RenderBackground(Bitmap screen)
    {
        screen.Clear(0x2b2b2b);
        _levelElements.Clear();
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                int index = (y * Width) + x;
                int xScreenPos = (y * H4D2Art.TileIsoWidth / 2) + (x * H4D2Art.TileIsoWidth / 2);
                int yScreenPos = (y * -H4D2Art.TileIsoHalfHeight) + (x * H4D2Art.TileIsoHalfHeight);
                double xTilePos = x * TilePhysicalSize;
                double yTilePos = -y * TilePhysicalSize;
                
                TileType tileType = TileTypes[index];
                switch (tileType)
                {
                    case TileType.Wall:
                        _levelElements.Add(new Wall(this, new Position(xTilePos, yTilePos)));
                        break;
                    case TileType.ZombieWall:
                        _levelElements.Add(new ZombieWall(this, new Position(xTilePos, yTilePos)));
                        break;
                    case TileType.EdgeWall:
                        _levelElements.Add(new EdgeWall(this, new Position(xTilePos, yTilePos)));
                        break;
                    case TileType.Floor:
                    case TileType.SurvivorFloor:
                    default:
                        Bitmap floorBitmap = (x + y) % 2 == 0 ? 
                            H4D2Art.Floors[0] :
                            H4D2Art.Floors[1];
                        screen.Draw(floorBitmap, xScreenPos, yScreenPos);
                        break;
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

    private void _RenderIsometrics(Bitmap screen)
    {
        var isometrics = new List<Isometric>();
        isometrics.AddRange(_entities);
        isometrics.AddRange(_particles);
        isometrics.AddRange(_levelElements);
        isometrics.Sort(Comparators.IsometricRendering);
        foreach (Isometric isometric in isometrics)
        {
            isometric.Render(screen);
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
            _zombieSpawnQueue.Enqueue(zombie);
        }
    }
    
    private void _ReplenishZombies(double elapsedTime)
    {
        _zombieSpawnTimer.Update(elapsedTime);
        if (_zombieSpawnQueue.Count > 0 && _zombieSpawnTimer.IsFinished)
        {
            _entities.Add(_zombieSpawnQueue.Dequeue());
            _zombieSpawnTimer.Reset();
        }
        
        List<Zombie> zombies = GetLivingMobs<Zombie>();
        if (zombies.Count + _zombieSpawnQueue.Count < _minZombiesAlive)
        {
            SpawnZombies();
        }
    }

    private Position _RandomZombieSpawnPosition()
    {
        int randomIndex = RandomSingleton.Instance.Next(_zombieSpawnLocations.Count);
        int tileIndex = _zombieSpawnLocations[randomIndex];
        int x = tileIndex % Width;
        int y = tileIndex / Width;
        (double, double) mobSpawnOffset = Isometric.ScreenSpaceToWorldSpace(
            _mobSpawnXOffset,
            _mobSpawnYOffset
        );
        return new Position(
            (x * TilePhysicalSize) + mobSpawnOffset.Item1,
            (-y * TilePhysicalSize) + mobSpawnOffset.Item2
        );
    }
    
    private Zombie _CreateRandomLevelZombie(Position position)
    {
        if(Probability.Percent(95))
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

    private void _SpawnSurvivors(Camera camera, int x, int y)
    {
        camera.MoveX(-((camera.Width / 2) - (H4D2Art.TileSize / 2)) + ((x + y) * (H4D2Art.TileSize / 2)));
        camera.MoveY(-H4D2Art.TileCenterOffset);
        camera.MoveY(((x - y) * H4D2Art.TileIsoHalfHeight) - (camera.Height / 2));
        (double, double) mobSpawnOffset = Isometric.ScreenSpaceToWorldSpace(
            _mobSpawnXOffset,
            _mobSpawnYOffset
        );
        Position survivorSpawnPos = new Position(
            (x * TilePhysicalSize) + mobSpawnOffset.Item1,
            (-y * TilePhysicalSize) + mobSpawnOffset.Item2
        );
        _entities.Add(new Coach(this, survivorSpawnPos.Copy()));
        _entities.Add(new Nick(this, survivorSpawnPos.Copy()));
        _entities.Add(new Ellis(this, survivorSpawnPos.Copy()));
        _entities.Add(new Rochelle(this, survivorSpawnPos.Copy()));
    }

    private void _SpawnHealthPickup(Tile tile) => _SpawnHealthPickup(tile.X, tile.Y);
    
    private void _SpawnHealthPickup(int x, int y)
    {
        int randomConsumable = RandomSingleton.Instance.Next(3);
        Position consumablePos = new Position(
            (x * TilePhysicalSize) + _pickupXOffset,
            (-y * TilePhysicalSize) + _pickupYOffset
        );
        Consumable consumable = randomConsumable switch
        {
            0 => new FirstAidKit(this, consumablePos),
            1 => new Pills(this, consumablePos),
            _ => new Adrenaline(this, consumablePos)
        };
        _entities.Add(consumable);
    }
    
    private bool _SpawnThrowable()
    {
        int triesRemaining = 5;
        bool validLocationFound = false;
        int tileIndex = 0;
        
        while (triesRemaining > 0)
        {
            tileIndex = RandomSingleton.Instance.Next(TileTypes.Length);
            if (
                TileTypes[tileIndex] != TileType.Wall &&
                !IsTileAdjacentToWall(tileIndex) &&
                !_healthPickupLocations.Contains(tileIndex) &&
                !_throwablePickupLocations.Contains(tileIndex)
            )
            {
                validLocationFound = true;
                break;
            }
            
            triesRemaining--;
        }

        if (!validLocationFound)
            return false;
        
        Tile tile = GetTileFromIndex(tileIndex);
        int randomThrowable =
            RandomSingleton.Instance.Next(3);
        Position throwablePos = new Position(
            (tile.X * TilePhysicalSize) + _pickupXOffset,
            (-tile.Y * TilePhysicalSize) + _pickupYOffset
        );
        Throwable throwable = randomThrowable switch
        {
            0 => new Molotov(this, throwablePos),
            1 => new PipeBomb(this, throwablePos),
            _ => new BileBomb(this, throwablePos)
        };
        _entities.Add(throwable);
        _throwablePickupLocations.Add(tileIndex);

        return true;
    }
}