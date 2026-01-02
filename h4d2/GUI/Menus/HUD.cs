using System.Text;
using H4D2.Infrastructure;
using H4D2.Infrastructure.H4D2;
using H4D2.Spawners.SpecialSpawners;

namespace H4D2.GUI.Menus;

public class HUD : Menu
{
    private const int _padding = 1;
    private const int _creditsColor = 0x64ff68;
    
    private readonly ISpecialSpawnerView _spawnerView;
    private readonly StringBuilder _creditsStringBuilder;
    private readonly Header _credits;
    private readonly SpawnerButton[] _spawnerButtons;
    private readonly Subheader[] _costs;
    
    public HUD(ISpecialSpawnerView spawnerView, int width, int height) : base(width, height)
    {
        _spawnerView = spawnerView;

        int numSelections = _spawnerView.SpecialSelections.Count;
        _spawnerButtons = new SpawnerButton[numSelections];
        _costs = new Subheader[numSelections];
        for (int i = 0; i < numSelections; i++)
        {
            _spawnerButtons[i] = new SpawnerButton(
                 i * (SpawnerButton.Width + _padding) + _padding,
                (_padding * 2) + H4D2Art.TextHeight + SpawnerButton.Height
            );

            int cost = _spawnerView.SpecialSelections[i].Cost;
            _costs[i] = new Subheader(
                $"${cost}",
                i * (SpawnerButton.Width + _padding) + _padding,
                _padding + H4D2Art.TextHeight,
                _textColor
            );
        }
        
        _creditsStringBuilder = new StringBuilder("$0");
        _credits = new Header(
            _creditsStringBuilder.ToString(),
            0,
            (_padding + H4D2Art.TextHeight) + 
            (_padding + SpawnerButton.Height) +
            ((_padding + H4D2Art.TextHeight) * 2) +
            _padding * 2,
            _creditsColor
        );
    }

    public override void Update(Input input)
    {
        _creditsStringBuilder
            .Clear()
            .Append('$')
            .Append(_spawnerView.Credits);
        _credits.UpdateText(_creditsStringBuilder.ToString());

        foreach (SpawnerButton sb in _spawnerButtons)
        {
            sb.Update(input);
        }
    }

    public override void Render(Bitmap screen)
    {
        _spawnerView.Render(screen);
        _credits.Render(screen);

        for (int i = 0; i < _spawnerButtons.Length; i++)
        {
            _spawnerButtons[i].Render(screen);
            _costs[i].Render(screen);
        }
    }
}