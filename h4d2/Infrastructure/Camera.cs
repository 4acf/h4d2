namespace H4D2.Infrastructure;

public class Camera
{
    public int XOffset { get; private set; }
    public int YOffset { get; private set; }

    public Camera()
    {
        XOffset = 0;
        YOffset = 0;
    }
    
    public Camera(int xOffset, int yOffset)
    {
        XOffset = xOffset;
        YOffset = yOffset;
    }

    public void MoveXY(int x, int y)
    {
        XOffset -= x;
        YOffset -= y;
    }

    public void MoveX(int x)
    {
        XOffset -= x;
    }

    public void MoveY(int y)
    {
        YOffset -= y;
    }
}