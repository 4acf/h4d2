using H4D2.Entities.Mobs.Survivors;
using H4D2.Infrastructure;
using H4D2.Levels;

namespace H4D2.Entities.Pickups.Throwable;

public class BileBomb : Throwable
{
    public BileBomb(Level level, Position position)
        : base(level, position, ThrowableConfigs.BileBomb)
    {
    
    }
    
    public override void PickUp(Survivor survivor)
    {
        if (!Removed)
        {
            base.PickUp(survivor);
        }
    }

    protected override void RenderShadow(Bitmap screen, int xCorrected, int yCorrected)
    {
        screen.BlendFill(
            xCorrected + Art.PickupSize - 6,
            yCorrected - Art.PickupSize - 1,
            xCorrected + Art.PickupSize - 4,
            yCorrected - Art.PickupSize - 1,
            Art.ShadowColor,
            Art.ShadowBlend            
        );
    }
}