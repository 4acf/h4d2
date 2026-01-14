using H4D2.Entities.Mobs.Survivors;
using H4D2.Infrastructure;
using H4D2.Infrastructure.H4D2;
using H4D2.Levels;

namespace H4D2.Entities.Projectiles;

public class Tongue : Projectile
{
    public bool IsConnected { get; private set; }

    private const double _shootSpeed = 250.0;
    private const double _pullSpeed = 25.0;
    private const int _color = 0x8c5972;

    private bool _isStopped;
    private double _speed;
    private readonly ReadonlyPosition _startPosition;
    private readonly Survivor _pinTarget;
    private readonly List<(int, int)> _tonguePixels;
    private readonly List<(int, int)> _tongueShadowPixels;
    
    public Tongue(Level level, Position startPosition, Survivor pinTarget, double directionRadians)
        : base(level, startPosition, ProjectileConfig.TongueBoundingBox, 0, directionRadians)
    {
        _isStopped = false;
        _speed = _shootSpeed;
        _startPosition = startPosition.ReadonlyCopy();
        _pinTarget = pinTarget;
        
        _tonguePixels = [];
        _tongueShadowPixels = [];
        _PrecalculateTonguePixels();
        _PrecalculateTongueShadowPixels();
    }

    private void _PrecalculateTonguePixels()
    {
        double startXCorrected = (_startPosition.X - _startPosition.Y) * ScaleX;
        double startYCorrected = ((_startPosition.X + _startPosition.Y) * ScaleY) + _startPosition.Z;

        ReadonlyPosition targetPosition = _pinTarget.CenterMass;
        double targetXCorrected = (targetPosition.X - targetPosition.Y) * ScaleX;
        double targetYCorrected = ((targetPosition.X + targetPosition.Y) * ScaleY) + targetPosition.Z;
        
        double xDifference = targetXCorrected - startXCorrected;
        double yDifference = targetYCorrected - startYCorrected;
        
        int steps = (int)(Math.Sqrt(xDifference * xDifference + yDifference * yDifference) + 1);
        for (int i = 0; i < steps; i++)
        {
            _tonguePixels.Add
            ((
                (int)(startXCorrected + xDifference * i / steps),
                (int)(startYCorrected + yDifference * i / steps)
            ));
        }
    }
    
    private void _PrecalculateTongueShadowPixels()
    {
        double startXCorrected = (_startPosition.X - _startPosition.Y) * ScaleX;
        double startYCorrected = ((_startPosition.X + _startPosition.Y) * ScaleY);

        ReadonlyPosition targetPosition = _pinTarget.CenterMass;
        double targetXCorrected = (targetPosition.X - targetPosition.Y) * ScaleX;
        double targetYCorrected = ((targetPosition.X + targetPosition.Y) * ScaleY);
        
        double xDifference = targetXCorrected - startXCorrected;
        double yDifference = targetYCorrected - startYCorrected;
        
        int steps = (int)(Math.Sqrt(xDifference * xDifference + yDifference * yDifference) + 1);
        for (int i = 0; i < steps; i++)
        {
            _tongueShadowPixels.Add
            ((
                (int)(startXCorrected + xDifference * i / steps),
                (int)(startYCorrected + yDifference * i / steps)
            ));
        }
    }
    
    public void Remove()
    {
        Removed = true;
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
    }

    protected override void Render(H4D2BitmapCanvas screen, int xCorrected, int yCorrected)
    {
        if (_tonguePixels.Count == 0)
            return;
        
        double distanceToEnd = MathHelpers.Distance(
            xCorrected,
            yCorrected,
            _tonguePixels[0].Item1,
            _tonguePixels[0].Item2
        );
        
        for (int i = 0; i < _tonguePixels.Count; i++)
        {
            double distanceToPixel = MathHelpers.Distance(
                _tonguePixels[0].Item1,
                _tonguePixels[0].Item2,
                _tonguePixels[i].Item1,
                _tonguePixels[i].Item2
            );
            if (distanceToPixel > distanceToEnd)
                return;
            screen.SetPixel(_tonguePixels[i].Item1, _tonguePixels[i].Item2, _color);
        }
    }

    protected override void RenderShadow(ShadowBitmap shadows, int xCorrected, int yCorrected)
    {
        if (_tongueShadowPixels.Count == 0)
            return;
        
        double distanceToEnd = MathHelpers.Distance(
            xCorrected,
            yCorrected,
            _tongueShadowPixels[0].Item1,
            _tongueShadowPixels[0].Item2
        );
        
        for (int i = 0; i < _tongueShadowPixels.Count; i++)
        {
            double distanceToPixel = MathHelpers.Distance(
                _tongueShadowPixels[0].Item1,
                _tongueShadowPixels[0].Item2,
                _tongueShadowPixels[i].Item1,
                _tongueShadowPixels[i].Item2
            );
            if (distanceToPixel > distanceToEnd)
                return;
            shadows.SetPixel(_tongueShadowPixels[i].Item1, _tongueShadowPixels[i].Item2);
        }
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