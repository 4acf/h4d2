using System.Text.Json;

namespace H4D2.Infrastructure.H4D2;

public sealed class SaveManager
{
    private class SavedSettings
    {
        public double MusicVolume { get; set; } = 0.5;
        public double SFXVolume { get; set; } = 0.5;
        public bool FullscreenEnabled { get; set; } = false;
    }
    
    private static readonly string _appDataPath = 
        Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            global::H4D2.H4D2.WindowTitle
        );  
    
    private static readonly string _settingsFilePath = Path.Combine(_appDataPath, "settings.json");
    private static readonly string _levelRecordsFilePath = Path.Combine(_appDataPath, "records.json");

    private readonly SavedSettings _savedSettings;
    private readonly Dictionary<int, double> _levelRecords;
    
    private static readonly Lazy<SaveManager> _instance = 
        new(() => new SaveManager());

    private static readonly JsonSerializerOptions _jsonSerializerOptions = 
        new() { WriteIndented = true };
    
    public static SaveManager Instance => _instance.Value;
    
    private SaveManager()
    {
        _CreateFilesIfNotFound();
        
        _savedSettings = 
            _ReadFromFile<SavedSettings>(_settingsFilePath) ?? 
            new SavedSettings();
        _levelRecords = 
            _ReadFromFile<Dictionary<int, double>>(_levelRecordsFilePath) ?? 
            new Dictionary<int, double>();
    }
    
    public double GetMusicVolume() => _savedSettings.MusicVolume;
    public double GetSFXVolume() => _savedSettings.SFXVolume;
    public bool GetFullscreenEnabled() => _savedSettings.FullscreenEnabled;
    public double? GetLevelRecord(int id) => 
        _levelRecords.TryGetValue(id, out double value) ? value : null;
    
    public void SaveNewMusicVolume(double volume)
    {
        if (volume < AudioManager.MinVolume || volume > AudioManager.MaxVolume)
            return;
        _savedSettings.MusicVolume = volume;
        _WriteToFile(_settingsFilePath, _savedSettings);
    }
    
    public void SaveNewSFXVolume(double volume)
    {
        if (volume < AudioManager.MinVolume || volume > AudioManager.MaxVolume)
            return;
        _savedSettings.SFXVolume = volume;
        _WriteToFile(_settingsFilePath, _savedSettings);
    }

    public void SaveNewFullscreenState(bool fullscreenEnabled)
    {
        _savedSettings.FullscreenEnabled = fullscreenEnabled;
        _WriteToFile(_settingsFilePath, _savedSettings);
    }

    public void SaveNewLevelRecord(int id, double time)
    {
        if(!_levelRecords.TryGetValue(id, out double value) || value > time)
            _levelRecords[id] = time;
        _WriteToFile(_levelRecordsFilePath, _levelRecords);
    }

    private static void _CreateFilesIfNotFound()
    {
        Directory.CreateDirectory(_appDataPath);
        
        if (!File.Exists(_settingsFilePath) || new FileInfo(_settingsFilePath).Length == 0)
            File.WriteAllText(
                _settingsFilePath,
                JsonSerializer.Serialize(new SavedSettings(), _jsonSerializerOptions)
            );
        
        if (!File.Exists(_levelRecordsFilePath) || new FileInfo(_levelRecordsFilePath).Length == 0)
            File.WriteAllText(
                _levelRecordsFilePath,
                JsonSerializer.Serialize(new Dictionary<int, double>(), _jsonSerializerOptions)
            );
    }
    
    private static T? _ReadFromFile<T>(string filepath)
    {
        try
        {
            return JsonSerializer.Deserialize<T>(File.ReadAllText(filepath));
        }
        catch
        {
            return default;
        }
    }
    
    private static void _WriteToFile<T>(string filepath, T objectToWrite)
    {
        File.WriteAllText(
            filepath,
            JsonSerializer.Serialize(objectToWrite, _jsonSerializerOptions)
        );
    }
}