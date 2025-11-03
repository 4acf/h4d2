using H4D2.Entities.Mobs.Survivors;
using H4D2.Infrastructure;
using H4D2.Levels;

namespace H4D2.Entities.Pickups.ThrowablePickup;

public abstract class ThrowablePickup : Pickup
{
    protected readonly int _throwablePickupType;
    
    protected ThrowablePickup(Level level, Position position, ThrowablePickupConfig config)
        : base(level, position, config)
    {
        _throwablePickupType = config.ThrowablePickupType;
    }

    public override void PickUp(Survivor survivor)
    {
        base.PickUp(survivor);
    }
    
    protected override void Render(Bitmap screen, int xCorrected, int yCorrected)
    {
        Bitmap bitmap = Art.Pickups[_pickupType][_throwablePickupType];
        screen.Draw(bitmap, xCorrected, yCorrected);
    }
}