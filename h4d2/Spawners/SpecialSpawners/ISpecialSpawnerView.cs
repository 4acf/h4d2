using H4D2.Infrastructure;
using H4D2.Infrastructure.H4D2;

namespace H4D2.Spawners.SpecialSpawners;

public interface ISpecialSpawnerView
{
    public int? SelectedIndex { get; }
    public int Credits { get; }
    public IReadOnlyList<ISpecialSelectionView> SpecialSelections { get; }
    public void SelectSpecial(int selection);
    public void Render(H4D2BitmapCanvas screen);
}