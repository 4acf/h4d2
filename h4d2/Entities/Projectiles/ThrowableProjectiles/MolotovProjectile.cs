using H4D2.Infrastructure;
using H4D2.Infrastructure.H4D2;
using H4D2.Levels;
using H4D2.Particles.DebrisParticles.Emitters;

namespace H4D2.Entities.Projectiles.ThrowableProjectiles;

public class MolotovProjectile : ThrowableProjectile
{
    private const int _fuelParticles = 50;
    
    public MolotovProjectile(Level level, Position position, double directionRadians)
        : base(level, position, ThrowableProjectileConfigs.Molotov, directionRadians)
    {
        
    }
    
    public override void Update(double elapsedTime)
    {
        _UpdatePosition(elapsedTime);
        _UpdateSprite(elapsedTime);
    }
    
    protected override void RenderShadow(ShadowBitmap shadows, int xCorrected, int yCorrected)
    {
        shadows.Fill(
            xCorrected + H4D2Art.ProjectileSize - 6,
            yCorrected - H4D2Art.ProjectileSize - 1,
            xCorrected + H4D2Art.ProjectileSize - 3,
            yCorrected - H4D2Art.ProjectileSize - 1        
        );
    }
    
    protected override void _Collide(Entity? entity)
    {
        if (entity != null)
        {
            _velocity.X *= _bounce * -1;
            _velocity.Y *= _bounce * -1;
        }
        else
        {
            for (int i = 0; i < _fuelParticles; i++)
            {
                var fuelSplatterDebris = new FuelSplatter(_level, CenterMass.MutableCopy());
                _level.AddParticle(fuelSplatterDebris);
            }
            base._Collide(entity);
            Removed = true;
        }
    }
    
    protected override void _CollideWall(double xComponent, double yComponent, double zComponent)
    {
        _velocity.X *= _bounce * -1;
        _velocity.Y *= _bounce * -1;
    }
}