using H4D2.Infrastructure;
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
    private double _timeToLiveSeconds;
    private readonly double _maxLifeSeconds;
    private readonly double _parentXVelocity;
    private readonly double _parentYVelocity;
    private readonly int _randomDx;
    private readonly int _randomDy;
    
    public Smoke(Level level, Position position, double parentXVelocity, double parentYVelocity)
        : base(level, position)
    {
        _timeToLiveSeconds = RandomSingleton.Instance.NextDouble();
        _timeToLiveSeconds = MathHelpers.ClampDouble(_timeToLiveSeconds, _minLifetime, _maxLifetime);
        _maxLifeSeconds = _timeToLiveSeconds;
        _parentXVelocity = parentXVelocity;
        _parentYVelocity = parentYVelocity;
        _randomDx = RandomSingleton.Instance.Next(3) - 1;
        _randomDy = RandomSingleton.Instance.Next(3) - 1;
    }

    public override void Update(double elapsedTime)
    {
        _timeToLiveSeconds -= elapsedTime;
        if (_timeToLiveSeconds <= 0)
        {
            Removed = true;
            return;
        }

        double deltaDecay = Math.Pow(_decay, _baseFramerate * elapsedTime);
        double deltaInertia = _inertia * (_baseFramerate * elapsedTime);
        _xVelocity *= deltaDecay;
        _yVelocity *= deltaDecay;
        _xVelocity += _parentXVelocity * deltaInertia;
        _yVelocity += _parentYVelocity * deltaInertia;
        _zVelocity += _gravity * elapsedTime;
        _AttemptMove();
    }

    private void _AttemptMove()
    {
        int steps = (int)(Math.Sqrt(_xVelocity * _xVelocity + _yVelocity * _yVelocity + _zVelocity * _zVelocity) + 1);
        for (int i = 0; i < steps; i++)
        {
            _Move(_xVelocity / steps, 0, 0);
            _Move(0,_yVelocity / steps, 0);
            _Move(0, 0, _zVelocity / steps);
        }
    }
    
    private void _Move(double xComponent, double yComponent, double zComponent)
    {
        _position.X += xComponent;
        _position.Y += yComponent;
        _position.Z += zComponent;
    }
    
    protected override void Render(Bitmap screen, int xCorrected, int yCorrected)
    {
        double opacity = 1 - (_timeToLiveSeconds) / _maxLifeSeconds;
        opacity = MathHelpers.ClampDouble(opacity, _minOpacity, _maxOpacity);
        screen.SetPixelBlend(xCorrected + _randomDx, yCorrected + _randomDy, _color, opacity);
    }
}