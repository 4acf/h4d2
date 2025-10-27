using H4D2.Infrastructure;
using H4D2.Levels;

namespace H4D2.Particles;

public class Smoke : Particle
{
    private readonly int _color;
    private double _timeToLiveSeconds;
    private readonly double _maxLifeSeconds;
    private readonly double _parentXVelocity;
    private readonly double _parentYVelocity;
    private readonly int _randomDx;
    private readonly int _randomDy;
    private const double _gravity = 2.0;
    private const double _decay = 0.5;
    private const double _inertia = 0.1;
    
    public Smoke(Level level, double xPosition, double yPosition, double zPosition, double parentXVelocity, double parentYVelocity, int color)
        : base(level, xPosition, yPosition, zPosition)
    {
        _color = color;
        _timeToLiveSeconds = RandomSingleton.Instance.NextDouble();
        _timeToLiveSeconds = MathHelpers.ClampDouble(_timeToLiveSeconds, 0.1, 0.3);
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

        double deltaDecay = Math.Pow(_decay, 60 * elapsedTime);
        double deltaInertia = _inertia * 60 * elapsedTime;
        _xVelocity *= deltaDecay;
        _yVelocity *= deltaDecay;
        _xVelocity += _parentXVelocity * deltaInertia;
        _yVelocity += _parentYVelocity * deltaInertia;
        _zVelocity += _gravity * elapsedTime;
        _AttemptMove();
    }

    protected void _AttemptMove()
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
        XPosition += xComponent;
        YPosition += yComponent;
        ZPosition += zComponent;
    }
    
    protected override void Render(Bitmap screen, int xCorrected, int yCorrected)
    {
        double opacity = 1 - (_timeToLiveSeconds) / _maxLifeSeconds;
        opacity = MathHelpers.ClampDouble(opacity, 0, 0.5);
        screen.SetPixelBlend(xCorrected + _randomDx, yCorrected + _randomDy, _color, opacity);
    }

    protected override void RenderShadow(Bitmap screen, int xCorrected, int yCorrected)
    {
        // do nothing
    }
}