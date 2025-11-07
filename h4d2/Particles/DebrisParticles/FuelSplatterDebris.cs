using H4D2.Infrastructure;
using H4D2.Levels;

namespace H4D2.Particles.DebrisParticles;

public class FuelSplatterDebris : Debris
{
    private const double _fuelSplatterDrag = 0.98;
    private const double _fuelSplatterBounce = 0.6;
    
    public FuelSplatterDebris(Level level, Position position)
        : base(level, position, _fuelSplatterDrag, _fuelSplatterBounce)
    {
    }
    
    public override void Update(double elapsedTime)
    {
        base.Update(elapsedTime);
        if (RandomSingleton.Instance.Next(2) != 0)
            return;
        var fuel = new FuelDebris(_level, _position.Copy());
        fuel.DampVelocities(elapsedTime, _xVelocity, _yVelocity, _zVelocity);
        _level.AddParticle(fuel);
    }
}