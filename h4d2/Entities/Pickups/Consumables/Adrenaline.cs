using H4D2.Entities.Mobs.Survivors;
using H4D2.Infrastructure;
using H4D2.Infrastructure.H4D2;
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

    protected override void RenderShadow(ShadowBitmap shadows, int xCorrected, int yCorrected)
    {
        shadows.Fill(
            xCorrected + H4D2Art.PickupSize - 6,
            yCorrected - H4D2Art.PickupSize - 1,
            xCorrected + H4D2Art.PickupSize - 4,
            yCorrected - H4D2Art.PickupSize - 1         
        );
    }
}