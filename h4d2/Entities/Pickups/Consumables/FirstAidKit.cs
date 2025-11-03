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
        base.PickUp(survivor);
    }

    protected override void RenderShadow(Bitmap screen, int xCorrected, int yCorrected)
    {
        screen.BlendFill(
            xCorrected + Art.PickupSize - 7,
            yCorrected - Art.PickupSize - 1,
            xCorrected + Art.PickupSize - 3,
            yCorrected - Art.PickupSize - 1,
            Art.ShadowColor,
            Art.ShadowBlend            
        );
    }
}