using H4D2.Infrastructure;
using H4D2.Infrastructure.H4D2;

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
    private readonly int _x;
    private readonly int _y;
    private bool _isSelected;
    private bool _isMouseOver;
    
    public SpawnerButton(Bitmap specialBitmap, int x, int y)
    {
        _specialBitmap = specialBitmap;
        _x = x;
        _y = y;
        _isSelected = false;
        _isMouseOver = false;
    }
    
    public void Update(bool isSelected, Input input)
    {
        _isSelected = isSelected;
        
        _UpdateMouseOverState(input.MousePositionScreen);
        if (_isMouseOver && input.IsMousePressed)
        {
            Clicked?.Invoke(this, EventArgs.Empty);
        }
    }
    
    public void Render(Bitmap screen)
    {
        int selectedState = _isSelected ? 1 : 0;
        Bitmap buttonBitmap = H4D2Art.Buttons.Spawner[selectedState];
        screen.DrawAbsolute(buttonBitmap, _x, _y);

        int selectedYOffs = selectedState == 1 ? _selectedYOffs : 0;
        screen.DrawAbsolute(
            _specialBitmap,
            _x + _specialXOffs,
            _y + _specialYOffs + selectedYOffs
        );
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