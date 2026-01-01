using H4D2.Infrastructure;
using H4D2.Spawners;

namespace H4D2.GUI.Menus;

public class HUD : Menu
{
    private readonly SpecialSpawner _spawner;
    
    public HUD(SpecialSpawner spawner, int width, int height) : base(width, height)
    {
        _spawner = spawner;
    }

    public override void Update(Input input)
    {
        
    }

    public override void Render(Bitmap screen)
    {
        
    }
}