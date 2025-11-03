using H4D2.Entities.Mobs.Survivors;
using H4D2.Infrastructure;
using H4D2.Levels;

namespace H4D2.Entities.Pickups.Throwable;

public abstract class Throwable : Pickup
{
    protected readonly int _throwableType;
    
    protected Throwable(Level level, Position position, ThrowableConfig config)
        : base(level, position, config)
    {
        _throwableType = config.ThrowableType;
    }

    public override void PickUp(Survivor survivor)
    {
        base.PickUp(survivor);
    }
    
    protected override void Render(Bitmap screen, int xCorrected, int yCorrected)
    {
        Bitmap bitmap = Art.Pickups[_pickupType][_throwableType];
        screen.Draw(bitmap, xCorrected, yCorrected);
    }
}