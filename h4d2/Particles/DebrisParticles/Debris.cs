using H4D2.Infrastructure;
using H4D2.Levels;

namespace H4D2.Particles.DebrisParticles;

public abstract class Debris : Particle
{
    protected const double _gravity = 4.8;
    protected const double _groundFriction = 0.85;
    
    protected readonly double _drag;
    protected readonly double _bounce; 
    protected readonly CountdownTimer _despawnTimer;
    
    protected Debris(Level level, Position position, DebrisConfig config)
        : base(level, position)
    {
        _drag = config.Drag;
        _bounce = config.Bounce;
        double t = RandomSingleton.Instance.NextDouble();
        double lifetime = config.MinLifetime + (t * (config.MaxLifetime - config.MinLifetime));
        _despawnTimer = new CountdownTimer(lifetime);
        
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
        _despawnTimer.Update(elapsedTime);
        if (_despawnTimer.IsFinished)
        {
            Removed = true;
            return;
        }

        double elapsedTimeConstant = _baseFramerate * elapsedTime;
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
        double xDest = _position.X + xComponent;
        double yDest = _position.Y + yComponent;
        double zDest = _position.Z + zComponent;

        if (_IsOutOfLevelBounds(xDest, yDest, zDest))
        {
            if (zDest < 0) _position.Z = 0;
            _Collide(xComponent, yComponent, zComponent);
            return;
        }
        
        _position.X = xDest;
        _position.Y = yDest;
        _position.Z = zDest;
    }

    private void _Collide(double xComponent, double yComponent, double zComponent)
    {
        if (xComponent != 0) _xVelocity *= _bounce * -1;
        if (yComponent != 0) _yVelocity *= _bounce * -1;
        if (zComponent != 0) _zVelocity *= _bounce * -1;
    }
}