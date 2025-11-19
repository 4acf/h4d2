using H4D2.Entities.Mobs.Survivors;
using H4D2.Infrastructure;
using H4D2.Levels;

namespace H4D2.Entities.Projectiles;

public class Tongue : Projectile
{
    public bool IsConnected { get; private set; }

    private const double _speed = 150.0;
    private const int _color = 0x8c5972;
    
    private readonly ReadonlyPosition _startPosition;
    private readonly Survivor _pinTarget;
    
    public Tongue(Level level, Position startPosition, Survivor pinTarget, double directionRadians)
        : base(level, startPosition, ProjectileConfig.TongueBoundingBox, 0, directionRadians)
    {
        _startPosition = startPosition.ReadonlyCopy();
        _pinTarget = pinTarget;
    }

    public void Remove()
    {
        Removed = true;
    }
    
    public override void Update(double elapsedTime)
    {
        if (_pinTarget.Removed)
        {
            Remove();
            return;
        }

        if (IsConnected)
        {
            _velocity.Stop();
            return;
        }
        
        double timeAdjustedSpeed = _speed * elapsedTime;
        _velocity.X = Math.Cos(_directionRadians) * timeAdjustedSpeed;
        _velocity.Y = Math.Sin(_directionRadians) * timeAdjustedSpeed;
        _AttemptMove();
    }

    protected override void Render(Bitmap screen, int xCorrected, int yCorrected)
    {
        double startYCorrected = _startPosition.Y + _startPosition.Z;
        double yCorrectedDouble = _position.Y + _position.Z;
        
        double xDifference = _position.X - _startPosition.X;
        double yDifference = yCorrectedDouble - startYCorrected;
        
        int steps = (int)(Math.Sqrt(xDifference * xDifference + yDifference * yDifference) + 1);
        for (int i = 0; i < steps; i++)
        {
            screen.SetPixel(
                (int)(_startPosition.X + xDifference * i / steps),
                (int)(startYCorrected + yDifference * i / steps),
                _color
            );
        }
    }

    protected override void RenderShadow(ShadowBitmap shadows, int xCorrected, int yCorrected)
    {
        double xCorrectedDouble = _position.X;
        double yCorrectedDouble = _position.Y;
        
        double xDifference = xCorrectedDouble - _startPosition.X;
        double yDifference = yCorrectedDouble - _startPosition.Y;
        
        int steps = (int)(Math.Sqrt(xDifference * xDifference + yDifference * yDifference) + 1);
        for (int i = 0; i < steps; i++)
        {
            int x = (int)(_startPosition.X + xDifference * i / steps);
            int y = (int)(_startPosition.Y + yDifference * i / steps);
            shadows.SetPixel(x, y);
        }
    }

    protected override void _Collide(Entity? entity)
    {
        if (entity == null || entity != _pinTarget)
            return;
        IsConnected = true;
    }
}