using H4D2.Infrastructure;
using H4D2.Levels;

namespace H4D2.Particles;
using Cfg = ParticleConfig;

public abstract class Debris : Particle
{
    protected const double _gravity = 4.8;
    protected const double _groundFriction = 0.85;
    private const double _minLifetime = 0.6;
    private const double _maxLifetime = 1.0;
    
    protected readonly double _drag;
    protected readonly double _bounce;
    protected double _timeToLiveSeconds;
    
    protected Debris(Level level, double xPosition, double yPosition, double zPosition, double drag, double bounce)
        : base(level, xPosition, yPosition, zPosition)
    {
        _drag = drag;
        _bounce = bounce;
        _timeToLiveSeconds = RandomSingleton.Instance.NextDouble();
        _timeToLiveSeconds = MathHelpers.ClampDouble(_timeToLiveSeconds, _minLifetime, _maxLifetime);

        do
        {
            _xVelocity = (RandomSingleton.Instance.NextDouble() * 2) - 1;
            _yVelocity = (RandomSingleton.Instance.NextDouble() * 2) - 1;
            _zVelocity = (RandomSingleton.Instance.NextDouble() * 2) - 1;
        } while(_xVelocity * _xVelocity + _yVelocity * _yVelocity + _zVelocity * _zVelocity > 1);
        double hypotenuse = Math.Sqrt(_xVelocity * _xVelocity + _yVelocity * _yVelocity + _zVelocity * _zVelocity);
        _xVelocity /= hypotenuse;
        _yVelocity /= hypotenuse;
        _zVelocity /= hypotenuse;
    }

    public override void Update(double elapsedTime)
    {
        _timeToLiveSeconds -= elapsedTime;
        if (_timeToLiveSeconds <= 0)
        {
            Removed = true;
            return;
        }

        double elapsedTimeConstant = Cfg.BaseFramerate * elapsedTime;
        if (IsOnGround)
        {
            _xVelocity *= Math.Pow(_groundFriction, elapsedTimeConstant);
            _yVelocity *= Math.Pow(_groundFriction, elapsedTimeConstant);
        }
        else
        {
            _xVelocity *= Math.Pow(_drag, elapsedTimeConstant);
            _yVelocity *= Math.Pow(_drag, elapsedTimeConstant);
        }
        _zVelocity -= _gravity * elapsedTime;
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

    private bool _IsOutOfLevelBounds(double xPosition, double yPosition, double zPosition)
    {
        return
            xPosition < 0 ||
            xPosition > _level.Width ||
            yPosition < 0 ||
            yPosition > _level.Height ||
            zPosition < 0;
    }
    
    private void _Move(double xComponent, double yComponent, double zComponent)
    {
        double xDest = XPosition + xComponent;
        double yDest = YPosition + yComponent;
        double zDest = ZPosition + zComponent;

        if (_IsOutOfLevelBounds(xDest, yDest, zDest))
        {
            if (zDest < 0) ZPosition = 0;
            _Collide(xComponent, yComponent, zComponent);
            return;
        }
        
        XPosition = xDest;
        YPosition = yDest;
        ZPosition = zDest;
    }

    private void _Collide(double xComponent, double yComponent, double zComponent)
    {
        if (xComponent != 0) _xVelocity *= _bounce * -1;
        if (yComponent != 0) _yVelocity *= _bounce * -1;
        if (zComponent != 0) _zVelocity *= _bounce * -1;
    }
}