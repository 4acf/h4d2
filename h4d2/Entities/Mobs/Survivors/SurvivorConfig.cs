using H4D2.Infrastructure;
using H4D2.Infrastructure.H4D2;

namespace H4D2.Entities.Mobs.Survivors;

public class SurvivorConfig : MobConfig
{
    public required int Character { get; init; }
}

public static class SurvivorConfigs
{
    private const int _defaultHealth = 100;
    private const int _runSpeed = 300;
    private const int _whiteSkinColor = 0xffaf80;
    private const int _blackSkinColor = 0x895e46;
    private static readonly BoundingBoxDimensions _boundingBoxDimensions 
        = new(2, 2, 10, Art.SpriteSize, 7);
    private static readonly BoundingBox _boundingBox 
        = new(CollisionGroup.Survivor, _boundingBoxDimensions);
    
    public static readonly SurvivorConfig Coach = new()
    {
        Character = 0,
        Health = _defaultHealth,
        RunSpeed = _runSpeed,
        GibColor = _blackSkinColor,
        BoundingBox = _boundingBox
    };
    
    public static readonly SurvivorConfig Nick = new()
    {
        Character = 1,
        Health = _defaultHealth,
        RunSpeed = _runSpeed,
        GibColor = _whiteSkinColor,
        BoundingBox = _boundingBox
    };
    
    public static readonly SurvivorConfig Ellis = new()
    {
        Character = 2,
        Health = _defaultHealth,
        RunSpeed = _runSpeed,
        GibColor = _whiteSkinColor,
        BoundingBox = _boundingBox
    };
    
    public static readonly SurvivorConfig Rochelle = new()
    {
        Character = 3,
        Health = _defaultHealth,
        RunSpeed = _runSpeed,
        GibColor = _blackSkinColor,
        BoundingBox = _boundingBox
    };
    
    public static readonly SurvivorConfig Bill = new()
    {
        Character = 4,
        Health = _defaultHealth,
        RunSpeed = _runSpeed,
        GibColor = _whiteSkinColor,
        BoundingBox = _boundingBox
    };
    
    public static readonly SurvivorConfig Francis = new()
    {
        Character = 5,
        Health = _defaultHealth,
        RunSpeed = _runSpeed,
        GibColor = _whiteSkinColor,
        BoundingBox = _boundingBox
    };
    
    public static readonly SurvivorConfig Louis = new()
    {
        Character = 6,
        Health = _defaultHealth,
        RunSpeed = _runSpeed,
        GibColor = _blackSkinColor,
        BoundingBox = _boundingBox
    };
    
    public static readonly SurvivorConfig Zoey = new()
    {
        Character = 7,
        Health = _defaultHealth,
        RunSpeed = _runSpeed,
        GibColor = _whiteSkinColor,
        BoundingBox = _boundingBox
    };
}