using H4D2.Entities.Mobs.Survivors;
using H4D2.Infrastructure;
using H4D2.Levels;

namespace H4D2.Entities.Pickups.Consumables;

public class FirstAidKit : Consumable
{
    public FirstAidKit(Level level, Position position)
        : base(level, position, ConsumableConfigs.FirstAidKit)
    {
        
    }

    public override void PickUp(Survivor survivor)
    {
        if (!Removed && !survivor.IsFullHealth)
        {
            survivor.ConsumeFirstAidKit();
            base.PickUp(survivor);
        }
    }

    protected override void RenderShadow(ShadowBitmap shadows, int xCorrected, int yCorrected)
    {
        shadows.Fill(
            xCorrected + Art.PickupSize - 7,
            yCorrected - Art.PickupSize - 1,
            xCorrected + Art.PickupSize - 3,
            yCorrected - Art.PickupSize - 1      
        );
    }
}