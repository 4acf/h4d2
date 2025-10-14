using H4D2.Entities;
using H4D2.Infrastructure;

namespace H4D2.Levels;

public class Level
{
    private List<Entity> _entities;
    
    public Level()
    {
        _entities = new List<Entity>();
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