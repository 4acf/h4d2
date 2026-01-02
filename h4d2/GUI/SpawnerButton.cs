using H4D2.Infrastructure;
using H4D2.Infrastructure.H4D2;

namespace H4D2.GUI;

public class SpawnerButton
{
    public event EventHandler? Clicked;

    public const int Width = H4D2Art.SpawnerButtonWidth;
    public const int Height = H4D2Art.SmallButtonHeight;
    
    private readonly int _x;
    private readonly int _y;
    private bool _isSelected;
    private bool _isMouseOver;
    
    public SpawnerButton(int x, int y)
    {
        _x = x;
        _y = y;
        _isSelected = false;
        _isMouseOver = false;
    }
    
    public void Update(Input input)
    {
        _UpdateMouseOverState(input.MousePositionScreen);
        if (_isMouseOver && input.IsMousePressed)
        {
            _isSelected = !_isSelected;
            Clicked?.Invoke(this, EventArgs.Empty);
        }
    }
    
    public void Render(Bitmap screen)
    {
        int selectedState = _isSelected ? 1 : 0;
        Bitmap bitmap = H4D2Art.Buttons.Spawner[selectedState];
        screen.DrawAbsolute(bitmap, _x, _y);
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