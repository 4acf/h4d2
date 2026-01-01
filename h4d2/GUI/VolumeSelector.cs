using H4D2.Infrastructure;

namespace H4D2.GUI;

public class VolumeSelector
{
    public event EventHandler? ValueUpdated; 
    
    private const int _sections = 10;
    private const int _sectionWidth = 6;
    private const int _sectionHeight = 9;
    private const int _paddingBetween = 2;
    
    public const int Width = _sections * (_sectionWidth + _paddingBetween);
    
    private readonly int _x;
    private readonly int _y;
    private bool _isMouseOver;
    private int _numSelected;
    
    public VolumeSelector(int x, int y, double initialVolume)
    {
        _x = x;
        _y = y;
        _isMouseOver = false;
        _numSelected = (int)(initialVolume * _sections);
    }

    public double GetVolume()
    {
        return (double)_numSelected / _sections;
    }
    
    public void Update(Input input)
    {
        _UpdateMouseOverState(input.MousePositionScreen);
        if (_isMouseOver && input.IsMousePressed)
        {
            double percentage = (input.MousePositionScreen.X - _x) / (Width);
            const double threshold = (_sectionWidth / 2.0) / (_sectionWidth + _paddingBetween) / _sections;
            if (percentage < threshold)
                _numSelected = 0;
            else
                _numSelected = (int)Math.Ceiling(percentage * _sections);
            ValueUpdated?.Invoke(this, EventArgs.Empty);
        }
    }

    public void Render(Bitmap screen)
    {
        for (int i = 0; i < _sections; i++)
        {
            int color = (i + 1) <= _numSelected ? 0xffffff : 0x202020;
            int startingX = _x + (i * (_sectionWidth + _paddingBetween));
            screen.FillAbsolute
            (
                startingX, 
                _y - _sectionHeight - 1,
                startingX + _sectionWidth - 1,
                _y - _sectionHeight - 1,
                0x0
            );
            screen.FillAbsolute
            (
                startingX, 
                _y - _sectionHeight,
                startingX + _sectionWidth - 1,
                _y,
                color
            );
            
        }
    }
    
    private void _UpdateMouseOverState(ReadonlyPosition mousePosition)
    {
        int xUpperBound = (_x + (_sectionWidth + _paddingBetween) * _sections) - _paddingBetween;
        
        if
        (
            _x <= mousePosition.X &&
            mousePosition.X <= xUpperBound &&
            _y - _sectionHeight <= mousePosition.Y &&
            mousePosition.Y <= _y
        )
            _isMouseOver = true;
        else
            _isMouseOver = false;
    }
}