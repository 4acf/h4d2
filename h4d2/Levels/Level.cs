using H4D2.Entities;
using H4D2.Entities.Mobs.Commons;
using H4D2.Entities.Mobs.Specials;
using H4D2.Entities.Mobs.Survivors;
using H4D2.Entities.Mobs.Uncommons;
using H4D2.Infrastructure;

namespace H4D2.Levels;

public class Level
{
    public readonly int Width;
    public readonly int Height;
    private List<Entity> _entities;
    
    public Level(int width, int height)
    {
        Width = width;
        Height = height;
        
        _entities = new List<Entity>();
        _entities.Add(new Louis(this, 152, 130));
        _entities.Add(new Francis(this, 152, 130));
        _entities.Add(new Zoey(this, 152, 130));
        _entities.Add(new Bill(this, 152, 130));
        _entities.Add(new Rochelle(this, 152, 130));
        _entities.Add(new Ellis(this, 152, 130));
        _entities.Add(new Nick(this, 152, 130));
        _entities.Add(new Coach(this, 152, 130));
        
        for (int i = 0; i < 10; i++) {
            _entities.Add(new Common(this, 152, 130));
        }

        _entities.Add(new Riot(this, 152, 130));
        _entities.Add(new Worker(this, 152, 130));
        _entities.Add(new Mudman(this, 152, 130));
        _entities.Add(new Clown(this, 152, 130));
        _entities.Add(new Hazmat(this, 152, 130));
        
        _entities.Add(new Witch(this, 152, 130));
        _entities.Add(new Tank(this, 152, 130));
        _entities.Add(new Spitter(this, 152, 130));
        _entities.Add(new Jockey(this, 152, 130));
        _entities.Add(new Charger(this, 152, 130));
        _entities.Add(new Smoker(this, 152, 130));
        _entities.Add(new Boomer(this, 152, 130));
        _entities.Add(new Hunter(this, 152, 130));
    }
    
    public void UpdateEntities(double elapsedTime)
    {
        foreach (Entity entity in _entities)
        {
            entity.Update(elapsedTime);
        }
    }
    
    public void RenderBackground(Bitmap screen)
    {
        
    }
    
    public void RenderShadows(Bitmap screen)
    {
        foreach (Entity entity in _entities)
        {
            entity.RenderShadow(screen);
        }
    }
    
    public void RenderEntities(Bitmap screen)
    {
        foreach (Entity entity in _entities)
        {
            entity.Render(screen);
        }
    }
}