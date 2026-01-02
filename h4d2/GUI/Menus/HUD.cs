using System.Text;
using H4D2.Infrastructure;
using H4D2.Spawners.SpecialSpawners;

namespace H4D2.GUI.Menus;

public class HUD : Menu
{
    private const int _paddingBetweenButtons = 1;
    private const int _paddingPriceLeft = 1;
    private const int _creditsColor = 0x64ff68;
    
    private readonly ISpecialSpawnerView _spawnerView;
    private readonly StringBuilder _creditsStringBuilder;
    private readonly Header _credits;
    
    public HUD(ISpecialSpawnerView spawnerView, int width, int height) : base(width, height)
    {
        _spawnerView = spawnerView;
        _creditsStringBuilder = new StringBuilder($"$0");
        _credits = new Header(_creditsStringBuilder.ToString(), 100, 100, 0x64ff68);
    }

    public override void Update(Input input)
    {
        _creditsStringBuilder.Clear();
        _creditsStringBuilder.Append('$');
        _creditsStringBuilder.Append(_spawnerView.Credits);
        _credits.UpdateText(_creditsStringBuilder.ToString());
    }

    public override void Render(Bitmap screen)
    {
        _spawnerView.Render(screen);
        _credits.Render(screen);
    }
}