using SkiaSharp;

namespace H4D2.Infrastructure;

public static class Art
{
    public const int SpriteSize = 16;
    public const int ParticleSize = 8;
    public const int PickupSize = 8;
    public const int ProjectileSize = 8;
    public const int ShadowColor = 0x0;
    public const double ShadowBlend = 0.9;
    public static readonly Bitmap[][] Survivors = _LoadSurvivors();
    public static readonly Bitmap[][] Commons = _LoadCommons();
    public static readonly Bitmap[][] Uncommons = _LoadUncommons();
    public static readonly Bitmap[][] Specials = _LoadSpecials();
    public static readonly Bitmap[][] Pickups = _LoadPickups();
    public static readonly Bitmap[][] Projectiles = _LoadProjectiles();
    private static readonly Bitmap[][] _particles = _LoadParticles();
    public static Bitmap[] Explosion => _particles[0];
    public static Bitmap[] HealParticle => _particles[1];
    public static Bitmap[] Fire => _particles[2];
    
    private static Bitmap[][] _LoadBitmaps(string resourceName, int spriteSize, int rows, int columns)
    {
        var result = new Bitmap[rows][];
        for (int i = 0; i < rows; i++)
        {
            result[i] = new Bitmap[columns];
        }

        SKBitmap fullResourceBitmap = ResourceLoader.LoadEmbeddedResource($"h4d2.Resources.{resourceName}");
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                result[i][j] = new Bitmap(
                    fullResourceBitmap,
                    spriteSize,
                    i,
                    j
                );
            }
        }
        
        return result;
    }
    private static Bitmap[][] _LoadSurvivors() => _LoadBitmaps("survivor.png", SpriteSize, 8, 23);
    private static Bitmap[][] _LoadCommons() => _LoadBitmaps("common.png", SpriteSize, 9, 23);
    private static Bitmap[][] _LoadUncommons() => _LoadBitmaps("uncommon.png", SpriteSize, 5, 23);
    private static Bitmap[][] _LoadSpecials() => _LoadBitmaps("special.png", SpriteSize, 8, 9);
    private static Bitmap[][] _LoadPickups() => _LoadBitmaps("pickup.png", PickupSize, 2, 3);
    private static Bitmap[][] _LoadProjectiles() => _LoadBitmaps("projectile.png", ProjectileSize, 3, 4);
    private static Bitmap[][] _LoadParticles() => _LoadBitmaps("particle.png", ParticleSize, 3, 4);
}