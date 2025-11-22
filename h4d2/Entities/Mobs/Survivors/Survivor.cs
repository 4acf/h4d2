using H4D2.Entities.Hazards;
using H4D2.Entities.Mobs.Zombies;
using H4D2.Entities.Mobs.Zombies.Specials;
using H4D2.Entities.Pickups;
using H4D2.Entities.Projectiles.ThrowableProjectiles;
using H4D2.Infrastructure;
using H4D2.Infrastructure.H4D2;
using H4D2.Levels;
using H4D2.Particles.Clouds;
using H4D2.Particles.DebrisParticles.Granules;
using H4D2.Weapons;

namespace H4D2.Entities.Mobs.Survivors;

public abstract class Survivor : Mob
{
    public bool IsFullHealth => _health == _maxHealth;
    public bool IsBiled { get; protected set; }
    public bool IsPinned { get; protected set; }
    
    private const int _boundaryTolerance = 25;
    private const int _runSpeed = 300;
    private const int _limpSpeed = 150;
    private const int _walkSpeed = 85;
    private const int _adrenalineRunSpeed = 320;
    private const int _adrenalineEffectSeconds = 15;
    private const int _healthBarRed = 0xe61515;
    private const int _healthBarGreen = 0x56de47;
    private const double _biledDuration = 20.0;
    private const double _bileParticleCooldown = 0.1;
    private const double _gravity = 4.0;
    private const int _jockeyFramesOffset = 23;
    private const int _hunterFramesOffset = 26;
    private const int _smokedFramesOffset = 29;
    private const int _chargedFramesOffset = 32;
    private const int _slamFramesOffset = 37;
    
    private readonly int _character;
    private readonly int _maxHealth;
    protected Weapon? _weapon;
    private Zombie? _target;
    private bool _isShooting;
    private double _aimDirectionRadians;
    private bool _isAdrenalineBoosted;
    private CountdownTimer? _adrenalineTimer;
    private readonly CountdownTimer _biledTimer;
    private readonly CountdownTimer _bileParticleTimer;
    private int _bileOverlayIndex;
    private Special? _pinner;
    private ReadonlyPosition? _tongueOffset;
    
    protected Survivor(Level level, Position position, SurvivorConfig config) 
        : base(level, position, config)
    {
        IsBiled = false;
        IsPinned = false;
        
        _character = config.Character;
        _maxHealth = config.Health;
        _target = null;
        _isShooting = false;
        _isAdrenalineBoosted = false;
        _aimDirectionRadians = 0.0;
        _adrenalineTimer = null;
        _biledTimer = new CountdownTimer(_biledDuration);
        _bileParticleTimer = new CountdownTimer(_bileParticleCooldown);
        _bileOverlayIndex = 0;
        _pinner = null;
        _tongueOffset = null;
    }

    public override void Update(double elapsedTime)
    {
        _UpdateStatusEffects(elapsedTime);
        if (!IsPinned)
        {
            _UpdateTarget();
            _UpdateWeapon(elapsedTime);
            _UpdateSpeed(elapsedTime);
        }
        _UpdatePosition(elapsedTime);
        _UpdateSprite(elapsedTime);
    }

    public void ConsumeFirstAidKit()
    {
        _EmitHealParticles();
        int missingHealth = _maxHealth - _health;
        double healthToRestore = 0.8 * missingHealth;
        _health += (int)healthToRestore;
    }

    public void ConsumePills()
    {
        _EmitHealParticles();
        int missingHealth = _maxHealth - _health;
        int healthToRestore = Math.Min(50, missingHealth);
        _health += healthToRestore;
    }

    public void ConsumeAdrenaline()
    {
        _EmitHealParticles();
        int missingHealth = _maxHealth - _health;
        int healthToRestore = Math.Min(25, missingHealth);
        _health += healthToRestore;
        _isAdrenalineBoosted = true;
        _adrenalineTimer = new CountdownTimer(_adrenalineEffectSeconds);
    }

    public void ThrowMolotov()
    {
        var molotovProjectile
            = new MolotovProjectile(_level, CenterMass.MutableCopy(), _aimDirectionRadians);
        _level.AddProjectile(molotovProjectile);
    }
    
    public void ThrowPipeBomb()
    {
        var pipeBombProjectile 
            = new PipeBombProjectile(_level, CenterMass.MutableCopy(), _aimDirectionRadians);
        _level.AddProjectile(pipeBombProjectile);
    }

    public void ThrowBileBomb()
    {
        var bileBombProjectile
            = new BileBombProjectile(_level, CenterMass.MutableCopy(), _aimDirectionRadians);
        _level.AddProjectile(bileBombProjectile);
    }

    public void Biled()
    {
        if (!IsBiled)
        {
            IsBiled = true;
            _biledTimer.Reset();
            _level.SpawnZombies();
            _bileOverlayIndex = RandomSingleton.Instance.Next(H4D2Art.BileOverlays.Length);
        }
    }

    public void Pinned(Special special)
    {
        IsPinned = true;
        _pinner = special;
        if (special is Jockey)
        {
            _collisionExcludedEntity = special;
        }
    }
    
    public void Cleared()
    {
        IsPinned = false;
        _pinner = null;
        _collisionExcludedEntity = null;
        _tongueOffset = null;
    }
    
    private void _EmitHealParticles()
    {
        var healCloud = new HealCloud(_level, CenterMass.MutableCopy());
        _level.AddParticle(healCloud);
    }

    private void _UpdateStatusEffects(double elapsedTime)
    {
        _hazardDamageTimer.Update(elapsedTime);
        if (IsBiled)
        {
            _bileParticleTimer.Update(elapsedTime);
            if (_bileParticleTimer.IsFinished)
            {
                var footBile = new InvolatileBile(_level, FootPosition.MutableCopy());
                _level.AddParticle(footBile);

                var cmBile = new InvolatileBile(_level, CenterMass.MutableCopy());
                _level.AddParticle(cmBile);
                
                _bileParticleTimer.Reset();
            }
            
            _biledTimer.Update(elapsedTime);
            if (_biledTimer.IsFinished)
            {
                IsBiled = false;
            }
        }
    }
    
    private void _UpdateTarget()
    {
        ReadonlyPosition survivorPosition = CenterMass;
        _target = _level.GetNearestEntity<Zombie>(Position);
        
        if (_target == null || !_target.IsAlive)
        {
            _target = _level.GetNearestEntity<Zombie>(Position);
            if (_target == null) return;
            ReadonlyPosition targetPosition = _target.CenterMass;
            _aimDirectionRadians = Math.Atan2(targetPosition.Y - survivorPosition.Y, targetPosition.X - survivorPosition.X);
            _aimDirectionRadians = MathHelpers.NormalizeRadians(_aimDirectionRadians);
        }
        else
        {
            if (!_target.IsAlive)
            {
                _target = null;
            }
            else
            {
                ReadonlyPosition targetPosition = _target.CenterMass;
                _aimDirectionRadians = Math.Atan2(targetPosition.Y - survivorPosition.Y, targetPosition.X - survivorPosition.X);
                _aimDirectionRadians = MathHelpers.NormalizeRadians(_aimDirectionRadians);
            }
        }
    }
    
    private void _UpdateWeapon(double elapsedTime)
    {
        if (_weapon == null) return;
        _weapon.Update(elapsedTime);
        if (_weapon.CanShoot() && _target != null)
        {
            //_weapon.Shoot(CenterMass.MutableCopy(), _aimDirectionRadians);
            _isShooting = true;
        }
        else
        {
            if(_weapon.AmmoLoaded == 0 || _target == null)
                _isShooting = false;
        }
    }
    
    private void _UpdateSpeed(double elapsedTime)
    {
        if (_adrenalineTimer != null)
        {
            _adrenalineTimer.Update(elapsedTime);
            if (_adrenalineTimer.IsFinished)
            {
                _isAdrenalineBoosted = false;
                _adrenalineTimer = null;
            } 
        }

        if (_isAdrenalineBoosted)
        {
            _speed = _adrenalineRunSpeed;
            return;
        }
        
        _speed = _health switch
        {
            >= 40 => _runSpeed,
            > 1 => _limpSpeed,
            _ => _walkSpeed
        };
    }
    
    private void _UpdatePosition(double elapsedTime)
    {
        if (IsPinned)
        {
            if (_pinner is Jockey or Charger)
                _BindPositionToSpecial(_pinner);
            else if(_pinner is Smoker smoker)
                _UpdateSmokedPosition(smoker);
            return;
        }
        
        if (IsOnGround)
        {
            _velocity.X *= 0.5;
            _velocity.Y *= 0.5;

            double targetDirection = _CalculateBestDirection();
            double directionDiff = targetDirection - _directionRadians;
            directionDiff = Math.Atan2(Math.Sin(directionDiff), Math.Cos(directionDiff));
            _directionRadians += directionDiff * (elapsedTime * _turnSpeed);
            _directionRadians = MathHelpers.NormalizeRadians(_directionRadians);
        
            double moveSpeed = (_speed * _speedFactor) * elapsedTime;
            _velocity.X += Math.Cos(_directionRadians) * moveSpeed;
            _velocity.Y += Math.Sin(_directionRadians) * moveSpeed;
        }
        else
        {
            _velocity.Z -= _gravity * elapsedTime;
        }
        _AttemptMove();
    }

    private void _BindPositionToSpecial(Special special)
    {
        _directionRadians = special.DirectionRadians;
        _position.X = special.Position.X;
        _position.Y = special.Position.Y;
    }

    private void _UpdateSmokedPosition(Smoker smoker)
    {
        if (!smoker.IsTongueConnected)
            return;
        ReadonlyPosition? tonguePositionNullable = smoker.TonguePosition;
        if (tonguePositionNullable == null) return;
        ReadonlyPosition tonguePosition = tonguePositionNullable.Value;
        
        _tongueOffset ??= new ReadonlyPosition(
            tonguePosition.X - _position.X,
            tonguePosition.Y - _position.Y,
            0.0
        );

        if (_tongueOffset == null) return;
        ReadonlyPosition usableTongueOffset = _tongueOffset.Value;

        _position.X = tonguePosition.X - usableTongueOffset.X;
        _position.Y = tonguePosition.Y - usableTongueOffset.Y;
    }
    
    private double _CalculateBestDirection()
    {
        double direction = RandomSingleton.Instance.NextDouble() * (2 * Math.PI);
        direction = _CorrectDirectionToAvoidWalls(direction);
        return direction;
    }

    private double _CorrectDirectionToAvoidWalls(double direction)
    {
        ReadonlyPosition centerMass = CenterMass;
        
        if (centerMass.X < _boundaryTolerance)
        {
            if ((Math.PI / 2) < direction && direction < (3 * Math.PI / 2))
            {
                direction = Math.Atan2(Math.Sin(direction), Math.Cos(direction) * -1);
            }
        }
        
        if (centerMass.Y < _boundaryTolerance)
        {
            if (direction > Math.PI)
            {
                direction = Math.Atan2(Math.Sin(direction) * -1, Math.Cos(direction));
            }
        }

        if (_level.Width - centerMass.X < _boundaryTolerance)
        {
            if ((3 * Math.PI / 2) < direction || direction < (Math.PI / 2))
            {
                direction = Math.Atan2(Math.Sin(direction), Math.Cos(direction) * -1);
            }
        }

        if (_level.Height - centerMass.Y < _boundaryTolerance)
        {
            if (direction < Math.PI)
            {
                direction = Math.Atan2(Math.Sin(direction) * -1, Math.Cos(direction));
            }
        }

        return direction;
    }
    
    private void _UpdateSprite(double elapsedTime)
    {
        _frameUpdateTimer.Update(elapsedTime);
        
        if (IsPinned)
        {
            switch (_pinner)
            {
                case Jockey:
                    _UpdateJockeyedSprite();
                    break;
                case Hunter hunter:
                    _UpdatePouncedSprite(hunter);
                    break;
                case Smoker smoker:
                    _UpdateSmokedSprite(smoker);
                    break;
                case Charger charger:
                    _UpdateChargedSprite(charger);
                    break;
            }
            return;
        }
        
        if (_isShooting)
            _UpdateShootingSprite();
        else
            _UpdateRunningSprite();
    }

    private void _UpdateJockeyedSprite()
    {
        SpriteDirection spriteDirection = Direction.Cardinal(_directionRadians);
        _xFlip = spriteDirection.XFlip;
        
        while (_frameUpdateTimer.IsFinished)
        {
            _walkStep = (_walkStep + 1) % 4;
            int nextFrame = 0;
            if (spriteDirection.Offset == 1)
            {
                nextFrame = _walkStep switch
                {
                    0 => 3,
                    1 or 3 => 4,
                    2 => 5,
                    _ => nextFrame
                };
            }
            else
            {
                nextFrame = _walkStep switch
                {
                    0 or 2 => 0 + (3 * spriteDirection.Offset),
                    1 => 1 + (3 * spriteDirection.Offset),
                    3 => 2 + (3 * spriteDirection.Offset),
                    _ => nextFrame
                };
            }

            _lowerFrame = nextFrame;
            _upperFrame = _jockeyFramesOffset + spriteDirection.Offset;
            _frameUpdateTimer.AddDuration();
        }
    }

    private void _UpdatePouncedSprite(Hunter hunter)
    {
        SpriteDirection spriteDirection = Direction.Cardinal(hunter.DirectionRadians);
        _xFlip = spriteDirection.XFlip;
        _lowerFrame = H4D2Art.Survivors[_character].Length - 1;
        _upperFrame = _hunterFramesOffset + spriteDirection.Offset;
    }

    private void _UpdateSmokedSprite(Smoker smoker)
    {
        SpriteDirection spriteDirection = Direction.Cardinal(_aimDirectionRadians);
        _xFlip = spriteDirection.XFlip;
        
        if (smoker.IsTongueConnected)
        {
            _upperFrame = _smokedFramesOffset + spriteDirection.Offset;
        }
        else
        {
            _upperFrame = _upperBitmapOffset + (spriteDirection.Offset * 3);    
        }
        
        _lowerFrame = spriteDirection.Offset * 3;
    }

    private void _UpdateChargedSprite(Charger charger)
    {
        double directionRadians = charger.IsStumbling ?
            MathHelpers.NormalizeRadians(charger.DirectionRadians + Math.PI) :
            charger.DirectionRadians;
        
        SpriteDirection spriteDirection = Direction.Intercardinal(directionRadians);
        _xFlip = spriteDirection.XFlip;
        
        if (!charger.IsStumbling && charger.IsSlamming)
        {
            while (_frameUpdateTimer.IsFinished)
            {
                int nextFrame = 0;
                nextFrame = charger.SlamStep switch
                {
                    <= 4 or 11 => 2,
                    5 => 1,
                    _ => 0
                };
                _upperFrame = nextFrame + _slamFramesOffset + (spriteDirection.Offset * 3);
                _frameUpdateTimer.AddDuration();
            }
        }
        else
        {
            _upperFrame = _chargedFramesOffset + (spriteDirection.Offset);
        }
        _lowerFrame = H4D2Art.Survivors[_character].Length - 1;
    }
    
    private void _UpdateShootingSprite()
    {
        SpriteDirection spriteDirection = Direction.Intercardinal(_aimDirectionRadians);
        _xFlip = spriteDirection.XFlip;
        
        while (_frameUpdateTimer.IsFinished)
        {
            _walkStep = (_walkStep + 1) % 4;
            if (_velocity.X == 0 && _velocity.Y == 0) _walkStep = 0;
            int nextLowerFrame = 0;
            if (spriteDirection.Offset == 2)
            {
                nextLowerFrame = _walkStep switch
                {
                    0 => 3,
                    1 or 3 => 4,
                    2 => 5,
                    _ => nextLowerFrame
                };
            }
            else
            {
                nextLowerFrame = _walkStep switch
                {
                    0 or 2 => 0,
                    1 => 1,
                    3 => 2,
                    _ => nextLowerFrame
                };
            }
            _lowerFrame = nextLowerFrame;
            _upperFrame = _attackingBitmapOffset + spriteDirection.Offset;
            _frameUpdateTimer.AddDuration();
        }
    }
    
    private void _UpdateRunningSprite()
    {
        SpriteDirection spriteDirection = Direction.Cardinal(_directionRadians);
        _xFlip = spriteDirection.XFlip;
        
        while (_frameUpdateTimer.IsFinished)
        {
            _walkStep = (_walkStep + 1) % 4;
            if (_velocity.X == 0 && _velocity.Y == 0) _walkStep = 0;
            int nextFrame = 0;
            if (spriteDirection.Offset == 1)
            {
                nextFrame = _walkStep switch
                {
                    0 => 3,
                    1 or 3 => 4,
                    2 => 5,
                    _ => nextFrame
                };
            }
            else
            {
                nextFrame = _walkStep switch
                {
                    0 or 2 => 0 + (3 * spriteDirection.Offset),
                    1 => 1 + (3 * spriteDirection.Offset),
                    3 => 2 + (3 * spriteDirection.Offset),
                    _ => nextFrame
                };
            }

            _lowerFrame = nextFrame;
            _upperFrame = nextFrame + _upperBitmapOffset;
            _frameUpdateTimer.AddDuration();
        }
    }
    
    protected override void Render(Bitmap screen, int xCorrected, int yCorrected)
    {
        _RenderHealthBar(screen, xCorrected, yCorrected);
        
        Bitmap lowerBitmap = H4D2Art.Survivors[_character][_lowerFrame];
        Bitmap upperBitmap = H4D2Art.Survivors[_character][_upperFrame];

        if (IsBiled)
        {
            Bitmap bileOverlay = H4D2Art.BileOverlays[_bileOverlayIndex];
            screen.DrawBiledCharacter(lowerBitmap, bileOverlay, xCorrected, yCorrected, _xFlip);
            screen.DrawBiledCharacter(upperBitmap, bileOverlay, xCorrected, yCorrected, _xFlip);
        }
        else
        {
            screen.Draw(lowerBitmap, xCorrected, yCorrected, _xFlip);
            screen.Draw(upperBitmap, xCorrected, yCorrected, _xFlip);
        }
    }

    private void _RenderHealthBar(Bitmap screen, int xCorrected, int yCorrected)
    {
        double percentage = _health / (double)_maxHealth;
        int greenPixels = (int)Math.Ceiling(percentage * 10);
        
        screen.Fill(
            xCorrected + H4D2Art.SpriteSize - 13 + greenPixels,
            yCorrected - 2,
            xCorrected + H4D2Art.SpriteSize - 4,
            yCorrected - 2,
            _healthBarRed
        );
        
        screen.Fill(
            xCorrected + H4D2Art.SpriteSize - 13,
            yCorrected - 2,
            xCorrected + H4D2Art.SpriteSize - 14 + greenPixels,
            yCorrected - 2,
            _healthBarGreen
        );
    }
    
    protected override void RenderShadow(ShadowBitmap shadows, int xCorrected, int yCorrected)
    {
        shadows.Fill(
            xCorrected + H4D2Art.SpriteSize - 10,
            yCorrected - H4D2Art.SpriteSize - 1,
            xCorrected + H4D2Art.SpriteSize - 7,
            yCorrected - H4D2Art.SpriteSize - 1
        );
    }

    protected override void _Collide(Entity? entity)
    {
        switch (entity)
        {
            case Pickup pickup:
                pickup.PickUp(this);
                break;
            case Hazard hazard:
                _TakeHazardDamage(hazard.Damage);
                break;
            case Witch witch:
                witch.Alert();
                break;
            default:
                base._Collide(entity);
                break;
        }
    }
}