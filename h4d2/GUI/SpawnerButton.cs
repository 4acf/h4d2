using H4D2.Infrastructure;
using H4D2.Infrastructure.H4D2;
using H4D2.Spawners.SpecialSpawners;

namespace H4D2.GUI;

public class SpawnerButton
{
    public event EventHandler? Clicked;

    public const int Width = H4D2Art.SpawnerButtonWidth;
    public const int Height = H4D2Art.SmallButtonHeight;
    private const int _specialXOffs = 2;
    private const int _specialYOffs = 1;
    private const int _selectedYOffs = -3;
    
    private readonly Bitmap _specialBitmap;
    private readonly int _indexInArray;
    private readonly int _x;
    private readonly int _y;
    private bool _isSelected;
    private bool _isBuyable;
    private double _percentageRemaining;
    private bool _isMouseOver;
    
    public SpawnerButton(Bitmap specialBitmap, int i, int x, int y)
    {
        _specialBitmap = specialBitmap;
        _indexInArray = i;
        _x = x;
        _y = y;
        _isSelected = false;
        _isBuyable = false;
        _percentageRemaining = 0.0;
        _isMouseOver = false;
    }
    
    public void Update(ISpecialSpawnerView spawnerView, Input input)
    {
        _isSelected = 
            spawnerView.SelectedIndex != null &&
            spawnerView.SelectedIndex == _indexInArray;

        int balance = spawnerView.Credits;
        _isBuyable = spawnerView.SpecialSelections[_indexInArray].IsBuyable(balance);

        _percentageRemaining = !_isBuyable ? 
            spawnerView.SpecialSelections[_indexInArray].PercentageRemaining :
            0.0; 
        
        _UpdateMouseOverState(input.MousePositionScreen);
        if (_isMouseOver && input.IsMousePressed)
        {
            input.SetClickProcessed();
            spawnerView.SelectSpecial(_indexInArray + 1);
        }
    }
    
    public void Render(H4D2BitmapCanvas screen)
    {
        int selectedState = _isSelected ? 1 : 0;
        Bitmap buttonBitmap = H4D2Art.GUI.Buttons.Spawner[selectedState];
        int selectedYOffs = selectedState == 1 ? _selectedYOffs : 0;
        
        screen.DrawAbsolute(buttonBitmap, _x, _y);
        screen.DrawAbsolute(
            _specialBitmap,
            _x + _specialXOffs,
            _y + _specialYOffs + selectedYOffs
        );
        if (!_isBuyable)
        {
            screen.DrawSpawnerButtonOverlay(buttonBitmap, _x, _y, _percentageRemaining);
        }

    }

    private void _UpdateMouseOverState(ReadonlyPosition mousePosition)
    {
        if
        (
            _x <= mousePosition.X &&
            mousePosition.X <= _x + Width &&
            _y - Height <= mousePosition.Y &&
            mousePosition.Y <= _y
        )
            _isMouseOver = true;
        else
            _isMouseOver = false;
    }
}