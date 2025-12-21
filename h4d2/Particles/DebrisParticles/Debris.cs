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
            _velocity.X = (RandomSingleton.Instance.NextDouble() * 2) - 1;
            _velocity.Y = (RandomSingleton.Instance.NextDouble() * 2) - 1;
            _velocity.Z = (RandomSingleton.Instance.NextDouble() * 2) - 1;
        } while(_velocity.HypotenuseSquared > 1);
        double hypotenuse = Math.Sqrt(_velocity.HypotenuseSquared);
        _velocity.X /= hypotenuse;
        _velocity.Y /= hypotenuse;
        _velocity.Z /= hypotenuse;
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
            _velocity.X *= Math.Pow(_groundFriction, elapsedTimeConstant);
            _velocity.Y *= Math.Pow(_groundFriction, elapsedTimeConstant);
        }
        else
        {
            _velocity.X *= Math.Pow(_drag, elapsedTimeConstant);
            _velocity.Y *= Math.Pow(_drag, elapsedTimeConstant);
        }
        _velocity.Z -= _gravity * elapsedTime;
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
        var destination = new ReadonlyPosition(
            _position.X + xComponent,
            _position.Y + yComponent,
            _position.Z + zComponent
        );

        if (destination.Z < 0)
        {
            _CollideFloor();
            _position.Z = 0;
            return;
        }
        
        if (_level.IsBlockedByWall(destination))
        {
            _CollideWall();
            return;
        }
        
        _position.X = destination.X;
        _position.Y = destination.Y;
        _position.Z = destination.Z;
    }

    private void _CollideFloor()
    {
        _velocity.Z *= _bounce * -1;
    }

    private void _CollideWall()
    {
        _velocity.X *= _bounce * -1;
        _velocity.Y *= _bounce * -1;
    }
}