using H4D2.Entities.Projectiles;
using H4D2.Infrastructure;
using H4D2.Infrastructure.H4D2;
using H4D2.Levels;

namespace H4D2.Particles;

public class TongueSegment : Particle
{
    public TongueSegment(Level level, Position position) 
        : base(level, position) { }

    public void Remove()
    {
        Removed = true;
    }

    protected override void Render(H4D2BitmapCanvas screen, int xCorrected, int yCorrected)
    {
        screen.SetPixel(xCorrected, yCorrected, Tongue.Color);
    }

    protected override void RenderShadow(ShadowBitmap shadows, int xCorrected, int yCorrected)
    {
        shadows.SetPixel(xCorrected, yCorrected);
    }
}