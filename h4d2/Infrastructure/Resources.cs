namespace H4D2.Infrastructure;

public static class Resources
{
    private static readonly string _baseDirectory =
        Path.Combine(AppContext.BaseDirectory, "Resources");
    
    public static string MusicPrefix(string musicFilename) =>
        Path.Combine(_baseDirectory, "audio", "music", musicFilename);
    
    public static string SFXPrefix(string sfxFilename) => 
        Path.Combine(_baseDirectory, "audio", "sfx", sfxFilename);
    
    public const string EmbeddedPrefix = "h4d2.Resources";
}