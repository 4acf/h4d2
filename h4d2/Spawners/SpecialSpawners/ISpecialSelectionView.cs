using H4D2.Infrastructure;

namespace H4D2.Spawners.SpecialSpawners;

public interface ISpecialSelectionView
{
    public Bitmap Bitmap { get; }
    public int Cost { get; }
    public double PercentageRemaining { get; }
}