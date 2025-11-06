using H4D2.Infrastructure;
using H4D2.Levels;
using H4D2.Particles;

namespace H4D2.Entities.Projectiles.ThrowableProjectiles;

public class BileBombProjectile : ThrowableProjectile
{
    private const int _bileParticles = 25;
    private const double _maxLifetime = 20.0;
    
    private bool _collided;
    private double _secondsSinceCollision;
    
    public BileBombProjectile(Level level, Position position, double directionRadians)
        : base(level, position, ThrowableProjectileConfigs.BileBomb, directionRadians)
    {
        _collided = false;
        _secondsSinceCollision = 0;
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