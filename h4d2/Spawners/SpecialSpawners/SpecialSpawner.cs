using H4D2.Infrastructure;
using H4D2.Infrastructure.H4D2;
using H4D2.Levels;

namespace H4D2.Spawners.SpecialSpawners;

public enum SpecialDescriptor
{
    Hunter,
    Boomer,
    Smoker,
    Charger,
    Jockey,
    Spitter,
    Tank,
    Witch
}

public readonly record struct BuyInfo
{
    public readonly int Cost;
    public readonly double CooldownSeconds;

    public BuyInfo(int cost, double cooldownSeconds)
    {
        Cost = cost;
        CooldownSeconds = cooldownSeconds;
    }
}

public class SpecialSpawner : ISpecialSpawnerView
{
    public int? SelectedIndex => _selected?.IndexInArray;
    public int Credits => _level.Credits;
    public IReadOnlyList<ISpecialSelectionView> SpecialSelections => _specialSelections;
    
    private readonly Level _level;
    private SpecialSelection? _selected;
    private readonly Position _spawnAdjustedMousePosition;
    private readonly SpecialSelection[] _specialSelections;
    private readonly Camera _camera;
    
    public SpecialSpawner(Level level, LevelConfig config, Camera camera)
    {
        _level = level;
        _camera = camera;
        _spawnAdjustedMousePosition = new Position(0, 0);
        _selected = null;
        
        _specialSelections = new SpecialSelection[config.BuyableSpecials.Count];
        KeyValuePair<SpecialDescriptor, BuyInfo>[] sortedBuyableSpecials = config.BuyableSpecials
            .OrderBy(s => s.Value.Cost)
            .ToArray();
        
        for(int i = 0; i < sortedBuyableSpecials.Length; i++)
        {
            KeyValuePair<SpecialDescriptor, BuyInfo> special = sortedBuyableSpecials[i];
            _specialSelections[i] = new SpecialSelection(i, special.Key, special.Value, _spawnAdjustedMousePosition);
        }
    }

    public void Update(Input input, double elapsedTime)
    {
        foreach(SpecialSelection special in _specialSelections)
            special.UpdateCooldown(elapsedTime);
        _UpdatePosition(input.MousePositionScreen);
        if (input.IsNumberPressed)
        {
            _SelectSpecial(input.LastNumberPressed);
        }

        if (input.IsMousePressed)
            _Spawn();
    }
    
    private void _UpdatePosition(ReadonlyPosition mousePosition)
    {
        (double, double) positionOffset = Isometric.ScreenSpaceToWorldSpace(
            mousePosition.X,
            mousePosition.Y
        );

        (double, double) cameraOffset = Isometric.ScreenSpaceToWorldSpace(
            _camera.XOffset,
            _camera.YOffset
        );

        (double, double) spriteOffset = Isometric.ScreenSpaceToWorldSpace(
            H4D2Art.SpriteSize / 2.0,
            -H4D2Art.SpriteSize
        );
        
        _spawnAdjustedMousePosition.X = positionOffset.Item1 - cameraOffset.Item1 - spriteOffset.Item1;
        _spawnAdjustedMousePosition.Y = positionOffset.Item2 - cameraOffset.Item2 - spriteOffset.Item2;
    }
    
    private void _SelectSpecial(int selection)
    {
        if (selection > _specialSelections.Length)
            return;
        SpecialSelection newSelection = _specialSelections[selection - 1];
        if (!newSelection.IsBuyable(_level.Credits))
            return;
        _selected = _selected != newSelection ?
            newSelection :
            null;
    }

    private void _Spawn()
    {
        if (_selected == null)
            return;
        if(_selected.Spawn(_level))
            _selected = null;
    }

    public void Render(Bitmap screen)
    {
        if (_selected == null)
            return;
        Bitmap specialBitmap = H4D2Art.SpecialProfiles[_selected.SpecialIndex];
        int x = (int)Math.Floor((_spawnAdjustedMousePosition.X - _spawnAdjustedMousePosition.Y) * Isometric.ScaleX);
        int y = (int)Math.Floor((_spawnAdjustedMousePosition.X + _spawnAdjustedMousePosition.Y) * Isometric.ScaleY);
        if (!_level.IsValidSpecialSpawnPosition(_selected))
            screen.DrawInvalidSpecial(specialBitmap, x, y);
        else
            screen.Draw(specialBitmap, x, y);
    }
}