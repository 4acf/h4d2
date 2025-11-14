using H4D2.Entities.Mobs.Survivors;
using H4D2.Infrastructure;
using H4D2.Levels;
using H4D2.Particles.DebrisParticles.Granules;

namespace H4D2.Entities.Projectiles;

public class DeathPuke : Projectile
{
    private const int _color = 0x5a6e38;
    private const double _gravity = 4.0;
    private const double _minSpeed = 25.0;
    private const double _maxSpeed = 75.0;
    private const double _startingZVelocity = 0.5;

    private readonly double _speed;

    public DeathPuke(Level level, Position position, double directionRadians)
        : base(level, position, ProjectileConfig.PukeBoundingBox, 0, directionRadians)
    {
        _speed = MathHelpers.GaussianRandom((_minSpeed + _maxSpeed) / 2, (_maxSpeed - _minSpeed) / 2);
        _velocity.Z = _startingZVelocity;
    }

    public override void Update(double elapsedTime)
    {
        double timeAdjustedSpeed = _speed * elapsedTime;
        _velocity.X = Math.Cos(_directionRadians) * timeAdjustedSpeed;
        _velocity.Y = Math.Sin(_directionRadians) * timeAdjustedSpeed;
        _velocity.Z -= _gravity * elapsedTime;
        _AttemptMove();
    }

    protected override void Render(Bitmap screen, int xCorrected, int yCorrected)
    {
        screen.SetPixel(xCorrected, yCorrected, _color);
    }

    protected override void RenderShadow(ShadowBitmap shadows, int xCorrected, int yCorrected)
    {
        shadows.SetPixel(xCorrected, yCorrected);
    }

    protected override void _Collide(Entity? entity)
    {
        base._Collide(entity);
        switch (entity)
        {
            case null:
            {
                var bile = new InvolatileBile(_level, _position.Copy());
                _level.AddParticle(bile);
                break;
            }
            case Survivor survivor:
                survivor.Biled();
                break;
        }

        Removed = true;
    }
}