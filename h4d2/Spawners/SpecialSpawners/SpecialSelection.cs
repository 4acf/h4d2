using H4D2.Entities;
using H4D2.Entities.Mobs.Zombies.Specials;
using H4D2.Entities.Mobs.Zombies.Specials.Pinners;
using H4D2.Infrastructure;
using H4D2.Infrastructure.H4D2;
using H4D2.Levels;

namespace H4D2.Spawners.SpecialSpawners;

public class SpecialSelection : ISpecialSelectionView
{
    public int IndexInArray { get; }
    public Bitmap Bitmap => H4D2Art.GUI.SpecialProfiles[SpecialIndex];
    public int Cost { get; }
    public double PercentageRemaining => _cooldownTimer.Percentage;
    
    public readonly int SpecialIndex;
    public readonly BoundingBox BoundingBox;
    public ReadonlyPosition CenterMass => BoundingBox.CenterMass(_mousePosition.ReadonlyCopy());
    private readonly CountdownTimer _cooldownTimer;
    private readonly Position _mousePosition;
    
    public SpecialSelection(int i, SpecialDescriptor descriptor, BuyInfo buyInfo, Position mousePosition)
    {
        IndexInArray = i;
        Cost = buyInfo.Cost;
        _mousePosition = mousePosition;
        _cooldownTimer = new CountdownTimer(buyInfo.CooldownSeconds);
        _cooldownTimer.Update(buyInfo.CooldownSeconds);

        SpecialIndex = descriptor switch
        {
            SpecialDescriptor.Hunter => SpecialIndices.Hunter,
            SpecialDescriptor.Boomer => SpecialIndices.Boomer,
            SpecialDescriptor.Smoker => SpecialIndices.Smoker,
            SpecialDescriptor.Charger => SpecialIndices.Charger,
            SpecialDescriptor.Jockey => SpecialIndices.Jockey,
            SpecialDescriptor.Spitter => SpecialIndices.Spitter,
            SpecialDescriptor.Tank => SpecialIndices.Tank,
            _ => SpecialIndices.Witch
        };
        
        BoundingBox = descriptor switch
        {
            SpecialDescriptor.Hunter => SpecialBoundingBoxes.Hunter,
            SpecialDescriptor.Boomer => SpecialBoundingBoxes.Boomer,
            SpecialDescriptor.Smoker => SpecialBoundingBoxes.Smoker,
            SpecialDescriptor.Charger => SpecialBoundingBoxes.Charger,
            SpecialDescriptor.Jockey => SpecialBoundingBoxes.Jockey,
            SpecialDescriptor.Spitter => SpecialBoundingBoxes.Spitter,
            SpecialDescriptor.Tank => SpecialBoundingBoxes.Tank,
            _ => SpecialBoundingBoxes.Witch
        };
    }
    
    public void UpdateCooldown(double elapsedTime)
    {
        if (_cooldownTimer.IsFinished)
            return;
        _cooldownTimer.Update(elapsedTime);
    }
    
    public bool IsBuyable(int balance)
    {
        return balance >= Cost && _cooldownTimer.IsFinished;
    }
    
    public bool HasLineOfSight(Level level, Entity target)
    {
        ReadonlyPosition readonlyMousePosition = _mousePosition.ReadonlyCopy();
        return 
            level.HasLineOfSight(BoundingBox.NWPosition(readonlyMousePosition), target.NWPosition) && 
            level.HasLineOfSight(BoundingBox.NEPosition(readonlyMousePosition), target.NEPosition) && 
            level.HasLineOfSight(BoundingBox.SWPosition(readonlyMousePosition), target.SWPosition) && 
            level.HasLineOfSight(BoundingBox.SEPosition(readonlyMousePosition), target.SEPosition);
    }

    public bool Spawn(Level level)
    {
        if (!level.IsValidSpecialSpawnPosition(this))
            return false;
        Position position = _mousePosition.Copy();
        Special special = SpecialIndex switch
        {
            SpecialIndices.Hunter => new Hunter(level, position),
            SpecialIndices.Boomer => new Boomer(level, position),
            SpecialIndices.Smoker => new Smoker(level, position),
            SpecialIndices.Charger => new Charger(level, position),
            SpecialIndices.Jockey => new Jockey(level, position),
            SpecialIndices.Spitter => new Spitter(level, position),
            SpecialIndices.Tank => new Tank(level, position),
            SpecialIndices.Witch => new Witch(level, position),
            _ => new Tank(level, position)
        };
        level.AddSpecial(special);
        level.SpendCredits(Cost);
        _cooldownTimer.Reset();
        
        (int audioX, int audioY) = special.AudioLocation;
        AudioManager.Instance.PlaySFX(SFX.SpecialSpawn, audioX, audioY);
        
        return true;
    }
}