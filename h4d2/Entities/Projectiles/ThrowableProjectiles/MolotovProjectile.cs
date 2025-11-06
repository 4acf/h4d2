using H4D2.Infrastructure;
using H4D2.Levels;
using H4D2.Particles;

namespace H4D2.Entities.Projectiles.ThrowableProjectiles;

public class MolotovProjectile : ThrowableProjectile
{
    private const double _bounce = 0.6;
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
    
    protected override void RenderShadow(Bitmap screen, int xCorrected, int yCorrected)
    {
        screen.BlendFill(
            xCorrected + Art.ProjectileSize - 6,
            yCorrected - Art.ProjectileSize - 1,
            xCorrected + Art.ProjectileSize - 3,
            yCorrected - Art.ProjectileSize - 1,
            Art.ShadowColor,
            Art.ShadowBlend            
        );
    }
    
    protected override void _Collide(Entity? entity)
    {
        if (entity != null)
        {
            _xVelocity *= _bounce * -1;
            _yVelocity *= _bounce * -1;
        }
        else
        {
            for (int i = 0; i < _fuelParticles; i++)
            {
                var fuelSplatterDebris = new FuelSplatterDebris(_level, CenterMass.MutableCopy());
                _level.AddParticle(fuelSplatterDebris);
            }
            base._Collide(entity);
            Removed = true;
        }
    }
}