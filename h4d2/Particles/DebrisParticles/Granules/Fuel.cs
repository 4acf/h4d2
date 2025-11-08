using H4D2.Entities.Hazards;
using H4D2.Infrastructure;
using H4D2.Levels;

namespace H4D2.Particles.DebrisParticles.Granules;

public class Fuel : Granule
{
    public Fuel(Level level, Position position, double xv, double yv, double zv)
        : base(level, position, GranuleConfigs.Fuel, xv, yv, zv)
    {
        if (RandomSingleton.Instance.Next(7) == 0)
        {
            var fire = new Fire(_level, _position.CopyAndTranslate(
                -Art.ParticleSize / 2.0,
                Art.ParticleSize,
                0)
            );
            _level.AddHazard(fire);
        }
    }
}