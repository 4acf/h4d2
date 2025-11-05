using H4D2.Infrastructure;
using H4D2.Levels;
using H4D2.Particles;

namespace H4D2.Entities.Projectiles;
using Cfg = ProjectileConfig;

public class BileBombProjectile : Projectile
{
    private const double _frameDuration = 1.0 / 8.0;
    private const double _startingZVelocity = 1.0;
    private const double _gravity = 2.2;
    private const double _drag = 0.999;
    private const int _bileParticles = 25;
    private const double _maxLifetime = 20.0;
    
    private readonly int _type;
    private int _spinStep;
    private double _timeSinceLastFrameUpdate;
    private readonly bool _xFlip;
    private bool _collided;
    private double _secondsSinceCollision;
    
    public BileBombProjectile(Level level, Position position, double directionRadians)
        : base(level, position, Cfg.BileBombBoundingBox, 0, directionRadians)
    {
        _type = 2;
        _spinStep = 0;
        _xFlip = (Math.PI / 2) < directionRadians && directionRadians < (3 * Math.PI / 2);
        _collided = false;
        _secondsSinceCollision = 0;
        
        _xVelocity = Math.Cos(_directionRadians);
        _yVelocity = Math.Sin(_directionRadians);
        _zVelocity = _startingZVelocity;
    }
    
    public override void Update(double elapsedTime)
    {
        if (_collided)
        {
            _secondsSinceCollision += elapsedTime;
            if (_secondsSinceCollision >= _maxLifetime)
                Removed = true;
        }
        _UpdatePosition(elapsedTime);
        _UpdateSprite(elapsedTime);
    }
    
    private void _UpdatePosition(double elapsedTime)
    {
        double elapsedTimeConstant = 60 * elapsedTime;
        _xVelocity *= Math.Pow(_drag, elapsedTimeConstant);
        _yVelocity *= Math.Pow(_drag, elapsedTimeConstant);
        _zVelocity -= _gravity * elapsedTime;
        _AttemptMove();
    }
    
    private void _UpdateSprite(double elapsedTime)
    {
        _timeSinceLastFrameUpdate += elapsedTime;
        
        while (_timeSinceLastFrameUpdate >= _frameDuration)
        {
            _spinStep = (_spinStep + 1) % 4;
            _timeSinceLastFrameUpdate -= _frameDuration;
        }
    }
    
    protected override void Render(Bitmap screen, int xCorrected, int yCorrected)
    {
        if (_collided)
            return;
        
        Bitmap bitmap = Art.Projectiles[_type][_spinStep];
        screen.Draw(bitmap, xCorrected, yCorrected, _xFlip);
    }

    protected override void RenderShadow(Bitmap screen, int xCorrected, int yCorrected)
    {
        if (_collided)
            return;
            
        screen.BlendFill(
            xCorrected + Art.ProjectileSize - 6,
            yCorrected - Art.ProjectileSize - 1,
            xCorrected + Art.ProjectileSize - 4,
            yCorrected - Art.ProjectileSize - 1,
            Art.ShadowColor,
            Art.ShadowBlend            
        );
    }
    
    protected override void _Collide(Entity? entity)
    {
        if (_collided)
            return;
        
        base._Collide(entity);
        _level.SpawnZombies();
        for (int i = 0; i < _bileParticles; i++)
        {
            var bileSplatterDebris = new BileSplatterDebris(_level, CenterMass.MutableCopy());
            _level.AddParticle(bileSplatterDebris);
        }
        _collided = true;
    }
}