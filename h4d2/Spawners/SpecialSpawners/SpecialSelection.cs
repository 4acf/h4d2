using H4D2.Entities.Mobs.Zombies.Specials;
using H4D2.Infrastructure;
using H4D2.Infrastructure.H4D2;

namespace H4D2.Spawners.SpecialSpawners;

public class SpecialSelection : ISpecialSelectionView
{
    public Bitmap Bitmap => H4D2Art.SpecialProfiles[SpecialIndex];
    public int Cost { get; }
    public double PercentageRemaining => _cooldownTimer.Percentage;
    
    public readonly int SpecialIndex;
    public readonly SpecialDescriptor Descriptor;
    public readonly BoundingBox BoundingBox;
    private readonly CountdownTimer _cooldownTimer;
    
    public SpecialSelection(SpecialDescriptor descriptor, BuyInfo buyInfo)
    {
        Descriptor = descriptor;
        Cost = buyInfo.Cost; 
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
}