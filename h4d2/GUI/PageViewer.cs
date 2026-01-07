using H4D2.Infrastructure.H4D2;

namespace H4D2.GUI;

public class PageViewer
{
    public const int Scale = 4;

    private const int _offColor = 0x333333;
    private const int _onColor = 0xffffff;
    
    private readonly int _numPages;
    private readonly int _x;
    private readonly int _y;
    private int _page;
    
    public PageViewer(int numPages, int y, int screenWidth, int page = 0)
    {
        _numPages = numPages;

        int componentWidth = (numPages * Scale) + ((numPages - 1) * Scale);
        _x = (screenWidth / 2) - (componentWidth / 2);
        
        _y = y;
        _page = page;
    }

    public void Update(int page)
    {
        _page = page;
    }

    public void Render(H4D2BitmapCanvas screen)
    {
        int x = _x;
        for (int i = 0; i < _numPages; i++)
        {
            bool isOn = i == _page;
            int color = isOn ? _onColor : _offColor;
            screen.FillAbsolute(x, _y, x + Scale - 1, _y + Scale - 1, color);
            x += Scale * 2;
        }
    }
}