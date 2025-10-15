using H4D2.Entities;
using H4D2.Entities.Mobs.Specials;
using H4D2.Entities.Mobs.Survivors;
using H4D2.Infrastructure;

namespace H4D2.Levels;

public class Level
{
    public int Width { get; init; }
    public int Height { get; init; }
    private List<Entity> _entities;
    
    public Level(int width, int height)
    {
        Width = width;
        Height = height;
        
        _entities = new List<Entity>();
        _entities.Add(new Zoey(this, 152, 120));
        _entities.Add(new Louis(this, 152, 120));
        _entities.Add(new Francis(this, 152, 120));
        _entities.Add(new Bill(this, 152, 120));
        _entities.Add(new Rochelle(this, 152, 120));
        _entities.Add(new Ellis(this, 152, 120));
        _entities.Add(new Nick(this, 152, 120));
        _entities.Add(new Coach(this, 152, 120));
    }

    public void RenderBackground(Bitmap screen)
    {
        
    }

    public void UpdateEntities(double elapsedTime)
    {
        foreach (Entity entity in _entities)
        {
            entity.Update(elapsedTime);
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