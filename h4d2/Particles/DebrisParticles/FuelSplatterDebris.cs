using H4D2.Infrastructure;
using H4D2.Levels;
using H4D2.Particles.DebrisParticles.Granules;

namespace H4D2.Particles.DebrisParticles;

public class FuelSplatterDebris : Debris
{
    public FuelSplatterDebris(Level level, Position position)
        : base(level, position, DebrisConfigs.FuelSplatter)
    {
        
    }
    
    public override void Update(double elapsedTime)
    {
        base.Update(elapsedTime);
        if (RandomSingleton.Instance.Next(2) != 0)
            return;
        var fuel = new Fuel(_level, _position.Copy(), _xVelocity, _yVelocity, _zVelocity);
        _level.AddParticle(fuel);
    }
}