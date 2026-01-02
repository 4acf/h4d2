using H4D2.Entities;
using H4D2.Entities.Mobs.Zombies.Specials;
using H4D2.Entities.Mobs.Zombies.Specials.Pinners;
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
    public int Credits => _level.Credits;
    
    private readonly Level _level;
    private SpecialSelection? _selected;
    private readonly Position _spawnAdjustedMousePosition;
    private readonly SpecialSelection[] _specialSelections;
    private readonly Camera _camera;
    
    public ReadonlyPosition? CenterMass =>
        _selected?.BoundingBox.CenterMass(_spawnAdjustedMousePosition.ReadonlyCopy());
    public ReadonlyPosition? NWPosition => 
        _selected?.BoundingBox.NWPosition(_spawnAdjustedMousePosition.ReadonlyCopy());
    public ReadonlyPosition? NEPosition => 
        _selected?.BoundingBox.NEPosition(_spawnAdjustedMousePosition.ReadonlyCopy());
    public ReadonlyPosition? SWPosition => 
        _selected?.BoundingBox.SWPosition(_spawnAdjustedMousePosition.ReadonlyCopy());
    public ReadonlyPosition? SEPosition => 
        _selected?.BoundingBox.SEPosition(_spawnAdjustedMousePosition.ReadonlyCopy());
    
    public SpecialSpawner(Level level, LevelConfig config, Camera camera)
    {
        _level = level;
        _camera = camera;
        _spawnAdjustedMousePosition = new Position(0, 0);
        _selected = null;
        _specialSelections = new SpecialSelection[config.BuyableSpecials.Count];
        int i = 0;
        foreach (KeyValuePair<SpecialDescriptor, BuyInfo> special in config.BuyableSpecials)
        {
            _specialSelections[i] = new SpecialSelection(special.Key, special.Value);
            i++;
        }
        Array.Sort(_specialSelections, (a, b) => a.Cost.CompareTo(b.Cost));
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
        if (!_level.IsValidSpecialSpawnPosition(this))
            return;
        Position position = _spawnAdjustedMousePosition.Copy();
        Special special = _selected.SpecialIndex switch
        {
            SpecialIndices.Hunter => new Hunter(_level, position),
            SpecialIndices.Boomer => new Boomer(_level, position),
            SpecialIndices.Smoker => new Smoker(_level, position),
            SpecialIndices.Charger => new Charger(_level, position),
            SpecialIndices.Jockey => new Jockey(_level, position),
            SpecialIndices.Spitter => new Spitter(_level, position),
            SpecialIndices.Tank => new Tank(_level, position),
            SpecialIndices.Witch => new Witch(_level, position),
            _ => new Tank(_level, position)
        };
        _level.AddSpecial(special);
        _selected = null;
    }

    public bool HasLineOfSight(Entity target)
    {
        if (NWPosition == null || NEPosition == null || SWPosition == null || SEPosition == null)
            return true;
        return 
            _level.HasLineOfSight(NWPosition.Value, target.NWPosition) && 
            _level.HasLineOfSight(NEPosition.Value, target.NEPosition) && 
            _level.HasLineOfSight(SWPosition.Value, target.SWPosition) && 
            _level.HasLineOfSight(SEPosition.Value, target.SEPosition);
    }

    public void Render(Bitmap screen)
    {
        if (_selected == null)
            return;
        Bitmap specialBitmap = H4D2Art.SpecialProfiles[_selected.SpecialIndex];
        int x = (int)Math.Floor((_spawnAdjustedMousePosition.X - _spawnAdjustedMousePosition.Y) * Isometric.ScaleX);
        int y = (int)Math.Floor((_spawnAdjustedMousePosition.X + _spawnAdjustedMousePosition.Y) * Isometric.ScaleY);
        if (!_level.IsValidSpecialSpawnPosition(this))
            screen.DrawInvalidSpecial(specialBitmap, x, y);
        else
            screen.Draw(specialBitmap, x, y);
    }
}