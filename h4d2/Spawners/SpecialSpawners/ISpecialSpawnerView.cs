using H4D2.Infrastructure;

namespace H4D2.Spawners.SpecialSpawners;

public interface ISpecialSpawnerView
{
    public int? SelectedIndex { get; }
    public int Credits { get; }
    public IReadOnlyList<ISpecialSelectionView> SpecialSelections { get; }
    public void Render(Bitmap screen);
}