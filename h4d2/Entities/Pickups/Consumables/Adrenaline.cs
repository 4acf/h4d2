using H4D2.Entities.Mobs.Survivors;
using H4D2.Infrastructure;
using H4D2.Levels;

namespace H4D2.Entities.Pickups.Consumables;

public class Adrenaline : Consumable
{
    public Adrenaline(Level level, Position position)
        : base(level, position, ConsumableConfigs.Adrenaline)
    {
        
    }

    public override void PickUp(Survivor survivor)
    {
        if (!Removed)
        {
            survivor.ConsumeAdrenaline();
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