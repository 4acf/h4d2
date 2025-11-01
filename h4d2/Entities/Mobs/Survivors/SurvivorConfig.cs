using H4D2.Infrastructure;

namespace H4D2.Entities.Mobs.Survivors;

public class SurvivorConfig : MobConfig
{
    public required int Character { get; init; }
}

public static class SurvivorConfigs
{
    public const int DefaultHealth = 100;
    private const int _runSpeed = 300;
    private const int _whiteSkinColor = 0xffaf80;
    private const int _blackSkinColor = 0x895e46;
    private static readonly BoundingBoxDimensions _boundingBoxDimensions 
        = new(2, 2, 10, Art.SpriteSize, 7);
    private static readonly BoundingBox _boundingBox 
        = new(0b100, 0b10, _boundingBoxDimensions);
    
    public static readonly SurvivorConfig Coach = new()
    {
        Character = 0,
        Health = DefaultHealth,
        RunSpeed = _runSpeed,
        GibColor = _blackSkinColor,
        BoundingBox = _boundingBox
    };
    
    public static readonly SurvivorConfig Nick = new()
    {
        Character = 1,
        Health = DefaultHealth,
        RunSpeed = _runSpeed,
        GibColor = _whiteSkinColor,
        BoundingBox = _boundingBox
    };
    
    public static readonly SurvivorConfig Ellis = new()
    {
        Character = 2,
        Health = DefaultHealth,
        RunSpeed = _runSpeed,
        GibColor = _whiteSkinColor,
        BoundingBox = _boundingBox
    };
    
    public static readonly SurvivorConfig Rochelle = new()
    {
        Character = 3,
        Health = DefaultHealth,
        RunSpeed = _runSpeed,
        GibColor = _blackSkinColor,
        BoundingBox = _boundingBox
    };
    
    public static readonly SurvivorConfig Bill = new()
    {
        Character = 4,
        Health = DefaultHealth,
        RunSpeed = _runSpeed,
        GibColor = _whiteSkinColor,
        BoundingBox = _boundingBox
    };
    
    public static readonly SurvivorConfig Francis = new()
    {
        Character = 5,
        Health = DefaultHealth,
        RunSpeed = _runSpeed,
        GibColor = _whiteSkinColor,
        BoundingBox = _boundingBox
    };
    
    public static readonly SurvivorConfig Louis = new()
    {
        Character = 6,
        Health = DefaultHealth,
        RunSpeed = _runSpeed,
        GibColor = _blackSkinColor,
        BoundingBox = _boundingBox
    };
    
    public static readonly SurvivorConfig Zoey = new()
    {
        Character = 7,
        Health = DefaultHealth,
        RunSpeed = _runSpeed,
        GibColor = _whiteSkinColor,
        BoundingBox = _boundingBox
    };
}