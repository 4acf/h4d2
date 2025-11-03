using H4D2.Infrastructure;
using H4D2.Levels;

namespace H4D2.Entities.Pickups.Throwable;

public class PipeBomb : Throwable
{
    public PipeBomb(Level level, Position position)
        : base(level, position, ThrowableConfigs.PipeBomb)
    {
    
    }

    protected override void RenderShadow(Bitmap screen, int xCorrected, int yCorrected)
    {
        screen.BlendFill(
            xCorrected + Art.PickupSize - 6,
            yCorrected - Art.PickupSize - 1,
            xCorrected + Art.PickupSize - 3,
            yCorrected - Art.PickupSize - 1,
            Art.ShadowColor,
            Art.ShadowBlend            
        );
    }
}