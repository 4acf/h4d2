using H4D2.Entities.Mobs.Survivors;
using H4D2.Entities.Projectiles;
using H4D2.Infrastructure;
using H4D2.Infrastructure.H4D2;
using H4D2.Levels;

namespace H4D2.Entities.Mobs.Zombies.Specials;

public class Smoker : Special
{
    public bool IsTongueConnected => _tongue is { IsConnected: true };
    public ReadonlyPosition? TonguePosition => _tongue?.Position;
    
    private const int _pullFramesOffset = 9;
    private const int _scratchFramesOffset = 19;
    private const double _tongueRange = 100.0;
    private const double _scratchRange = 5.0;
    private const double _pullCooldown = 15.0;
    private const double _pullAttackDelay = 0.75;
    private const double _scratchAttackDelay = 0.45;

    private int _pullStep;
    private int _scratchStep;
    private bool _isPulling;
    private bool _isScratching;
    private bool _isPinning => _isPulling || _isScratching;
    private readonly CountdownTimer _pullCooldownTimer;
    private readonly CountdownTimer _pullAttackTimer;
    private readonly CountdownTimer _scratchAttackTimer;
    private Survivor? _pinTarget;
    private Tongue? _tongue;
    
    public Smoker(Level level, Position position) 
        : base(level, position, SpecialConfigs.Smoker)
    {
        _pullStep = -1;
        _scratchStep = 0;
        _isPulling = false;
        _isScratching = false;
        _pullCooldownTimer = new CountdownTimer(_pullCooldown);
        _pullCooldownTimer.Update(_pullCooldown);
        _pullAttackTimer = new CountdownTimer(_pullAttackDelay);
        _scratchAttackTimer = new CountdownTimer(_scratchAttackDelay);
        _pinTarget = null;
        _tongue = null;
    }

    public override void Update(double elapsedTime)
    {
        base.Update(elapsedTime);
        // SMOKE PARTICLES
    }

    protected override void _UpdateAttackState(double elapsedTime)
    {
        _pullCooldownTimer.Update(elapsedTime);
        if (!_pullCooldownTimer.IsFinished)
            return;
        
        if (_target == null || _target.Removed || _target is not Survivor survivor)
            return;
        
        if (_isPinning)
        {
            _UpdatePinState(elapsedTime);
            return;
        }
        
        ReadonlyPosition targetPosition = survivor.CenterMass;
        ReadonlyPosition zombiePosition = CenterMass;
        double distance = ReadonlyPosition.Distance(targetPosition, zombiePosition);
        
        if (distance > _tongueRange)
            return;
        
        _directionRadians = Math.Atan2(targetPosition.Y - zombiePosition.Y, targetPosition.X - zombiePosition.X);
        _directionRadians = MathHelpers.NormalizeRadians(_directionRadians);

        _Pin(survivor);
    }

    private void _UpdatePinState(double elapsedTime)
    {
        if (_pinTarget == null || _pinTarget.Removed)
        {
            _isPulling = false;
            _isScratching = false;
            _pinTarget = null;
            _tongue!.Remove();
            _pullCooldownTimer.Reset();
            return;   
        }
        
        if (_isPulling)
        {
            _pullAttackTimer.Update(elapsedTime);
            if (_pullAttackTimer.IsFinished && _tongue!.IsConnected)
            {
                _pinTarget.HitBy(this);
                _pullAttackTimer.Reset();
            }
            
            ReadonlyPosition targetPosition = _pinTarget.CenterMass;
            ReadonlyPosition zombiePosition = CenterMass;
            double distance = ReadonlyPosition.Distance(targetPosition, zombiePosition);
        
            if (distance > _scratchRange)
                return;

            _tongue!.StopPulling();
            _isPulling = false;
            _isScratching = true;
            
            return;
        }
        
        _scratchAttackTimer.Update(elapsedTime);
        if (_scratchAttackTimer.IsFinished)
        {
            _pinTarget.HitBy(this);
            _scratchAttackTimer.Reset();
        }
    }
    
    protected override void _UpdatePosition(double elapsedTime)
    {
        if (_isPinning)
        {
            _velocity.Stop();
        }
        else
        {
            base._UpdatePosition(elapsedTime);
        }
    }

    protected override void _UpdateSprite(double elapsedTime)
    {
        if (_isPulling)
        {
            _UpdatePullSprite(elapsedTime);
        }
        else if (_isScratching)
        {
            _UpdateScratchSprite(elapsedTime);
        }
        else
        {
            _pullStep = -1;
            base._UpdateSprite(elapsedTime);
        }
    }

    private void _UpdatePullSprite(double elapsedTime)
    {
        _frameUpdateTimer.Update(elapsedTime);

        SpriteDirection spriteDirection = Direction.Intercardinal(_directionRadians);
        _xFlip = spriteDirection.XFlip;

        while (_frameUpdateTimer.IsFinished)
        {
            _pullStep = _pullStep == 1 ? _pullStep : _pullStep + 1;
            _frame = _pullFramesOffset + (_pullStep + (2 * spriteDirection.Offset));
            _frameUpdateTimer.AddDuration();
        }
    }

    private void _UpdateScratchSprite(double elapsedTime)
    {
        _frameUpdateTimer.Update(elapsedTime);
        
        SpriteDirection spriteDirection = Direction.Intercardinal(_directionRadians);
        _xFlip = spriteDirection.XFlip;

        while (_frameUpdateTimer.IsFinished)
        {
            _scratchStep = _scratchStep == 0 ? 1 : 0;
            _frame = _scratchFramesOffset + _scratchStep + (spriteDirection.Offset * 2);
            _frameUpdateTimer.AddDuration();
        }
    }
    
    private void _Pin(Survivor survivor)
    {
        _tongue = new Tongue(_level, CenterMass.MutableCopy(), survivor, _directionRadians);
        _level.AddProjectile(_tongue);
        _isPulling = true;
        _pinTarget = survivor;
        survivor.Pinned(this);
    }
    
    protected override void _Die()
    {
        base._Die();
        _pinTarget?.Cleared();
        _tongue?.Remove();
        // SMOKE PARTICLES
    }

    protected override void RenderShadow(ShadowBitmap shadows, int xCorrected, int yCorrected)
    {
        shadows.Fill(
            xCorrected + H4D2Art.SpriteSize - 10,
            yCorrected - H4D2Art.SpriteSize,
            xCorrected + H4D2Art.SpriteSize - 7,
            yCorrected - H4D2Art.SpriteSize
        );
    }
}