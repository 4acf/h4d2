using H4D2.Infrastructure;

namespace H4D2.Spawners.SpecialSpawners;

public interface ISpecialSpawnerView
{
    public int Credits { get; }
    public void Render(Bitmap screen);
}