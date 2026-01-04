namespace H4D2.Infrastructure;

public static class TimeFormatter
{
    public static string Format(double time)
    {
        int minutes = (int)time / 60;
        double seconds = time % 60;
        return $"{minutes:00}:{seconds:00.00}";
    }
}