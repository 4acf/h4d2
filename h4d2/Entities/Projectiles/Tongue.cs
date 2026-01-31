using H4D2.Entities.Mobs.Survivors;
using H4D2.Infrastructure;
using H4D2.Infrastructure.H4D2;
using H4D2.Levels;
using H4D2.Particles;

namespace H4D2.Entities.Projectiles;

public class Tongue : Projectile
{
    public bool IsConnected { get; private set; }

    public const int Color = 0x8c5972;
    private const double _shootSpeed = 250.0;
    private const double _pullSpeed = 25.0;

    private bool _isStopped;
    private double _speed;
    private readonly ReadonlyPosition _startPosition;
    private readonly Survivor _pinTarget;
    private readonly List<(int, int)> _tonguePixels;
    private readonly List<(int, int)> _tongueShadowPixels;
    private readonly Queue<TongueSegment> _waitingToBeAdded;
    private readonly Stack<TongueSegment> _waitingToBeRemoved;
    
    public Tongue(Level level, Position startPosition, Survivor pinTarget, double directionRadians)
        : base(level, startPosition, ProjectileConfig.TongueBoundingBox, 0, directionRadians)
    {
        _isStopped = false;
        _speed = _shootSpeed;
        _startPosition = startPosition.ReadonlyCopy();
        _pinTarget = pinTarget;
        
        _tonguePixels = [];
        _tongueShadowPixels = [];
        _waitingToBeAdded = [];
        _waitingToBeRemoved = [];
        _PrecalculateTongueSegments();
    }

    private void _PrecalculateTongueSegments()
    {
        ReadonlyPosition targetPosition = _pinTarget.CenterMass;
        
        double xDifference = targetPosition.X - _startPosition.X;
        double yDifference = targetPosition.Y - _startPosition.Y;
        
        int steps = (int)(Math.Sqrt(xDifference * xDifference + yDifference * yDifference) + 1);
        steps *= 2;
        for (int i = 0; i < steps; i++)
        {
            var position = new Position(
                _startPosition.X + xDifference * i / steps,
                _startPosition.Y + yDifference * i / steps,
                _startPosition.Z
            );
            var tongueSegment = new TongueSegment(_level, position);
            _waitingToBeAdded.Enqueue(tongueSegment);
        }
    }
    
    public void Remove()
    {
        Removed = true;
        while (_waitingToBeAdded.Count > 0)
        {
            TongueSegment tongueSegment = _waitingToBeAdded.Dequeue();
            tongueSegment.Remove();
        }

        while (_waitingToBeRemoved.Count > 0)
        {
            TongueSegment tongueSegment = _waitingToBeRemoved.Pop();
            tongueSegment.Remove();
        }
    }

    public void StopPulling()
    {
        _isStopped = true;
    }
    
    public override void Update(double elapsedTime)
    {
        if (_pinTarget.Removed)
        {
            Remove();
            return;
        }

        if (_isStopped)
            return;
        
        double directionRadians = DirectionRadians;
        if (IsConnected)
        {
            directionRadians += Math.PI;
            directionRadians = MathHelpers.NormalizeRadians(directionRadians);
        }
        
        double timeAdjustedSpeed = _speed * elapsedTime;
        _velocity.X = Math.Cos(directionRadians) * timeAdjustedSpeed;
        _velocity.Y = Math.Sin(directionRadians) * timeAdjustedSpeed;
        _AttemptMove();
        _UpdateSegments();
    }

    private void _UpdateSegments()
    {
        double distanceToEnd = ReadonlyPosition.Distance(_startPosition, Position);

        if (!IsConnected)
        {
            while (
                _waitingToBeAdded.Count > 0 && 
                ReadonlyPosition.Distance(_startPosition, _waitingToBeAdded.Peek().Position) <= distanceToEnd
            )
            {
                TongueSegment tongueSegment = _waitingToBeAdded.Dequeue();
                _level.AddParticle(tongueSegment);
                _waitingToBeRemoved.Push(tongueSegment);
            }

            return;
        }

        while (
            _waitingToBeRemoved.Count > 0 &&
            ReadonlyPosition.Distance(_startPosition, _waitingToBeRemoved.Peek().Position) >= distanceToEnd
        )
        {
            TongueSegment tongueSegment = _waitingToBeRemoved.Pop();
            tongueSegment.Remove();
        }
    }
    
    protected override void Render(H4D2BitmapCanvas screen, int xCorrected, int yCorrected)
    {
        screen.SetPixel(xCorrected, yCorrected, Color);
    }

    protected override void RenderShadow(ShadowBitmap shadows, int xCorrected, int yCorrected)
    {
        shadows.SetPixel(xCorrected, yCorrected);
    }

    protected override void _Collide(Entity? entity)
    {
        if (entity == null || entity != _pinTarget)
            return;
        IsConnected = true;
        _collisionExcludedEntity = _pinTarget;
        _speed = _pullSpeed;
    }

    protected override void _CollideWall(double xComponent, double yComponent, double zComponent)
    {
        if (!IsConnected)
        {
            _pinTarget.Cleared();
            Remove();
        }
    }
}