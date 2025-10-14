using H4D2.Entities;
using H4D2.Entities.Mobs.Survivors;
using H4D2.Infrastructure;

namespace H4D2.Levels;

public class Level
{
    private List<Entity> _entities;
    
    public Level()
    {
        _entities = new List<Entity>();
        _entities.Add(new Coach(0, 240 - (0 * 16)));
        _entities.Add(new Nick(0, 240 - (1 * 16)));
        _entities.Add(new Ellis(0, 240 - (2 * 16)));
        _entities.Add(new Rochelle(0, 240 - (3 * 16)));
        _entities.Add(new Bill(0, 240 - (4 * 16)));
        _entities.Add(new Francis(0, 240 - (5 * 16)));
        _entities.Add(new Louis(0, 240 - (6 * 16)));
        _entities.Add(new Zoey(0, 240 - (7 * 16)));
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