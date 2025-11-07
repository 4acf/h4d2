namespace H4D2.Infrastructure;

public class CountdownTimer
{
    public bool IsFinished => _secondsLeft <= 0;
    
    private double _secondsLeft;
    private readonly double _duration;
    
    public CountdownTimer(double duration)
    {
        _secondsLeft = duration;
        _duration = duration;
    }

    public void Update(double elapsedTime)
    {
        _secondsLeft -= elapsedTime;
    }

    public void Reset()
    {
        _secondsLeft = _duration;
    }

    public void AddDuration()
    {
        _secondsLeft += _duration;
    }
}