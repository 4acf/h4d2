using H4D2.Entities;
using H4D2.Entities.Mobs.Zombies.Specials;
using H4D2.Entities.Mobs.Zombies.Specials.Pinners;
using H4D2.Infrastructure;
using H4D2.Infrastructure.H4D2;
using H4D2.Levels;

namespace H4D2.Spawners;

public class SpecialSpawner
{
    public ReadonlyPosition? CenterMass =>
        _selectedBoundingBox?.CenterMass(_spawnAdjustedMousePosition.ReadonlyCopy());
    private readonly Level _level;
    private BoundingBox? _selectedBoundingBox;
    private int? _selectedIndex;
    private readonly Position _spawnAdjustedMousePosition;
    
    public ReadonlyPosition? NWPosition => 
        _selectedBoundingBox?.NWPosition(_spawnAdjustedMousePosition.ReadonlyCopy());
    public ReadonlyPosition? NEPosition => 
        _selectedBoundingBox?.NEPosition(_spawnAdjustedMousePosition.ReadonlyCopy());
    public ReadonlyPosition? SWPosition => 
        _selectedBoundingBox?.SWPosition(_spawnAdjustedMousePosition.ReadonlyCopy());
    public ReadonlyPosition? SEPosition => 
        _selectedBoundingBox?.SEPosition(_spawnAdjustedMousePosition.ReadonlyCopy());
    
    public SpecialSpawner(Level level)
    {
        _level = level;
        _selectedBoundingBox = null;
        _selectedIndex = null;
        _spawnAdjustedMousePosition = new Position(0, 0);
    }

    public void UpdatePosition(Position mousePosition, Camera camera)
    {
        (double, double) positionOffset = Isometric.ScreenSpaceToWorldSpace(
            mousePosition.X,
            mousePosition.Y
        );

        (double, double) cameraOffset = Isometric.ScreenSpaceToWorldSpace(
            camera.XOffset,
            camera.YOffset
        );

        (double, double) spriteOffset = Isometric.ScreenSpaceToWorldSpace(
            H4D2Art.SpriteSize / 2.0,
            -H4D2Art.SpriteSize
        );
        
        _spawnAdjustedMousePosition.X = positionOffset.Item1 - cameraOffset.Item1 - spriteOffset.Item1;
        _spawnAdjustedMousePosition.Y = positionOffset.Item2 - cameraOffset.Item2 - spriteOffset.Item2;
    }
    
    public void SelectSpecial(int selection)
    {
        // the conversion from button -> special is hardcoded for now but in the future
        // i'll need to inject a conversion function so that different levels can have
        // different arrangements of specials and costs
        
        BoundingBox? newBoundingBox = selection switch
        {
            1 => SpecialBoundingBoxes.Hunter,
            2 => SpecialBoundingBoxes.Boomer,
            3 => SpecialBoundingBoxes.Smoker,
            4 => SpecialBoundingBoxes.Charger,
            5 => SpecialBoundingBoxes.Jockey,
            6 => SpecialBoundingBoxes.Spitter,
            7 => SpecialBoundingBoxes.Tank,
            8 => SpecialBoundingBoxes.Witch,
            _ => null
        };

        _selectedBoundingBox = newBoundingBox == _selectedBoundingBox ?
            null :
            newBoundingBox;
        
        _selectedIndex = _selectedBoundingBox != null ?
            selection - 1 : 
            null;
    }

    public void Spawn()
    {
        if (_selectedIndex == null)
            return;
        if (!_level.IsValidSpecialSpawnPosition(this))
            return;
        Position position = _spawnAdjustedMousePosition.Copy();
        Special special = _selectedIndex switch
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
        _selectedBoundingBox = null;
        _selectedIndex = null;
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
        if (_selectedIndex == null)
            return;
        Bitmap specialBitmap = H4D2Art.SpecialProfiles[_selectedIndex.Value];
        int x = (int)Math.Floor((_spawnAdjustedMousePosition.X - _spawnAdjustedMousePosition.Y) * Isometric.ScaleX);
        int y = (int)Math.Floor((_spawnAdjustedMousePosition.X + _spawnAdjustedMousePosition.Y) * Isometric.ScaleY);
        if (!_level.IsValidSpecialSpawnPosition(this))
            screen.DrawInvalidSpecial(specialBitmap, x, y);
        else
            screen.Draw(specialBitmap, x, y);
    }
}