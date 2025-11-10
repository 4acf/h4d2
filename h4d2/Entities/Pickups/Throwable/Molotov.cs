using H4D2.Entities.Mobs.Survivors;
using H4D2.Infrastructure;
using H4D2.Levels;

namespace H4D2.Entities.Pickups.Throwable;

public class Molotov : Throwable
{
    public Molotov(Level level, Position position)
        : base(level, position, ThrowableConfigs.Molotov)
    {
        
    }
    
    public override void PickUp(Survivor survivor)
    {
        if (!Removed)
        {
            survivor.ThrowMolotov();
            base.PickUp(survivor);
        }
    }

    protected override void RenderShadow(ShadowBitmap shadows, int xCorrected, int yCorrected)
    {
        shadows.Fill(
            xCorrected + Art.PickupSize - 6,
            yCorrected - Art.PickupSize - 1,
            xCorrected + Art.PickupSize - 3,
            yCorrected - Art.PickupSize - 1         
        );
    }
}