using H4D2.Infrastructure;
using H4D2.Infrastructure.H4D2;
using H4D2.Levels;
using H4D2.Particles.DebrisParticles.Emitters;
using H4D2.Particles.DebrisParticles.Granules;

namespace H4D2.Entities.Projectiles.ThrowableProjectiles;

public class SpitProjectile : ThrowableProjectile
{
    private const int _numSpitParticles = 2;
    private const int _numSpitSplatters = 50;
    private const double _speedMultiplier = 1.75;
    private const double _gravityMultiplier = 0.75;
    
    public SpitProjectile(Level level, Position position, double directionRadians)
        : base(level, position, ThrowableProjectileConfigs.Spit, directionRadians)
    {
        if (_level.IsWall(Level.GetTilePosition(CenterMass)))
            Removed = true;
        
        _velocity.X *= _speedMultiplier;
        _velocity.Y *= _speedMultiplier;
        _velocity.Z *= _gravityMultiplier;
    }

    public override void Update(double elapsedTime)
    {
        _UpdateParticles();
        _UpdatePosition(elapsedTime);
        _UpdateSprite(elapsedTime);
    }

    private void _UpdateParticles()
    {
        ReadonlyPosition centerMass = FootPosition;
        for (int i = 0; i < _numSpitParticles; i++)
        {
            int randomDx = RandomSingleton.Instance.Next(2);
            int randomDy = RandomSingleton.Instance.Next(2);
            int randomDz = RandomSingleton.Instance.Next(4);
            var position = new Position(centerMass.X + randomDx, centerMass.Y + randomDy, centerMass.Z + randomDz);
            var spit = new InvolatileSpit(_level, position, _velocity.ReadonlyCopy());
            _level.AddParticle(spit);
        }
    }
    
    protected override void _UpdatePosition(double elapsedTime)
    {
        _velocity.Z -= _gravity * elapsedTime;
        _AttemptMove();
    }

    protected override void RenderShadow(ShadowBitmap shadows, int xCorrected, int yCorrected)
    {
        shadows.Fill(
            xCorrected + H4D2Art.ProjectileSize - 6,
            yCorrected - H4D2Art.ProjectileSize - 1,
            xCorrected + H4D2Art.ProjectileSize - 2,
            yCorrected - H4D2Art.ProjectileSize - 1
        );
    }

    protected override void _Collide(Entity? entity)
    {
        base._Collide(entity);
        if (Removed)
            return;
        for (int i = 0; i < _numSpitSplatters; i++)
        {
            var spitSplatter = new SpitSplatter(_level, CenterMass.MutableCopy());
            _level.AddParticle(spitSplatter);
        }
        Removed = true;
    }
    
    protected override void _CollideWall()
    {
        _velocity.X *= _bounce * -1;
        _velocity.Y *= _bounce * -1;
    }
}