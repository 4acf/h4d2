using System.Collections.Immutable;
using H4D2.Infrastructure;
using H4D2.Infrastructure.H4D2;
using H4D2.Spawners;
using H4D2.Spawners.SpecialSpawners;

namespace H4D2.Levels;

public class LevelConfig
{
    public required int ID { get; init; }
    public required string Name { get; init; }
    public required Bitmap Layout { get; init; }
    public required int MaxConsumables { get; init; }
    public required int MaxThrowables { get; init; }
    public required ImmutableArray<SurvivorDescriptor> Survivors { get; init; }
    public required ImmutableDictionary<SpecialDescriptor, BuyInfo> BuyableSpecials { get; init; }
    public required ImmutableArray<ConsumableDescriptor> Consumables { get; init; }
    public required ImmutableArray<ThrowableDescriptor> Throwables { get; init; }
    public required ImmutableArray<UncommonDescriptor> Uncommons { get; init; }
}

public static class StandardLevelConfig
{
    public static readonly ImmutableArray<SurvivorDescriptor> L4D2Survivors =
    [
        SurvivorDescriptor.Coach,
        SurvivorDescriptor.Nick,
        SurvivorDescriptor.Ellis,
        SurvivorDescriptor.Rochelle
    ];
    
    public static readonly ImmutableArray<SurvivorDescriptor> L4D1Survivors =
    [
        SurvivorDescriptor.Bill,
        SurvivorDescriptor.Francis,
        SurvivorDescriptor.Louis,
        SurvivorDescriptor.Zoey
    ];

    public const int MaxConsumables = 3;
    public const int MaxThrowables = 3;

    public static readonly ImmutableDictionary<SpecialDescriptor, BuyInfo> BuyableSpecials = 
    new Dictionary<SpecialDescriptor, BuyInfo>
    {
        {SpecialDescriptor.Spitter, new BuyInfo(30, 15.0)},
        {SpecialDescriptor.Boomer, new BuyInfo(50, 10.0)},
        {SpecialDescriptor.Hunter, new BuyInfo(100, 15.0)},
        {SpecialDescriptor.Jockey, new BuyInfo(125, 15.0)},
        {SpecialDescriptor.Charger, new BuyInfo(150, 10.0)},
        {SpecialDescriptor.Smoker, new BuyInfo(250, 20.0)},
        {SpecialDescriptor.Tank, new BuyInfo(1000, 15.0)},
        {SpecialDescriptor.Witch, new BuyInfo(1250, 30.0)}
    }.ToImmutableDictionary();

    public static readonly ImmutableArray<ConsumableDescriptor> Consumables =
    [
        ConsumableDescriptor.FirstAidKit,
        ConsumableDescriptor.Pills,
        ConsumableDescriptor.Adrenaline
    ];

    public static readonly ImmutableArray<ThrowableDescriptor> Throwables =
    [
        ThrowableDescriptor.Molotov,
        ThrowableDescriptor.PipeBomb,
        ThrowableDescriptor.BileBomb
    ];

    public static readonly ImmutableArray<UncommonDescriptor> Uncommons =
    [
        UncommonDescriptor.Hazmat,
        UncommonDescriptor.Clown,
        UncommonDescriptor.Mudman,
        UncommonDescriptor.Worker,
        UncommonDescriptor.Riot
    ];
}

public static class LevelCollection
{
    public const int NumLevels = 15;
    
    public static readonly ImmutableArray<LevelConfig> Levels =
    [
        new()
        {
            ID = 0,
            Name = "Pilot",
            Layout = H4D2Art.LevelLayouts[0],
            MaxConsumables = StandardLevelConfig.MaxConsumables,
            MaxThrowables = StandardLevelConfig.MaxThrowables,
            Survivors = StandardLevelConfig.L4D2Survivors,
            BuyableSpecials = StandardLevelConfig.BuyableSpecials,
            Consumables = StandardLevelConfig.Consumables,
            Throwables = StandardLevelConfig.Throwables,
            Uncommons = [UncommonDescriptor.Hazmat]
        },
        new()
        {
          ID = 1,
          Name = "Pilot 2",
          Layout = H4D2Art.LevelLayouts[1],
          MaxConsumables = StandardLevelConfig.MaxConsumables,
          MaxThrowables = StandardLevelConfig.MaxThrowables,
          Survivors = StandardLevelConfig.L4D2Survivors,
          BuyableSpecials = StandardLevelConfig.BuyableSpecials,
          Consumables = StandardLevelConfig.Consumables,
          Throwables = StandardLevelConfig.Throwables,
          Uncommons = [UncommonDescriptor.Hazmat, UncommonDescriptor.Worker]
        },
        new()
        {
            ID = 2,
            Name = "Holdout",
            Layout = H4D2Art.LevelLayouts[2],
            MaxConsumables = StandardLevelConfig.MaxConsumables,
            MaxThrowables = StandardLevelConfig.MaxThrowables,
            Survivors = StandardLevelConfig.L4D2Survivors,
            BuyableSpecials = StandardLevelConfig.BuyableSpecials,
            Consumables = StandardLevelConfig.Consumables,
            Throwables = StandardLevelConfig.Throwables,
            Uncommons = [UncommonDescriptor.Mudman]
        },
        new()
        {
            ID = 3,
            Name = "Taco Bucket Supreme",
            Layout = H4D2Art.LevelLayouts[3],
            MaxConsumables = StandardLevelConfig.MaxConsumables,
            MaxThrowables = StandardLevelConfig.MaxThrowables,
            Survivors = StandardLevelConfig.L4D2Survivors,
            BuyableSpecials = StandardLevelConfig.BuyableSpecials,
            Consumables = StandardLevelConfig.Consumables,
            Throwables = StandardLevelConfig.Throwables,
            Uncommons = [UncommonDescriptor.Worker, UncommonDescriptor.Riot]
        },
        new()
        {
            ID = 4,
            Name = "My Buddy Keith",
            Layout = H4D2Art.LevelLayouts[4],
            MaxConsumables = StandardLevelConfig.MaxConsumables * 2,
            MaxThrowables = StandardLevelConfig.MaxThrowables * 2,
            Survivors = [..Enumerable.Repeat(SurvivorDescriptor.Ellis, 8)],
            BuyableSpecials = StandardLevelConfig.BuyableSpecials,
            Consumables = [ConsumableDescriptor.Adrenaline],
            Throwables = [ThrowableDescriptor.PipeBomb],
            Uncommons = [UncommonDescriptor.Clown]
        },
        new()
        {
            ID = 5,
            Name = "H4D1",
            Layout = H4D2Art.LevelLayouts[5],
            MaxConsumables = StandardLevelConfig.MaxConsumables,
            MaxThrowables = StandardLevelConfig.MaxThrowables,
            Survivors = StandardLevelConfig.L4D1Survivors,
            BuyableSpecials = 
                new Dictionary<SpecialDescriptor, BuyInfo>
                {
                    {SpecialDescriptor.Boomer, new BuyInfo(25, 10.0)},
                    {SpecialDescriptor.Hunter, new BuyInfo(50, 15.0)},
                    {SpecialDescriptor.Smoker, new BuyInfo(125, 20.0)},
                    {SpecialDescriptor.Tank, new BuyInfo(1000, 15.0)},
                    {SpecialDescriptor.Witch, new BuyInfo(1250, 30.0)}
                }.ToImmutableDictionary(),
            Consumables = StandardLevelConfig.Consumables,
            Throwables = StandardLevelConfig.Throwables,
            Uncommons = [UncommonDescriptor.Worker]
        },
        new()
        {
            ID = 6,
            Name = "Party Room",
            Layout = H4D2Art.LevelLayouts[6],
            MaxConsumables = StandardLevelConfig.MaxConsumables,
            MaxThrowables = StandardLevelConfig.MaxThrowables,
            Survivors = StandardLevelConfig.L4D1Survivors,
            BuyableSpecials = StandardLevelConfig.BuyableSpecials,
            Consumables = StandardLevelConfig.Consumables,
            Throwables = StandardLevelConfig.Throwables,
            Uncommons = [UncommonDescriptor.Mudman]
        },
        new()
        {
            ID = 7,
            Name = "Mercy Hospital",
            Layout = H4D2Art.LevelLayouts[7],
            MaxConsumables = StandardLevelConfig.MaxConsumables * 2,
            MaxThrowables = StandardLevelConfig.MaxThrowables,
            Survivors = StandardLevelConfig.L4D1Survivors,
            BuyableSpecials = StandardLevelConfig.BuyableSpecials,
            Consumables = StandardLevelConfig.Consumables,
            Throwables = StandardLevelConfig.Throwables,
            Uncommons = [UncommonDescriptor.Hazmat, UncommonDescriptor.Worker]
        },
        new()
        {
            ID = 8,
            Name = "Last Stand",
            Layout = H4D2Art.LevelLayouts[8],
            MaxConsumables = StandardLevelConfig.MaxConsumables,
            MaxThrowables = StandardLevelConfig.MaxThrowables,
            Survivors = StandardLevelConfig.L4D1Survivors,
            BuyableSpecials = StandardLevelConfig.BuyableSpecials,
            Consumables = StandardLevelConfig.Consumables,
            Throwables = StandardLevelConfig.Throwables,
            Uncommons = [UncommonDescriptor.Mudman, UncommonDescriptor.Riot]
        },
        new()
        {
            ID = 9,
            Name = "I Hate Bowling",
            Layout = H4D2Art.LevelLayouts[9],
            MaxConsumables = StandardLevelConfig.MaxConsumables,
            MaxThrowables = StandardLevelConfig.MaxThrowables,
            Survivors = [..Enumerable.Repeat(SurvivorDescriptor.Francis, 8)],
            BuyableSpecials = 
                new Dictionary<SpecialDescriptor, BuyInfo>
                {
                    {SpecialDescriptor.Charger, new BuyInfo(10, 0.1)}
                }.ToImmutableDictionary(),
            Consumables = StandardLevelConfig.Consumables,
            Throwables = StandardLevelConfig.Throwables,
            Uncommons = [UncommonDescriptor.Clown]
        },
        new()
        {
            ID = 10,
            Name = "Main Menu",
            Layout = H4D2Art.LevelLayouts[10],
            MaxConsumables = StandardLevelConfig.MaxConsumables,
            MaxThrowables = StandardLevelConfig.MaxThrowables,
            Survivors = StandardLevelConfig.L4D2Survivors,
            BuyableSpecials = StandardLevelConfig.BuyableSpecials,
            Consumables = StandardLevelConfig.Consumables,
            Throwables = StandardLevelConfig.Throwables,
            Uncommons = StandardLevelConfig.Uncommons
        },
        new()
        {
            ID = 11,
            Name = "Ghosts",
            Layout = H4D2Art.LevelLayouts[11],
            MaxConsumables = StandardLevelConfig.MaxConsumables,
            MaxThrowables = StandardLevelConfig.MaxThrowables,
            Survivors = StandardLevelConfig.L4D2Survivors,
            BuyableSpecials = StandardLevelConfig.BuyableSpecials,
            Consumables = StandardLevelConfig.Consumables,
            Throwables = StandardLevelConfig.Throwables,
            Uncommons = [UncommonDescriptor.Clown]
        },
        new()
        {
            ID = 12,
            Name = "Kill Everybody",
            Layout = H4D2Art.LevelLayouts[12],
            MaxConsumables = StandardLevelConfig.MaxConsumables,
            MaxThrowables = StandardLevelConfig.MaxThrowables,
            Survivors = [..StandardLevelConfig.L4D1Survivors, ..StandardLevelConfig.L4D2Survivors],
            BuyableSpecials = StandardLevelConfig.BuyableSpecials,
            Consumables = StandardLevelConfig.Consumables,
            Throwables = StandardLevelConfig.Throwables,
            Uncommons = StandardLevelConfig.Uncommons
        },
        new()
        {
            ID = 13,
            Name = "Ants",
            Layout = H4D2Art.LevelLayouts[13],
            MaxConsumables = StandardLevelConfig.MaxConsumables,
            MaxThrowables = StandardLevelConfig.MaxThrowables,
            Survivors = [..StandardLevelConfig.L4D1Survivors, ..StandardLevelConfig.L4D2Survivors],
            BuyableSpecials = StandardLevelConfig.BuyableSpecials,
            Consumables = StandardLevelConfig.Consumables,
            Throwables = StandardLevelConfig.Throwables,
            Uncommons = 
            [
                UncommonDescriptor.Clown, 
                UncommonDescriptor.Mudman
            ]
        },
        new()
        {
            ID = 14,
            Name = "One Man Cheeseburger Apocalypse",
            Layout = H4D2Art.LevelLayouts[14],
            MaxConsumables = StandardLevelConfig.MaxConsumables,
            MaxThrowables = StandardLevelConfig.MaxThrowables,
            Survivors = [SurvivorDescriptor.MegaCoach],
            BuyableSpecials = new Dictionary<SpecialDescriptor, BuyInfo>
            {
                {SpecialDescriptor.Spitter, new BuyInfo(30, 15.0)},
                {SpecialDescriptor.Boomer, new BuyInfo(50, 10.0)},
                {SpecialDescriptor.Hunter, new BuyInfo(100, 15.0)},
                {SpecialDescriptor.Jockey, new BuyInfo(125, 15.0)},
                {SpecialDescriptor.Charger, new BuyInfo(150, 10.0)},
                {SpecialDescriptor.Tank, new BuyInfo(1000, 15.0)},
                {SpecialDescriptor.Witch, new BuyInfo(1250, 30.0)}
            }.ToImmutableDictionary(),
            Consumables = [ConsumableDescriptor.FirstAidKit, ConsumableDescriptor.Pills],
            Throwables = StandardLevelConfig.Throwables,
            Uncommons = StandardLevelConfig.Uncommons
        }
    ];
}