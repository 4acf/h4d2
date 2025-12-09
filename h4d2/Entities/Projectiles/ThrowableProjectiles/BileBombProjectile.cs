using H4D2.Infrastructure;
using H4D2.Infrastructure.H4D2;
using H4D2.Levels;
using H4D2.Particles.DebrisParticles.Emitters;

namespace H4D2.Entities.Projectiles.ThrowableProjectiles;

public class BileBombProjectile : ThrowableProjectile
{
    private const int _bileParticles = 25;
    private const double _lifetime = 20.0;
    
    private bool _collided;
    private readonly CountdownTimer _despawnTimer;
    
    public BileBombProjectile(Level level, Position position, double directionRadians)
        : base(level, position, ThrowableProjectileConfigs.BileBomb, directionRadians)
    {
        _collided = false;
        _despawnTimer = new CountdownTimer(_lifetime);
    }
    
    public override void Update(double elapsedTime)
    {
        if (_collided)
        {
            _despawnTimer.Update(elapsedTime);
            if (_despawnTimer.IsFinished)
                Removed = true;
        }
        _UpdatePosition(elapsedTime);
        _UpdateSprite(elapsedTime);
    }
    
    protected override void Render(Bitmap screen, int xCorrected, int yCorrected)
    {
        if (_collided)
            return;
        
        Bitmap bitmap = H4D2Art.Projectiles[_type][_spinStep];
        screen.Draw(bitmap, xCorrected, yCorrected, _xFlip);
    }

    protected override void RenderShadow(ShadowBitmap shadows, int xCorrected, int yCorrected)
    {
        if (_collided)
            return;
            
        shadows.Fill(
            xCorrected + H4D2Art.ProjectileSize - 6,
            yCorrected - H4D2Art.ProjectileSize - 1,
            xCorrected + H4D2Art.ProjectileSize - 4,
            yCorrected - H4D2Art.ProjectileSize - 1
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
            var bileSplatterDebris = new BileSplatter(_level, CenterMass.MutableCopy());
            _level.AddParticle(bileSplatterDebris);
        }
        _collided = true;
    }

    protected override void _CollideWall()
    {
        _velocity.X *= _bounce * -1;
        _velocity.Y *= _bounce * -1;
    }
}