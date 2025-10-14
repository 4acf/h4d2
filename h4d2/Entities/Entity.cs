using H4D2.Infrastructure;

namespace H4D2.Entities;

public abstract class Entity
{
    public int XPosition { get; protected set; }
    public int YPosition { get; protected set; }

    protected Entity(int xPosition, int yPosition)
    {
        XPosition = xPosition;
        YPosition = yPosition;
    }
    
    public abstract void Update(double timeElapsed);
    public abstract void Render(Bitmap screen);
}