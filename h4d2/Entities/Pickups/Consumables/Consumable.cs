using H4D2.Infrastructure;
using H4D2.Levels;

namespace H4D2.Entities.Pickups.Consumables;

public abstract class Consumable : Pickup
{
    protected readonly int _consumableType;
    
    protected Consumable(Level level, Position position, ConsumableConfig config)
        : base(level, position, config)
    {
        _consumableType = config.ConsumableType;
    }
    
    protected override void Render(Bitmap screen, int xCorrected, int yCorrected)
    {
        Bitmap bitmap = Art.Pickups[_pickupType][_consumableType];
        screen.Draw(bitmap, xCorrected, yCorrected);
    }
}