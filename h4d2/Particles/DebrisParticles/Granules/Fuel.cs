using H4D2.Entities.Hazards;
using H4D2.Infrastructure;
using H4D2.Infrastructure.H4D2;
using H4D2.Levels;

namespace H4D2.Particles.DebrisParticles.Granules;

public class Fuel : Granule
{
    public Fuel(Level level, Position position, ReadonlyVelocity parentVelocity)
        : base(level, position, GranuleConfigs.Fuel, parentVelocity)
    {
        if (Probability.OneIn(15))
        {
            (double, double) offsets = ScreenSpaceToWorldSpace(
                -H4D2Art.ParticleSize / 2.0,
                H4D2Art.ParticleSize
            );
            var fire = new Fire(_level, _position.CopyAndTranslate(
                offsets.Item1,
                offsets.Item2,
                0)
            );
            _level.AddHazard(fire);
        }
    }

    protected override void Render(H4D2BitmapCanvas screen, int xCorrected, int yCorrected)
    {
        // do nothing
        // this way only the shadow is rendered which gives it that liquid look
    }
}