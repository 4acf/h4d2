using H4D2.Levels;

namespace H4D2.Infrastructure.H4D2;

public static class H4D2Art
{
    public const int SpriteSize = 16;
    public const int ParticleSize = 8;
    public const int PickupSize = 8;
    public const int ProjectileSize = 8;
    public const int TileSize = 24;
    public const int TileIsoWidth = 24;
    public const int TileIsoHeight = 12;
    public const int TileIsoHalfHeight = 6;
    public const int TileCenterOffset = 19;
    public const int SmallButtonWidth = 24;
    public const int SmallButtonHeight = 24;
    public const int SpawnerButtonWidth = 20;
    public const int LargeButtonWidth = 72;
    public const int LargeButtonHeight = 24;
    public const int ShadowColor = 0x0;
    public const double ShadowBlend = 0.9;
    
    public static readonly Bitmap[][] Survivors = _LoadSurvivors();
    public static readonly Bitmap[][] Commons = _LoadCommons();
    public static readonly Bitmap[][] Uncommons = _LoadUncommons();
    public static readonly Bitmap[][] Specials = _LoadSpecials();
    public static readonly Bitmap[][] Pickups = _LoadPickups();
    public static readonly Bitmap[][] Projectiles = _LoadProjectiles();
    public static readonly Bitmap[] LevelLayouts = _LoadLevels();
    
    public static class Overlays
    {
        private static readonly Bitmap[][] _bileOverlays = _LoadBileOverlays();
        public static readonly Bitmap[] BileOverlays = _bileOverlays.SelectMany(x => x).ToArray();
    }
    
    public static class Tiles
    {
        private static readonly Bitmap[][] _tiles = _LoadTiles();
        public static readonly Bitmap[] Floors = _tiles[0];
        public static readonly Bitmap[] Walls = _tiles[1];
    }
    
    public static class Particles
    {
        private static readonly Bitmap[][] _particles = _LoadParticles();
        public static readonly Bitmap[] Explosion = _particles[0];
        public static readonly Bitmap[] HealParticle = _particles[1];
        public static readonly Bitmap[] Fire = _particles[2];
        public static readonly Bitmap[] NullParticle = _particles[3];
    }

    public static class GUI
    {
        public static readonly Bitmap Title = Art.LoadBitmap($"{Resources.EmbeddedPrefix}.gui.title.png");
        public static readonly TextBitmap[] Text = _LoadPixufFont();
        public static readonly int TextHeight = Text.Length > 0 ? Text[0].Height : 0;
        public static readonly Bitmap[] SpecialProfiles = Specials[8];
        public static class Buttons
        {
            private static readonly Bitmap[][] _smallButtons = _LoadSmallButtons();
            private static readonly Bitmap[][] _largeButtons = _LoadLargeButtons();
            public static readonly Bitmap[] Play = _largeButtons[0];
            public static readonly Bitmap[] Settings = _largeButtons[1];
            public static readonly Bitmap[] Exit = _largeButtons[2];
            public static readonly Bitmap[] MainMenu = _largeButtons[3];
            public static readonly Bitmap[] Resume = _largeButtons[4];
            public static readonly Bitmap[] Levels = _largeButtons[5];
            public static readonly Bitmap[] Navigation = _smallButtons[0];
            public static readonly Bitmap[] Spawner = _smallButtons[1];
        }
    }

    public static Stream GetRandomWindowIcon()
    {
        const int numWindowIcons = 16;
        int random = RandomSingleton.Instance.Next(numWindowIcons);
        string filename = random switch
        {
            0 => $"{Resources.EmbeddedPrefix}.window_icons.coach.png",
            1 => $"{Resources.EmbeddedPrefix}.window_icons.nick.png",
            2 => $"{Resources.EmbeddedPrefix}.window_icons.ellis.png",
            3 => $"{Resources.EmbeddedPrefix}.window_icons.rochelle.png",
            4 => $"{Resources.EmbeddedPrefix}.window_icons.bill.png",
            5 => $"{Resources.EmbeddedPrefix}.window_icons.francis.png",
            6 => $"{Resources.EmbeddedPrefix}.window_icons.louis.png",
            7 => $"{Resources.EmbeddedPrefix}.window_icons.zoey.png",
            8 => $"{Resources.EmbeddedPrefix}.window_icons.hunter.png",
            9 => $"{Resources.EmbeddedPrefix}.window_icons.boomer.png",
            10 => $"{Resources.EmbeddedPrefix}.window_icons.smoker.png",
            11 => $"{Resources.EmbeddedPrefix}.window_icons.charger.png",
            12 => $"{Resources.EmbeddedPrefix}.window_icons.jockey.png",
            13 => $"{Resources.EmbeddedPrefix}.window_icons.spitter.png",
            14 => $"{Resources.EmbeddedPrefix}.window_icons.tank.png",
            _ => $"{Resources.EmbeddedPrefix}.window_icons.witch.png"
        };
        return Art.LoadImage(filename);
    }
    
    private static Bitmap[][] _LoadSurvivors() => 
        Art.LoadBitmaps($"{Resources.EmbeddedPrefix}.survivor.png", SpriteSize, 8, 53);
    private static Bitmap[][] _LoadCommons() => 
        Art.LoadBitmaps($"{Resources.EmbeddedPrefix}.common.png", SpriteSize, 9, 23);
    private static Bitmap[][] _LoadUncommons() => 
        Art.LoadBitmaps($"{Resources.EmbeddedPrefix}.uncommon.png", SpriteSize, 6, 23);
    private static Bitmap[][] _LoadSpecials() => 
        Art.LoadBitmaps($"{Resources.EmbeddedPrefix}.special.png", SpriteSize, 9, [21, 14, 24, 39, 15, 18, 27, 10, 8]);
    private static Bitmap[][] _LoadPickups() => 
        Art.LoadBitmaps($"{Resources.EmbeddedPrefix}.pickup.png", PickupSize, 2, [4,3]);
    private static Bitmap[][] _LoadProjectiles() => 
        Art.LoadBitmaps($"{Resources.EmbeddedPrefix}.projectile.png", ProjectileSize, 4, 4);
    private static Bitmap[][] _LoadParticles() => 
        Art.LoadBitmaps($"{Resources.EmbeddedPrefix}.particle.png", ParticleSize, 4, 4);
    private static Bitmap[][] _LoadBileOverlays() => 
        Art.LoadBitmaps($"{Resources.EmbeddedPrefix}.bileoverlay.png", SpriteSize, 2, 2);
    private static Bitmap[][] _LoadTiles() =>
        Art.LoadBitmaps($"{Resources.EmbeddedPrefix}.levels.tiles.png", TileSize, 2, 3);
    private static TextBitmap[] _LoadPixufFont() =>
        Art.LoadFontBitmaps($"{Resources.EmbeddedPrefix}.pixuf.png", Pixuf.Characters, Pixuf.Widths, 7);
    private static Bitmap[][] _LoadSmallButtons() =>
        Art.LoadBitmaps($"{Resources.EmbeddedPrefix}.gui.smallbuttons.png", SmallButtonWidth, 2, 2);
    private static Bitmap[][] _LoadLargeButtons() =>
        Art.LoadBitmaps($"{Resources.EmbeddedPrefix}.gui.largebuttons.png", LargeButtonWidth, LargeButtonHeight, 6, 2);

    private static Bitmap[] _LoadLevels()
    {
        var levels = new Bitmap[LevelCollection.NumLevels];
        for (int i = 0; i < LevelCollection.NumLevels; i++)
        {
            levels[i] = Art.LoadBitmap($"{Resources.EmbeddedPrefix}.levels.level{i}.png"); 
        }
        return levels;
    }
}