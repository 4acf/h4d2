using System.Text.Json;

namespace H4D2.Infrastructure.H4D2;

public class SaveManager
{
    private class SavedSettings
    {
        public double MusicVolume { get; set; } = 1.0;
        public double SFXVolume { get; set; } = 1.0;
    }
    
    private const string _settingsFilePath = "settings.json";
    private const string _levelRecordsFilePath = "records.json";

    private readonly SavedSettings _savedSettings;
    private readonly Dictionary<int, double> _levelRecords;
    
    public SaveManager()
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
    public double? GetLevelRecord(int id) => _levelRecords.TryGetValue(id, out double value) ? value : null;
    
    public void UpdateMusicVolume(double volume)
    {
        if (volume < 0.0 || volume > 1.0)
            return;
        _savedSettings.MusicVolume = volume;
        _WriteToFile(_settingsFilePath, _savedSettings);
    }

    public void UpdateSFXVolume(double volume)
    {
        if (volume < 0.0 || volume > 1.0)
            return;
        _savedSettings.SFXVolume = volume;
        _WriteToFile(_settingsFilePath, _savedSettings);
    }

    public void UpdateLevelRecord(int id, double time)
    {
        if(!_levelRecords.TryGetValue(id, out double value) || value > time)
            _levelRecords[id] = time;
        _WriteToFile(_levelRecordsFilePath, _levelRecords);
    }

    private static void _CreateFilesIfNotFound()
    {
        if (!File.Exists(_settingsFilePath) || new FileInfo(_settingsFilePath).Length == 0)
            File.WriteAllText(
                _settingsFilePath,
                JsonSerializer.Serialize(new SavedSettings())
            );
        
        if (!File.Exists(_levelRecordsFilePath) || new FileInfo(_levelRecordsFilePath).Length == 0)
            File.WriteAllText(
                _levelRecordsFilePath,
                JsonSerializer.Serialize(new Dictionary<int, double>())
            );
    }
    
    private static T? _ReadFromFile<T>(string filepath)
    {
        return JsonSerializer.Deserialize<T>(File.ReadAllText(filepath));
    }
    
    private static void _WriteToFile<T>(string filepath, T objectToWrite)
    {
        File.WriteAllText(filepath, JsonSerializer.Serialize(objectToWrite));
    }
}