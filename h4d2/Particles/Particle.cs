using H4D2.Infrastructure;
using H4D2.Levels;
using H4D2.Particles.Clouds.Cloudlets;
using H4D2.Particles.DebrisParticles;
using H4D2.Particles.DebrisParticles.Granules;
using H4D2.Particles.Smokes;

namespace H4D2.Particles;

public abstract class Particle : Isometric
{
    public static readonly Comparison<Particle> Comparator = (a, b) =>
    {
        int rank = Rank(a.GetType()).CompareTo(Rank(b.GetType()));
        if (rank != 0)
            return rank;

        if (a.GetType() == typeof(Flame) && b.GetType() == typeof(Flame))
            return a.Position.Y.CompareTo(b.Position.Y);

        return 0;

        int Rank(Type t)
        {
            if (t == typeof(Fuel)) return 0;
            if (t == typeof(GibDebris)) return 1;
            if (t == typeof(Blood)) return 3;
            if (t == typeof(ExplosionFlame)) return 4;
            if (t == typeof(Flame)) return 5;
            if (t == typeof(Smoke)) return 6;
            return 2;
        }
    };
    
    protected const double _baseFramerate = 60.0;
    
    protected Particle(Level level, Position position)
        : base(level, position)
    {
        
    }
    
    public abstract void Update(double elapsedTime);
}