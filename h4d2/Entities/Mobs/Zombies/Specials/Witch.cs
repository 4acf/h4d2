using H4D2.Entities.Mobs.Survivors;
using H4D2.Entities.Projectiles;
using H4D2.Infrastructure;
using H4D2.Infrastructure.H4D2;
using H4D2.Levels;

namespace H4D2.Entities.Mobs.Zombies.Specials;

public class Witch : Special
{
    private const int _idleFrame = 9;
    private const int _alertedFrame = 0;
    private const double _attackRange = 5.0;
    private const double _attackDelay = 2.0;
    private const double _chaseDelay = 2.0;
    
    private bool _isAlerted;
    private readonly CountdownTimer _cooldownBeforeChasingTimer;
    private readonly CountdownTimer _attackDelayTimer;
    
    public Witch(Level level, Position position) 
        : base(level, position, SpecialConfigs.Witch)
    {
        _frame = _idleFrame;
        _isAlerted = false;
        _cooldownBeforeChasingTimer = new CountdownTimer(_chaseDelay);
        _attackDelayTimer = new CountdownTimer(_attackDelay);
    }

    public void Alert()
    {
        _isAlerted = true;
        (int audioX, int audioY) = AudioLocation;
        AudioManager.Instance.PlaySFX(SFX.WitchAlert, audioX, audioY);
    }

    public override void HitBy(Projectile projectile)
    {
        base.HitBy(projectile);
        if(!_isAlerted)
            Alert();
    }

    public override void HitBy(Zombie zombie)
    {
        base.HitBy(zombie);
        if(!_isAlerted)
            Alert();
    }

    protected override void _TakeHazardDamage(int damage)
    {
        base._TakeHazardDamage(damage);
        if(!_isAlerted)
            Alert();
    }

    public override void Update(double elapsedTime)
    {
        if(_isAlerted && !_cooldownBeforeChasingTimer.IsFinished)
            _cooldownBeforeChasingTimer.Update(elapsedTime);
        base.Update(elapsedTime);
    }

    protected override void _UpdateAttackState(double elapsedTime)
    {
        if (!_isAlerted)
            return;
        
        _attackDelayTimer.Update(elapsedTime);
        if (_target == null || _target.Removed)
            return;

        if (_target is not Survivor survivor)
            return;
        
        ReadonlyPosition targetPosition = _target.CenterMass;
        ReadonlyPosition zombiePosition = CenterMass;
        double distance = ReadonlyPosition.Distance(targetPosition, zombiePosition);
        
        if (distance > _attackRange)
            return;
        
        if (_attackDelayTimer.IsFinished)
        {
            survivor.HitBy(this);
            _attackDelayTimer.Reset();
        }
    }

    protected override void _UpdatePosition(double elapsedTime)
    {
        if (!_isAlerted || !_cooldownBeforeChasingTimer.IsFinished)
        {
            _velocity.Stop();
            _AttemptMove();
            return;
        }
        base._UpdatePosition(elapsedTime);
    }

    protected override void _UpdateSprite(double elapsedTime)
    {
        if (!_isAlerted)
        {
            _frame = _idleFrame;
            return;
        }

        if (!_cooldownBeforeChasingTimer.IsFinished)
        {
            _frame = _alertedFrame;
            return;
        }
        
        base._UpdateSprite(elapsedTime);
    }
}