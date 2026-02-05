using H4D2.Infrastructure;
using H4D2.Infrastructure.H4D2;
using H4D2.Levels;

namespace H4D2.Particles.Smokes;

public class Smoke : Particle
{
    private const double _gravity = 2.0;
    private const double _decay = 0.5;
    private const double _inertia = 0.1;
    private const double _minLifetime = 0.1;
    private const double _maxLifetime = 0.3;
    private const double _minOpacity = 0.0;
    private const double _maxOpacity = 0.5;
    
    private const int _color = 0x0;
    private readonly CountdownTimer _despawnTimer;
    private readonly ReadonlyVelocity _parentVelocity;
    private readonly int _randomDx;
    private readonly int _randomDy;
    
    public Smoke(Level level, Position position, ReadonlyVelocity parentVelocity)
        : base(level, position)
    {
        double randomDouble = RandomSingleton.Instance.NextDouble();
        double lifetime = randomDouble * (_maxLifetime - _minLifetime) + _minLifetime;
        _despawnTimer = new CountdownTimer(lifetime);
        _parentVelocity = parentVelocity;
        _randomDx = RandomSingleton.Instance.Next(3) - 1;
        _randomDy = RandomSingleton.Instance.Next(3) - 1;
    }

    public override void Update(double elapsedTime)
    {
        _despawnTimer.Update(elapsedTime);
        if (_despawnTimer.IsFinished)
        {
            Removed = true;
            return;
        }

        double deltaDecay = Math.Pow(_decay, _baseFramerate * elapsedTime);
        double deltaInertia = _inertia * (_baseFramerate * elapsedTime);
        _velocity.X *= deltaDecay;
        _velocity.Y *= deltaDecay;
        _velocity.X += _parentVelocity.X * deltaInertia;
        _velocity.Y += _parentVelocity.Y * deltaInertia;
        _velocity.Z += _gravity * elapsedTime;
        _AttemptMove();
    }

    private void _AttemptMove()
    {
        int steps = (int)(Math.Sqrt(_velocity.HypotenuseSquared) + 1);
        for (int i = 0; i < steps; i++)
        {
            _Move(_velocity.X / steps, 0, 0);
            _Move(0,_velocity.Y / steps, 0);
            _Move(0, 0, _velocity.Z / steps);
        }
    }
    
    private void _Move(double xComponent, double yComponent, double zComponent)
    {
        _position.X += xComponent;
        _position.Y += yComponent;
        _position.Z += zComponent;
    }
    
    protected override void Render(H4D2BitmapCanvas screen, int xCorrected, int yCorrected)
    {
        double opacity = 1 - _despawnTimer.Percentage;
        opacity = Math.Clamp(opacity, _minOpacity, _maxOpacity);
        screen.SetPixelBlend(xCorrected + _randomDx, yCorrected + _randomDy, _color, opacity);
    }
}