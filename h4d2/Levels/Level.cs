using System.Collections.Immutable;
using H4D2.Entities;
using H4D2.Entities.Hazards;
using H4D2.Entities.Mobs;
using H4D2.Entities.Mobs.Zombies.Commons;
using H4D2.Entities.Mobs.Survivors;
using H4D2.Entities.Mobs.Zombies;
using H4D2.Entities.Mobs.Zombies.Specials;
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
using H4D2.Spawners;
using H4D2.Spawners.SpecialSpawners;

namespace H4D2.Levels;

public class Level
{
    public event EventHandler? GameOver;
    
    public const double TilePhysicalSize = 16;
   
    private const int _padding = 2;
    private const double _consumableSpawnCooldownSeconds = 30.0;
    private const double _throwableSpawnCooldownSeconds = 30.0;
    private const double _zombieSpawnCooldownSeconds = 1.0 / 60.0;
    private const int _maxParticles = 10000;
    private const int _commonKillCredit = 5;
    private const int _uncommonKillCredit = 10;
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
    
    public readonly int ID;
    public readonly int Width;
    public readonly int Height;
    public readonly CollisionManager<CollisionGroup> CollisionManager;
    private readonly LevelConfig _config;
    private readonly CountdownTimer _consumableSpawnTimer;
    private readonly CountdownTimer _throwableSpawnTimer;
    private readonly CountdownTimer _zombieSpawnTimer;

    public bool IsGameOver => GetLivingMobs<Survivor>().Count == 0;
    public int Credits { get; private set; }
    private readonly List<Entity> _entities;
    private readonly List<Particle> _particles;
    private readonly List<LevelElement> _levelElements;
    public readonly ImmutableArray<TileType> TileTypes;
    public readonly CostMap CostMap;
    private readonly List<int> _zombieSpawnLocations; 
    private readonly List<int> _consumableSpawnLocations;
    private readonly HashSet<int> _activeConsumableLocations;
    private readonly HashSet<int> _activeThrowableLocations;
    private readonly Queue<Zombie> _zombieSpawnQueue;
    private readonly EntityCollisionMap _entityCollisionMap;
    private bool _gameOverEventPublished;
    private bool _oneSurvivorThemePlayed;
    private readonly ZombieSpawnParams _zombieSpawnParams;
    
    public Level(LevelConfig config, CollisionManager<CollisionGroup> collisionManager, Camera camera)
    {
        _config = config;
        _consumableSpawnTimer = new CountdownTimer(_consumableSpawnCooldownSeconds);
        _throwableSpawnTimer = new CountdownTimer(_throwableSpawnCooldownSeconds);
        _throwableSpawnTimer.Update(_throwableSpawnCooldownSeconds / 2.0);
        _zombieSpawnTimer = new CountdownTimer(_zombieSpawnCooldownSeconds);
        
        _entities = [];
        _particles = [];
        _levelElements = [];
        CollisionManager = collisionManager;
        _gameOverEventPublished = false;
        _oneSurvivorThemePlayed = false;

        ID = config.ID;
        Width = config.Layout.Width + _padding;
        Height = config.Layout.Height + _padding;
        _zombieSpawnLocations = [];
        _consumableSpawnLocations = [];
        _activeConsumableLocations = [];
        _activeThrowableLocations = [];
        _zombieSpawnQueue = [];
        _zombieSpawnParams = config.ZombieSpawnParams;
        
        var tileTypes = new TileType[Width * Height];
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                int paddedX = x - (_padding / 2);
                int paddedY = y - (_padding / 2);
                
                if (paddedX < 0 || paddedY < 0 || paddedX >= config.Layout.Width || paddedY >= config.Layout.Height)
                {
                    tileTypes[(y * Width) + x] = TileType.EdgeWall;
                    continue;
                }

                int color = config.Layout.ColorAt(paddedX, paddedY);
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
                        _consumableSpawnLocations.Add(tileIndex);
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
        _entityCollisionMap = new EntityCollisionMap(this);
        Credits = 0;

        while (_activeConsumableLocations.Count < Math.Min(config.MaxConsumables, _consumableSpawnLocations.Count))
        {
            _SpawnConsumable();
        }
    }
    
    public int TileIndex(int x, int y)
    {
        return (y * Width) + x;
    }

    public int TileIndex(Tile tile)
    {
        return TileIndex(tile.X, tile.Y);
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
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (i == 0 && j == 0) continue;
                bool isWall = IsWall(tile.x + i, tile.y + j);
                if (isWall) return true;
            }
        }
        return false;
    }

    public bool IsWall(Tile tile)
    {
        return IsWall(tile.X, tile.Y);
    }

    public bool IsTileOutOfBounds(Tile tile)
    {
        return tile.X < 0 || tile.Y < 0 || tile.X >= Width || tile.Y >= Height;
    }
    
    public bool IsWall(int x, int y)
    {
        int index = TileIndex(x, y);
        if (index < 0 || index >= TileTypes.Length)
            return true;
        TileType tileType = TileTypes[index];
        return tileType == TileType.Wall || tileType ==  TileType.ZombieWall || tileType == TileType.EdgeWall;
    }
    
    public bool IsBlockedByWall(Entity entity, ReadonlyPosition destination)
    {
        (double, double)[] intercardinals =
        [
            entity.BoundingBox.NE(destination.X, destination.Y),
            entity.BoundingBox.SE(destination.X, destination.Y),
            entity.BoundingBox.SW(destination.X, destination.Y),
            entity.BoundingBox.NW(destination.X, destination.Y)
        ];

        foreach ((double, double) position in intercardinals)
        {
            Tile tile = GetTilePosition(position);
            int index = TileIndex(tile);
            if (IsBlocked(index))
                return true;
        }
        
        return false;
        
        bool IsBlocked(int i)
        {
            if (i < 0 || i >= TileTypes.Length)
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
        Tile tile = GetTilePosition((destination.X, destination.Y));
        int index = TileIndex(tile);
        if (index < 0 || index >= TileTypes.Length)
            return true;
        if (TileTypes[index] == TileType.Wall || TileTypes[index] == TileType.ZombieWall)
            return true;
        return false;
    }

    public bool IsValidSpecialSpawnPosition(SpecialSelection selection)
    {
        ReadonlyPosition selectionCenterMass = selection.CenterMass;
        
        Tile tile = GetTilePosition(selectionCenterMass);
        if (IsTileOutOfBounds(tile) || 
            IsWall(tile) ||
            IsTileAdjacentToWall(tile))
            return false;
        
        IEnumerable<Survivor> survivors = _entities.OfType<Survivor>();
        foreach (Survivor survivor in survivors)
        {
            if (selection.HasLineOfSight(this, survivor))
                return false;
        }
        return true;
    }
    
    public bool HasLineOfSight(ReadonlyPosition startPos, ReadonlyPosition targetPos)
    {
        double xPhysOffs = TilePhysicalOffset.Item1;
        double yPhysOffs = TilePhysicalOffset.Item2;
        
        Tile originalTile = GetTilePosition(startPos);
        int currentX = originalTile.X;
        int currentY = originalTile.Y;
        
        Tile targetTile = GetTilePosition(targetPos);

        double directionRadians = Math.Atan2(targetPos.Y - startPos.Y, targetPos.X - startPos.X);
        directionRadians = MathHelpers.NormalizeRadians(directionRadians);

        double xDir = Math.Cos(directionRadians);
        double yDir = Math.Sin(directionRadians);
        int stepX = xDir > 0 ? 1 : -1;
        int stepY = yDir > 0 ? -1 : 1;
        
        double deltaDistX = (xDir == 0) ? double.MaxValue : Math.Abs(1 / xDir);
        double deltaDistY = (yDir == 0) ? double.MaxValue : Math.Abs(1 / yDir);
        
        double posX = (startPos.X + xPhysOffs) / TilePhysicalSize;
        double posY = -((startPos.Y + yPhysOffs) / TilePhysicalSize);
        double sideDistX = stepX == 1 ? 
            (currentX + 1 - posX) * deltaDistX :
            (posX - currentX) * deltaDistX;
        double sideDistY = stepY == 1 ? 
            (currentY + 1 - posY) * deltaDistY :
            (posY - currentY) * deltaDistY;

        int targetIndex = TileIndex(targetTile);
        while (TileIndex(currentX, currentY) != targetIndex)
        {
            if (sideDistX < sideDistY)
            {
                sideDistX += deltaDistX;
                currentX += stepX;
            }
            else
            {
                sideDistY += deltaDistY;
                currentY += stepY;
            }

            if (IsWall(currentX, currentY))
                return false;
        }

        return true;
    }
    
    public Entity? GetFirstCollidingEntity(Entity e1, ReadonlyPosition position, Entity? exclude)
    {
        Tile tile = GetTilePosition(e1.BoundingBox.CenterMass(position));
        var entitiesInTile = _entityCollisionMap.GetTileEntities(tile);
        foreach (Entity e2 in entitiesInTile)
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
        (int audioX, int audioY) = grenade.AudioLocation;
        AudioManager.Instance.PlaySFX(RandomExplosion(), audioX, audioY);
        
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

        return;
        
        SFX RandomExplosion()
        {
            const int numExplosionSounds = 3;
            int random = RandomSingleton.Instance.Next(numExplosionSounds);
            return random switch
            {
                0 => SFX.Explosion1,
                1 => SFX.Explosion2,
                _ => SFX.Explosion3
            };
        }
    }

    public void Explode(PipeBombProjectile pipeBomb)
    {
        (int audioX, int audioY) = pipeBomb.AudioLocation;
        AudioManager.Instance.PlaySFX(SFX.Explosion1, audioX, audioY);
        
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
        (int audioX, int audioY) = boomer.AudioLocation;
        AudioManager.Instance.PlaySFX(SFX.ExplosionBoomer, audioX, audioY);
        
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

    public void SpendCredits(int credits)
    {
        int posCredits = Math.Abs(credits);
        if (posCredits > Credits)
            return;
        Credits -= posCredits;
    }
    
    public void Update(double elapsedTime)
    {
        if (IsGameOver)
        {
            if (_gameOverEventPublished)
                return;
            _UpdateEntities(elapsedTime);
            GameOver?.Invoke(this, EventArgs.Empty);
            _gameOverEventPublished = true;
        }
        else
        {
            _UpdateMusic();
            _UpdateCollisionMap();
            _UpdateEntities(elapsedTime);
            _UpdateParticles(elapsedTime);
        }
    }
    
    public void Render(Camera camera, H4D2BitmapCanvas screen, ShadowBitmap shadows)
    {
        _RenderBackground(camera, screen);
        _RenderShadows(screen, shadows);
        _RenderIsometrics(screen);
    }

    private void _UpdateMusic()
    {
        if (_oneSurvivorThemePlayed || _config.OneSurvivorRemainingTheme == null)
            return;
        int survivorCount = _entities
            .OfType<Survivor>()
            .Count();
        if (survivorCount != 1)
            return;
        AudioManager.Instance.PlayMusic(_config.OneSurvivorRemainingTheme.Value);
        _oneSurvivorThemePlayed = true;
    }
    
    private void _UpdateCollisionMap()
    {
        _entityCollisionMap.Update();
        foreach (Entity entity in _entities)
        {
            _entityCollisionMap.AddEntityToTile(entity, GetTilePosition(entity.CenterMass));
        }
    }
    
    private void _UpdateEntities(double elapsedTime)
    {
        _ReplenishZombies(elapsedTime);

        int numThrowables = _entities
            .OfType<Throwable>()
            .Count(t => !t.Removed);
        if(numThrowables < _config.MaxThrowables)
            _throwableSpawnTimer.Update(elapsedTime);
        if (_throwableSpawnTimer.IsFinished && _SpawnThrowable())
        { 
            _throwableSpawnTimer.Reset();
        }
        
        int numConsumables = _entities
            .OfType<Consumable>()
            .Count(c => !c.Removed);
        if(numConsumables < Math.Min(_config.MaxConsumables, _consumableSpawnLocations.Count))
            _consumableSpawnTimer.Update(elapsedTime);
        if (_consumableSpawnTimer.IsFinished && _SpawnConsumable())
        {
            _consumableSpawnTimer.Reset();
        }
        
        _entities.Sort(Comparators.EntityUpdating);
        var indicesToRemove = new List<int>();
        for (int i = 0; i < _entities.Count; i++)
        {
            if (_entities[i].Removed)
            {
                indicesToRemove.Add(i);
                _HandleSpecialCaseEntityRemovals(_entities[i]);
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

    private void _HandleSpecialCaseEntityRemovals(Entity entity)
    {
        switch (entity)
        {
            case Throwable:
                _activeThrowableLocations.Remove(
                    TileIndex(GetTilePosition(entity.CenterMass))
                );
                break;
            case Consumable:
                _activeConsumableLocations.Remove(
                    TileIndex(GetTilePosition(entity.CenterMass))
                );
                break;
            case Common:
                Credits += _commonKillCredit;
                break;
            case Uncommon:
                Credits += _uncommonKillCredit;
                break;
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
    
    private void _RenderBackground(Camera camera, H4D2BitmapCanvas screen)
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
                
                bool tooFarLeft = xScreenPos + H4D2Art.TileIsoWidth < (camera.XOffset * -1);
                bool tooFarRight = xScreenPos > (camera.XOffset * -1) + camera.Width;
                bool tooFarDown = yScreenPos < (camera.YOffset * -1);
                bool tooFarUp = yScreenPos - (2 * H4D2Art.TileIsoHeight) > (camera.YOffset * -1) + camera.Height;
                
                if (tooFarLeft || tooFarRight || tooFarDown || tooFarUp)
                    continue;
                
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
                            H4D2Art.Tiles.Floors[0] :
                            H4D2Art.Tiles.Floors[1];
                        screen.Draw(floorBitmap, xScreenPos, yScreenPos);
                        break;
                }
            }
        }
    }
    
    private void _RenderShadows(H4D2BitmapCanvas screen, ShadowBitmap shadows)
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

    private void _RenderIsometrics(H4D2BitmapCanvas screen)
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
            RandomSingleton.Instance.Next(_zombieSpawnParams.MinSpawnWaveSize, _zombieSpawnParams.MaxSpawnWaveSize);
        randomNewZombies = Math.Min(randomNewZombies, _zombieSpawnParams.MaxZombiesAlive);
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
        if (zombies.Count + _zombieSpawnQueue.Count < _zombieSpawnParams.MinZombiesAlive)
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
        int randomUncommonIndex = RandomSingleton.Instance.Next(_config.Uncommons.Length);
        UncommonDescriptor uncommon = _config.Uncommons[randomUncommonIndex];
        return UncommonSpawner.Spawn(uncommon, this, position);
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
        foreach (SurvivorDescriptor survivor in _config.Survivors)
        {
            _entities.Add(SurvivorSpawner.Spawn(survivor, this, survivorSpawnPos.Copy()));            
        }
    }

    private bool _SpawnConsumable()
    {
        int triesRemaining = 5;
        bool validLocationFound = false;
        int tileIndex = 0;
        
        while (triesRemaining > 0)
        {
            int randomLocation = RandomSingleton.Instance.Next(_consumableSpawnLocations.Count);
            tileIndex = _consumableSpawnLocations[randomLocation];
            if (!_activeConsumableLocations.Contains(tileIndex))
            {
                validLocationFound = true;
                break;
            }
            triesRemaining--;
        }

        if (!validLocationFound)
            return false;
        
        int randomConsumable = RandomSingleton.Instance.Next(_config.Consumables.Length);
        Tile tile = GetTileFromIndex(tileIndex);
        Position consumablePos = new Position(
            (tile.X * TilePhysicalSize) + _pickupXOffset,
            (-tile.Y * TilePhysicalSize) + _pickupYOffset
        );
        ConsumableDescriptor consumable = _config.Consumables[randomConsumable];
        _entities.Add(ConsumableSpawner.Spawn(consumable, this, consumablePos));
        _activeConsumableLocations.Add(tileIndex);
        return true;
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
                !_consumableSpawnLocations.Contains(tileIndex) &&
                !_activeThrowableLocations.Contains(tileIndex)
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
            RandomSingleton.Instance.Next(_config.Throwables.Length);
        Position throwablePos = new Position(
            (tile.X * TilePhysicalSize) + _pickupXOffset,
            (-tile.Y * TilePhysicalSize) + _pickupYOffset
        );
        ThrowableDescriptor throwable = _config.Throwables[randomThrowable];
        _entities.Add(ThrowableSpawner.Spawn(throwable, this, throwablePos));
        _activeThrowableLocations.Add(tileIndex);
        return true;
    }
}