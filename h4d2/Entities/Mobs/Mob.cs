using H4D2.Entities.Mobs.Zombies;
using H4D2.Infrastructure;
using H4D2.Levels;
using H4D2.Levels.Pathfinders;
using H4D2.Particles.DebrisParticles;
using H4D2.Particles.DebrisParticles.Emitters;

namespace H4D2.Entities.Mobs;

public abstract class Mob : Entity
{
    public bool IsAlive => _health > 0;
    
    protected const double _turnSpeed = 5.0;
    protected const double _speedFactor = 15.0 / 220.0;
    protected const int _upperBitmapOffset = 9;
    protected const int _attackingBitmapOffset = 18;
    private const int _gibs = 3;
    protected const double _frameDuration = 1.0 / 8.0;
    protected const double _hazardDamageCooldownSeconds = 0.5;
    private const double _knockbackScale = 2.5;
    private const double _knockbackZVelocity = 1.0;
    
    protected int _health;
    protected double _speed;

    protected double _directionRadians;
    protected bool _xFlip;
    protected int _walkStep;
    protected int _lowerFrame;
    protected int _upperFrame;
    protected readonly int _gibColor;
    protected readonly CountdownTimer _frameUpdateTimer;
    protected readonly CountdownTimer _hazardDamageTimer;
    protected readonly Pathfinder _pathfinder;
    
    protected Mob(Level level, Position position, MobConfig config) 
        : base(level, position, config.BoundingBox)
    {
        _health = config.Health;
        _speed = config.RunSpeed;
        _directionRadians = 0.0;
        _xFlip = false;
        _walkStep = 0;
        _lowerFrame = 0;
        _upperFrame = _upperBitmapOffset;
        _gibColor = config.GibColor;
        _frameUpdateTimer = new CountdownTimer(_frameDuration);
        _hazardDamageTimer = new CountdownTimer(_hazardDamageCooldownSeconds);
        _pathfinder = new Pathfinder(level);
    }

    protected Mob(Level level, Position position, MobConfig config, double speed)
        : base(level, position, config.BoundingBox)
    {
        _health = config.Health;
        _speed = speed;
        _directionRadians = 0.0;
        _xFlip = false;
        _walkStep = 0;
        _lowerFrame = 0;
        _upperFrame = _upperBitmapOffset;
        _gibColor = config.GibColor;
        _frameUpdateTimer = new CountdownTimer(_frameDuration);
        _hazardDamageTimer = new CountdownTimer(_hazardDamageCooldownSeconds);
        _pathfinder = new Pathfinder(level);
    }
    
    public virtual void HitBy(Zombie zombie)
    {
        if (Removed)
            return;
        _health -= zombie.Damage;
        if (!IsAlive)
        {
            _Die();
        }
        var bloodSplatter = new BloodSplatter(_level, CenterMass.MutableCopy());
        _level.AddParticle(bloodSplatter);
    }

    public virtual void KnockbackHitBy(Zombie zombie)
    {
        HitBy(zombie);
        _velocity.X = Math.Cos(zombie.DirectionRadians) * _knockbackScale;
        _velocity.Y = Math.Sin(zombie.DirectionRadians) * _knockbackScale;
        _velocity.Z = _knockbackZVelocity;
    }
    
    protected virtual void _TakeHazardDamage(int damage)
    {
        if (Removed)
            return;
        if (!_hazardDamageTimer.IsFinished)
            return;
        _hazardDamageTimer.Reset();
        _health -= damage;
        if (!IsAlive)
        {
            _Die();
        }
        var bloodSplatter = new BloodSplatter(_level, CenterMass.MutableCopy());
        _level.AddParticle(bloodSplatter);
    }
    
    protected virtual void _Die()
    {
        for (int i = 0; i < _gibs; i++)
        {
            Position position = CenterMass.MutableCopy();
            position.Z += i;
            var deathSplatter = new GibDebris(_level, position, _gibColor);
            _level.AddParticle(deathSplatter);
        }
        Removed = true;
    }

    protected bool _HasLineOfSight(Entity entity)
    {
        const double physSize = Level.TilePhysicalSize;
        double xPhysOffs = Level.TilePhysicalOffset.Item1;
        double yPhysOffs = Level.TilePhysicalOffset.Item2;
        
        ReadonlyPosition myPosition = CenterMass;
        Tile myStartingTile = Level.GetTilePosition(myPosition);
        int currentX = myStartingTile.X;
        int currentY = myStartingTile.Y;
        
        ReadonlyPosition targetPosition = entity.CenterMass;
        Tile targetTile = Level.GetTilePosition(targetPosition);

        double directionRadians = Math.Atan2(targetPosition.Y - myPosition.Y, targetPosition.X - myPosition.X);
        directionRadians = MathHelpers.NormalizeRadians(directionRadians);

        double xDir = Math.Cos(directionRadians);
        double yDir = Math.Sin(directionRadians);
        int stepX = xDir > 0 ? 1 : -1;
        int stepY = yDir > 0 ? -1 : 1;
        
        double deltaDistX = (xDir == 0) ? double.MaxValue : Math.Abs(1 / xDir);
        double deltaDistY = (yDir == 0) ? double.MaxValue : Math.Abs(1 / yDir);
        
        double posX = (myPosition.X + xPhysOffs) / physSize;
        double posY = -((myPosition.Y + yPhysOffs) / physSize);
        double sideDistX = stepX == 1 ? 
            (currentX + 1 - posX) * deltaDistX :
            (posX - currentX) * deltaDistX;
        double sideDistY = stepY == 1 ? 
            (currentY + 1 - posY) * deltaDistY :
            (posY - currentY) * deltaDistY;

        int targetIndex = _level.TileIndex(targetTile);
        while (_level.TileIndex(currentX, currentY) != targetIndex)
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

            if (_level.IsWall(currentX, currentY))
                return false;
        }

        return true;
    }
}